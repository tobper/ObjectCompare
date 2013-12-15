using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ObjectCompare.Expressions
{
    public static class ExpressionBuilder
    {
        private static readonly MethodInfo ReferenceEqualsMethod = GetReferenceEqualsMethod();
        private static readonly ConstructorInfo ObjectPairConstructor = GetObjectPairConstructor();
        private static readonly MethodInfo ObjectPairHashSetContainsMethod = GetObjectPairHashSetContainsMethod();
        private static readonly MethodInfo ObjectPairHashSetAddMethod = GetObjectPairHashSetAddMethod();

        public static Func<T, T, ObjectPairHashSet, bool> CompileQuery<T>(ObjectComparerSettings settings)
        {
            var type = typeof(T);
            var typeInfo = type.GetTypeInfo();
            var param1 = type.AsParameter("object1");
            var param2 = type.AsParameter("object2");
            var visitedObjects = typeof(ObjectPairHashSet).AsParameter("visitedObjects");

            var returnLabel = Expression.Label(typeof(bool), "result");
            var context = new ExpressionBuilderContext
            {
                Settings = settings,
                ReturnFalse = returnLabel.AsReturn(false),
                VisitedObjects = visitedObjects,
                CompiledQueries = new Dictionary<Type, LambdaExpression>()
            };

            var statements = new[]
            {
                Compare(context, typeInfo, param1, param2),
                returnLabel.AsExpression(true)
            };

            return statements.
                AsBlock().
                AsLambda<Func<T, T, ObjectPairHashSet, bool>>(param1, param2, visitedObjects).
                Compile();
        }

        private static Expression Compare(
            ExpressionBuilderContext context,
            TypeInfo valueTypeInfo,
            ParameterExpression value1,
            ParameterExpression value2)
        {
            return valueTypeInfo.IsValueType
                ? CompareValues(context, value1, value2)
                : CompareReferences(context, valueTypeInfo, value1, value2);
        }

        private static Expression CompareValues(
            ExpressionBuilderContext context,
            Expression value1,
            Expression value2)
        {
            var notEquals = Expression.NotEqual(value1, value2);

            return Expression.IfThen(notEquals, context.ReturnFalse);
        }

        private static Expression CompareReferences(
            ExpressionBuilderContext context,
            TypeInfo objectTypeInfo,
            ParameterExpression object1,
            ParameterExpression object2)
        {
            Expression compare =
                CompareEquatables(context, objectTypeInfo, object1, object2) ??
                CompareCollections(context, objectTypeInfo, object1, object2) ??
                CompareEnumerables(context, objectTypeInfo, object1, object2) ??
                CompareMembers(context, objectTypeInfo, object1, object2);

            if (compare == null)
                return Expression.Empty();

            var objectPair = typeof(ObjectPair).AsVariable("objectPair");
            var object1HasValue = object1.IsNotNull();
            var object2HasValue = object2.IsNotNull();
            var referenceEquals = ReferenceEqualsMethod.AsCall(object1, object2);

            return
                Expression.IfThenElse(
                    // if ((object1 != null) != (object2 != null))
                    Expression.NotEqual(object1HasValue, object2HasValue),
                    // return false;
                    context.ReturnFalse,
                    // else
                    Expression.IfThen(
                        // if (object1 != null && ReferenceEquals(object1, object2) == false
                        Expression.AndAlso(object1HasValue, referenceEquals.IsFalse()),
                        Expression.Block(
                            // var objectPair
                            new[] { objectPair },
                            // objectPair = new ObjectPair(object1, object2)
                            AssignNewObjectPairToVariable(objectPair, object1, object2),
                            Expression.IfThen(
                                // if (visitedObjects.Contains(objectPair) == false)
                                CheckIfObjectPairIsNew(context, objectPair),
                                Expression.Block(
                                    // visitedObjects.Add(objectPair)
                                    AddObjectPairToVisitedObjects(context, objectPair),
                                    // Compare...
                                    compare)))));
        }

        private static Expression AssignNewObjectPairToVariable(ParameterExpression objectPair, ParameterExpression object1, ParameterExpression object2)
        {
            return objectPair.AsNew(ObjectPairConstructor, object1, object2);
        }

        private static Expression CheckIfObjectPairIsNew(ExpressionBuilderContext context, ParameterExpression objectPair)
        {
            return context.VisitedObjects.
                InstanceCall(ObjectPairHashSetContainsMethod, objectPair).
                IsFalse();
        }

        private static Expression AddObjectPairToVisitedObjects(ExpressionBuilderContext context, ParameterExpression objectPair)
        {
            return context.VisitedObjects.InstanceCall(ObjectPairHashSetAddMethod, objectPair);
        }

        private static Expression CompareEquatables(
            ExpressionBuilderContext context,
            TypeInfo objectTypeInfo,
            ParameterExpression object1,
            ParameterExpression object2)
        {
            if (context.Settings.UseEquatable)
            {
                var objectType = objectTypeInfo.AsType();
                var equatableType = typeof(IEquatable<>).MakeGenericType(objectType);

                if (SupportsInterface(objectTypeInfo, equatableType))
                {
                    var equalsMethod = equatableType.GetRuntimeMethod("Equals", new[] { objectType });
                    var objectsAreEqual = equalsMethod.AsInstanceCall(object1, object2);
                    var objectsAreNotEqual = objectsAreEqual.IsFalse();

                    return Expression.IfThen(objectsAreNotEqual, context.ReturnFalse);
                }
            }

            return null;
        }

        private static Expression CompareCollections(
            ExpressionBuilderContext context,
            TypeInfo objectTypeInfo,
            ParameterExpression object1,
            ParameterExpression object2)
        {
            var collectionType = typeof(ICollection<>);
            var genericInterface = GetGenericInterface(objectTypeInfo, collectionType);

            if (genericInterface != null)
            {
                var call = typeof(CompiledObjectComparer<>).
                    MakeGenericType(genericInterface.GenericTypeArguments[0]).
                    GetTypeInfo().
                    DeclaredMethods.Single(m => m.Name == "CollectionEquals").
                    AsCall(object1, object2, context.VisitedObjects);

                return Expression.IfThen(call.IsFalse(), context.ReturnFalse);
            }

            return null;
        }

        private static Expression CompareEnumerables(
            ExpressionBuilderContext context,
            TypeInfo objectTypeInfo,
            ParameterExpression object1,
            ParameterExpression object2)
        {
            var enumerableType = typeof(IEnumerable<>);
            var genericInterface = GetGenericInterface(objectTypeInfo, enumerableType);

            if (genericInterface != null)
            {
                var call = typeof(CompiledObjectComparer<>).
                    MakeGenericType(genericInterface.GenericTypeArguments[0]).
                    GetTypeInfo().
                    DeclaredMethods.Single(m => m.Name == "EnumerableEquals").
                    AsCall(object1, object2, context.VisitedObjects);

                return Expression.IfThen(call.IsFalse(), context.ReturnFalse);
            }

            return null;
        }

        private static Type GetGenericInterface(TypeInfo objectTypeInfo, Type typeDefinition)
        {
            var types = objectTypeInfo.ImplementedInterfaces.ToList();
            types.Add(objectTypeInfo.AsType());

            var interfaces =
                from iface in types
                where iface.IsConstructedGenericType
                where iface.GetGenericTypeDefinition() == typeDefinition
                select iface;

            return interfaces.FirstOrDefault();
        }

        private static Expression CompareMembers(
            ExpressionBuilderContext context,
            TypeInfo objectType,
            ParameterExpression object1,
            ParameterExpression object2)
        {
            var statements = new List<Expression>();

            var properties = GetValidProperties(context, objectType);
            if (properties.Any())
            {
                statements.AddRange(
                    from property in properties
                    select CompareProperty(context, object1, object2, property));
            }

            var fields = GetValidFields(context, objectType);
            if (fields.Any())
            {
                statements.AddRange(
                    from field in fields
                    select CompareField(context, object1, object2, field));
            }

            return statements.Any()
                ? statements.AsBlock()
                : null;
        }

        private static PropertyInfo[] GetValidProperties(
            ExpressionBuilderContext context,
            TypeInfo objectType)
        {
            var properties =
                from property in objectType.DeclaredProperties
                let isPublic = property.GetMethod.IsPublic
                let isPrivate = !isPublic
                where
                    (isPublic && context.Settings.PublicProperties) ||
                    (isPrivate && context.Settings.ProtectedProperties)
                select  property;

            return properties.ToArray();
        }

        private static FieldInfo[] GetValidFields(
            ExpressionBuilderContext context,
            TypeInfo objectType)
        {
            var fields =
                from field in objectType.DeclaredFields
                where field.IsPrivate && context.Settings.PrivateFields
                select field;

            return fields.ToArray();
        }

        private static BlockExpression CompareProperty(
            ExpressionBuilderContext context,
            ParameterExpression object1,
            ParameterExpression object2,
            PropertyInfo property)
        {
            var propertyType = property.PropertyType;
            var propertyVariable1 = propertyType.AsVariable(object1.Name + "_" + property.Name);
            var propertyVariable2 = propertyType.AsVariable(object2.Name + "_" + property.Name);

            var variables = new[]
            {
                propertyVariable1,
                propertyVariable2
            };

            var body = new[]
            {
                property.GetMethod.
                    AsInstanceCall(object1).
                    AsAssign(propertyVariable1),
                property.GetMethod.
                    AsInstanceCall(object2).
                    AsAssign(propertyVariable2),
                CompareChild(context, propertyType, propertyVariable1, propertyVariable2)
            };

            return body.AsBlock(variables);
        }

        private static Expression CompareField(
            ExpressionBuilderContext context,
            ParameterExpression object1,
            ParameterExpression object2,
            FieldInfo field)
        {
            var fieldType = field.FieldType;
            var fieldVariable1 = fieldType.AsVariable(object1.Name + "_" + field.Name);
            var fieldVariable2 = fieldType.AsVariable(object2.Name + "_" + field.Name);

            var variables = new[]
            {
                fieldVariable1,
                fieldVariable2
            };

            var body = new[]
            {
                field.
                    Value(object1).
                    AsAssign(fieldVariable1),
                field.
                    Value(object2).
                    AsAssign(fieldVariable2),
                CompareChild(context, fieldType, fieldVariable1, fieldVariable2)
            };

            return body.AsBlock(variables);
        }

        private static Expression CompareChild(
            ExpressionBuilderContext context,
            Type childType,
            ParameterExpression child1,
            ParameterExpression child2)
        {
            var call = typeof(CompiledObjectComparer<>).
                MakeGenericType(childType).
                GetTypeInfo().
                DeclaredMethods.Single(m => m.Name == "Equals" && m.GetParameters().Length == 3).
                AsCall(child1, child2, context.VisitedObjects);

            return Expression.IfThen(call.IsFalse(), context.ReturnFalse);
        }

        private static bool SupportsInterface(TypeInfo type, Type iface)
        {
            return
                type.AsType() == iface ||
                type.ImplementedInterfaces.Contains(iface);
        }

        private static MethodInfo GetReferenceEqualsMethod()
        {
            return typeof(object).
                GetTypeInfo().
                DeclaredMethods.Single(m => m.Name == "ReferenceEquals");
        }

        private static ConstructorInfo GetObjectPairConstructor()
        {
            return typeof(ObjectPair).
                GetTypeInfo().
                DeclaredConstructors.Single();
        }

        private static MethodInfo GetObjectPairHashSetContainsMethod()
        {
            return typeof(HashSet<ObjectPair>).
                GetTypeInfo().
                DeclaredMethods.Single(m => m.Name == "Contains");
        }

        private static MethodInfo GetObjectPairHashSetAddMethod()
        {
            return typeof(HashSet<ObjectPair>).
                GetTypeInfo().
                DeclaredMethods.Single(m => m.Name == "Add");
        }
    }
}
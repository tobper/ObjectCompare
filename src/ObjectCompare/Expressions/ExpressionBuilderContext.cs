using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ObjectCompare.Expressions
{
    public class ExpressionBuilderContext
    {
        public ObjectComparerSettings Settings { get; set; }
        public GotoExpression ReturnFalse { get; set; }
        public ParameterExpression VisitedObjects { get; set; }
        public IDictionary<Type, LambdaExpression> CompiledQueries { get; set; }
    }
}
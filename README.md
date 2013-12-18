ObjectCompare
-------------

Recursive object comparer for .Net.

### Order of execution

1. Value type check

   Performs an equal check (==) if the type is a value type.

1. Null value check

   Values are equal if both values are null.
   Values are **not** equal if one of the values is null.

1. ReferenceEquals check

   Values are equal if Object.ReferenceEquals returns true.

1. Visited objects check

   Ensures that the object pair has not already been evaluated to prevent infinite loops which will lead to stack overflows.

   ```c#
   class X
   {
       public Y Y { get; set; }
   }

   class Y
   {
       public X X { get; set; }
   }
   ```

   The following code will work because the combination of x1 and x2 has already been evaluated when x.Y.X is reached.  

   ```c#
   var x1 = new X { Y = new Y() };
   var x2 = new X { Y = new Y() };
   x1.Y.X = x1;
   x2.Y.X = x2;

   ObjectComparer.Equals(x1, x2);
   ```

   More complicated relationships will work the same way; x1.Y.X is the same object as x2.X.Y.X.Y.X.

   ```c#
   var x1 = new X { Y = new Y() };
   var x2 = new X { Y = new Y { X = new X { Y = new Y() } } };
   x1.Y.X = x1;
   x2.Y.X.Y.X = x2;

   ObjectComparer.Equals(x1, x2);
   ```
 

1. IEquatable<T> check

   Calls `Equals<T>(T other)` to determine if the objects are equal if the type implements `IEquatable<T>`.

1. Member check

   Compares public properties, protected properties and private fields according to specified [settings](#settings).

### Settings

- PrivateFields

  Specify true if private fields should be included in the member check.  
  Default values is false.

- ProtectedProperties

  Specify true if protected properties should be included in the member check.  
  Default values is false.

- PublicProperties

  Specify true if public properties should be included in the member check.  
  Default values is true.

- UseEquatable

  Specify true if IEquatable<T> should be used if types implements it.  
  Default values is true.

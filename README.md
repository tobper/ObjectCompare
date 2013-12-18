ObjectCompare
-------------

Recursive object comparer for .Net.

### Usage

```c#
ObjectComparer.Equals(1, 2);
```

```c#
var x = new { Name = "John" };
var y = new { Name = "Jane" };

ObjectComparer.Equals(x, y);
```

```c#
var x = new[] { 1, 2 };
var y = new[] { 2, 3 };

ObjectComparer.Equals(x, y);
```

### Order of execution

Each object pair will be compared according to the following order of execution.

1. Value type check

   Objects are equal if the type is a value type and an equal check (==) returns true;

1. Null value check

   Objects are equal if both references are null.  
   Objects are **not** equal if one of the references is null.

1. ReferenceEquals check

   Objects are equal if `Object.ReferenceEquals(x1, x2)` returns true.

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

1. IEquatable&lt;T&gt; check

   Objects are equal if the type implements `IEquatable<T>` and `x1.Equals(x2)` returns true.

1. ICollection&lt;T&gt; check

   Objects are not equal if the type implements `ICollection<T>` and `x1.Count != x2.Count`.  

1. IEnumerable&lt;T&gt; check

   Objects are equal if the type implements `IEbumerable<T>`, all the items are equal and the enumerables are of the same length.

1. Member check

   Compares public properties, protected properties and private fields according to specified [settings](#settings).  
Each member pair will be compared from the top of this list. 

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

### Contact

Feel free to send a mail to objectcompare@ohnoes.eu with questions or comments.

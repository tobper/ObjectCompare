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

   *Todo*

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

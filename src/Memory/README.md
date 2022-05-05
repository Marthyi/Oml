Welcome to Oml Packages project

## Oml.Memory
- 100% code coverage
- Access easily to your embedded resources.

### Example
```csharp	

var collection.Add(new User("sean", "paul"));
var collection.Add(new User("sean", "connery"));
var collection.Add(new User("sean", "penn"));

	 collection.OptimizeStringMemory();

// memory is reduced from 3 instance to only one in GC for the firsname (sean).

````

It removes from memory all duplicated string properties and string fields from collection.
string object is immutable so we can point to the same string value from all objects.
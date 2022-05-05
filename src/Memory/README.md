Welcome to Oml Packages project

## Oml.Resources
- 100% code coverage
- Access easily to your embedded resources.

### Read binary content
```csharp	
	Stream stream = EmbeddedResources.ReadAsStream(CurrentAssembly, "file.zip");
````

### Read text content
```csharp	
	string text = EmbeddedResources.ReadAsText(CurrentAssembly, "file.csv");
````

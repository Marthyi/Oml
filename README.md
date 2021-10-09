Welcome to Oml Packages project

## Oml.Resources
- 100% code coverage
- Access easily to your embedded resources.

```csharp
	Stream stream = EmbeddedResources.ReadAsStream(CurrentAssembly, "file.zip");
	string text = EmbeddedResources.ReadAsText(CurrentAssembly, "file.txt");
````

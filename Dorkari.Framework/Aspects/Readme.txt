1. Adding aspect as multicast policies

- Add PostSharp NuGet to the project
- Add to the project AssemblyInfo.cs
[assembly: LogMethodExecutionAttribute(AttributeTargetTypes = "Base.Namespace.*",
                                AttributeTargetMembers = "SomeMethodName")]

2. Adding to individual entities

- Add PostSharp NuGet to the project
- Add [LogException] attribute to required method
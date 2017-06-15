1. Adding Unity interceptors as policies

- Add Unity NuGet to the project
- Add to the method UnityResolverBuilder.RegisterPolicies()
- Example
container.Configure<Interception>()
    .AddPolicy("logging")
    .AddMatchingRule(new NamespaceMatchingRule("Base.Namespace.Models"))
    .AddMatchingRule(new MemberNameMatchingRule("SomeMethodName"))
    .AddCallHandler(new LoggingCallHandler());
container.Configure<Interception>()
    .AddPolicy("exception")
    .AddMatchingRule(new NamespaceMatchingRule("Dorkari.Core.Application.*"))
    .AddCallHandler(new ExceptionCallHandler());

2. Adding to individual entities

- Add Unity NuGet to the project
- Add [LogException(1)] attribute to required method (1 is order of interceptor in pipeline)

** For either case, target interfaces has to register themselves with (allowPolicyInjection = true)
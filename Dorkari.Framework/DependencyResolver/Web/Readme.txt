This shows how to use a unity resolver for standard MVC & Web API projects

Add the following to Global.asax Application_Start()

DependencyResolverBase resolver = UnityResolverBuilder.Build(); //NOTE: To use another container, replace this with another implementation
resolver.RegisterInstance<IObjectResolver>(resolver); //NOTE: Now IResolver will be available app-wide (IObjectResolver registering itself, how cool is that!)
//Keep a app-wide static copy of resolver if required
System.Web.Mvc.DependencyResolver.SetResolver(resolver); //MVC resolver
config.DependencyResolver = resolver; //Web API resolver
# TODO
---

### Issues
1. Use patch DTOs of Patient and PatientContact.
1. Add validation for requests.
	1. Make a note of how validation occurs in web apis.
1. Add proper exception handling to try/catch blocks.
	1. Use applicaton lifetime service to shutdown application when exception is encountered.
		* https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostapplicationlifetime?view=dotnet-plat-ext-3.1
	2. *Solved* https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonelement.getproperty?view=netcore-3.1#System_Text_Json_JsonElement_GetProperty_System_String_
	2. *Solved* Now that Db is handled by a special class, move all exceptions.
1. *Solved* https://www.reddit.com/r/dotnet/comments/gozrvm/help_entity_type_with_clr_type/
	1. Remove the `HasNoKey`.

### Enhancements
1. Research having models be in their own project.
	* This maybe: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/
2. Implement HTTPS & HTTPS redirection in docker.
3. Research into using patch requests instead of put for updates.
4. https://github.com/Giorgi/EntityFramework.Exceptions
	* Not needed anymore since DBHandlerException may cover all bases (for now).
5. Use a custom UnhandledException event handler
	* https://docs.microsoft.com/en-us/dotnet/api/system.appdomain.unhandledexception?view=netcore-3.1

# C# Notes

### C# Notes
1. The best way to handle an exception is to not handle it at all. The exception will automatically cause the program to fatally close. If you choose to have custom implementation, modify the `AppDomain.UnhandledException` event.
2. WHen re-throwing an exception, log the error with the exception, and re-throw using `throw;`, *NOT* `throw e;`. The latter will remove critical stack trace information about the caught exception.
3. It's better to do queries with `IQueryable<T>` instead of `IEnumerable<T>`
since the former will do the query on the database and only transfer the
applicable rows over the network. The later will transfer all rows over
the network and do the query locally.
4. `IEnumerable<T>` can be used to switch away from Linq-To-Sql queries with
`IQueryable<T>` to Linq-To-Object queries, which can use functions.
	* `context.Observations.Select(o => o.Id).AsEnumerable().Select(x => MySuperSmartMethod(x))`
5. `IEnumerable` is an interface that defines one method `GetEnumerator` which returns an `IEnumerator` interface, this in turn allows readonly access to a collection. A collection that implements `IEnumerable` can be used with a foreach statement.

### .NET Core Notes
1. `DataType` annotations are only for display formatting, not validation. Use
the validation attributes in `System.ComponentModel.DataAnnotations` for
validation.
2. You are able to use data annotations for validation checking for HTTP request sources.
	* E.g. `[Required] [FromQuery] string firstName`
	* Required validation is not needed for `[FromBody]`.
3. To validate whether a Guid isn't empty or a structure doesn't have default values, use this reference.
	* https://andrewlock.net/creating-an-empty-guid-validation-attribute/
4. The general convention for log levels:
    * Trace - Only when I would be "tracing" the code and trying to find one part of a function specifically.
    * Debug - Information that is diagnostically helpful to people more than just developers (IT, sysadmins, etc.).
    * Info - Generally useful information to log (service start/stop, configuration assumptions, etc). Info I want to always have available but usually don't care about under normal circumstances. This is my out-of-the-box config level.
    * Warn - Anything that can potentially cause application oddities, but for which I am automatically recovering. (Such as switching from a primary to backup server, retrying an operation, missing secondary data, etc.)
    * Error - Any error which is fatal to the operation, but not the service or application (can't open a required file, missing data, etc.). These errors will force user (administrator, or direct user) intervention. These are usually reserved (in my apps) for incorrect connection strings, missing services, etc.
    * Critical - Any error that is forcing a shutdown of the service or application to prevent data loss (or further data loss). I reserve these only for the most heinous errors and situations where there is guaranteed to have been data corruption or loss.
5. Using the `StringBuilder` class to create strings rather than string concatenation is more efficient
    * https://docs.microsoft.com/en-us/troubleshoot/dotnet/csharp/string-concatenation
    * https://dotnetcoretutorials.com/2020/02/06/performance-of-string-concatenation-in-c/


### ORM/EF Core Notes
1. Here is the fluent API code for owned classes.
	* `modelBuilder.Entity<PatientContact>().OwnsOne(e => e.Address).WithOwner(e => e.PatientContact)`
2. Use this package to add database probes for a wide variety of DBMS.
	* https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks

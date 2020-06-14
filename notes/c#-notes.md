# C# Notes
---

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

### ASP.NET Core Notes
1. `DataType` annotations are only for display formatting, not validation. Use
the validation attributes in `System.ComponentModel.DataAnnotations` for
validation.

### ORM/EF Core Notes
1. Here is the fluent API code for owned classes.
	* `modelBuilder.Entity<PatientContact>().OwnsOne(e => e.Address).WithOwner(e => e.PatientContact)`

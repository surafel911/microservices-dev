# .NET Notes

### C# Notes
1. The best way to handle an exception is to not handle it at all. The exception will automatically cause the program to fatally close. If you choose to have custom implementation, modify the AppDomain.UnhandledException event.

### .NET Core Notes
1. `DataType` annotations are only for display formatting, not validation. Use
the validation attributes in `System.ComponentModel.DataAnnotations` for
validation.

### ORM/EF Core Notes
1. It's better to do queries with `IQueryable<T>` instead of `IEnumerable<T>`
since the former will do the query on the database and only transfer the
applicable records over the network. The later will transfer all records over
the network and do the query locally.
2. Here is the fluent API code for owned classes.
	* modelBuilder.Entity<PatientContact>().OwnsOne(e => e.Address).WithOwner(e => e.PatientContact);

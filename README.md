## A framework for fast reads of data models in .NET, using Redis

ReadModels is optimized for situations where read-operations must execute as fast as possible, 
at the cost of extra processing during write-operations. 

### Features:
	
 - Persists POCO-based models of data for high-performance read operations
 - Generates inverted indexes (sets) based on user-defined index classes
 - Combines indexes into composite indexes (intersections) for multi-parameter, structured searches
 - Simple query API, based on pre-defined index objects
 - Uses Redis key-value storage
 - Framework for adding indexes follows the SOLID principles of OCP (open-closed) and SRP (single responsibility), so that new indexes can be added by simply creating a new class that overrides a couple of methods.
 
The framework is expected to be used in an architecture where write-operations to a master, transactional store (i.e., the "write-model"), will cause the updates to the read-model 
to occur asynchronously, achieving eventual consistency with the write-model.

As the read-model is updated to reflect changes in the write-model, indexes are maintained based on pre-defined search and lookup requirements.

In this way, the performance of read-operations can be optimized without relying on standarad caching techniques, avoiding the issues related to caching such as stale data, invalidation schemes and warm-up times. (Caching can of course be added on top, to even further improve overall read-performance.) 


### Example

```csharp
//basic read model
public class Person
{
	public int Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	// etc...
}

// Requirement: able to search by FirstName and sort by LastName

// 1.  Create an index class for searching by FirstName
public class FirstNameIndex : Index<Person>
{
	public override IEnumerable<string> CreateKeys(Person entitiy)
	{
		yield return CreateKey(entitiy.FirstName);
	}
}

// 2. Create a Sort class for sorting by LastName
public class OrderByLastName : Sort<Person>
{
	public override string FindValue(Person entity)
	{
		// this will actually sort by LastName, then FirstName
		return entity.LastName + " " + entity.FirstName;
	}
}

// 3.  Register our new sort and index classes with the container
// (typical code, not shown)

/**** Test ****/

// setup, add some people
var persister = container.Resolve<IEntityPersister<Person>>();
persister.Store(new Person() { Id = 1, FirstName = "John", LastName = "Smith" });
persister.Store(new Person() { Id = 2, FirstName = "John", LastName = "Doe" });
persister.Store(new Person() { Id = 3, FirstName = "Jane", LastName = "Doe" });	

// act - build a query and pass it to a repository	
var query = new IndexQuery<Person>();
query.PageSize = 20;
query.PageNumber = 1;
query.Sort = new OrderByLastName();
query.AddIndex(new FirstNameIndex(), "John");
// can add more indexes to the query for intersection behavior...

var repository = container.Resolve<IEntityRepository<Person>>();
var result = respository.Find(query);

// assert
Assert.Equal(2, result.Results.Count()); // found 2 Johns
Assert.True(result.Results.ElementAt(0).Id == 2); // first item is John Doe
Assert.True(result.Results.ElementAt(1).Id == 1); // second item is John Smith		



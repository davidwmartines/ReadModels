## A framework for fast reads of data models in .NET, using Redis

ReadModels is optimized for situations where read-operations must execute as fast as possible, 
at the cost of extra processing during write-operations. 

The framework is expected to be used in an architecture where write-operations to a master, transactional store (i.e., the "write-model"), will cause the updates to the read-model 
to occur asynchronously, achieving eventual consistency with the write-model.

As the read-model is updated to reflect changes in the write-model, indexes are maintained based on pre-defined search and lookup requirements.

In this way, the performance of read-operations can be optimized without relying on standarad caching techniques, avoiding the issues related to caching such as stale data, invalidation schemes and warm-up times. (Caching can of course be added on top, to even further improve overall read-performance.) 

### Features:
	
 - Persists POCO-based models of data for high-performance read operations
 - Generates inverted indexes (sets) based on user-defined index classes
 - Combines indexes into composite indexes (intersections) for multi-parameter, structured searches
 - Simple query API, based on pre-defined index objects
 - Uses Redis key-value storage
 - Framework for adding indexes follows the SOLID principles of OCP (open-closed) and SRP (single responsibility), so that new indexes can be added by simply creating a new class that overrides a couple of methods.
 
TODO: examples...
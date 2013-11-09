# An Open Source framework for .NET, using Redis

Optimized for situations where read-operations must execute as fast as possible, 
at the cost of extra processing during write-operations.  
This is to be used in an architecture where write-operations will cause the updates to the read-model 
to occur asynchronously, achieving eventual consistency with the master transational store (i.e., the "write-model").

### Features:
	
	* Persists POCO-based models of data for high-performance read operations
	* Generates inverted indexes (sets) based on user-defined index classes
	* Combines indexes into composite indexes (intersections) for multi-parameter, structured searches
	* Simple query API, based on pre-defined index objects
	* Uses Redis key-value storage
	* Framework for adding indexes follows the SOLID principles of OCP (open-closed) and SRP (single responsibility), so that new indexes can be added by simple creating a new class that override a couple of methods.
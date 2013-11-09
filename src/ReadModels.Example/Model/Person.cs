using System;
using System.Collections.Generic;

namespace ReadModels.Example.Model
{
	public class Person
	{
		public int Id { get; set; }
		public string NamePrefix { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string NameSufix { get; set; }
		public string FullName { get; set; }
		public DateTime? DateOfBirth { get; set; }

		public IEnumerable<PersonLocation> Locations { get; set; }
	
	}
}

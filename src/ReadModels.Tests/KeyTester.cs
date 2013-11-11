using System;
using NUnit.Framework;

namespace ReadModels.Tests
{
	public static class KeyTester
	{
		public static void AssertIndexKey(Type entityType, string propertyName, string propertyValue, string actualKey)
		{
			var expected = string.Concat(entityType.Name, ":", propertyName, ":", propertyValue).ToUpperInvariant();
			Assert.AreEqual(expected, actualKey);
		}

	}
}

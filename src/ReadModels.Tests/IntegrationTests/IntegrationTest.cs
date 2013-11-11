using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ServiceStack.Redis;

namespace ReadModels.Tests.IntegrationTests
{
	[TestFixture]
	public class IntegrationTest
	{
		protected IRedisClient RedisClient;
		protected Container Container;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			RedisClient = new RedisClient("localhost");
			Container = new Container();
			Container.Configure(RedisClient);
		}

		[SetUp]
		public void SetUp()
		{
			RedisClient.FlushDb();
		}

		//[TearDown]
		//public void TearDown()
		//{
		//}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			RedisClient.Dispose();
		}

	}
}

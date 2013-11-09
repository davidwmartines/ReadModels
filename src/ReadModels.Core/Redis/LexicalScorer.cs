using System;
using System.Security.Cryptography;
using System.Text;
using ServiceStack.Redis;

namespace HelloRedis.Redis
{
	public static class LexicalScorer
	{
		private static SHA1CryptoServiceProvider _sha1 = new SHA1CryptoServiceProvider();

		public static double GetLexicalScore(this string value)
		{
			var bytes = Encoding.UTF8.GetBytes(value.PadLeft(8));
			//var hash = _sha1.ComputeHash(bytes);
			return BitConverter.ToDouble(bytes, 0);
			//var score = RedisClient.GetLexicalScore(value);
		
			//if (value.Length > 4)
			//{
			//	var chunk = value.Substring(4);
			//	while (chunk.Length > 0)
			//	{
			//		var chunkScore = RedisClient.GetLexicalScore(chunk);
			//		score += chunkScore;
			//		if (chunk.Length > 4)
			//		{
			//			chunk = chunk.Substring(4);
			//		}
			//		else
			//		{
			//			break;
			//		}
			//	}
			//}
			//return score;
		}
	}
}

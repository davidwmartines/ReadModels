using NUnit.Framework;

namespace HelloRedis.Redis
{
	[TestFixture]
	public class LexicalScorerTest
	{

		[Test]
		public void CalcluatesScore_4Chars()
		{
			var string1 = "abcd";
			var string2 = "abce";

			var score1 = LexicalScorer.GetLexicalScore(string1);
			var score2 = LexicalScorer.GetLexicalScore(string2);

			Assert.True(score2 > score1);
		}

		[Test]
		public void CalcluatesScore_5Chars()
		{
			var string1 = "abcde";
			var string2 = "abcdf";

			var score1 = LexicalScorer.GetLexicalScore(string1);
			var score2 = LexicalScorer.GetLexicalScore(string2);

			Assert.True(score2 > score1);
		}

		[Test]
		public void CalcluatesScore_8Chars()
		{
			var string1 = "abcdefgh";
			var string2 = "abcdefgi";

			var score1 = LexicalScorer.GetLexicalScore(string1);
			var score2 = LexicalScorer.GetLexicalScore(string2);

			Assert.True(score2 > score1);
		}

		[Test]
		public void CalcluatesScore_27Chars()
		{
			var string1 = "abcdefghijklmnopqrstuvwxyza";
			var string2 = "abcdefghijklmnopqrstuvwxyzb";

			var score1 = LexicalScorer.GetLexicalScore(string1);
			var score2 = LexicalScorer.GetLexicalScore(string2);

			Assert.True(score2 > score1);
		}

		[Test]
		public void CalcluatesScore_VariedChars()
		{
			var string1 = "abcdefg";
			var string2 = "bcd";

			var score1 = LexicalScorer.GetLexicalScore(string1);
			var score2 = LexicalScorer.GetLexicalScore(string2);

			Assert.True(score2 > score1);
		}
	}
}

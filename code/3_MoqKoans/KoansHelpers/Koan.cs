using NUnit.Framework;

namespace MoqKoansCore.KoansHelpers
{
	public class Koan
	{
		private const string EMPTY_ASSERT_MSG = "Replace the method call \"Assert___()\" with an appropriate Assert statement.";
		private const string EMPTY_ANSWER_MSG = "Replace \"___\" with your answer to make the test pass.";

		private static readonly object _obj = new object();

		protected static object ___ => _obj;

	    protected static int ____ => 0;

	    protected void Assert___(params object[] objs)
		{
            
			throw new AssertionException(EMPTY_ASSERT_MSG);
		}
	}
}

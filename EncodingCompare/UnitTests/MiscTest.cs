using Shared;
using Xunit;

namespace UnitTests
{
    public class MiscTest
    {
        [Fact(Skip = "One-time run")]
        public void ConvertDicionary()
        {
            RandomWords.ConvertDictionary();
        }
    }
}

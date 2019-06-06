using CluedIn.Core.Data;

namespace Crawling.MySql.Integration.Test.MySqlClient
{
    public class ClueStorage
    {
        private int count = 0;

        public void AddClue(Clue obj)
        {
            count++;

            // _testOutputHelper.WriteLine($"Clue ID: {clue.OriginEntityCode.Value} Object: {clue.Serialize()}");
        }

        public bool HasClues()
        {
            return count > 0;
        }
    }
}
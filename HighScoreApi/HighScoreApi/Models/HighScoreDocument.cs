using System;

namespace HighScoreApi
{
    public class HighScoreDocument
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime TimeOfEntry { get; set; }
    }
}

using System.Collections.Generic;

namespace YoYo_Web_App.Domain.Models
{
    public class TestResult
    {
        public FitnessRating FitnessRating { get; private set; }

        public List<FitnessRating> PossibleResults { private set; get; }

        public TestResult()
        {
            PossibleResults = new List<FitnessRating>();
        }

        public TestResult(FitnessRating currentRating, List<FitnessRating> otherPossibleRatings)
        {
            FitnessRating = currentRating;
            PossibleResults = otherPossibleRatings;
        }
    }
}

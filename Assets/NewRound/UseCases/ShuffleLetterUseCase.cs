using TopicTwister.NewRound.Shared.Interfaces;


namespace TopicTwister.NewRound.UseCases
{
    public class ShuffleLetterUseCase : IShuffleLetterUseCase
    {
        public string GetRandomLetter()
        {
            /*int number = Random.Range(0, 26);
            char letter = (char)(((int)'A') + number);
            return $"{ letter }";*/
            return "á";
        }
    }
}

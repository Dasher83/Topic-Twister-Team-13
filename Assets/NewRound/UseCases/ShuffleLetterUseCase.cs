using System;
using TopicTwister.NewRound.Shared.Interfaces;


namespace TopicTwister.NewRound.UseCases
{
    public class ShuffleLetterUseCase : IShuffleLetterUseCase
    {
        public string GetRandomLetter()
        {
            Random random = new Random();
            int number = random.Next(0, 26 + 1);
            char letter = (char)(((int)'A') + number);
            return $"{ letter }";
        }
    }
}

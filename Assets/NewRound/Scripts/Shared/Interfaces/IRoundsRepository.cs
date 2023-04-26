using TopicTwister.NewRound.Models;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface IRoundsRepository
    {
        Round Save(Round round);
    }
}

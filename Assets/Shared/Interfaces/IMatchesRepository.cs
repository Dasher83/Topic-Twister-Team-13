using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IMatchesRepository: IMatchesReadOnlyRepository
    {
        Result<Match> Save(Match match);
        Result<bool> Delete(int id);
    }
}

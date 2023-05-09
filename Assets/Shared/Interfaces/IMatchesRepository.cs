using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IMatchesRepository: IMatchesReadOnlyRepository
    {
        Operation<Match> Insert(Match match);
        Operation<bool> Delete(int id);
    }
}

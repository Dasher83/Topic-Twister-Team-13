using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IMatchesRepository: IMatchesReadOnlyRepository
    {
        Operation<Match> Save(Match match);
        Operation<bool> Delete(int id);
    }
}

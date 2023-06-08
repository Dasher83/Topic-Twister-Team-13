using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUserRoundsRepository: IUserRoundsReadOnlyRepository
    {
        Operation<UserRound> Insert(UserRound userRound);
    }
}

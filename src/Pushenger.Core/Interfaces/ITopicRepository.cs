using Pushenger.Core.Entities;
using Pushenger.Core.Utilities.Result;

namespace Pushenger.Core.Interfaces
{
    public interface ITopicRepository
    {
        IResult Insert(Topic topic);
    }
}

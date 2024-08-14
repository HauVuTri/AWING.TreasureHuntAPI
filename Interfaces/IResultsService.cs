using AWING.TreasureHuntAPI.Models;

namespace AWING.TreasureHuntAPI.Interfaces
{
    public interface IResultsService
    {
        Task<Result> CalculateAndStoreResult(int mapId, int userId);
        Task<IEnumerable<TreasureMap>> GetAllResultByUser(int userId);
        Task<Result?> GetResultByMapId(int mapId, int userId);
    }
}

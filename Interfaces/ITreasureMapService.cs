using AWING.TreasureHuntAPI.Models;
using AWING.TreasureHuntAPI.Models.DTOs;

namespace AWING.TreasureHuntAPI.Interfaces
{
    public interface ITreasureMapService
    {
        Task<TreasureMap> CreateTreasureMap(CreateTreasureMapDto dto, int userId);
        Task<TreasureMap?> GetTreasureMapById(int mapId, int userId);
        Task<IEnumerable<TreasureMap>> GetAllTreasureMaps(int userId);
        Task<bool> DeleteTreasureMap(int mapId, int userId);
        Task<IEnumerable<TreasureCell> > GetCellsByMapId(int mapId, int userId);
    }
}

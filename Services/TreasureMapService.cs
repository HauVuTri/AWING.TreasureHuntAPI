using AWING.TreasureHuntAPI.Interfaces;
using AWING.TreasureHuntAPI.Models.DTOs;
using AWING.TreasureHuntAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AWING.TreasureHuntAPI.Services
{
    public class TreasureMapService : ITreasureMapService
    {
        private readonly TreasureHuntDbContext _context;

        public TreasureMapService(TreasureHuntDbContext context)
        {
            _context = context;
        }

        public async Task<TreasureMap> CreateTreasureMap(CreateTreasureMapDto dto, int userId)
        {
            var treasureMap = new TreasureMap
            {
                UserId = userId,
                RowsCount = dto.RowsCount,
                ColsCount = dto.ColsCount,
                P = dto.P,
                CreatedAt = DateTime.Now
            };

            // Populate the TreasureCells collection based on the matrix
            for (int i = 0; i < dto.RowsCount; i++)
            {
                for (int j = 0; j < dto.ColsCount; j++)
                {
                    treasureMap.TreasureCells.Add(new TreasureCell
                    {
                        RowNum = i,
                        ColNum = j,
                        ChestNumber = dto.Matrix[i][j],
                        TreasureMap = treasureMap
                    });
                }
            }

            _context.TreasureMaps.Add(treasureMap);
            await _context.SaveChangesAsync();

            return treasureMap;
        }


        public async Task<TreasureMap?> GetTreasureMapById(int mapId, int userId)
        {
            return await _context.TreasureMaps.AsNoTracking().Include(m => m.TreasureCells)
                .Where(m => m.MapId == mapId && m.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TreasureMap>> GetAllTreasureMaps(int userId)
        {
            return await _context.TreasureMaps
                .Where(m => m.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> DeleteTreasureMap(int mapId, int userId)
        {
            var map = await _context.TreasureMaps
                .Where(m => m.MapId == mapId && m.UserId == userId)
                .FirstOrDefaultAsync();

            if (map == null)
            {
                return false;
            }

            _context.TreasureMaps.Remove(map);
            await _context.SaveChangesAsync();
            return true;
        }

       

        public async Task<IEnumerable<TreasureCell>> GetCellsByMapId(int mapId, int userId)
        {
            return await _context.TreasureCells
               .Where(m => m.MapId == mapId)
               .Where(m => m.TreasureMap.UserId == userId)
               .ToListAsync();
        }
    }
}

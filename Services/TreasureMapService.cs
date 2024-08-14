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

        /// <summary>
        /// Tạo mới map
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<TreasureMap> CreateTreasureMap(CreateTreasureMapDto dto, int userId)
        {
            // Find potential matching maps based on Rows, Cols, and P
            var potentialMatches = await _context.TreasureMaps
                .Include(tm => tm.TreasureCells)
                .Where(tm =>
                    tm.UserId == userId &&
                    tm.RowsCount == dto.Rows &&
                    tm.ColsCount == dto.Cols &&
                    tm.P == dto.P)
                .ToListAsync();

            // Check each potential match for identical TreasureCells
            foreach (var existingMap in potentialMatches)
            {
                bool isIdentical = true;

                // Compare each cell in the matrix
                foreach (var cell in existingMap.TreasureCells)
                {
                    if (dto.Matrix[cell.RowNum][cell.ColNum] != cell.ChestNumber)
                    {
                        isIdentical = false;
                        break;
                    }
                }

                // If an identical map is found, return it
                if (isIdentical)
                {
                    return existingMap;
                }
            }

            // No identical map found, create a new one
            var treasureMap = new TreasureMap
            {
                UserId = userId,
                RowsCount = dto.Rows,
                ColsCount = dto.Cols,
                P = dto.P,
                CreatedAt = DateTime.Now
            };

            // Populate the TreasureCells collection based on the matrix
            for (int i = 0; i < dto.Rows; i++)
            {
                for (int j = 0; j < dto.Cols; j++)
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

            // Add and save the new TreasureMap
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

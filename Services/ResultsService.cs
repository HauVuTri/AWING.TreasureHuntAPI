using AWING.TreasureHuntAPI.Interfaces;
using AWING.TreasureHuntAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AWING.TreasureHuntAPI.Services
{
    public class ResultsService : IResultsService
    {
        private readonly TreasureHuntDbContext _context;

        public ResultsService(TreasureHuntDbContext context)
        {
            _context = context;
        }

        public async Task<Result> CalculateAndStoreResult(int mapId, int userId)
        {
            var map = await _context.TreasureMaps
                .Include(m => m.TreasureCells)
                .Where(m => m.MapId == mapId && m.UserId == userId)
                .FirstOrDefaultAsync();

            if (map == null)
            {
                throw new Exception("Treasure map not found.");
            }

            // Perform the calculation for minimum fuel
            double? minimumFuel = CalculateMinimumFuel(map);
            if (minimumFuel == null)
            {
                return new Result()
                {
                };
            }
            // Store the result
            var result = new Result
            {
                MapId = map.MapId,
                FuelUsed = minimumFuel,
                CalculatedAt = DateTime.Now
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<Result?> GetResultByMapId(int mapId, int userId)
        {
            return await _context.Results
                .Where(r => r.MapId == mapId && r.TreasureMap.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public double? CalculateMinimumFuel(TreasureMap map)
        {
            int n = map.RowsCount;
            int m = map.ColsCount;
            int p = map.P;

            // Organize the chest locations based on their numbers
            List<(int, int)>[] chestLocations = new List<(int, int)>[p + 1];
            for (int i = 0; i <= p; i++)
            {
                chestLocations[i] = new List<(int, int)>();
            }

            foreach (var cell in map.TreasureCells)
            {
                chestLocations[cell.ChestNumber].Add((cell.RowNum, cell.ColNum));
            }

            // Dijkstra's algorithm with priority queue
            var pq = new SortedSet<(double fuel, int x, int y, int currentChest)>();
            double[,,] minFuel = new double[n, m, p + 1];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    for (int k = 0; k <= p; k++)
                        minFuel[i, j, k] = double.MaxValue;

            pq.Add((0, 0, 0, 0)); // (fuel, x, y, currentChest)
            minFuel[0, 0, 0] = 0;

            while (pq.Count > 0)
            {
                var (currentFuel, x, y, currentChest) = pq.Min;
                pq.Remove(pq.Min);

                if (currentChest == p) continue;

                foreach (var (nx, ny) in chestLocations[currentChest + 1])
                {
                    double fuelCost = Math.Sqrt(Math.Pow(nx - x, 2) + Math.Pow(ny - y, 2));
                    double newFuel = currentFuel + fuelCost;

                    if (newFuel < minFuel[nx, ny, currentChest + 1])
                    {
                        minFuel[nx, ny, currentChest + 1] = newFuel;
                        pq.Add((newFuel, nx, ny, currentChest + 1));
                    }
                }
            }

            double minTotalFuel = double.MaxValue;
            foreach (var (x, y) in chestLocations[p])
            {
                if (minFuel[x, y, p] < minTotalFuel)
                {
                    minTotalFuel = minFuel[x, y, p];
                }
            }

            return minTotalFuel == double.MaxValue?null: minTotalFuel;
        }

        public async Task<IEnumerable<TreasureMap>> GetAllResultByUser(int userId)
        {
            return await _context.TreasureMaps.AsNoTracking()
                 .Where(m => m.UserId == userId)
                 .Include(m => m.Results)
                 .ToListAsync();
        }
    }
}

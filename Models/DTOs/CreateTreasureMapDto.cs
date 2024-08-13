namespace AWING.TreasureHuntAPI.Models.DTOs
{
    public class CreateTreasureMapDto
    {
        public int RowsCount { get; set; }
        public int ColsCount { get; set; }
        public int P { get; set; }
        public int[][] Matrix { get; set; } = null!;
    }
}

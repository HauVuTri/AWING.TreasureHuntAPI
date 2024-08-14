namespace AWING.TreasureHuntAPI.Models.DTOs
{
    public class CreateTreasureMapDto
    {
        public int Rows { get; set; }
        public int Cols { get; set; }
        public int P { get; set; }
        public int[][] Matrix { get; set; } = null!;
    }
}

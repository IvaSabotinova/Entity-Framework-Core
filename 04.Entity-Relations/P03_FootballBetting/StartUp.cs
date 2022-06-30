using P03_FootballBetting.Data;

namespace P03_FootballBetting
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            FootballBettingContext footballBettingContext = new FootballBettingContext();
            footballBettingContext.Database.EnsureDeleted();
            footballBettingContext.Database.EnsureCreated();
        }
    }
}

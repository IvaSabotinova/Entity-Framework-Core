using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting.Data.Models
{
    public class Bet
    {
      
        public int BetId { get; set; }

        public decimal Amount { get; set; }

        public double Prediction { get; set; } 

        public DateTime DateTime { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }

        public Game Game { get; set; }
    }
}

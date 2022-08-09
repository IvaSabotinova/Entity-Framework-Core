using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public PurchaseType Type { get; set; }

        [Required]
        public string ProductKey { get; set; }    
        public DateTime Date { get; set; }

        [ForeignKey(nameof(Card))]
        public int CardId { get; set; }
        public Card Card { get; set; }

        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }
        public Game Game { get; set; }
    }

}
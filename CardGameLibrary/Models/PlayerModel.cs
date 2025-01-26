using CardGameLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLibrary.Models
{
    public class PlayerModel
    {
        public string FirstName { get; set; }
        public List<PlayingCardModel> Hand {  get; set; }
        public int AceCounter { get; set; } = 0;
        public int TotalValue { get; set; } = 0;
        public PlayerStatus Status { get; set; }
    }
}

using CardGameLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLibrary.Models
{
    public class PlayingCardModel
    {
        public CardSuit Suit { get; set; }
        public CardValue Value { get; set; }
        private int _numberValue;

        public int NumberValue
        {
            get
            {
                if (Value == CardValue.Jack || Value == CardValue.Queen || Value == CardValue.King)
                {
                    return 10;
                }
                else if (Value == CardValue.Ace)
                {
                    return 11;
                }
                else
                {
                    return (int)Value;
                }
            }
            set { _numberValue = value; }
        }

    }
}

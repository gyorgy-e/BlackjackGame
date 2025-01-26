using CardGameLibrary.Enums;
using CardGameLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CardGameLibrary
{
    public abstract class Deck
    {
        protected List<PlayingCardModel> fullDeck = new List<PlayingCardModel>();
        protected List<PlayingCardModel> drawPile = new List<PlayingCardModel>();

        protected void CreateDeck()
        {
            fullDeck.Clear();
            
            for (int suit = 0; suit < 4; suit++)
            {
                for (int val = 1; val < 14; val++)
                {
                    fullDeck.Add(new PlayingCardModel { Suit = (CardSuit)suit, Value = (CardValue)val });
                }
            }
        }

        protected void ShuffleDeck()
        {
            var rnd = new Random();
            drawPile = fullDeck.OrderBy(x => rnd.Next()).ToList();
        }

        public abstract List<PlayingCardModel> DealCards(PlayerModel player);

        public virtual PlayingCardModel DrawCard(PlayerModel player)
        {
            PlayingCardModel output = drawPile.Take(1).First();
            drawPile.Remove(output);
            player.TotalValue += output.NumberValue;
            return output;
        }
    }

    public class BlackjackDeck : Deck
    {
        public BlackjackDeck()
        {
            CreateDeck();
            ShuffleDeck();
        }

        public override List<PlayingCardModel> DealCards(PlayerModel player)
        {
            List<PlayingCardModel> output = new List<PlayingCardModel>();

            for (int i = 0; i < 2; i++)
            {
                output.Add(DrawCard(player));
            }

            return output;
        }

        public PlayingCardModel RequestCard(PlayerModel player)
        {
             return DrawCard(player);
        }
    }
}

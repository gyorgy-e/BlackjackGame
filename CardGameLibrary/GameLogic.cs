using CardGameLibrary.Models;
using CardGameLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardGameLibrary
{
    public class GameLogic
    {
        public static void HitOrStand(string action, BlackjackDeck deck, PlayerModel player)
        {
            if (action.ToLower() == "hit")
            {
                var newCard = deck.RequestCard(player);
                player.Hand.Add(newCard);
                if (newCard.Value == CardValue.Ace)
                {
                    player.AceCounter += 1;
                }
            }
            else if (action.ToLower() == "stand")
            {
                player.Status = PlayerStatus.Stand;
            }
        }

        private static void HasAceInHand(PlayerModel player)
        {
            foreach (var card in player.Hand)
            {
                if (card.Value == CardValue.Ace)
                {
                    player.AceCounter += 1;
                }
            }
        }

        public static void GetAceValue(PlayerModel player)
        {
            if (player.TotalValue > 21)
            {
                if (player.AceCounter > 0)
                {
                    player.TotalValue -= 10;
                    player.AceCounter -= 1;
                }
            }
        }

        public static bool IsOver21(PlayerModel player)
        {
            bool output = false;

            if (player.TotalValue > 21)
            {
                player.Status = PlayerStatus.Bust;
                output = true;
            }

            return output;
        }

        public static bool IsWinner(PlayerModel player, PlayerModel dealer)
        {
            bool output = false;

            if (player.Status == PlayerStatus.Stand)
            {
                if (player.TotalValue > dealer.TotalValue)
                {
                    output = true;
                }
            }

            return output;
        }

        public static bool AreNoPlayersLeft(List<PlayerModel> players, PlayerModel dealer, int numberOfPlayers)
        {
            bool output = false;
            int isBustCounter = 0;

            foreach (var player in players)
            {
                if (player.Status == PlayerStatus.Bust)
                {
                    isBustCounter += 1;
                }
            }

            if (isBustCounter == numberOfPlayers && dealer.Status != PlayerStatus.Bust)
            {
                output = true;
            }

            return output;
        }

        public static void DealerAction(PlayerModel dealer, BlackjackDeck deck)
        {
            if (dealer.AceCounter > 0)
            {
                GetAceValue(dealer);
            }

            if (dealer.TotalValue < 17)
            {
                var newCard = deck.RequestCard(dealer);
                dealer.Hand.Add(newCard);
            }
            else if (dealer.TotalValue >= 17)
            {
                dealer.Status = PlayerStatus.Stand;
            }
        }

        public static bool AreThereNoActivePlayers(List<PlayerModel> players, int numberOfPlayers)
        {
            int notActive = 0;
            bool output = false;

            foreach (var player in players)
            {
                if (player.Status != PlayerStatus.Active)
                {
                    notActive += 1;
                }
            }

            if (notActive == numberOfPlayers)
            {
                output = true;
            }
            
            return output;
        }

        public static bool DoesGameContinue(List<PlayerModel> players, PlayerModel dealer, int numberOfPlayers)
        {
            bool output = false;

            foreach (PlayerModel player in players)
            {
                if (player.Status != PlayerStatus.Stand || player.Status != PlayerStatus.Bust)
                {
                    output = true;
                }
            }

            if (dealer.Status == PlayerStatus.Stand && AreThereNoActivePlayers(players, numberOfPlayers))
            {
                output =  false;
            }

            if (dealer.Status == PlayerStatus.Bust)
            {
                output = false;
            }

            return output;
        }
    }
}

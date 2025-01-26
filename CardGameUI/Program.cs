using CardGameLibrary;
using CardGameLibrary.Models;
using CardGameLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CardGameUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IntroMessage();

            BlackjackDeck deck = new BlackjackDeck();

            int numberOfPlayers = GetNumberOfPlayers();

            List<PlayerModel> players = GetAllPlayers(deck, numberOfPlayers);

            PlayerModel dealer = CreateDealer(deck);

            do
            {
                PlayerTurn(players, numberOfPlayers, deck);
                DealerTurn(dealer, deck);
            } while (GameLogic.DoesGameContinue(players, dealer, numberOfPlayers));

            PrintWinner(players, dealer, numberOfPlayers);

            Console.ReadLine();
        }

        public static void IntroMessage()
        {
            Console.WriteLine("This is a blackjack game");
            Console.WriteLine("Created by: Eniko Gyorgy");
            Console.WriteLine("************************");
            Console.WriteLine();
            Console.Write("Press enter to start.");
            Console.ReadLine();
            Console.Clear();
        }

        public static void PrintWinner(List<PlayerModel> players, PlayerModel dealer, int numberOfPlayers)
        {
            if (dealer.Status == PlayerStatus.Bust)
            {
                foreach (var player in players)
                {
                    if (player.Status != PlayerStatus.Bust)
                    {
                        Console.WriteLine($"Congratulations for winning, {player.FirstName}.");
                    }
                }
            }
            else if (dealer.Status != PlayerStatus.Bust)
            {
                foreach (var player in players)
                {
                    bool isWinner = GameLogic.IsWinner(player, dealer);

                    if (isWinner)
                    {
                        Console.WriteLine($"Congratulations for winning, {player.FirstName}.");
                    }
                    else
                    {
                        Console.WriteLine($"You did not win, {player.FirstName}.");
                    }
                } 
            }

            if (GameLogic.AreNoPlayersLeft(players, dealer, numberOfPlayers))
            {
                Console.WriteLine($"The winner is the {dealer.FirstName}");
            }
        }

        public static void DealerTurn(PlayerModel dealer, BlackjackDeck deck)
        {
            GameLogic.DealerAction(dealer, deck);
            DisplayCardsInHand(dealer);
            PrintHandValue(dealer);
            bool isBust = GameLogic.IsOver21(dealer);
            if (isBust)
            {
                Console.WriteLine($"The {dealer.FirstName} has gone over 21.");
            }
            Console.WriteLine();
        }

        public static PlayerModel CreateDealer(BlackjackDeck deck)
        {
            PlayerModel dealer = new PlayerModel();
            dealer.FirstName = "Dealer";
            dealer.Hand = deck.DealCards(dealer);
            dealer.Status = PlayerStatus.Active;

            foreach (var card in dealer.Hand)
            {
                if (card.Value == CardValue.Ace)
                {
                    dealer.AceCounter += 1;
                }
            }

            Console.WriteLine("The first card of the dealer is:");
            Console.WriteLine($"{dealer.Hand[0].Value} of {dealer.Hand[0].Suit}");
            Console.WriteLine();

            return dealer;
        }

        public static void PlayerTurn(List<PlayerModel> players, int numberOfPlayers, BlackjackDeck deck)
        {
            foreach (var player in players)
            {
                if (player.Status == PlayerStatus.Active)
                {
                    DisplayCardsInHand(player);
                    PrintHandValue(player);
                    string action = AskForAction();
                    GameLogic.HitOrStand(action, deck, player);
                    DisplayNewCard(player, action);
                    PrintHandValue(player);
                    bool isBust = GameLogic.IsOver21(player);
                    if (isBust)
                    {
                        Console.WriteLine($"You have more than 21, {player.FirstName}.");
                    }
                    Console.WriteLine();
                }
            }
        }

        public static void PrintHandValue(PlayerModel player)
        {
            GameLogic.GetAceValue(player);
            Console.WriteLine($"The total value of the cards is: {player.TotalValue}");
        }

        public static string AskForAction()
        {
            string output = "";

            do
            {
                Console.Write("What would you like to do (hit/stand): ");
                output = Console.ReadLine(); 
            } while (output.ToLower() != "hit" && output.ToLower() != "stand");

            return output;
        }

        public static void DisplayNewCard(PlayerModel player, string action)
        {
            if (action.ToLower() == "hit")
            {
                Console.WriteLine("The new card is:");
                Console.WriteLine($"{player.Hand[player.Hand.Count - 1].Value} of {player.Hand[player.Hand.Count - 1].Suit}"); 
            }
        }

        public static void DisplayCardsInHand(PlayerModel player)
        {
            Console.WriteLine($"The cards in {player.FirstName}'s hands are:");
            foreach (var card in player.Hand)
            {
                Console.WriteLine($"{card.Value} of {card.Suit}");
            }
        }

        public static List<PlayerModel> GetAllPlayers(BlackjackDeck deck, int numberOfPlayers)
        {
            
            List<PlayerModel> output = new List<PlayerModel>();

            for (int i = 0; i < numberOfPlayers; i++)
            {
                PlayerModel player = CreatePlayer(deck, $"Player {i + 1}");
                output.Add(player);
                DisplayCardsInHand(player);
                Console.WriteLine();
            }

            return output;
        }

        public static PlayerModel CreatePlayer(BlackjackDeck deck, string playerTitle)
        {
            PlayerModel player = new PlayerModel();

            Console.WriteLine($"Information for { playerTitle }:");
            player.FirstName = AskUsersName();
            player.Hand = deck.DealCards(player);
            player.Status = PlayerStatus.Active;

            foreach (var card in player.Hand)
            {
                if (card.Value == CardValue.Ace)
                {
                    player.AceCounter += 1;
                } 
            }
            Console.WriteLine();

            return player;
        }

        public static string AskUsersName()
        {
            Console.Write("What is your first name: ");
            string output = Console.ReadLine();
            return output;
        }

        private static int GetNumberOfPlayers()
        {
            bool isValid = false;
            int output = 0;

            do
            {
                Console.Write("How many players are there (1-4): ");
                string playerNumber = Console.ReadLine();
                isValid = int.TryParse(playerNumber, out output);
                if (isValid == false || (output < 1 || output > 4))
                {
                    Console.WriteLine("That was not a valid number.");
                }
            }
            while (isValid == false || (output < 1 || output > 4));
            Console.WriteLine();

            return output;

        }
    }
}

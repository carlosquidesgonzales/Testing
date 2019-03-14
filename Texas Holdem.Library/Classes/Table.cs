using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Texas_Holdem.Library.Enums;
using Texas_Holdem.Library.Structs;

namespace Texas_Holdem.Library.Classes
{
    public class Table
    {
        Deck _deck; 
        public List<Player> winner;      
        private List<Player> highestPlayerCardsList;       
        private List<Player> highestHandValue;


        public List<Player> Players { get; } = new List<Player>();
        public Player Dealer = new Player("Dealer");



        public Table(string[] playerNames)
        {
            var arrayLength = playerNames.Length;
            if (!(arrayLength >= 2 && arrayLength <= 4))
            {
                throw new ArgumentException("Incorrect Number of players");
                //MessageBox.Show("Incorrect Number of players", "Invalid Player");
            }
            else
            {

                foreach (var names in playerNames)
                {
                    Players.Add(new Player(names));
                }

            }
        }

        private void DealPlayerCards()
        {
           
            foreach (var player in Players)
            {
                player.ClearHand();
                for (int i = 0; i < 2; i++)
                {
                    player.ReceiveCard(_deck.DrawCard(), true);
                }              
            }
        }

        public void DealNewHand()
        {
            _deck = new Deck();
            _deck.ShuffleDeck(1000);
            Dealer.ClearHand();
            DealPlayerCards();
        }

        public void DealerDrawsCard(int count = 1)
        {                      
            if(Dealer.CardCount.Equals(5)) return;

            for (int i = 0; i < count; i++)
            {
                var card = _deck.DrawCard();
                Dealer.ReceiveCard(card);

                    foreach (var player in Players)
                    {
                        player.ReceiveCard(card);
                    }
            }
        }

        public void EvaluatePlayerHands()
        {
            foreach (var evalPlayers in Players)
            {
                evalPlayers.EvaluateHand();
            }

          if(Dealer.Cards.Count.Equals(5))
                Dealer.EvaluateHand();          
        }

        public List<Player> DetermineWinner()
        {
            Players.Sort();
           
            winner = new List<Player>();
            highestHandValue = new List<Player>();

            for (int i = 0; i < Players.Count; i++)
            {
                if(Players[0].HandValue == Players[i].HandValue)
                    highestHandValue.Add(Players[i]);
            }


            #region Find the highest cards between multiple players with the same HandValue 
            if (highestHandValue.Count >= 2)
            {                
                CompareHands(highestHandValue);

                if (highestPlayerCardsList.Count >= 2)
                {
                    if (!highestPlayerCardsList[0].HandValue.Equals(Dealer.HandValue))
                    {
                        if (highestPlayerCardsList[0].HandValue > Dealer.HandValue)
                        {
                           winner.AddRange(highestPlayerCardsList);                        
                        }
                        else
                        {
                            winner.AddRange(highestPlayerCardsList);
                            highestPlayerCardsList.Add(Dealer);
                        }
                       

                    if (highestPlayerCardsList[0].HandValue == Dealer.HandValue)
                        CompareCardsWithMultiplePlayers(highestPlayerCardsList);                      
                    }
                   
                }
                else if (highestPlayerCardsList.Count == 0)
                {
                    winner.Add(highestHandValue[0]);
                    winner.Add(Dealer);
                }
                else
                {
                    if (highestPlayerCardsList[0].HandValue == Dealer.HandValue)
                    {
                        CompareCardsWithDealer(highestPlayerCardsList);
                    }                       
                    else if (highestPlayerCardsList[0].HandValue > Dealer.HandValue)
                    {
                        winner.AddRange(highestPlayerCardsList);
                    }
                    else
                    {
                        winner.Add(Dealer);
                    }
                   
                }                             
            }
            else
            {
                winner.Add(highestHandValue.ElementAt(0).HandValue > Dealer.HandValue
                    ? highestHandValue.ElementAt(0)
                    : Dealer);

                if(highestHandValue.ElementAt(0).HandValue == Dealer.HandValue)
                    CompareCardsWithDealer(highestHandValue);
            }
            #endregion
            return winner;        
        }

        private void CompareHands(List<Player> players)
        {
            highestPlayerCardsList = new List<Player>();
            players = players.OrderByDescending(p => p.HighCards.Sum(c => (int)c.Value)).ToList();
           
            switch (players[0].HandValue)
            {
                case Hands.RoyalFlush:
                case Hands.StraightFlush:
                case Hands.FullHouse:
                case Hands.Flush:
                case Hands.Straight:
                case Hands.Nothing:
                    CompareCards(players);
                    break;
                case Hands.TwoPair:
                    players.Sort();
                    for (int i = 0; i < players.Count; i++)
                    {
                        if ((players[0].HighCards[1].Value == players[i].HighCards[1].Value) ||
                            (players[0].HighCards[0].Value == players[i].HighCards[0].Value))
                        {
                            highestPlayerCardsList.Add(players[i]);
                        }

                    }              
                    break;
                case Hands.FourOfAKind:
                case Hands.ThreeOfAKind:
                case Hands.Pair:
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[0].HighCards[0].Value == players[i].HighCards[0].Value)
                        {
                            highestPlayerCardsList.Add(players[i]);
                        }

                    }                
                    break;
            }

        }

        private void CompareCardsWithDealer(List<Player> player)
        {          
            for (var i = 4; i >= 0; i--)
                if (!player[0].BestCards[i].Value.Equals(Dealer.BestCards[i].Value))
                {
                    if ((player[0].BestCards[i].Value > Dealer.BestCards[i].Value))
                    {
                        winner.Clear();
                        winner.Add(player.ElementAt(0));
                        break;
                    }
                    else
                    {
                        winner.Clear();
                        winner.Add(Dealer);
                        break;
                    }
                }
                else
                {
                    winner.AddRange(player);
                    winner.Add(Dealer);
                }


            
        }

        private void CompareCardsWithMultiplePlayers(List<Player> players)
        {

            foreach (var p in players)
            {
                for (var i = 4; i >= 0; i--)
                    if (!p.BestCards[i].Value.Equals(Dealer.BestCards[i].Value))
                    {
                        if ((p.BestCards[i].Value > Dealer.BestCards[i].Value))
                        {
                            winner.Clear();
                            winner.AddRange(players);
                            break;
                        }
                        else
                        {
                            winner.Add(Dealer);
                            break;
                        }
                    }
                    else
                    {
                        winner.AddRange(players);
                        winner.Add(Dealer);
                    }
            }          
        }

        private void CompareCards(List<Player> players)
        {
            highestPlayerCardsList = new List<Player>();
            players = players.OrderByDescending(p => p.BestCards.Sum(c => (int)c.Value)).ToList();

            for (int i = 0; i < players.Count; i++)
            {
                if ((players[0].BestCards[0].Value == players[i].BestCards[0].Value) &&
                    (players[0].BestCards[1].Value == players[i].BestCards[1].Value) &&
                    (players[0].BestCards[2].Value == players[i].BestCards[2].Value) &&
                    (players[0].BestCards[3].Value == players[i].BestCards[3].Value) &&
                    (players[0].BestCards[4].Value == players[i].BestCards[4].Value))
                {
                    highestPlayerCardsList.Add(players[i]);
                }
            }         
        }     
    }
}

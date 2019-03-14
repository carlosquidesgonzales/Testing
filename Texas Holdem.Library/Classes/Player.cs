using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texas_Holdem.Library.Enums;
using Texas_Holdem.Library.Structs;

namespace Texas_Holdem.Library.Classes
{
    public class Player:IComparable<Player>
    {
        private Hand _hand;
       

        public string Name { get; }
        public List<Card> Cards => _hand.Cards;
        public List<Card> BestCards => _hand.BestCards;
        public List<Card> PlayerCards => _hand.PlayerCards;
        public List<Card> HighCards => _hand.HighCards;
        public Hands HandValue {
            get { return _hand.HandValue; }
        }
       

        public Suits Suit
        {
            get { return _hand.Suit; }
        }

        public int CardCount => Cards.Count;

        public Player(string name)
        {
            Name = name;
            _hand = new Hand(new HandEvaluator());
        }

       

        public void ReceiveCard(Card card, bool isPlayerCard = false)
        {
            _hand.AddCard(card, isPlayerCard);
        }

        public void ClearHand()
        {
            _hand.Clear();
        }

        public void EvaluateHand()
        {
            _hand.EvaluateHand();
        }


        public int CompareTo(Player x)
        {
            if (this.HandValue > x.HandValue) return -1;
            else if (this.HandValue < x.HandValue) return 1;
            else return 0;
        }
   
    }
}

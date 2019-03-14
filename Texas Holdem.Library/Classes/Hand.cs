using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texas_Holdem.Library.Enums;
using Texas_Holdem.Library.Interfaces;
using Texas_Holdem.Library.Structs;

namespace Texas_Holdem.Library.Classes
{
    public class Hand
    {
        private IHandEvaluator _eval;
        public List<Card> Cards { get; } = new List<Card>();
        public List<Card> BestCards { get; } = new List<Card>();
        public List<Card> PlayerCards { get; } = new List<Card>();
        public Hands HandValue { get; private set; }
        public Suits Suit { get; private set; }
        public List<Card> HighCards { get; } = new List<Card>();
        public Hand(IHandEvaluator eval)
        {
            _eval = eval;
        }

        public void Clear()
        {
            HandValue = Hands.Nothing;
            Suit = Suits.Unknown;

            Cards.Clear();
            BestCards.Clear();
            PlayerCards.Clear();
        }

        public void AddCard(Card card, bool isPlayerCard)
        {
            if (isPlayerCard && PlayerCards.Count < 2)
            {
                Cards.Add(card);
            }              
            else
            {
                Cards.Add(card);
            }
        }

        public void EvaluateHand()
        {
            if (Cards.Count < 2) return;
            BestCards.Clear();
            HighCards.Clear();
            var result = _eval.EvaluateHand(Cards);
           
            BestCards.AddRange(result.Cards);
            HighCards.AddRange(result.HighCards);

            HandValue = result.HandValue;
            Suit = result.Suit;

        }

    }

}

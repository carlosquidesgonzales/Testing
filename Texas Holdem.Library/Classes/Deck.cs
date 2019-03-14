using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texas_Holdem.Library.Enums;
using Texas_Holdem.Library.Structs;

namespace Texas_Holdem.Library.Classes
{
    public class Deck
    {
        List<Card> _cards = new List<Card>();

        private void NewDeck()
        {
            _cards.Clear();

            foreach (Suits suit in Enum.GetValues(typeof(Suits)))
            {
                if(suit.Equals(Suits.Unknown)) continue;
                foreach (Values value in Enum.GetValues(typeof(Values)))
                {

                    var card = new Card(value, suit);
                    _cards.Add(card);

                }              
            }
        }

        public void ShuffleDeck(int shuffles)
        {
            NewDeck();
            
            var rnd = new Random();
            for (int i = 0; i < shuffles; i++)
            {
                List<Card> tmpDeck = new List<Card>();
                while (_cards.Count > 0)
                {
                    var index = rnd.Next(_cards.Count);

                    var card = _cards[index];
                    _cards.RemoveAt(index);
                    tmpDeck.Add(card);
                }
                _cards = tmpDeck;
            }
        }

        public Card DrawCard()
        {
            var card = _cards.ElementAt(0);
            _cards.Remove(card);
            return card;
        }
    }
}

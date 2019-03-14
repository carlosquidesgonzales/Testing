using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texas_Holdem.Library.Classes;
using Texas_Holdem.Library.Enums;
using Texas_Holdem.Library.Structs;

namespace Texas_Holdem.Library.Interfaces
{
    public interface IHandEvaluator
    {
       (List<Card> HighCards, List<Card> Cards, Hands HandValue, Suits Suit) EvaluateHand(List<Card> cards);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Texas_Holdem.Library.Classes;
using Texas_Holdem.Library.Enums;

namespace Texas_Holdem.Library.Structs
{
    public struct Card:IComparable
    {
        public Values Value { get; }
        public Suits Suit { get; }
        

        public string Output {
            get
            {
                var symbol = (char)Suit;
                var value = (int)Value <= 10 ? ((int)Value).ToString() :
                    Value.ToString().Substring(0, 1);
                return $"{value}\n{symbol}";
            }
        }

        public Card(Values values, Suits suits)
        {
            Value = values;
            Suit = suits;          
        }

        public int CompareTo(object obj)
        {
            var obj1 = (int)Value;
            var obj2 = (int)((Card)obj).Value;
            return obj1.CompareTo(obj2);
        }
    }
}

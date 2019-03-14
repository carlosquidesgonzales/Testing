using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Texas_Holdem.Library.Enums;
using Texas_Holdem.Library.Interfaces;
using Texas_Holdem.Library.Structs;

namespace Texas_Holdem.Library.Classes
{
    class HandEvaluator:IHandEvaluator
    {
        private List<Card> BestCards = new List<Card>();
        private List<Card> HighCards = new List<Card>();
        private Hands HandValue;
        private Suits Suit;
       

        public (List<Card> HighCards, List<Card> Cards, Hands HandValue, Suits Suit) EvaluateHand(List<Card> cards)
        {
            BestCards.Clear();
            HandValue = Hands.Nothing;
            Suit = Suits.Unknown;

            if (cards.Count > 2 )
            {
                //var sortedCards = cards.OrderBy(c => c.Value).ToList();
                cards.Sort();
               
                EvaluateCards(cards);
                BestCards.Sort();
            }
           
            return (HighCards, BestCards, HandValue, Suit);
        }

        public void EvaluateCards(List<Card> cards)
        {
            if (cards.Count == 5)
            {                
                var suit1 = cards[0].Suit;
                var suit2 = cards[1].Suit;
                var suit3 = cards[2].Suit;
                var suit4 = cards[3].Suit;
                var suit5 = cards[4].Suit;
                var value1 = cards[0].Value;
                var value2 = cards[1].Value;
                var value3 = cards[2].Value;
                var value4 = cards[3].Value;
                var value5 = cards[4].Value;

                #region Has Flush
                var hasFlush = cards.Count(c => c.Suit.Equals(suit1)).Equals(5);
                if (hasFlush)
                {                    
                    HandValue = Hands.Flush;
                }
                #endregion

                #region Has Straight
                var hasStraight =
                    (value2.Equals(value1 + 1) && value3.Equals(value2 + 1) &&
                     value4.Equals(value3 + 1) && value5.Equals(value4 + 1)) ||
                    (value1.Equals(Values.Two) && value2.Equals(Values.Three) &&
                     value3.Equals(Values.Four) && value4.Equals(Values.Five) &&
                     value5.Equals(Values.Ace));

                var isHightStraight = hasStraight && value4.Equals(Values.King) &&
                                      value5.Equals(Values.Ace);

                var isLowStraight = hasStraight && value2.Equals(Values.Two) &&
                                    value5.Equals(Values.Ace);

                var royalFlush = (isHightStraight && hasFlush && suit1.Equals(Suits.Hearts));

                var straightFlush = (isHightStraight && hasFlush && suit1.Equals(Suits.Spades));

                var fourOfAKind = (cards.Count(c => c.Value.Equals(value3)).Equals(4));

                var threeOfAKind = cards.Count(c => c.Value.Equals(value3)).Equals(3);

                var hasFullHouse = (threeOfAKind && (cards.Count(c => c.Value.Equals(value1)).Equals(2) ||
                                                     cards.Count(c => c.Value.Equals(value5)).Equals(2)));
         
                #endregion


                IsRoyalFlush(cards, royalFlush);
                if(royalFlush) return;

                IsStraightFlush(cards, straightFlush);
                if(straightFlush) return;

                IsFlush(cards, hasFlush);
                if(hasFlush) return;

                IsFourOfAKind(cards, fourOfAKind);
                if(fourOfAKind) return;

                IsFullHouse(cards, hasFullHouse); 
                if(hasFullHouse) return;
                
                IsStraight(cards, hasStraight);
                if(hasStraight) return;

                IsThreeOfAKind(cards, threeOfAKind);
                if(threeOfAKind) return;

                IsPairOrTwoPairs(cards);
                BestCards.AddRange(cards.OrderBy(c => c.Value).ToList());             
            }




            if (cards.Count == 7)
            {
                var suit1 = cards[0].Suit;
                var suit2 = cards[1].Suit;
                var suit3 = cards[2].Suit;
                var suit4 = cards[3].Suit;
                var suit5 = cards[4].Suit;
                var suit6 = cards[5].Suit;
                var suit7 = cards[6].Suit;

                var value1 = cards[0].Value;
                var value2 = cards[1].Value;
                var value3 = cards[2].Value;
                var value4 = cards[3].Value;
                var value5 = cards[4].Value;
                var value6 = cards[5].Value;
                var value7 = cards[6].Value;

                BestCards.Clear();

                #region Variables

                #region Has Flush
                var hasFlush = ((cards.Count(c => c.Suit.Equals(suit2)).Equals(5))||(cards.Count(c => c.Suit.Equals(suit6)).Equals(5)));
                if (hasFlush)
                {
                    HandValue = Hands.Flush;
                }
                #endregion

                #region Has Straight
                var hasStraight = ((value1.Equals(Values.Three) && value2.Equals(Values.Four) &&
                                 value3.Equals(Values.Five) && value4.Equals(Values.Six) &&
                                 value5.Equals(Values.Seven)) ||
                                 (value2.Equals(Values.Three) && value3.Equals(Values.Four) &&
                                  value4.Equals(Values.Five) && value5.Equals(Values.Six) &&
                                  value6.Equals(Values.Seven)) ||
                                 (value3.Equals(Values.Three) && value4.Equals(Values.Four) &&
                                  value5.Equals(Values.Five) && value6.Equals(Values.Six) &&
                                  value7.Equals(Values.Seven)));
                

                var isHightStraight = hasStraight && value6.Equals(Values.King) &&
                                      value7.Equals(Values.Ace);

                var isLowStraight = hasStraight && value2.Equals(Values.Three) &&
                                    value7.Equals(Values.Seven);

                var royalFlush = (isHightStraight && hasFlush && cards[4].Value.Equals(Suits.Hearts));

                var straightFlush = (hasStraight && hasFlush && cards[4].Value.Equals(Suits.Spades));

                #region Three of a Kind
                var threeOfAKind = ((cards.Count(c => c.Value.Equals(value3)).Equals(3)) || 
                                   (cards.Count(c => c.Value.Equals(value6)).Equals(3)) || 
                                   (cards.Count(c => c.Value.Equals(value7)).Equals(3)));

                var fourOfAKind = (cards.Count(c => c.Value.Equals(value1)).Equals(4)) ||
                                  (cards.Count(c => c.Value.Equals(value4)).Equals(4));


                var sameSuits = cards.GroupBy(x => x.Suit).Where(g => g.Count() >= 5).SelectMany(g => g).ToList();

                var flush1 = (sameSuits.Count >= 5 || hasFlush);

                var straight = (hasStraight && flush1);
                #endregion

                if (threeOfAKind)
                {
                    HandValue = Hands.ThreeOfAKind;
                }
                #endregion Three of a Kind?
                
                var fullHouse = (threeOfAKind && (cards.Count(c => c.Value.Equals(value1)).Equals(2) ||
                                                  cards.Count(c => c.Value.Equals(value3)).Equals(2) ||
                                                  cards.Count(c => c.Value.Equals(value5)).Equals(2) ||
                                                  cards.Count(c => c.Value.Equals(value7)).Equals(2)));

                IsRoyalFlush(cards, royalFlush);
                if(royalFlush)return;

                IsStraightFlush(cards, straightFlush);
                if(straightFlush) return;

                IsFlush(cards, flush1);
                if(flush1)return;

                IsFourOfAKind(cards, fourOfAKind);
                if(fourOfAKind) return;

                IsFullHouse(cards, fullHouse);
                if(fullHouse) return;

                IsStraight(cards, straight);
                if(flush1) return;

                IsThreeOfAKind(cards, threeOfAKind);
                if(threeOfAKind) return;

                IsPairOrTwoPairs(cards);
                

                #endregion


                #region MyRegion     
                //// Has Flush
                //var hasFlush = cards.Count(c => c.Suit.Equals(cards.ElementAt(1).Suit)).Equals(5) ||
                //               cards.Count(c => c.Suit.Equals(cards.ElementAt(5).Suit)).Equals(5);

                //var flush = BestCards
                //    .GroupBy(x => x.Suit)
                //    .Where(g => g.Count() == 5)
                //    .SelectMany(g => g)
                //    .ToList();

                //// Has Straight
                //var hasStraight1 = (cards[1].Value.Equals(Values.Three) &&
                //                    cards[2].Value.Equals(Values.Four) &&
                //                    cards[3].Value.Equals(Values.Five) &&
                //                    cards[4].Value.Equals(Values.Six) &&
                //                    cards[5].Value.Equals(Values.Seven));

                //var hasStraight2 = (cards[0].Value.Equals(Values.Three) &&
                //                    cards[1].Value.Equals(Values.Four) &&
                //                    cards[2].Value.Equals(Values.Five) &&
                //                    cards[3].Value.Equals(Values.Six) &&
                //                    cards[4].Value.Equals(Values.Seven));

                //var hasStraight3 = (cards[2].Value.Equals(Values.Three) &&
                //                    cards[3].Value.Equals(Values.Four) &&
                //                    cards[4].Value.Equals(Values.Five) &&
                //                    cards[5].Value.Equals(Values.Six) &&
                //                    cards[6].Value.Equals(Values.Seven));
                //var straight = (hasStraight1 || hasStraight2 || hasStraight3);

                //// High Straight
                //var isHighStraight = cards[2].Value.Equals(Values.Ten) &&
                //                     cards[3].Value.Equals(Values.Jack) &&
                //                     cards[4].Value.Equals(Values.Queen) &&
                //                     cards[5].Value.Equals(Values.King) &&
                //                     cards[6].Value.Equals(Values.Ace);

                //// Three of a Kind
                //var hasThreeOfAKind1 = cards.Count(c => c.Value.Equals(cards[1].Value)).Equals(3);
                //var hasThreeOfAKind2 = cards.Count(c => c.Value.Equals(cards[4].Value)).Equals(3);
                //var hasThreeOfAKind3 = cards.Count(c => c.Value.Equals(cards[6].Value)).Equals(3);
                //var hasThreeOfAKind = (hasThreeOfAKind1 || hasThreeOfAKind2);



                //var threeValues = cards
                //    .GroupBy(x => x.Value)
                //    .Where(g => g.Count() == 3)
                //    .SelectMany(g => g)
                //    .ToList();

                //var threeValues2 = cards
                //    .GroupBy(x => x.Value)
                //    .Where(g => g.Count() == 2)
                //    .SelectMany(g => g)
                //    .ToList();               
                //#endregion

                //IsRoyalFlush(cards, isHighStraight, hasFlush);
                //IsStraightFlush(cards, straight, hasFlush);
                //IsFourOfAKind(cards);
                //IsFullHouse(cards, hasThreeOfAKind);
                //IsFlush(cards, hasFlush, flush);
                //IsStraight(cards, straight);
                //IsThreeOfAKind(cards, hasThreeOfAKind);
                //IsPairOrTwoPairs(cards);
                #endregion
            }


        }
       
        private void IsRoyalFlush(List<Card> cards, bool royalFlush)
        {       
            if (royalFlush)
            {
                if (cards.Count == 7)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        cards.RemoveAt(i);
                        if (i == 1)
                            break;
                    }

                    BestCards.AddRange(cards);
                }
               

                Suit = cards[0].Suit;
                HandValue = Hands.RoyalFlush;
            }
            return;
           
        }
      
        private void IsStraightFlush(List<Card> cards, bool hasStraightFlush)
        {
            if (hasStraightFlush)
            {
                //RemoveCardsFromStraight(cards);  
                if (cards.Count == 7)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        cards.RemoveAt(i);
                        if (i == 1)
                        {
                            break;
                        }
                    }
                }
                    
                Suit = cards[0].Suit;
                HandValue = Hands.StraightFlush;
               
            }
            return;
        }

        private void IsFourOfAKind(List<Card> cards, bool fourOfAKind)
        {
           
            if (fourOfAKind)
            {              
                if (cards.Count == 7)
                    AddBestCards(cards);


                HighCards.AddRange(cards.GroupBy(x => x.Value).Where(g => g.Count() == 3).Select(x => x.First()));
                Suit = cards[1].Suit;
                HandValue = Hands.FourOfAKind;
                
            }
            return;

        }

        private void IsFullHouse(List<Card> cards, bool fullHouse)
        {
        
          
            if (fullHouse)                                                                      
            {
                if (cards.Count == 7)
                    AddBestCards(cards);

                Suit = cards[2].Suit;
                HandValue = Hands.FullHouse;
                return;
            }
            return;
        }

        private void IsFlush(List<Card> cards, bool flush)
        {
            if (flush)
            {
                if (cards.Count == 7)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        cards.RemoveAt(i);
                        if (i == 1)
                            break;
                    }
                    BestCards.AddRange(cards);
                    HandValue = Hands.Flush;
                }               
                
            }
            return;
        }

        private void IsStraight(List<Card> cards, bool hasStraight)
        {
            if (hasStraight)
            {

                //RemoveCardsFromStraight(cards);   

                if (cards.Count == 7)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        cards.RemoveAt(i);
                        if (i == 1)
                        {
                            break;
                        }
                    }
                    BestCards.AddRange(cards);
                }
                    
                Suit = cards[2].Suit;
                HandValue = Hands.Straight;
                //return;
            }
            return;
        }

        private void IsThreeOfAKind(List<Card> cards,  bool threeOfAKind)
        {
            

            if (threeOfAKind)
            {
                if (cards.Count == 7)
                    AddBestCards(cards);

               
                HighCards.AddRange(cards.GroupBy(x => x.Value).Where(g => g.Count() == 4).Select(x => x.First()));
                Suit = cards[3].Suit;
                HandValue = Hands.ThreeOfAKind;
               
            }
            return;
        }

        private void IsPairOrTwoPairs(List<Card> cards)
        {
            #region Has Pairs?
            HighCards.Clear();
            List<Values> pairs = new List<Values>();

            var pairs1 = cards
                .GroupBy(x => x.Value)
                .Where(g => g.Count() == 2)
                .SelectMany(g => g)
                .ToList();

            if (cards.Count(c => c.Value.Equals(cards[0].Value)).Equals(2))
                pairs.Add(cards[0].Value);
            if (cards.Count(c => c.Value.Equals(cards[2].Value)).Equals(2))
                pairs.Add(cards[2].Value);
            if (cards.Count(c => c.Value.Equals(cards[4].Value)).Equals(2))
                pairs.Add(cards[4].Value);

            if(cards.Count == 7)
                if (cards.Count(c => c.Value.Equals(cards[6].Value)).Equals(2))
                    pairs.Add(cards[6].Value);

            #endregion

            #region Two Pairs
            pairs1.Sort();
            if (pairs.Count >= 2)
            {
                if (cards.Count == 7)
                    AddBestCards(cards);

                HighCards.AddRange(pairs1.GroupBy(p => p.Value).Select(x => x.First()));
                
                HandValue = Hands.TwoPair;

                return;
            }
            #endregion

            #region Pair 
            if (pairs.Count.Equals(1))
            {
                if (cards.Count == 7)
                    AddBestCards(cards);

                HighCards.AddRange(pairs1.GroupBy(p => p.Value).Select(x => x.First()));
                HandValue = Hands.Pair;

            }

            if(pairs.Count.Equals(0))
            {
                BestCards.Clear();
                if (cards.Count == 7)
                {
                    for (int i = 0; i < cards.Count; i++)
                    {
                        cards.RemoveAt(i);
                        if (i == 1)
                            break;
                    }
                    BestCards.AddRange(cards);

                }
              
                HandValue = Hands.Nothing;
            }
            #endregion       
        }



        private void AddBestCards(List<Card> cards)
        {
            List<Card> newCard = new List<Card>();

            // group matching items where the group contains more than one item
            var sameCardValue = cards.GroupBy(c => c.Value).Where(g => g.Count() >= 2).SelectMany(g => g).ToList();

            var uniqueCards = cards.GroupBy(x => x.Value).Where(g => g.Count() == 1).SelectMany(g => g).ToList();

            var sortUniqueCards = uniqueCards.OrderBy(c => c).ToList();

            if (sameCardValue.Count == 6)
            {
                for (int i = 0; i < sameCardValue.Count; i++)
                {   
                    sameCardValue.RemoveAt(i);
                    if (i == 1)
                        break;
                }
            }
            else
            {
                for (int i = 0; i < sortUniqueCards.Count; i++)
                {
                    sortUniqueCards.RemoveAt(i);
                    if (i == 1)
                        break;
                }
            }
           
            newCard.AddRange(sameCardValue);
            newCard.AddRange(sortUniqueCards);
            BestCards.AddRange(newCard);          
        }

        private void RemoveCardsFromStraight(List<Card> straightCards)
        {
            List<Card> newCard = new List<Card>();
            
                if (straightCards[6].Value == Values.Seven)
                {
                    for (int i = 0; i < straightCards.Count; i++)
                    {
                    straightCards.RemoveAt(i);
                    if (i == 1)
                        {
                           break;
                        }
                        
                    }                                     
                    BestCards.AddRange(straightCards);
                }

                if (straightCards[5].Value == Values.Seven)
                {
                    for (int i = 0; i < straightCards.Count; i++)
                    {

                        if (i == 0)
                        {
                            straightCards.RemoveAt(i);
                        }

                        if (i == 6)
                        {
                            straightCards.RemoveAt(i);
                        }

                    }

                    BestCards.AddRange(straightCards);                  
                }

           
                if (straightCards[4].Value == Values.Seven)
                {
                    for (int i = 0; i < straightCards.Count; i++)
                    {
                        straightCards.RemoveAt(i);
                        if (i == 5)
                        {
                        straightCards.RemoveAt(i);
                        }
                        if (i == 6)
                        {
                        straightCards.RemoveAt(i);
                        }

                    }
                    BestCards.AddRange(straightCards);
                }             
        }
    }

}

using System.Collections.Generic;

namespace R8GoFish
{
    class CardComparerByValue : IComparer<Card>
    {
        public int Compare(Card x, Card y)
        {
            if (x == null || y == null) return 0;

            if (x.CardValue > y.CardValue)
                return 1;
            if (x.CardValue < y.CardValue)
                return -1;
            if (x.Suit > y.Suit)
                return 1;
            if (x.Suit < y.Suit)
                return -1;
            return 0;
        }
    }
}

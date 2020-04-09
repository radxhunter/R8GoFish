namespace R8GoFish
{
    partial class Card
    {
        public Card(Suits suit, CardValues cardValue)
        {
            Suit = suit;
            CardValue = cardValue;
        }
        public Suits Suit { get; set; }
        public CardValues CardValue { get; set; }
        public string Name => CardValue + " of " + Suit;

        public static bool DoesCardMatch(Card cardToCheck, Suits suit)
            => cardToCheck.Suit == suit;
        public static bool DoesCardMatch(Card cardToCheck, CardValues cardValue)
            => cardToCheck.CardValue == cardValue;

        public override string ToString()
            => Name;
    }
}

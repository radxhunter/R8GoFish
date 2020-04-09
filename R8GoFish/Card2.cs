namespace R8GoFish
{
    partial class Card
    {
        public static string Plural(CardValues cardValue)
        {
            if (cardValue == CardValues.Six)
                return "Sixes";
            return cardValue + "s";
        }
    }
}

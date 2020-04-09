using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace R8GoFish
{
    class Player
    {
        public string Name { get; }
        private readonly Random random;
        private readonly Deck cards;
        private readonly TextBox textBoxOnForm;
        public Player(string name, Random random, TextBox textBoxOnForm)
        {
            Name = name;
            this.random = random;
            this.textBoxOnForm = textBoxOnForm;
            cards = new Deck(new Card[] { });

            this.textBoxOnForm.Text += Name + " has just joined the game\r\n";
        }
        public IEnumerable<CardValues> PullOutBooks()
        {
            List<CardValues> books = new List<CardValues>();
            for (int i = 0; i <= 13; i++)
            {
                CardValues cardValue = (CardValues)i;
                int howMany = 0;
                for (int card = 0; card < cards.Count; card++)
                    if (cards.Peek(card).CardValue == cardValue)
                        howMany++;
                if (howMany != 4) continue;

                books.Add(cardValue);
                cards.PullOutValues(cardValue);
            }
            return books;
        }
        public CardValues GetRandomValue()
        {
            List<CardValues> valuesInTheDeck = new List<CardValues>();
            for (int i = 1; i <= 13; i++)
            {
                CardValues cardValue = (CardValues)i;
                for (int card = 0; card < cards.Count; card++)
                    if (cards.Peek(card).CardValue == cardValue && !valuesInTheDeck.Contains(cardValue))
                        valuesInTheDeck.Add(cardValue);
            }

            int randomValueInt = random.Next(valuesInTheDeck.Count);
            CardValues randomCardValue = valuesInTheDeck[randomValueInt];
            return randomCardValue;
            // This method gets a random cardValue—but it has to be a cardValue that's in the deck!
        }
        public Deck DoYouHaveAny(CardValues cardValue)
        {
            Deck ownedCards = cards.PullOutValues(cardValue);
            textBoxOnForm.Text += Name + " has " + ownedCards.Count + " " + Card.Plural(cardValue) + "\r\n";
            return ownedCards;
            // This is where an opponent asks if I have any cards of a certain cardValue
            // Use Deck.PullOutValues() to pull out the values. Add a line to the TextBox
            // that says, "Joe has 3 sixes"—use the new Card.Plural() static method
        }
        public void AskForACard(List<Player> players, int myIndex, Deck stock)
        {
            if (stock.Count > 0)
            {
                if (cards.Count == 0)
                    cards.Add(stock.Deal());
                CardValues randomCardValue = GetRandomValue();
                AskForACard(players, myIndex, stock, randomCardValue);
            }
        }
        public void AskForACard(List<Player> players, int myIndex, Deck stock, CardValues cardValue)
        {
            textBoxOnForm.Text += Name + " asks if anyone has a " + cardValue + Environment.NewLine;
            int totalCardsGiven = 0;
            for (int i = 0; i < players.Count; i++)
            {
                if (i != myIndex)
                {
                    Player player = players[i];
                    Deck CardsGiven = player.DoYouHaveAny(cardValue);
                    totalCardsGiven += CardsGiven.Count;
                    while (CardsGiven.Count > 0)
                        cards.Add(CardsGiven.Deal());
                }
            }

            if (totalCardsGiven != 0 || stock.Count <= 0) return;

            textBoxOnForm.Text += Name + " must draw from the stock." + Environment.NewLine;
            cards.Add(stock.Deal());
        }
        public int CardCount => cards.Count;
        public void TakeCard(Card card) { cards.Add(card); }
        public IEnumerable<string> GetCardNames() { return cards.GetCardNames(); }
        public Card Peek(int cardNumber) { return cards.Peek(cardNumber); }
        public void SortHand() { cards.SortByValue(); }
    }
}

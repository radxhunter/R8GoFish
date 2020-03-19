using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace R8GoFish
{
    class Player
    {
        public string Name { get; }
        private Random random;
        private Deck cards;
        private TextBox textBoxOnForm;
        public Player(String Name, Random random, TextBox textBoxOnForm)
        {
            this.Name = Name;
            this.random = random;
            this.textBoxOnForm = textBoxOnForm;
            cards = new Deck(new Card[] { });

            this.textBoxOnForm.Text += this.Name + " has just joined the game\r\n";
        }
        public IEnumerable<Values> PullOutBooks()
        {
            List<Values> books = new List<Values>();
            for (int i = 0; i <= 13; i++)
            {
                Values value = (Values)i;
                int howMany = 0;
                for (int card = 0; card < cards.Count; card++)
                    if (cards.Peek(card).Value == value)
                        howMany++;
                if (howMany == 4)
                {
                    books.Add(value);
                    cards.PullOutValues(value);
                }
            }
            return books;
        }
        public Values GetRandomValue()
        {
            List<Values> valuesInTheDeck = new List<Values>();
            for (int i = 1; i <= 13; i++)
            {
                Values value = (Values)i;
                for (int card = 0; card < cards.Count; card++)
                    if (cards.Peek(card).Value == value && !valuesInTheDeck.Contains(value))
                        valuesInTheDeck.Add(value);
            } 
            
            int randomValueInt = random.Next(valuesInTheDeck.Count); 
            Values randomValue = valuesInTheDeck[randomValueInt];
            return randomValue;
            // This method gets a random value—but it has to be a value that's in the deck!
        }
        public Deck DoYouHaveAny(Values value)
        {
            Deck ownedCards = cards.PullOutValues(value);
            textBoxOnForm.Text += Name + " has " + ownedCards.Count + " " + Card.Plural(value) + "\r\n";
            return ownedCards;
            // This is where an opponent asks if I have any cards of a certain value
            // Use Deck.PullOutValues() to pull out the values. Add a line to the TextBox
            // that says, "Joe has 3 sixes"—use the new Card.Plural() static method
        }
        public void AskForACard(List<Player> players, int myIndex, Deck stock)
        {
            if (stock.Count > 0)
            {
                if (cards.Count == 0)
                    cards.Add(stock.Deal());
                Values randomValue = GetRandomValue();
                AskForACard(players, myIndex, stock, randomValue);
            }
        }
        public void AskForACard(List<Player> players, int myIndex, Deck stock, Values value)
        {
            textBoxOnForm.Text += Name + " asks if anyone has a " + value + Environment.NewLine;
            int totalCardsGiven = 0;
            for (int i = 0; i < players.Count; i++)
            {
                if (i != myIndex)
                {
                    Player player = players[i];
                    Deck CardsGiven = player.DoYouHaveAny(value);
                    totalCardsGiven += CardsGiven.Count;
                    while (CardsGiven.Count > 0)
                        cards.Add(CardsGiven.Deal());
                }
            }
            if (totalCardsGiven == 0 && stock.Count > 0)
            {
                textBoxOnForm.Text += Name + " must draw from the stock." + Environment.NewLine;
                cards.Add(stock.Deal());
            }
        }
        public int CardCount { get { return cards.Count; } }
        public void TakeCard(Card card) { cards.Add(card); }
        public IEnumerable<string> GetCardNames() { return cards.GetCardNames(); }
        public Card Peek(int cardNumber) { return cards.Peek(cardNumber); }
        public void SortHand() { cards.SortByValue(); }
    }
}

using System;
using System.Collections.Generic;

namespace R8GoFish
{
    class Deck
    {
        private List<Card> _cards;
        private readonly Random _random = new Random();
        public Deck()
        {
            _cards = new List<Card>();
            for (int suit = 0; suit < Enum.GetNames(typeof(Suits)).Length; suit++)
                for (int value = 0; value < Enum.GetNames(typeof(CardValues)).Length; value++)
                    _cards.Add(new Card((Suits)suit, (CardValues)value));
        }
        public Deck(IEnumerable<Card> initialCards)
        {
            _cards = new List<Card>(initialCards);
        }

        public int Count => _cards.Count;

        public void Add(Card cardToAdd)
        {
            _cards.Add(cardToAdd);
        }

        public Card Deal(int index)
        {
            var cardToDeal = _cards[index];
            _cards.RemoveAt(index);
            return cardToDeal;
        }
        public Card Deal()
            => Deal(0);

        public void Shuffle()
        {
            var newCards = new List<Card>();
            while (_cards.Count > 0)
            {
                int cardToMove = _random.Next(_cards.Count);
                newCards.Add(_cards[cardToMove]);
                _cards.RemoveAt(cardToMove);
            }
            _cards = newCards;
        }

        public IEnumerable<string> GetCardNames()
        {
            var cardNames = new string[_cards.Count];
            for (var i = 0; i < _cards.Count; i++)
                cardNames[i] = _cards[i].Name;
            return cardNames;
        }

        public void SortBySuit()
            => _cards.Sort(new CardComparerBySuit());
        public void SortByValue()
            => _cards.Sort(new CardComparerByValue());
        public Card Peek(int cardNumber) 
            => _cards[cardNumber];
        public bool ContainsValue(CardValues cardValue)
        {
            foreach (Card card in _cards)
                if (card.CardValue == cardValue)
                    return true;
            return false;
        }
        public Deck PullOutValues(CardValues cardValue)
        {
            Deck deckToReturn = new Deck(new Card[] { });
            for (int i = _cards.Count - 1; i >= 0; i--)
                if (_cards[i].CardValue == cardValue)
                    deckToReturn.Add(Deal(i));
            return deckToReturn;
        }
        public bool HasBook(CardValues cardValue)
        {
            int numberOfCards = 0;
            foreach (Card card in _cards)
                if (card.CardValue == cardValue)
                    numberOfCards++;

            return numberOfCards == 4;
        }
    }
}

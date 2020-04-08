using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace R8GoFish
{
    class Game
    {
        private readonly List<Player> _players;
        private readonly Dictionary<CardValues, Player> books;
        private readonly Deck stock;
        private readonly TextBox textBoxOnForm;
        public Game(string playerName, IReadOnlyCollection<string> opponentNames, TextBox textBoxOnForm)
        {
            var random = new Random();
            this.textBoxOnForm = textBoxOnForm;

            // Initialize a list with local player already in
            _players = new List<Player>(opponentNames.Count + 1) 
                { new Player(playerName, random, textBoxOnForm) };

            books = new Dictionary<CardValues, Player>();
            stock = new Deck();
            foreach (var name in opponentNames)
            {
                _players.Add(new Player(name, random, textBoxOnForm));
            }
            
            Deal();
            _players[0].SortHand();
        }
        private void Deal()
        {
            stock.Shuffle();
            foreach (var player in _players)
            {
                for (var j = 1; j <= 5; j++)
                {
                    if (stock.Count > 0)
                        player.TakeCard(stock.Deal());
                }
            }
            foreach (var player in _players)
                player.PullOutBooks();
            // This is where the game starts—this method's only called at the beginning
            // of the game. Shuffle the stock, deal five cards to each player, then use a
            // foreach loop to call each player's PullOutBooks() method.
        }
        public bool PlayOneRound(int selectedPlayerCard)
        {
            var cardCardValue = _players[0].Peek(selectedPlayerCard).CardValue;
            foreach (var player in _players)
            {
                if (_players.IndexOf(player) == 0)
                    player.AskForACard(_players, _players.IndexOf(player), stock, cardCardValue);
                else
                    player.AskForACard(_players, _players.IndexOf(player), stock);
                if (PullOutBooks(player))
                    player.TakeCard(stock.Deal());
                player.SortHand();
                if (stock.Count > 0) continue;

                textBoxOnForm.Text = "The stock is out of cards. Game over!";
                return true;
            }
            return false;
            // Play one round of the game. The parameter is the card the player selected
            // from his hand—get its cardValue. Then go through all of the _players and call
            // each one's AskForACard() methods, starting with the human player (who's
            // at index zero in the Players list—make sure he asks for the selected
            // card's cardValue). Then call PullOutBooks()—if it returns true, then the
            // player ran out of cards and needs to draw a new hand. After all the _players
            // have gone, sort the human player's hand (so it looks nice in the form).
            // Then check the stock to see if it's out of cards. If it is, reset the
            // TextBox on the form to say, "The stock is out of cards. Game over!" and return
            // true. Otherwise, the game isn't over yet, so return false.
        }
        public bool PullOutBooks(Player player)
        {
            foreach (var book in player.PullOutBooks())
                books.Add(book, player);
            return player.CardCount <= 0;
            // Pull out a player's books. Return true if the player ran out of cards, otherwise
            // return false. Each book is added to the Books dictionary. A player runs out of
            // cards when he’'s used all of his cards to make books—and he wins the game.
        }
        public string DescribeBooks()
        {
            string description = "";
            foreach (var book in books)
                description += book.Value.Name + " has a book of " + Card.Plural(book.Key) + Environment.NewLine;
            return description;
            // Return a long string that describes everyone's books by looking at the Books
            // dictionary: "Joe has a book of sixes. (line break) Ed has a book of Aces."
        }
        public string GetWinnerName()
        {
            Dictionary<string, int> winners = new Dictionary<string, int>();
            foreach (CardValues value in books.Keys)
            {
                string name = books[value].Name;
                if (winners.ContainsKey(name))
                    winners[name]++;
                else
                    winners.Add(name, 1);
            }
            int mostBooks = 0;
            foreach (string name in winners.Keys)
                if (winners[name] > mostBooks)
                    mostBooks = winners[name];
            bool tie = false;
            string winnerList = "";
            foreach (string name in winners.Keys)
                if (winners[name] == mostBooks)
                {
                    if (!string.IsNullOrEmpty(winnerList))
                    {
                        winnerList += " and ";
                        tie = true;
                    }
                    winnerList += name;
                }
            winnerList += " with " + mostBooks + " books";
            if (tie)
                return "A tie between " + winnerList;
            return winnerList;
        }
        // Here are a couple of short methods that were already written for you:
        public IEnumerable<string> GetPlayerCardNames()
        {
            return _players[0].GetCardNames();
        }
        public string DescribePlayerHands()
        {
            string description = "";
            foreach (var player in _players)
            {
                description += player.Name + " has " + player.CardCount;
                if (player.CardCount == 1)
                    description += " card." + Environment.NewLine;
                else
                    description += " cards." + Environment.NewLine;
            }
            description += "The stock has " + stock.Count + " cards left.";
            return description;
        }
    }
}

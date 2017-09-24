using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequence
{
    enum Suit
    {
        Red,
        Black,
        Hearts,
        Clubs,
        Diamonds,
        Spades
    }

    enum Face
    {
        Joker,
        Ace,
        _2,
        _3,
        _4,
        _5,
        _6,
        _7,
        _8,
        _9,
        _10,
        Jack,
        Queen,
        King
    }

    struct Card : IEquatable<Card>
    {
        public Suit Suit;
        public Face Face;

        public bool Equals(Card other)
        {
            return Suit == other.Suit && Face == other.Face;
        }

        string faceString()
        {
            switch (Face)
            {
                case Face.Ace:
                    return "A";
                case Face._10:
                    return "T";
                case Face.Jack:
                    return "J";
                case Face.Queen:
                    return "Q";
                case Face.King:
                    return "K";
                case Face.Joker:
                    return "☺";
                default:
                    return ((int)Face).ToString();
            }
        }

        string suitString()
        {
            switch (Suit)
            {
                case Suit.Hearts:
                    return "♥";
                case Suit.Diamonds:
                    return "♦";
                case Suit.Clubs:
                    return "♣";
                case Suit.Spades:
                    return "♠";
                case Suit.Red:
                    return "R";
                case Suit.Black:
                    return "B";
                default:
                    throw new Exception();
            }
        }

        public override string ToString()
        {
            return faceString() + suitString();
        }
    }

    struct CellCoord : IEquatable<CellCoord>
    {
        public CellCoord(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row;
        public int Col;

        public bool Equals(CellCoord other)
        {
            return Row == other.Row && Col == other.Col;
        }

        public CellCoord this[int dRow, int dCol]
        {
            get { return new CellCoord(Row + dRow, Col + dCol); }
        }
    }

    class Cell
    {
        public Card Card;
        public Team Chip = Team.None;
        public bool Locked = false;

        string teamString()
        {
            switch (Chip)
            {
                case Team.Blue:
                    return "X";
                case Team.Red:
                    return "O";
                case Team.None:
                default:
                    return " ";
            }
        }

        public override string ToString()
        {
            return Card.ToString() + teamString() + (Locked ? "." : " ");
        }
    }

    class Board
    {
        Cell[,] _cells = new Cell[10, 10];

        public Board()
        {
            var xs = new int[] { 0, 0, 0, 0, 1, 2, 3, 1, 1, 2 };
            var ys = new int[] { 1, 2, 3, 4, 4, 4, 4, 2, 3, 3 };
            var faces = Enum.GetValues(typeof(Face)).Cast<Face>().Except(new[] { Face.Jack, Face.Joker, Face.Queen, Face.King }).ToArray();
            for (int i = 0; i < xs.Length; i++)
            {
                _cells[5 + xs[i], 5 + ys[i]] = new Cell() { Card = new Sequence.Card() { Face = faces[i], Suit = Suit.Hearts } };
                _cells[4 - xs[i], 4 - ys[i]] = new Cell() { Card = new Sequence.Card() { Face = faces[i], Suit = Suit.Hearts } };

                _cells[4 - xs[i], 5 + ys[i]] = new Cell() { Card = new Sequence.Card() { Face = faces[i], Suit = Suit.Diamonds } };
                _cells[5 + xs[i], 4 - ys[i]] = new Cell() { Card = new Sequence.Card() { Face = faces[i], Suit = Suit.Diamonds } };

                _cells[4 - ys[i], 5 + xs[i]] = new Cell() { Card = new Sequence.Card() { Face = faces[i], Suit = Suit.Clubs } };
                _cells[5 + ys[i], 4 - xs[i]] = new Cell() { Card = new Sequence.Card() { Face = faces[i], Suit = Suit.Clubs } };

                _cells[5 + ys[i], 5 + xs[i]] = new Cell() { Card = new Sequence.Card() { Face = faces[i], Suit = Suit.Spades } };
                _cells[4 - ys[i], 4 - xs[i]] = new Cell() { Card = new Sequence.Card() { Face = faces[i], Suit = Suit.Spades } };
            }

            _cells[1, 1] = new Cell() { Card = new Card() { Face = Face.Queen, Suit = Suit.Hearts } };
            _cells[8, 8] = new Cell() { Card = new Card() { Face = Face.Queen, Suit = Suit.Hearts } };
            _cells[1, 8] = new Cell() { Card = new Card() { Face = Face.Queen, Suit = Suit.Diamonds } };
            _cells[8, 1] = new Cell() { Card = new Card() { Face = Face.Queen, Suit = Suit.Diamonds } };
            _cells[6, 3] = new Cell() { Card = new Card() { Face = Face.Queen, Suit = Suit.Clubs } };
            _cells[3, 6] = new Cell() { Card = new Card() { Face = Face.Queen, Suit = Suit.Clubs } };
            _cells[3, 3] = new Cell() { Card = new Card() { Face = Face.Queen, Suit = Suit.Spades } };
            _cells[6, 6] = new Cell() { Card = new Card() { Face = Face.Queen, Suit = Suit.Spades } };

            _cells[0, 0] = new Cell() { Card = new Card() { Face = Face.King, Suit = Suit.Hearts } };
            _cells[9, 9] = new Cell() { Card = new Card() { Face = Face.King, Suit = Suit.Hearts } };
            _cells[0, 9] = new Cell() { Card = new Card() { Face = Face.King, Suit = Suit.Diamonds } };
            _cells[9, 0] = new Cell() { Card = new Card() { Face = Face.King, Suit = Suit.Diamonds } };
            _cells[5, 4] = new Cell() { Card = new Card() { Face = Face.King, Suit = Suit.Clubs } };
            _cells[4, 5] = new Cell() { Card = new Card() { Face = Face.King, Suit = Suit.Clubs } };
            _cells[4, 4] = new Cell() { Card = new Card() { Face = Face.King, Suit = Suit.Spades } };
            _cells[5, 5] = new Cell() { Card = new Card() { Face = Face.King, Suit = Suit.Spades } };

            _cells[2, 2] = new Cell() { Card = new Card() { Face = Face.Joker, Suit = Suit.Red } };
            _cells[7, 7] = new Cell() { Card = new Card() { Face = Face.Joker, Suit = Suit.Red } };
            _cells[2, 7] = new Cell() { Card = new Card() { Face = Face.Joker, Suit = Suit.Black } };
            _cells[7, 2] = new Cell() { Card = new Card() { Face = Face.Joker, Suit = Suit.Black } };
        }

        public Cell this[CellCoord coord]
        {
            get { return _cells[coord.Col, coord.Row]; }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    builder.Append(_cells[col, row]);
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }

    enum Team
    {
        None,
        Blue,
        Red
    }

    class Player
    {
        public Team Team;
        public List<Card> Hand = new List<Card>();

        public override string ToString()
        {
            return Team.ToString() + ": " + string.Join(" ", Hand.Select(c => c.ToString()).ToArray());
        }
    }

    class Move
    {
        public Player Player;
        public Card CardPlayed;
        public CellCoord CellChosen;
    }

    enum FlipDirection
    {
        Right,
        Down,
        RightDown,
        RightUp
    }

    class Flip
    {
        public Player Player;
        public CellCoord CellChosen;
        public FlipDirection Direction;
    }

    class Deck
    {
        List<Card> _cards = new List<Card>();

        public Deck()
        {
            foreach (Face face in Enum.GetValues(typeof(Face)).Cast<Face>().Except(new[] { Face.Joker }))
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)).Cast<Suit>().Except(new[] { Suit.Black, Suit.Red }))
                {
                    _cards.Add(new Sequence.Card() { Face = face, Suit = suit });
                    _cards.Add(new Sequence.Card() { Face = face, Suit = suit });
                }
            }
            _cards.Add(new Card() { Face = Face.Joker, Suit = Suit.Black });
            _cards.Add(new Card() { Face = Face.Joker, Suit = Suit.Black });
            _cards.Add(new Card() { Face = Face.Joker, Suit = Suit.Red });
            _cards.Add(new Card() { Face = Face.Joker, Suit = Suit.Red });
        }

        public void Shuffle()
        {
            Random r = new Random();
            _cards.Sort((a, b) => (r.Next() % 2) * 2 - 1);
        }

        public Card Draw()
        {
            Card drawn = _cards[0];
            _cards.RemoveAt(0);
            return drawn;
        }

        public IEnumerable<Card> Draw(int n)
        {
            for (int i = 0; i < n; i++)
            {
                yield return Draw();
            }
        }
    }

    class Game
    {
        public List<Player> _players = new List<Player>();
        Deck _deck = new Deck();
        int _currentTurn = 0;
        Board _board = new Board();
        List<CellCoord> _rowSequences = new List<CellCoord>();
        List<CellCoord> _colSequences = new List<CellCoord>();
        List<CellCoord> _nwDiagSequences = new List<CellCoord>();
        List<CellCoord> _neDiagSequences = new List<CellCoord>();
        Dictionary<Team, int> _scores = new Dictionary<Team, int>();

        public Game(int playerCount)
        {
            if (playerCount == 2)
            {
                _players.AddRange(new Player[] {
                    new Player() { Team = Team.Blue },
                    new Player() { Team = Team.Red }
                });
            }
            else if (playerCount == 4)
            {
                _players.AddRange(new Player[] {
                    new Player() { Team = Team.Blue },
                    new Player() { Team = Team.Red },
                    new Player() { Team = Team.Blue },
                    new Player() { Team = Team.Red }
                });
            }

            _deck.Shuffle();
            foreach (Player player in _players)
            {
                player.Hand.AddRange(_deck.Draw(6));
            }
        }

        bool tryMove(Move move)
        {
            if (!_players.Contains(move.Player) || _players.IndexOf(move.Player) != _currentTurn)
            {
                return false;
            }

            if (_rowSequences.Any() || _colSequences.Any() || _nwDiagSequences.Any() || _neDiagSequences.Any())
            {
                return false;
            }

            if (!move.Player.Hand.Contains(move.CardPlayed))
            {
                return false;
            }

            if (move.CardPlayed.Face == Face.Jack)
            {
                switch (move.CardPlayed.Suit)
                {
                    // Wild
                    case Suit.Diamonds:
                    case Suit.Clubs:
                        if (_board[move.CellChosen].Chip != Team.None)
                        {
                            return false;
                        }
                        _board[move.CellChosen].Chip = move.Player.Team;
                        return true;
                    // Removal
                    case Suit.Hearts:
                    case Suit.Spades:
                        if (_board[move.CellChosen].Chip == Team.None || _board[move.CellChosen].Chip == move.Player.Team || _board[move.CellChosen].Locked)
                        {
                            return false;
                        }
                        _board[move.CellChosen].Chip = Team.None;
                        return true;
                    default:
                        throw new Exception("Whoops!");
                }
            }

            if (!move.CardPlayed.Equals(_board[move.CellChosen].Card))
            {
                return false;
            }

            return true;
        }

        void updateSequences(Player player, CellCoord cellChosen)
        {
            // horizontal, const row
            List<CellCoord> rowSequences = new List<CellCoord>();
            for (int col = Math.Max(0, cellChosen.Col - 4); col <= Math.Min(cellChosen.Col, 5); col++)
            {
                int row = cellChosen.Row;
                if (Enumerable.Range(0, 5).All(i => _board[new CellCoord(row, col + i)].Chip == player.Team) &&
                    Enumerable.Range(0, 5).Count(i => _board[new CellCoord(row, col + i)].Locked) <= 1)
                {
                    rowSequences.Add(new CellCoord(row, col));
                }
            }

            // vertical, const col
            List<CellCoord> colSequences = new List<CellCoord>();
            for (int row = Math.Max(0, cellChosen.Row - 4); row <= Math.Min(cellChosen.Row, 5); row++)
            {
                int col = cellChosen.Col;
                if (Enumerable.Range(0, 5).All(i => _board[new CellCoord(row + i, col)].Chip == player.Team) &&
                    Enumerable.Range(0, 5).Count(i => _board[new CellCoord(row + i, col)].Locked) <= 1)
                {
                    colSequences.Add(new CellCoord(row, col));
                }
            }

            // nw diag, const diff
            List<CellCoord> nwDiagSequences = new List<CellCoord>();
            for (int col = Math.Max(0, cellChosen.Col - 4); col <= Math.Min(cellChosen.Col, 5); col++)
            {
                int row = col - (cellChosen.Col - cellChosen.Row);
                if (Enumerable.Range(0, 5).All(i => _board[new CellCoord(row + i, col + i)].Chip == player.Team) &&
                    Enumerable.Range(0, 5).Count(i => _board[new CellCoord(row + i, col + i)].Locked) <= 1)
                {
                    nwDiagSequences.Add(new CellCoord(row, col));
                }
            }

            // ne diag, const sum
            List<CellCoord> neDiagSequences = new List<CellCoord>();
            for (int col = Math.Max(0, cellChosen.Col - 4); col <= Math.Min(cellChosen.Col, 5); col++)
            {
                int row = (cellChosen.Col + cellChosen.Row) - col;
                if (Enumerable.Range(0, 5).All(i => _board[new CellCoord(row - i, col + i)].Chip == player.Team) &&
                    Enumerable.Range(0, 5).Count(i => _board[new CellCoord(row - i, col + i)].Locked) <= 1)
                {
                    neDiagSequences.Add(new CellCoord(row, col));
                }
            }

            _rowSequences = rowSequences;
            _colSequences = colSequences;
            _nwDiagSequences = nwDiagSequences;
            _neDiagSequences = neDiagSequences;

            if (!_rowSequences.Any() && !_colSequences.Any() && !_nwDiagSequences.Any() && !_neDiagSequences.Any())
            {
                _currentTurn++;
                _currentTurn %= _players.Count;
            }
        }

        public bool MakeMove(Move move)
        {
            if (!tryMove(move))
            {
                return false;
            }

            move.Player.Hand.Remove(move.CardPlayed);
            move.Player.Hand.Add(_deck.Draw());

            updateSequences(move.Player, move.CellChosen);

            return true;
        }

        public bool DoFlip(Flip flip)
        {
            if (!_players.Contains(flip.Player) || _players.IndexOf(flip.Player) != _currentTurn)
            {
                return false;
            }

            switch (flip.Direction)
            {
                case FlipDirection.Right:
                    if (!_rowSequences.Contains(flip.CellChosen))
                    {
                        return false;
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        _board[flip.CellChosen[0, i]].Locked = true;
                    }
                    break;
                case FlipDirection.Down:
                    if (!_colSequences.Contains(flip.CellChosen))
                    {
                        return false;
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        _board[flip.CellChosen[i, 0]].Locked = true;
                    }
                    break;
                case FlipDirection.RightDown:
                    if (!_nwDiagSequences.Contains(flip.CellChosen))
                    {
                        return false;
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        _board[flip.CellChosen[i, i]].Locked = true;
                    }
                    break;
                case FlipDirection.RightUp:
                    if (!_neDiagSequences.Contains(flip.CellChosen))
                    {
                        return false;
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        _board[flip.CellChosen[-i, i]].Locked = true;
                    }
                    break;
            }

            updateSequences(flip.Player, flip.CellChosen);

            return true;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(_board.ToString());
            foreach (var player in _players)
                builder.AppendLine(player.ToString());
            return builder.ToString();
        }
    }

    class Program
    {
        static void PrintState(Game game)
        {
            Console.WriteLine(game.ToString());
        }

        static void Main(string[] args)
        {
            Game game = new Sequence.Game(2);
            PrintState(game);
            Console.ReadLine();
            game.MakeMove(new Sequence.Move()
            {
                Player = game._players[0],
                CardPlayed = game._players[0].Hand[0],
                CellChosen = new CellCoord(0, 0)
            });
            PrintState(game);
            Console.ReadLine();
        }
    }
}

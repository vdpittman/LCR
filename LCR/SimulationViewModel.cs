using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace LCR
{
    /// <summary>
    /// Class representing a simulation
    /// </summary>
    public class SimulationViewModel : ViewModelBase
    {
        private static readonly Random _random = new Random();

        private int _numberOfPlayers = 3;

        /// <summary>
        /// Gets or sets the number of players
        /// </summary>
        public int NumberOfPlayers
        {
            get
            {
                return _numberOfPlayers;
            }
            set 
            {
                if (value < 2)
                {
                    MessageBox.Show("Must have at least two players");
                }
                else if (_numberOfPlayers != value)
                {
                    _numberOfPlayers = value;
                    RaisePropertyChanged(nameof(NumberOfPlayers));
                }
            }
        }

        private int _numberOfGames = 100;

        /// <summary>
        /// Gets or sets the number of games to be played
        /// </summary>
        public int NumberOfGames
        {
            get
            {
                return _numberOfGames;
            }
            set
            {
                if (value < 1)
                {
                    MessageBox.Show("Must play at least one game per simulation");
                }
                else if (_numberOfGames != value)
                {
                    _numberOfGames = value;
                    RaisePropertyChanged(nameof(NumberOfGames));
                }
            }
        }

        private readonly DieViewModel _die = new DieViewModel();

        private ObservableCollection<PlayerViewModel> _players;

        /// <summary>
        /// Gets or sets the collection of players
        /// </summary>
        public ObservableCollection<PlayerViewModel> Players
        {
            get
            {
                return _players;
            }

            set
            {
                if (_players != value)
                {
                    _players = value;
                    RaisePropertyChanged(nameof(Players));
                }
            }
        }

        private int _shortestGameLength = 0;

        /// <summary>
        /// Gets or sets the shortest game length, in number of turns played
        /// </summary>
        public int ShortestGameLength
        {
            get
            {
                return _shortestGameLength;
            }

            set
            {
                if (_shortestGameLength != value)
                {
                    _shortestGameLength = value;
                    RaisePropertyChanged(nameof(ShortestGameLength));
                }
            }
        }

        private int _longestGameLength = 0;

        /// <summary>
        /// Gets or sets the longest game length, in number of turns played
        /// </summary>
        public int LongestGameLength
        {
            get
            {
                return _longestGameLength;
            }

            set
            {
                if (_longestGameLength != value)
                {
                    _longestGameLength = value;
                    RaisePropertyChanged(nameof(LongestGameLength));
                }
            }
        }

        /// <summary>
        /// Gets or sets the average game length, in number of turns played
        /// </summary>
        public double AverageGameLength { get => (double)_totalNumberOfTurns / NumberOfGames; }

        private bool _enablePlayButton = true;

        /// <summary>
        /// Gets or sets a value that enables the Run Simulation button
        /// </summary>
        public bool EnablePlayButton
        {
            get
            {
                return _enablePlayButton;
            }

            private set
            {
                if (_enablePlayButton != value)
                {
                    _enablePlayButton = value;
                    RaisePropertyChanged(nameof(EnablePlayButton));
                }
            }
        }

        private int _totalNumberOfTurns = 0;

        /// <summary>
        /// Runs the simulation
        /// </summary>
        public void RunSimulation()
        {
            try
            {
                EnablePlayButton = false;

                ShortestGameLength = 0;
                LongestGameLength = 0;
                _totalNumberOfTurns = 0;

                Players = new ObservableCollection<PlayerViewModel>(GetPlayers(NumberOfPlayers));

                for (int gameNumber = 0; gameNumber < _numberOfGames; gameNumber++)
                {
                    int gameLength = PlayGame();

                    _totalNumberOfTurns += gameLength;

                    if (gameNumber == 0)
                    {
                        ShortestGameLength = LongestGameLength = gameLength;
                    }
                    else
                    {
                        ShortestGameLength = Math.Min(ShortestGameLength, gameLength);
                        LongestGameLength = Math.Max(LongestGameLength, gameLength);
                    }

                    PlayerViewModel winner = Players.FirstOrDefault(player => player.NumberOfChips > 0);
                    Debug.Assert(winner != null);
                    if (winner != null)
                    {
                        winner.NumberOfWins++;
                    }
                }

                RaisePropertyChanged(nameof(AverageGameLength));
            }
            finally
            {
                EnablePlayButton = true;
            }
        }

        /// <summary>
        /// Plays one game
        /// </summary>
        /// <returns>The number of turns taken in the game</returns>
        private int PlayGame()
        {
            // Give each player three chips
            foreach (PlayerViewModel player in _players)
            {
                player.NumberOfChips = 3;
            }

            // Pick the first player at random
            PlayerViewModel currentPlayer = _players[_random.Next(NumberOfPlayers)];

            int turnNumber = 1;

            // Players take turns until only one has chips, who is then the winner
            while (!GameEnded())
            {
                if (currentPlayer.NumberOfChips > 0)
                {
                    // Get number of dice rolls
                    int numberOfDice = Math.Min(3, currentPlayer.NumberOfChips);

                    // Take action based on the face value of each die roll
                    for (int dieNumber = 0; dieNumber < numberOfDice; dieNumber++)
                    {
                        switch (_die.Roll())
                        {
                            case Face.Left:
                                // Give a chip to the player on the left
                                currentPlayer.GiveChipTo(GetPlayerBefore(currentPlayer));
                                break;

                            case Face.Right:
                                // Give a chip to the player on the right
                                currentPlayer.GiveChipTo(GetPlayerAfter(currentPlayer));
                                break;

                            case Face.Center:
                                // Put a chip in the center
                                // Note: No need to keep a count of chips in the center
                                currentPlayer.NumberOfChips--;
                                break;
                        }
                    }
                }

                currentPlayer = GetPlayerAfter(currentPlayer);
                turnNumber++;
            }

            return turnNumber;
        }

        private IEnumerable<PlayerViewModel> GetPlayers(int count)
        {
            return Enumerable.Range(0, count).Select(i => new PlayerViewModel(i));
        }

        private PlayerViewModel GetPlayerAfter(PlayerViewModel player)
        {
            Debug.Assert(player != null);

            int playerNumber = player.PlayerNumber + 1;

            return playerNumber >= NumberOfPlayers ? Players.First() : Players[playerNumber];
        }

        private PlayerViewModel GetPlayerBefore(PlayerViewModel player)
        {
            Debug.Assert(player != null);

            int playerNumber = player.PlayerNumber - 1;

            return playerNumber < 0 ? Players.Last() : Players[playerNumber];
        }

        private bool GameEnded()
        {
            return Players.Count(player => player.NumberOfChips > 0) == 1;
        }
    }
}

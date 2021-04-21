using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCR
{
    /// <summary>
    /// Class representing a player
    /// </summary>
    public class Player : ViewModelBase
    {
        private int _playerNumber = 0;

        /// <summary>
        /// Gets or sets the player number
        /// </summary>
        public int PlayerNumber
        {
            get
            {
                return _playerNumber;
            }
            private set
            {
                if (_playerNumber != value)
                {
                    _playerNumber = value;
                    RaisePropertyChanged(nameof(PlayerNumber));
                }
            }
        }

        private int _numberOfChips = 0;

        /// <summary>
        /// Gets or sets the number of chips
        /// </summary>
        public int NumberOfChips
        {
            get
            {
                return _numberOfChips;
            }
            set
            {
                if (_numberOfChips != value)
                {
                    _numberOfChips = value;
                    RaisePropertyChanged(nameof(NumberOfChips));
                }
            }
        }

        private int _numberOfWins = 0;

        /// <summary>
        /// Gets or sets the number of wins
        /// </summary>
        public int NumberOfWins
        {
            get
            {
                return _numberOfWins;
            }

            set
            {
                if (_numberOfWins != value)
                {
                    _numberOfWins = value;
                    RaisePropertyChanged(nameof(NumberOfWins));
                }
            }
        }

        /// <summary>
        /// Initializes an object of this class
        /// </summary>
        /// <param name="playerNumber">The player number</param>
        public Player(int playerNumber)
        {
            PlayerNumber = playerNumber;
        }

        /// <summary>
        /// Give a chip to the designated player
        /// </summary>
        /// <param name="player">The player to receive the chip</param>
        public void GiveChipTo(Player player)
        {
            Debug.Assert(player != null && NumberOfChips > 0);

            NumberOfChips--;
            player.NumberOfChips++;
        }
    }
}

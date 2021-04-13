using System;

namespace LCR
{
    /// <summary>
    /// Enumeration giving the possible face values of a die
    /// </summary>
    public enum Face
    {
        Dot1,
        Dot2,
        Dot3,
        Left,
        Center,
        Right
    }

    /// <summary>
    /// Class representing an individual die
    /// </summary>
    public class DieViewModel : ViewModelBase
    {
        private static readonly int _faceCount = Enum.GetNames(typeof(Face)).Length;

        private static readonly Random _random = new Random();

        private Face _face;

        /// <summary>
        /// Gets or sets the current face value
        /// </summary>
        public Face Face
        {
            get
            {
                return _face;
            }

            set
            {
                if (_face != value)
                {
                    _face = value;
                    RaisePropertyChanged(nameof(Face));
                }
            }
        }

        /// <summary>
        /// Initializes an object of this class
        /// </summary>
        public DieViewModel()
        {
            Roll();
        }

        /// <summary>
        /// Rolls the dice, producing a new random Face value
        /// </summary>
        /// <returns>The new face value</returns>
        public Face Roll()
        {
            Face = (Face)_random.Next(_faceCount);

            return Face;
        }

    }
}

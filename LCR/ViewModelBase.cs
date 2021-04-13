using System.ComponentModel;
using System.Diagnostics;

namespace LCR
{
    /// <summary>
    /// Base class for view models
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// The proerty-changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property-changed event
        /// </summary>
        /// <param name="propertyName">The name of the property that changed</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            Debug.Assert(propertyName != null);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ohtu1Project.ViewModels
{
    /// <summary>
    /// Base class for all view models in the application.
    /// Implements the INotifyPropertyChanged interface, allowing the view to be notified of changes to properties in the view model.
    /// </summary>
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static event PropertyChangedEventHandler StaticPropertyChanged;

        /// <summary>
        /// Method that raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Method that raises the StaticPropertyChanged event for the specified static property.
        /// </summary>
        /// <param name="name"></param>
        protected static void OnStaticPropertyChanged([CallerMemberName] string name = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(name));
        }
    }
}
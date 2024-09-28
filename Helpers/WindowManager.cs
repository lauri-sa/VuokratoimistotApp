using System.Windows;

namespace Ohtu1Project.Helpers
{
    /// <summary>
    /// A helper class that provides methods for managing the application's windows.
    /// </summary>
    internal class WindowManager
    {
        /// <summary>
        /// Opens the Window that is given as parameter.
        /// </summary>
        /// <param name="window">The Window to open.</param>
        public static void OpenWindow(Window window)
        {
            if (window != null)
            {
                window.ShowDialog();
            }
        }

        /// <summary>
        /// Closes the top-most window in the application.
        /// </summary>
        public static void CloseWindow()
        {
            App.Current.Windows[^1].Close();
        }

        /// <summary>
        /// Closes all but the main window of the application.
        /// </summary>
        public static void ReturnToMainWindow()
        {
            foreach(Window window in App.Current.Windows)
            {
                if(window != App.Current.MainWindow)
                {
                    window.Close();
                }
            }
        }
    }
}
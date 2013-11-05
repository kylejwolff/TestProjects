using System;
using System.Windows;

namespace GuidME
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // _ANY_ CLA causes the app to create a new guid, drop it on the clipboard, and immediately quit.
            if (Environment.GetCommandLineArgs().Length == 1)
                return;
            // If we're here, there was a commandline argument.
            Clipboard.SetText(Guid.NewGuid().ToString());
            Application.Current.Shutdown();
        }
    }
}

using System;
using System.Timers;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Threading;

namespace GuidME
{
    /// <summary>
    /// Hey, this is a cheap and quick (tho useful) app.  
    /// </summary>
    public partial class Window1 : Window
    {
        private System.Timers.Timer timer = new System.Timers.Timer(500);

        /// <summary>
        /// Initializes a new instance of the <see cref="Window1"/> class.
        /// </summary>
        public Window1()
        {
            InitializeComponent();
            // Was taking too long to find the proper timer.
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        /// <summary>
        /// Handles the Click event of the CreateGuid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void CreateGuid_Click(object sender, RoutedEventArgs e)
        {
            DoGuid();
        }

        /// <summary>
        /// Handles the Elapsed event of the timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // I thought there was a WPF timer that fired on the UI thread, but I couldn't find it; 
            // so I just use a regular timer and dispatch the call to DoGuid to get it on the STA-threaded UI thread.
            Dispatcher.Invoke(DispatcherPriority.Normal,
                (ThreadStart)(() =>
                {
                    DoGuid();
                }));
        }


        /// <summary>
        /// Creates a new guid and drops it on the Clipboard.  Also causes the form to visually indicate this action by blurring (why not?)
        /// </summary>
        /// <remarks>Must be run on the UI thread.  It modifies UI, and interacts with a COM server (clipboard, which has to happen on an STA threa).</remarks>
        private void DoGuid()
        {
            Clipboard.SetText(Guid.NewGuid().ToString());
            BeginStoryboard(this.Resources["sb"] as Storyboard);
        }

        /// <summary>
        /// Handles the Checked event of the autoCheckbox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void autoCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        /// <summary>
        /// Handles the Unchecked event of the autoCheckbox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void autoCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
    }
}

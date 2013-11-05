using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace NotifyDataErrorInfo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyDataErrorInfo
    {
        #region Text
        /// <summary>
        /// The <see cref="DependencyProperty"/> for <see cref="Text"/>.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                TextPropertyName,
                typeof(string),
                typeof(MainWindow),
                new UIPropertyMetadata(null, OnTextPropertyChanged));

        /// <summary>
        /// Called when the value of <see cref="TextProperty"/> changes on a given instance of <see cref="MainWindow"/>.
        /// </summary>
        /// <param name="d">The instance on which the property changed.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MainWindow).OnTextChanged(e.OldValue as string, e.NewValue as string);
        }

        /// <summary>
        /// Called when <see cref="Text"/> changes.
        /// </summary>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        private void OnTextChanged(string oldValue, string newValue)
        {
            var temp = ErrorsChanged;
            if(temp == null)
                return;
            temp(this, new DataErrorsChangedEventArgs(newValue));
        }

        /// <summary>
        /// The name of the <see cref="Text"/> <see cref="DependencyProperty"/>.
        /// </summary>
        public const string TextPropertyName = "Text";

        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion  
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the validation errors for a specified property or for the entire
        /// entity.
        /// </summary>
        /// <param name="propertyName">The name of the property to retrieve validation errors
        /// for; or null or <see cref="F:System.String.Empty" />, to retrieve entity-level
        /// errors.</param>
        /// <returns>The validation errors for the property or entity.</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if(Text == null)
                yield return "Shit!";
            yield break;
        }

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        /// <returns>true if the entity currently has validation errors; otherwise, false.
        /// </returns>
        /// <value></value>
        public bool HasErrors
        {
            get
            {
                return GetErrors(TextPropertyName) != null;
            }
        }

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for
        /// the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace BindingToObservables
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Collection
        /// <summary>
        /// The <see cref="DependencyProperty"/> for <see cref="Collection"/>.
        /// </summary>
        public static readonly DependencyProperty CollectionProperty =
            DependencyProperty.Register(
                CollectionPropertyName,
                typeof(ObservableCollection<string>),
                typeof(MainWindow),
                new UIPropertyMetadata(null));

        /// <summary>
        /// The name of the <see cref="Collection"/> <see cref="DependencyProperty"/>.
        /// </summary>
        public const string CollectionPropertyName = "Collection";

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<string> Collection
        {
            get { return (ObservableCollection<string>)GetValue(CollectionProperty); }
            set { SetValue(CollectionProperty, value); }
        }
        #endregion
        #region SelectedItem
        /// <summary>
        /// The <see cref="DependencyProperty"/> for <see cref="SelectedItem"/>.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                SelectedItemPropertyName,
                typeof(string),
                typeof(MainWindow),
                new UIPropertyMetadata(null));

        /// <summary>
        /// The name of the <see cref="SelectedItem"/> <see cref="DependencyProperty"/>.
        /// </summary>
        public const string SelectedItemPropertyName = "SelectedItem";

        /// <summary>
        /// 
        /// </summary>
        public string SelectedItem
        {
            get { return (string)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        #endregion
        #region Add Command
        /// <summary>
        /// Command to Add
        /// </summary>
        public DelegatedCommand<object> Add { get; private set; }
        /// <summary>
        /// Handles the <see cref="DelegatedCommand{T}.CanExecuteDelegate">CanExecute</see> method of the <see cref="DelegatedCommand{T}"/> for <see cref="Add"/>.
        /// </summary>
        /// <param name="argument">The <see cref="object"/> being passed to the command.</param>
        /// <returns><c>True</c> if <see cref="Add"/> can execute, <c>false</c> otherwise.</returns>
        private bool AddCanExecute(object argument)
        {
            return true;
        }
        /// <summary>
        /// Handles the <see cref="DelegatedCommand{T}.ExecuteDelegate">Execute</see> method of the <see cref="DelegatedCommand{T}"/> for <see cref="Add"/>.
        /// </summary>
        /// <param name="argument">The <see cref="object"/> being passed to the command.</param>
        private void AddExecute(object argument)
        {
            Collection.Add((++count).ToString());
        }
        private int count = 0;
        #endregion
        public MainWindow()
        {
            Add = new DelegatedCommand<object>
            {
                CanExecuteDelegate = x => AddCanExecute(x),
                ExecuteDelegate = x => AddExecute(x)
            };
            InitializeComponent();
            var oc = new ObservableCollection<string>();
            Collection = oc;
            oc.Add(0.ToString());
            SelectedItem = oc.First();
        }
    }

    public sealed class DelegatedCommand<T> : ICommand
    {

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return CanExecuteDelegate(parameter);
        }

        public Func<object, bool> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            ExecuteDelegate(parameter);
        }

        #endregion
    }
}

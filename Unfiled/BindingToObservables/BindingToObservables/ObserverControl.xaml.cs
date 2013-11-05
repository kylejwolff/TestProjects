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
    /// Interaction logic for ObserverControl.xaml
    /// </summary>
    public partial class ObserverControl : UserControl
    {
        #region Collection
        /// <summary>
        /// The <see cref="DependencyProperty"/> for <see cref="Collection"/>.
        /// </summary>
        public static readonly DependencyProperty CollectionProperty =
            DependencyProperty.Register(
                CollectionPropertyName,
                typeof(ObservableCollection<string>),
                typeof(ObserverControl),
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
                typeof(ObserverControl),
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
        public ObserverControl()
        {
            InitializeComponent();
        }
    }
}

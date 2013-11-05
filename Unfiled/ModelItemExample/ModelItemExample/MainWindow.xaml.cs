using System;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModelItemExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region ModelItemRoot
        /// <summary>
        /// The <see cref="DependencyPropertyKey"/> for ModelItemRoot.
        /// </summary>
        private static readonly DependencyPropertyKey ModelItemRootKey
            = DependencyProperty.RegisterReadOnly(
                ModelItemRootPropertyName,
                typeof(ModelItem),
                typeof(Control),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// The <see cref="DependencyProperty"/> for ModelItemRoot.
        /// </summary>
        public static readonly DependencyProperty ModelItemRootProperty
            = ModelItemRootKey.DependencyProperty;

        /// <summary>
        /// The name of the <see cref="ModelItemRoot"/> <see cref="DependencyProperty"/>.
        /// </summary>
        public const string ModelItemRootPropertyName = "ModelItemRoot";

        /// <summary>
        /// 
        /// </summary>
        public ModelItem ModelItemRoot
        {
            get { return (ModelItem)GetValue(ModelItemRootProperty); }
            private set { SetValue(ModelItemRootKey, value); }
        }
        #endregion

        #region EditingContext

        /// <summary>
        /// The <see cref="DependencyPropertyKey"/> for EditingContext.
        /// </summary>
        private static readonly DependencyPropertyKey EditingContextKey
        = DependencyProperty.RegisterReadOnly(
            EditingContextPropertyName,
            typeof(EditingContext),
            typeof(MainWindow),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// The <see cref="DependencyProperty"/> for EditingContext.
        /// </summary>
        public static readonly DependencyProperty EditingContextProperty
        = EditingContextKey.DependencyProperty;

        /// <summary>
        /// The name of the <see cref="EditingContext"/> <see cref="DependencyProperty"/>.
        /// </summary>
        public const string EditingContextPropertyName = "EditingContext";

        /// <summary>
        /// 
        /// </summary>
        public EditingContext EditingContext
        {
            get { return (EditingContext)GetValue(EditingContextProperty); }
            private set { SetValue(EditingContextKey, value); }
        }

        #endregion

        private ModelTreeManager _mtm;
        private UndoEngine _undo;
        // We cannot use the WF4 validation service as it has dependencies on internal types exposed by the WorkflowDesigner
        //private ValidationService _vs;
        private DesignerConfigurationService _dcs;

        public MainWindow()
        {
            InitializeComponent();
            EditingContext = new System.Activities.Presentation.EditingContext();

            _mtm = new ModelTreeManager(EditingContext);
            EditingContext.Services.Publish(_mtm);

            // we can add undo/redo
            _undo = new UndoEngine(EditingContext);
            EditingContext.Services.Publish(_undo);
            _undo.UndoRedoBufferChanged +=_undo_UndoRedoBufferChanged; 

            // add whatever services
            EditingContext.Services.Publish(new MessageBoxService());

            var root = new ObjectRoot();
            root.Text = "This is the root";
            root.Leaves.Add(new ObjectLeaf { Text = "this is leaf one" });
            _mtm.Load(root);

            ModelItemRoot = _mtm.Root;

            // the hard way
            var mi = ModelItemRoot.Properties["Leaves"].Collection.Add(new ObjectLeaf { Text = "this is leaf two" });

            mi.PropertyChanged += mi_PropertyChanged;
        }

        void _undo_UndoRedoBufferChanged(object sender, EventArgs e)
        {

        }

        void mi_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }



        private void OnUndoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _undo.Undo();
        }
        private void OnRedoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            _undo.Redo();
        }
    }

    public sealed class MessageBoxService
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message ?? "I'm too dumb to include a message!");
        }
    }

    // our configuration classes.  Just pocos, no love for databinding!
    public sealed class ObjectRoot
    {
        public string Text { get; set; }

        public LeafCollection Leaves { get; private set; }

        public ObjectRoot()
        {
            Leaves = new LeafCollection();
        }
    }
    public sealed class LeafCollection : Collection<ObjectLeaf>
    {
    }
    public sealed class ObjectLeaf
    {
        public string Text { get; set; }
    }
}
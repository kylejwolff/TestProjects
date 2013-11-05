using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel.DataAnnotations;

namespace AutomaticValidation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public sealed class ViewModel : ValidatingViewModel
    {
        private string _value;

        public ViewModel()
            : base()
        {
            Email = null;
        }
        [Required, EmailAddress]
        public string Email
        {
            get { return this._value; }
            set
            {
                this._value = value;
                OnPropertyChanged();
            }
        }
    }

    public abstract class ValidatingViewModel : INotifyPropertyChanged, IDataErrorInfo, INotifyDataErrorInfo
    {        
        private Dictionary<string, IEnumerable<string>> _errors = new Dictionary<string, IEnumerable<string>>();
        protected ValidatingViewModel() { }

        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            if(PropertyChanged == null)
                return;
            ValidateProperty(property);
            PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        private void ValidateProperty(string property)
        {
            var ctx = new ValidationContext(this) { MemberName = property };
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            if(Validator.TryValidateProperty(this.GetType().GetProperty(property).GetValue(this), ctx, results))
            {
                if(_errors.ContainsKey(property))
                {
                    _errors.Remove(property);
                    ErrorsChanged(this, new DataErrorsChangedEventArgs(property));
                }
                return;
            }
            var errors = results.Select(x => x.ErrorMessage).ToArray();
            _errors[property] = errors;
            ErrorsChanged(this, new DataErrorsChangedEventArgs(property));
        }

        public string this[string columnName]
        {
            get
            {
                return _errors.ContainsKey(columnName) ? string.Join(", ", _errors[columnName]) : null;
            }
        }

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>An error message indicating what is wrong with this object. The default
        /// is an empty string ("").</returns>
        /// <value></value>
        public string Error
        {
            get
            {
                return string.Join(Environment.NewLine, _errors.Values.SelectMany(x => x));
            }
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
            if(_errors.ContainsKey(propertyName))
                foreach(var error in _errors[propertyName])
                    yield return error;
            yield break;
        }

        /// <summary>
        /// Gets a value that indicates whether the entity has validation errors.
        /// </summary>
        /// <returns>true if the entity currently has validation errors; otherwise, false.
        /// </returns>
        /// <value></value>
        public bool HasErrors { get { return _errors.Values.Any(); } }

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for
        /// the entire entity.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = (o, e) => { };

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (o, e) => { };
    }
}

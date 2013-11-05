using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace TypeDescriptorsAndSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            TypeDescriptor.AddAttributes(typeof(Foo), new TypeDescriptionProviderAttribute(typeof(FooTypeDescriptor)));

            var foo = new Foo { Text = "lol" };

            var ser = new DataContractSerializer(typeof(Foo));
            var sb = new StringBuilder();
            var tw = XmlWriter.Create(sb, new XmlWriterSettings
            {
                Indent = true,
                CloseOutput = true,
                ConformanceLevel = System.Xml.ConformanceLevel.Fragment,
                OmitXmlDeclaration = true,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                NewLineOnAttributes = true,
                WriteEndDocumentOnClose = true,
            });
            ser.WriteObject(tw, foo);
            tw.Close();
            tw.Dispose();
            Console.WriteLine(sb.ToString());
            Console.ReadLine();
        }
    }

    public sealed class FooTypeDescriptor : TypeDescriptionProvider
    {
        public override ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
        {
            return new FooDescriptor(instance as Foo);
        }
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return new FooDescriptor(instance as Foo);
        }

    }

    [TypeDescriptionProvider(typeof(FooTypeDescriptor))]
    public sealed class Foo
    {
        public string Text { get; set; }
    }

    public sealed class FooDescriptor : CustomTypeDescriptor
    {
        private Foo _foo;

        public FooDescriptor(Foo foo)
        {
            this._foo = foo;
        }
        public override PropertyDescriptorCollection GetProperties()
        {
            return new PropertyDescriptorCollection(new PropertyDescriptor[]
            {
                new FooPropertyDescriptor<string>(_foo, "Text"),
                new FooPropertyDescriptor<string>(_foo, "Bar", "FUCK"),
            });
        }
    }

    public sealed class FooPropertyDescriptor<T> : PropertyDescriptor where T : class
    {
        private Foo _foo;
        private T _value;

        public FooPropertyDescriptor(Foo foo, string name, T value = null)
            : base(name, new Attribute[0])
        {
            // TODO: Complete member initialization
            this._foo = foo;
            _value = value;
        }
        /// <summary>
        /// When overridden in a derived class, returns whether resetting an object
        /// changes its value.
        /// </summary>
        /// <param name="component">The component to test for reset capability.</param>
        /// <returns>
        /// true if resetting the component changes its value; otherwise, false.
        /// </returns>
        public override bool CanResetValue(object component)
        {
            return false;
        }

        /// <summary>
        /// When overridden in a derived class, gets the current value of the property
        /// on a component.
        /// </summary>
        /// <param name="component">The component with the property for which to retrieve
        /// the value.</param>
        /// <returns>The value of a property for a given component.</returns>
        public override object GetValue(object component)
        {
            return _value == null ? GetRealProperty().GetValue(_foo) : _value;
        }

        private PropertyDescriptor GetRealProperty()
        {
            return TypeDescriptor.GetProperties(_foo, true).OfType<PropertyDescriptor>().First(x => x.Name == this.Name);
        }

        /// <summary>
        /// When overridden in a derived class, resets the value for this property
        /// of the component to the default value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be reset
        /// to the default value.</param>
        public override void ResetValue(object component)
        {

        }

        /// <summary>
        /// When overridden in a derived class, sets the value of the component
        /// to a different value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be set.</param>
        /// <param name="value">The new value.</param>
        public override void SetValue(object component, object value)
        {
            if(_value != null)
                _value = (T)value;
            else
                GetRealProperty().SetValue(component, value);
        }

        /// <summary>
        /// When overridden in a derived class, determines a value indicating whether
        /// the value of this property needs to be persisted.
        /// </summary>
        /// <param name="component">The component with the property to be examined for persistence.</param>
        /// <returns>true if the property should be persisted; otherwise, false.</returns>
        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the component this
        /// property is bound to.
        /// </summary>
        /// <returns>A <see cref="T:System.Type" /> that represents the type of component
        /// this property is bound to. When the <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)" />
        /// or <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)" />
        /// methods are invoked, the object specified might be an instance of this type.
        /// </returns>
        /// <value></value>
        public override Type ComponentType
        {
            get
            {
                return typeof(Foo);
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether
        /// this property is read-only.
        /// </summary>
        /// <returns>true if the property is read-only; otherwise, false.</returns>
        /// <value></value>
        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the property.
        /// </summary>
        /// <returns>A <see cref="T:System.Type" /> that represents the type of the property.
        /// </returns>
        /// <value></value>
        public override Type PropertyType
        {
            get
            {
                return typeof(T);
            }
        }
    }
}

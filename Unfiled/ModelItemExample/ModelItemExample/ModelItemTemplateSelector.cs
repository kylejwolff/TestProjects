using System;
using System.Activities.Presentation.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ModelItemExample
{
    public sealed class ModelItemTemplateSelector : DataTemplateSelector
    {
        public static readonly ModelItemTemplateSelector Instance = new ModelItemTemplateSelector();
        /// <summary>
        /// Last chance template to use when unable to locate a template and the <see cref="DefaultTemplate"/> is not set.
        /// </summary>
        public static readonly DataTemplate DefaultDataTemplate;

        /// <summary>
        /// Initializes the <see cref="TypeBasedDataTemplateSelector"/> class.
        /// </summary>
        static ModelItemTemplateSelector()
        {
            DefaultDataTemplate = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            var propertyPath = new Binding() { StringFormat = "Cannot find template for type {0}.  Optionally set the DataTemplate's x:Key property is set with the full name of this type." };
            factory.SetBinding(TextBlock.TextProperty, propertyPath);
            factory.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
            factory.SetValue(TextBlock.ForegroundProperty, Brushes.Red);
            DefaultDataTemplate.VisualTree = factory;
            DefaultDataTemplate.Seal();
        }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            if (item == null)
                return base.SelectTemplate(item, container);

            var mi = item as ModelItem;
            if (mi == null)
                return DefaultDataTemplate;

            FrameworkElement fe = null;
            // CP may be in a template or may be on its own.
            if (container is ContentPresenter)
                fe = ((container as ContentPresenter).TemplatedParent ??
                      (container as ContentPresenter).Parent) as FrameworkElement;
            else
                fe = container as FrameworkElement;

            if (fe == null)
                return null;
            var key = new System.Windows.DataTemplateKey(mi.ItemType);

            return fe.TryFindResource(key) as DataTemplate ??
                   fe.TryFindResource(mi.ItemType) as DataTemplate ??
                   fe.TryFindResource(mi.ItemType.FullName) as DataTemplate ??
                   DefaultDataTemplate;
        }
    }
}
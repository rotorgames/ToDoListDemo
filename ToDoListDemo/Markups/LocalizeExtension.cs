using System;
using System.Reflection;
using System.Resources;
using ToDoListDemo.LocalizableResources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoListDemo.Markups
{
    [ContentProperty(nameof(Text))]
    public class LocalizeExtension : IMarkupExtension
    {
        static readonly string ResourceId = typeof(AppResources).FullName;

        public string Text { get; set; }

        public IValueConverter Converter { get; set; }

        public object ConverterParameter { get; set; }

        public string StringFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the returned text result should be upper case.
        /// </summary>
        /// <value><c>true</c> if upper case; otherwise, <c>false</c>.</value>
        public bool UpperCase { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return string.Empty;

            var ci = AppResources.Culture;

            var resmgr = new ResourceManager(
                ResourceId,
                typeof(AppResources).GetTypeInfo().Assembly);

            object result = resmgr.GetString(Text, ci);

            if (result == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
                return Text;
#endif
            }

            if (Converter != null)
                result = Converter.Convert(result, typeof(string), ConverterParameter, ci);

            if (!string.IsNullOrEmpty(StringFormat))
                result = string.Format(StringFormat, result);

            if (UpperCase)
                result = result.ToString().ToUpper();

            return result;
        }
    }
}

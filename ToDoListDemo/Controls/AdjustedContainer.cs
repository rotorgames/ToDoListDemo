using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace ToDoListDemo.Controls
{
    public class AdjustedContainer : ContentView
    {
        public static readonly BindableProperty MinWidthProperty = BindableProperty.Create(nameof(MinWidth), typeof(AdjustedSize), typeof(AdjustedContainer), AdjustedSize.Auto);

        public AdjustedSize MinWidth
        {
            get { return (AdjustedSize)GetValue(MinWidthProperty); }
            set { SetValue(MinWidthProperty, value); }
        }

        public static readonly BindableProperty MaxWidthProperty = BindableProperty.Create(nameof(MaxWidth), typeof(AdjustedSize), typeof(AdjustedContainer), AdjustedSize.Auto);

        public AdjustedSize MaxWidth
        {
            get { return (AdjustedSize)GetValue(MaxWidthProperty); }
            set { SetValue(MaxWidthProperty, value); }
        }

        public static readonly BindableProperty MinHeightProperty = BindableProperty.Create(nameof(MinHeight), typeof(AdjustedSize), typeof(AdjustedContainer), AdjustedSize.Auto);

        public AdjustedSize MinHeight
        {
            get { return (AdjustedSize)GetValue(MinHeightProperty); }
            set { SetValue(MinHeightProperty, value); }
        }

        public static readonly BindableProperty MaxHeightProperty = BindableProperty.Create(nameof(MaxHeight), typeof(AdjustedSize), typeof(AdjustedContainer), AdjustedSize.Auto);

        public AdjustedSize MaxHeight
        {
            get { return (AdjustedSize)GetValue(MaxHeightProperty); }
            set { SetValue(MaxHeightProperty, value); }
        }

        public static readonly BindableProperty AdjustChildrenMeasuringProperty = BindableProperty.Create(nameof(AdjustChildrenMeasuring), typeof(bool), typeof(AdjustedContainer), true);

        public bool AdjustChildrenMeasuring
        {
            get { return (bool)GetValue(AdjustChildrenMeasuringProperty); }
            set { SetValue(AdjustChildrenMeasuringProperty, value); }
        }

        public AdjustedContainer()
        {
            CompressedLayout.SetIsHeadless(this, true);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(MinWidth):
                case nameof(MaxWidth):
                case nameof(MinHeight):
                case nameof(MaxHeight):
                case nameof(AdjustChildrenMeasuring):
                    InvalidateMeasure();
                    break;
            }
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            SizeRequest result;

            if(AdjustChildrenMeasuring)
                result = base.OnMeasure(
                    GetSize(MinWidth, MaxWidth, widthConstraint, widthConstraint),
                    GetSize(MinHeight, MaxHeight, heightConstraint, heightConstraint));
            else
                result = base.OnMeasure(widthConstraint, heightConstraint);

            var min = new Size(
                GetSize(MinWidth, MaxWidth, result.Minimum.Width, widthConstraint),
                GetSize(MinHeight, MaxHeight, result.Minimum.Height, heightConstraint));
            var request = new Size(
                GetSize(MinWidth, MaxWidth, result.Request.Width, widthConstraint),
                GetSize(MinHeight, MaxHeight, result.Request.Height, heightConstraint));

            return new SizeRequest(request, min);
        }

        double GetSize(AdjustedSize minSize, AdjustedSize maxSize, double value, double constrain)
        {
            var useConstrain = !double.IsInfinity(constrain);
            var result = value;

            if (minSize.IsRelative && useConstrain)
                result = Math.Max(value, constrain * minSize.Length);
            else if (!minSize.IsAuto && !minSize.IsRelative)
                result = Math.Max(value, minSize.Length);

            if (maxSize.IsRelative && useConstrain)
                result = Math.Min(result, constrain * maxSize.Length);
            else if (!maxSize.IsAuto && !maxSize.IsRelative)
                result = Math.Min(result, maxSize.Length);

            return result;
        }

        [TypeConverter(typeof(AdjustedSizeTypeConverter))]
        public struct AdjustedSize
        {
            bool _isLength;
            bool _isRelative;

            public static AdjustedSize Auto = new AdjustedSize();
            public float Length { get; }

            public bool IsAuto => !_isLength && !_isRelative;
            public bool IsRelative => _isRelative;

            public AdjustedSize(float length, bool isRelative = false)
            {
                if (length < 0)
                    throw new ArgumentException("should be a positive value", nameof(length));
                if (isRelative && length > 1)
                    throw new ArgumentException("relative length should be in [0, 1]", nameof(length));
                _isLength = !isRelative;
                _isRelative = isRelative;
                Length = length;
            }

            public static implicit operator AdjustedSize(float length)
            {
                return new AdjustedSize(length);
            }

            public static implicit operator AdjustedSize(double length)
            {
                return new AdjustedSize((float) length);
            }

            [Xamarin.Forms.Xaml.TypeConversion(typeof(AdjustedSize))]
            public class AdjustedSizeTypeConverter : TypeConverter
            {
                public override object ConvertFromInvariantString(string value)
                {
                    if (value != null)
                    {
                        if (value.Equals("auto", StringComparison.OrdinalIgnoreCase))
                            return Auto;
                        value = value.Trim();
                        if (value.EndsWith("%", StringComparison.OrdinalIgnoreCase) && float.TryParse(value.Substring(0, value.Length - 1), NumberStyles.Number, CultureInfo.InvariantCulture, out float relflex))
                            return new AdjustedSize(relflex / 100, isRelative: true);
                        if (float.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out float flex))
                            return new AdjustedSize(flex);
                    }
                    throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", value, typeof(AdjustedSize)));
                }
            }
        }
    }
}

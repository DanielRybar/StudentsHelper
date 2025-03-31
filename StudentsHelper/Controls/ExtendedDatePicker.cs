namespace StudentsHelper.Controls
{
    public class ExtendedDatePicker : DatePicker
    {
        public static BindableProperty UnderlineColorProperty = BindableProperty.Create(
            nameof(UnderlineColor), typeof(Color), typeof(ExtendedDatePicker), default(Color));
        public Color UnderlineColor
        {
            get => (Color)GetValue(UnderlineColorProperty);
            set => SetValue(UnderlineColorProperty, value);
        }
    }
}
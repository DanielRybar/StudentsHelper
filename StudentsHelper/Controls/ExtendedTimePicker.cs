namespace StudentsHelper.Controls
{
    public class ExtendedTimePicker : TimePicker
    {
        public static BindableProperty UnderlineColorProperty = BindableProperty.Create(
            nameof(UnderlineColor), typeof(Color), typeof(ExtendedTimePicker), default(Color));
        public Color UnderlineColor
        {
            get => (Color)GetValue(UnderlineColorProperty);
            set => SetValue(UnderlineColorProperty, value);
        }
    }
}
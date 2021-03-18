using Xamarin.Forms;

namespace IucMarket.Mobile.Components
{
    public class LocalizedTimePicker : TimePicker
	{
		public static readonly BindableProperty PositiveActionTextProperty = BindableProperty.Create(nameof(PositiveActionText), typeof(string), typeof(LocalizedDatePicker), "Update");

		public string PositiveActionText
		{
			get { return (string)GetValue(PositiveActionTextProperty); }
			set
			{
				SetValue(PositiveActionTextProperty, value);
			}
		}

		public static readonly BindableProperty NegativeActionTextProperty = BindableProperty.Create(nameof(NegativeActionText), typeof(string), typeof(LocalizedDatePicker), "Cancel");

		public string NegativeActionText
		{
			get { return (string)GetValue(NegativeActionTextProperty); }
			set
			{
				SetValue(NegativeActionTextProperty, value);
			}
		}
	}
}
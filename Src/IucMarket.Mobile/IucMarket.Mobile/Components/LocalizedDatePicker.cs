using System;
using Xamarin.Forms;

namespace IucMarket.Mobile.Components
{
	public class LocalizedDatePicker : DatePicker
	{
		public static readonly BindableProperty PositiveActionTextProperty = BindableProperty.Create(nameof(PositiveActionText), typeof(string), typeof(LocalizedDatePicker), "Set");

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
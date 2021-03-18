using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IucMarket.Mobile.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingBar : ContentView
    {
        public event EventHandler ItemTapped = delegate { };
        private List<Label> rates;
        private static int DefaultNumberOfRate = 5;
        private static int DefaultSpacing = 10;
        private static Color DefaultEmptyRateIconColor = Color.Black;
        private static Color DefaultFillRateIconColor = Color.Gold;
        public RatingBar()
        {
            InitializeComponent();
            rates = new List<Label>();

            numberOfRate = DefaultNumberOfRate;

            fillFontFamilyIcon = string.Empty;
            emptyRateIcon = string.Empty;
            fillRateIcon = string.Empty;

            emptyRateIconColor = DefaultEmptyRateIconColor;
            fillRateIconColor = DefaultFillRateIconColor;

            stkRattingbar.Spacing = DefaultSpacing;
        }

        // this event will fire when you click on icon(rate)
        private ICommand ItemTappedCommand
        {
            get
            {
                return new Command((Object obj) =>
                {
                    this.SelectedRateValue = (int)obj;
                    initRate(this);

                });
            }
        }

        #region Font size icon Property
        public static readonly BindableProperty FontSizeIconProperty = BindableProperty.Create
        (
            propertyName: nameof(FontSizeIcon),
            returnType: typeof(double),
            declaringType: typeof(RatingBar),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: FontSizeIconPropertyChanged
        );

        private double fontSizeIcon;
        public double FontSizeIcon
        {
            get { return (double)base.GetValue(FontSizeIconProperty); }
            set
            {
                fontSizeIcon = value;
                base.SetValue(FontSizeIconProperty, value);
            }
        }

        private static void FontSizeIconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            if (control != null)
            {
                control.FontSizeIcon = (double)newValue;
                initRate(control);
            }
        }
        #endregion

        #region   Font family icon Property
        public static readonly BindableProperty FillFontFamilyIconProperty = BindableProperty.Create
        (
            propertyName: nameof(FillFontFamilyIcon),
            returnType: typeof(string),
            declaringType: typeof(RatingBar),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: FillFontFamilyIconPropertyChanged
        );

        private string fillFontFamilyIcon;
        public string FillFontFamilyIcon
        {
            get { return (string)base.GetValue(FillFontFamilyIconProperty); }
            set 
            {
                fillFontFamilyIcon = value;
                base.SetValue(FillFontFamilyIconProperty, value); 
            }
        }

        private static void FillFontFamilyIconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            if (control != null)
            {
                control.FillFontFamilyIcon = (string)newValue;
                initRate(control);
            }

        }

        public static readonly BindableProperty EmptyFontFamilyIconProperty = BindableProperty.Create
        (
            propertyName: nameof(EmptyFontFamilyIcon),
            returnType: typeof(string),
            declaringType: typeof(RatingBar),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: EmptyFontFamilyIconPropertyChanged
        );

        private string emptyFontFamilyIcon;
        public string EmptyFontFamilyIcon
        {
            get { return (string)base.GetValue(EmptyFontFamilyIconProperty); }
            set
            {
                emptyFontFamilyIcon = value;
                base.SetValue(EmptyFontFamilyIconProperty, value);
            }
        }

        private static void EmptyFontFamilyIconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            if (control != null)
            {
                control.EmptyFontFamilyIcon = (string)newValue;
                initRate(control);
            }

        }
        #endregion

        #region Command binding property
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
         propertyName: nameof(Command),
         returnType: typeof(ICommand),
         declaringType: typeof(RatingBar)
         );

        public ICommand Command
        {
            get { return (ICommand)base.GetValue(CommandProperty); }
            set { base.SetValue(CommandProperty, value); }
        }

        //  this property is private becuase i don't wanna access it globally
        private static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
         propertyName: nameof(CommandParameter),
         returnType: typeof(object),
         declaringType: typeof(RatingBar),
         propertyChanged: CommandParameterPropertyChanged
         );
        private object CommandParameter
        {
            get { return base.GetValue(CommandParameterProperty); }
            set { base.SetValue(CommandParameterProperty, value); }
        }

        // on the change of command parameter replace empty rate image with fillrate image
        private static void CommandParameterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            if (control != null)
            {
                control.SelectedRateValue = (int)newValue;
                //fillRate(control.SelectedRateValue, control);
            }

        }

        #endregion

        #region EmptyRateIcon and FillrateIcon property
        public static readonly BindableProperty EmptyRateIconProperty = BindableProperty.Create(
            propertyName: nameof(EmptyRateIcon),
            returnType: typeof(string),
            declaringType: typeof(RatingBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: EmptyRateIconPropertyChanged
         );

        private string emptyRateIcon;
        public string EmptyRateIcon
        {
            get { return (string)base.GetValue(EmptyRateIconProperty); }
            set
            {
                emptyRateIcon = value;
                base.SetValue(EmptyRateIconProperty, value);
            }
        }

        private static void EmptyRateIconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            if (control != null)
            {
                control.EmptyRateIcon = (string)newValue;
                initRate(control);
            }

        }


        public static readonly BindableProperty FillRateIconProperty = BindableProperty.Create(
          propertyName: "FillRateIcon",
          returnType: typeof(string),
          declaringType: typeof(RatingBar),
          defaultValue: "",
          defaultBindingMode: BindingMode.TwoWay,
          propertyChanged: FillRateIconPropertyChanged
       );

        private string fillRateIcon;
        public string FillRateIcon
        {
            get { return (string)base.GetValue(FillRateIconProperty); }
            set
            {
                fillRateIcon = value;
                base.SetValue(FillRateIconProperty, value);
            }
        }

        private static void FillRateIconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            if (control != null)
            {
                control.FillRateIcon = (string)newValue;
                initRate(control);
            }

        }
        #endregion

        #region EmptyRateIconColor and FillRateIconColor property
        public static readonly BindableProperty EmptyRateIconColorProperty = BindableProperty.Create(
            propertyName: nameof(EmptyRateIconColor),
            returnType: typeof(Color),
            declaringType: typeof(RatingBar),
            defaultValue: DefaultEmptyRateIconColor,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: EmptyRateIconColorPropertyChanged
         );

        private Color emptyRateIconColor;
        public Color EmptyRateIconColor
        {
            get { return (Color)base.GetValue(EmptyRateIconColorProperty); }
            set
            {
                emptyRateIconColor = value;
                base.SetValue(EmptyRateIconProperty, value);
            }
        }

        private static void EmptyRateIconColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            if (control != null)
            {
                control.EmptyRateIconColor = (Color)newValue;
                initRate(control);
            }

        }


        public static readonly BindableProperty FillRateIconColorProperty = BindableProperty.Create(
          propertyName: nameof(FillRateIconColor),
          returnType: typeof(Color),
          declaringType: typeof(RatingBar),
          defaultValue: Color.Gold,
          defaultBindingMode: BindingMode.TwoWay,
          propertyChanged: FillRateIconColorPropertyChanged
       );

        private Color fillRateIconColor;
        public Color FillRateIconColor
        {
            get { return (Color)base.GetValue(FillRateIconColorProperty); }
            set
            {
                fillRateIconColor = value;
                base.SetValue(FillRateIconColorProperty, value);
            }
        }

        private static void FillRateIconColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            if (control != null)
            {
                control.FillRateIconColor = (Color)newValue;
                initRate(control);
            }

        }
        #endregion

        #region NumberOfRates property
        public static readonly BindableProperty NumberOfRatesProperty = BindableProperty.Create(
            propertyName: nameof(NumberOfRates),
            returnType: typeof(int),
            declaringType: typeof(RatingBar),
            defaultValue: DefaultNumberOfRate,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: NumberOfRatesPropertyChanged
         );

        private int numberOfRate;
        public int NumberOfRates
        {
            get { return (int)base.GetValue(NumberOfRatesProperty); }
            set 
            {
                numberOfRate = value;
                base.SetValue(NumberOfRatesProperty, value); 
            }
        }

        private static void NumberOfRatesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            if (control != null)
            {
                control.NumberOfRates = (int)newValue;
                foreach(var rate in control.rates)
                {
                    control.stkRattingbar.Children.Remove(rate);
                }
                control.rates = new List<Label>();
                initRate(control);
            }

        }
        #endregion

        #region Spacing property
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
            propertyName: nameof(Spacing),
            returnType: typeof(int),
            declaringType: typeof(RatingBar),
            defaultValue: DefaultSpacing,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: SpacingPropertyChanged
         );

        private int spacing;
        public int Spacing
        {
            get { return (int)base.GetValue(SpacingProperty); }
            set
            {
                spacing = value;
                base.SetValue(SpacingProperty, value);
            }
        }

        private static void SpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            if (control != null)
            {
                control.Spacing = (int)newValue;
                control.stkRattingbar.Spacing = control.Spacing;
                initRate(control);
            }

        }
        #endregion


        // this function will replace empty rate with fill rate
        private static void fillRate(int selectedValue, RatingBar obj)
        {
            for (int i = 0; i < obj.rates.Count; i++)
            {
                if(obj.fontSizeIcon > 0)
                    obj.rates[i].FontSize = obj.fontSizeIcon;
                if (selectedValue >= i + 1)
                {
                    obj.rates[i].FontFamily = obj.fillFontFamilyIcon;
                    if (selectedValue == i + 1)
                    {
                        obj.rates[i].Text = obj.rates[i].Text != obj.fillRateIcon ? obj.fillRateIcon : obj.emptyRateIcon;
                        obj.rates[i].TextColor = obj.rates[i].TextColor != obj.fillRateIconColor ? obj.fillRateIconColor : obj.emptyRateIconColor;
                    }
                    else
                    {
                        if (obj.rates[i].Text != obj.fillRateIcon)
                        {
                            obj.rates[i].Text = obj.fillRateIcon;
                            obj.rates[i].TextColor = obj.fillRateIconColor;
                        }

                    }
                }
                else
                {
                    obj.rates[i].FontFamily = obj.emptyFontFamilyIcon;
                    if (obj.rates[i].Text != obj.emptyRateIcon)
                    {
                        obj.rates[i].Text = obj.emptyRateIcon;
                        obj.rates[i].TextColor = obj.emptyRateIconColor;
                    }
                }
            }
        }

        private static void initRate(RatingBar obj)
        {
            if (obj.rates.Count == 0)
            {
                for (int i = 0; i < obj.numberOfRate; i++)
                {
                    var rate = new Label();
                    rate.GestureRecognizers.Add
                    (
                        new TapGestureRecognizer
                        {
                            Command = obj.ItemTappedCommand,
                            CommandParameter = i + 1
                        }
                    );
                    obj.rates.Add(rate);
                    obj.stkRattingbar.Children.Add(rate);
                }
            }
            fillRate(obj.SelectedRateValue, obj);
        }


        #region This will return the selected rate value and also you can set the default selected rate
        public static readonly BindableProperty SelectedRateValueProperty = BindableProperty.Create(
          propertyName: "SelectedRateValue",
          returnType: typeof(int),
          declaringType: typeof(RatingBar),
          defaultValue: 0,
          defaultBindingMode: BindingMode.TwoWay,
          propertyChanged: SelectedRateValuePropertyChanged
       );

        private static void SelectedRateValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            if (control != null)
            {
               control.SelectedRateValue = (int)newValue;
            }

        }
        public int SelectedRateValue
        {
            get { return (int)GetValue(SelectedRateValueProperty); }
            set
            {
                SetValue(SelectedRateValueProperty, value);
            }
        }
        #endregion

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            // Handle the pan

            double width = rates[0].Width;
            for (int i = 0; i < rates.Count; i++)
            {
                if (e.TotalX > i * width)
                {
                    fillRate(i + 1, this);
                    Command?.Execute(i + 1);
                }
            }
        }
    }
}
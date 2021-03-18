using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IucMarket.Mobile.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppTitleComponent : StackLayout
    {
        public AppTitleComponent()
        {
            InitializeComponent();
            appName.Text = AppName;
            title.Text = Title;
            subAppName.Text = SubAppName;
        }

        public static readonly BindableProperty AppNameProperty = BindableProperty.Create(nameof(AppName), typeof(string), typeof(AppTitleComponent), default(string), Xamarin.Forms.BindingMode.OneWay);
        public string AppName
        {
            get
            {
                return (string)GetValue(AppNameProperty);
            }

            set
            {
                SetValue(AppNameProperty, value);
            }
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(AppTitleComponent), default(string), Xamarin.Forms.BindingMode.OneWay);
        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }

            set
            {
                SetValue(TitleProperty, value);
            }
        }

        public static readonly BindableProperty SubAppNameProperty = BindableProperty.Create(nameof(SubAppName), typeof(string), typeof(AppTitleComponent), default(string), Xamarin.Forms.BindingMode.OneWay);
        public string SubAppName
        {
            get
            {
                return (string)GetValue(SubAppNameProperty);
            }

            set
            {
                SetValue(SubAppNameProperty, value);
            }
        }
        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == AppNameProperty.PropertyName)
            {
                appName.Text = AppName;
            }
            else if (propertyName == SubAppNameProperty.PropertyName)
            {
                subAppName.Text = SubAppName;
            }
            else if (propertyName == TitleProperty.PropertyName)
            {
                title.Text = Title;
            }
        }
    }
}
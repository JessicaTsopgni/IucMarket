using IucMarket.Mobile.Models;
using IucMarket.Mobile.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace IucMarket.Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        bool isAuthenticate;
        public bool IsAuthenticate
        {
            get { return isAuthenticate; }
            set { SetProperty(ref isAuthenticate, value); }
        }


        bool isNotAuthenticate;
        public bool IsNotAuthenticate
        {
            get { return isNotAuthenticate; }
            set { SetProperty(ref isNotAuthenticate, value); }
        }

        public BaseViewModel()
        {
            //IsAuthenticate = App.IsAuthenticate;
            //IsNotAuthenticate = !App.IsAuthenticate;
        }

        public void OnApparing()
        {
            IsAuthenticate = App.IsAuthenticate;
            IsNotAuthenticate = !App.IsAuthenticate;
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

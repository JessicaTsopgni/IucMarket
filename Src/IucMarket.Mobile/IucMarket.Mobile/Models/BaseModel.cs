using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace IucMarket.Mobile.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        private string id;
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public BaseModel()
        {
            
        }

        public BaseModel(string id)
        {
            Id = id;
        }


        public override bool Equals(object obj)
        {
            return obj is BaseModel model &&
                   (Id?.Equals(model.Id) ?? false);
        }

        public override int GetHashCode()
        {
            return 2108858624 + (Id?.GetHashCode() ?? 0);
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

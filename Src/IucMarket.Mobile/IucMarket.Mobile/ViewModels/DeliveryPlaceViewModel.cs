using Acr.UserDialogs;
using IucMarket.Common;
using IucMarket.Mobile.Common;
using IucMarket.Mobile.Models;
using IucMarket.Mobile.Services;
using IucMarket.Mobile.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IucMarket.Mobile.ViewModels
{
    public class DeliveryPlaceViewModel : BaseViewModel
    {
        public ObservableCollection<DeliveryPlaceModel> Places { get; }

       
        public DeliveryPlaceViewModel()
        {

            Places = new ObservableCollection<DeliveryPlaceModel>();
            foreach(DeliveryPlaceOptions options in Enum.GetValues(typeof(DeliveryPlaceOptions)))
            {
                Places.Add
                (
                    new DeliveryPlaceModel
                    (
                        ((int)options).ToString(), options.ToString().Replace("_", " ")
                    )
                );
            }

        }      
    }

}

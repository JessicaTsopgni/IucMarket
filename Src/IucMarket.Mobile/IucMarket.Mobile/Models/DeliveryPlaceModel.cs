using System;
using System.Collections.Generic;
using System.Text;

namespace IucMarket.Mobile.Models
{
    public class DeliveryPlaceModel :BaseModel
    {

        private string name;
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);
            }
        }

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                SetProperty(ref comment, value);
            }
        }

        public DeliveryPlaceModel()
        {

        }

        public DeliveryPlaceModel(string id, string name) : base(id)
        {
            Name = name;
        }

        public DeliveryPlaceModel(string id, string name, string comment) : base(id)
        {
            Name = name;
            Comment = comment;
        }
    }

}

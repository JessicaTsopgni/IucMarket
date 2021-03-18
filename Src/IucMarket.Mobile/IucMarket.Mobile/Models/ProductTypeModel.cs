using IucMarket.Mobile.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IucMarket.Mobile.Models
{
    public class CategoryModel:BaseModel
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
        
        public CategoryModel()
        {

        }

        public CategoryModel(string id, string name)
            :base(id)
        {
            Name = name;
        }

    }
}

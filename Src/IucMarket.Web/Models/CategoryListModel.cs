using IucMarket.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IucMarket.Web.Models
{
    public class CategoryListModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public CategoryListModel()
        {

        }

        public CategoryListModel(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

using ExpressiveAnnotations.Attributes;
using IucMarket.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IucMarket.Web.Models
{
    public class CategoryCreateModel
    {
        public string Id { get; set; }


        [Required]
        [Display(Name = "Name")]
        [StringLength(100)]
        public string Name { get; set; }

        public CategoryCreateModel()
        {
        }

        public CategoryCreateModel(string id, string name):this()
        {
            Id = id;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is CategoryCreateModel model &&
                   Id == model.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}

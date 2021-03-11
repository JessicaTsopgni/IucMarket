using ExpressiveAnnotations.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IucMarket.Web.Models
{
    public class ProductCreateModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "{0} length is {1}")]
        [RegularExpression(@"^[a-zA-Z0-9]\\S+$", ErrorMessage = "{0} is alphanumeric")]
        public string Reference { get; set; }

        [Required]
        [Display(Name = "Full name")]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double? Price { get; set; }

        [Required]
        public string Currency { get; set; }

        public UserCreateModel Owner { get; set; }
        public IEnumerable<UserCreateModel> Owners { get; set; }
        public bool Status { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<string> PictureNames { get; set; }

        public ProductCreateModel()
        {
            Status = true;
        }

        public ProductCreateModel(string id, string reference, string name, 
            string description, double? price, IEnumerable<string> pictureNames, UserCreateModel owner,
            IEnumerable<UserCreateModel> owners, bool status)
        {
            Id = id;
            Reference = reference;
            Name = name;
            Description = description;
            Price = price;
            PictureNames = pictureNames;
            Owner = owner;
            Owners = owners;
            Status = status;
        }
    }
}

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
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "{0} is alphanumeric")]
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

        [Required]
        [Display(Name = "Product owner")]
        public string OwnerId { get; set; }
        public IEnumerable<UserCreateModel> Owners { get; set; }
        public bool Status { get; set; }

        [RequiredIf("Id==null", ErrorMessage = "You must add an image")]
        [MinLength(1)]
        public List<FileInfoModel> Pictures { get; set; }

        public ProductCreateModel()
        {
            Status = true;
            Currency = "FCFA";
            Pictures = new List<FileInfoModel>();
        }

        public ProductCreateModel(IEnumerable<UserCreateModel> owners):this()
        {
            Owners = owners;
        }

        public ProductCreateModel(string id, string reference, string name, 
            string description, double? price, string currency, List<FileInfoModel> pictures, string ownerId,
            IEnumerable<UserCreateModel> owners, bool status)
        {
            Id = id;
            Reference = reference;
            Name = name;
            Description = description;
            Price = price;
            Currency = currency;
            Pictures = pictures;
            OwnerId = ownerId;
            Owners = owners;
            Status = status;
        }
    }


    public class FileInfoModel
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
        public string ContentType { get; set; }

        public FileInfoModel()
        {

        }

        public FileInfoModel(string path, string name, double size, string contentType)
        {
            Path = path;
            Name = name;
            Size = size;
            ContentType = contentType;
        }
    }

}

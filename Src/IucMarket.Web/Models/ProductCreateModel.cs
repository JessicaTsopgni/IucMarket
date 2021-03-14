using ExpressiveAnnotations.Attributes;
using Microsoft.AspNetCore.Components.Forms;
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
        public UserCreateModel Owner { set => Owners?.FirstOrDefault(x => x.Id == OwnerId); }
        public IEnumerable<UserCreateModel> Owners { get; set; }
        public bool Status { get; set; }

        [RequiredIf("Id==null", ErrorMessage = "You must add an image")]
        [MinLength(1, ErrorMessage = "You must add an image")]
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
        public IBrowserFile File { get; private set; }
        public string Path { get; private set; }
        public string FileName { get; private set; }
        public string ContentType { get; private set; }

        public FileInfoModel()
        {
            
        }
        public FileInfoModel(string path, string fileName, string contentType)
        {
            Path = path;
            FileName = fileName;
            ContentType = contentType;
        }

        public FileInfoModel(IBrowserFile file, string base64Content)
        {
            File = file;
            FileName = file.Name;
            Path = "data:" + file.ContentType + ";base64," + base64Content;
            ContentType = file.ContentType;
        }

    }

}

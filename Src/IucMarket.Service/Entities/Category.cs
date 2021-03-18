
using IucMarket.Common;

namespace IucMarket.Service.Entities
{
    internal class Category
    {

        public string Name { get; set; }

        public Category()
        {

        }

        public Category(string name)
        {
            Name = name.ToUcFirst();
        }
    }

}
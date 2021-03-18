
namespace IucMarket.Dtos
{
    public class CategoryAddCommand
    {
        public string Name { get; set; }

        public CategoryAddCommand()
        {

        }

        public CategoryAddCommand(string name)
        {
            Name = name;
        }
    }
}

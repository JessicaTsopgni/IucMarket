namespace IucMarket.Dtos
{
    public class CategoryDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public CategoryDto()
        {

        }

        public CategoryDto(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }

}
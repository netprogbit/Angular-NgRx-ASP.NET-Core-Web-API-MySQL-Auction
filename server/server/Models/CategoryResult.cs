namespace Server.Models
{
    public class CategoryResult
    {
        public long Id { get; set; }        
        public string Name { get; set; }
        public string ImageFileName { get; set; }

        public CategoryResult(long id, string name, string imageFileName)
        {
            Id = id;
            Name = name;
            ImageFileName = imageFileName;
        }
    }
}

namespace IdeaSoft.Test.Desktop.UI.Models
{
    public class SearchPersonDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        
        public override string ToString()
        {
            return $"{Name} {LastName}";
        }
    }
}

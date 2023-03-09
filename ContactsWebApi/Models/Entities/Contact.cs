namespace ContactsWebApi.Models.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public ICollection<ContactInformation>  ContactInformations { get; set; }
    }
}

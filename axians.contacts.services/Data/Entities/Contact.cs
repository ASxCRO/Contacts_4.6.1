namespace axians.contacts.services.Models
{
    public class Contact : BaseEntity<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
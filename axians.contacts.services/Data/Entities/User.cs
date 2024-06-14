using axians.contacts.services.Models;

namespace axians.contacts.services.Data.Entities
{
    public class User : BaseEntity<int>
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
    }
}

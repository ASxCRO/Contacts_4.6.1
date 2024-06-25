using System.Collections.Generic;

namespace axians.contacts.services.Models
{
    public class ContactGridViewModel : GridPager
    {
        public IEnumerable<Contact> Contacts { get; set; }
    }
}
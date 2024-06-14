using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace axians.contacts.services.Models
{
    public class ContactGridViewModel : GridPager
    {
        public IEnumerable<Contact> Contacts { get; set; }
    }
}
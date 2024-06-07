using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    public class ContactGridViewModel : GridPager
    {
        public IEnumerable<Contact> Contacts { get; set; }
    }
}
using axians.contacts.services.Services.Abstraction;
using Microsoft.AspNet.Identity;
using System;
using System.Web;

namespace api.Utils
{
    public class UserContext : IUserContext
    {
        public string UserName => HttpContext.Current.User.Identity.Name;
        public int UserId => Convert.ToInt32(HttpContext.Current.User.Identity.GetUserId());
    }
}
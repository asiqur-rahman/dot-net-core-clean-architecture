using Microsoft.AspNetCore.Mvc;
using Project.Core.Entities.Common.Security;
using System.Text.Json;

namespace Project.App.Controllers
{
    public class BaseController : Controller
    {

        protected UserPrincipal? UserSessionData => JsonSerializer.Deserialize<UserPrincipal>(HttpContext.Session.GetString("UserSessionData"));

        public Dictionary<string, string> ConvertClassToDictionary(object obj)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            // Get the properties of the class
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                string propertyName = property.Name;
                string propertyValue = Convert.ToString(property.GetValue(obj));

                dictionary.Add(propertyName, propertyValue);
            }

            return dictionary;
        }


    }
}

using Microsoft.AspNetCore.Mvc;

namespace Project.Web.Controllers
{
    public abstract class BaseController : Controller
    {
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

using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.Razor;
using Project.Core.Entities.Common.Security;
using System.Text.Json;

namespace Project.App.ViewComponents
{
    public abstract class BaseViewPage<TModel> : RazorPage<TModel>
    {
        [RazorInjectAttribute]
        protected UserPrincipal? UserSessionData => JsonSerializer.Deserialize<UserPrincipal>(Context.Session.GetString("UserSessionData"));

    }
}

using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClienteRegalosDulces.Helpers
{
    public static class HtmlHelpers
    {
        public static string IsSelected(this IHtmlHelper htmlHelper, string action = "", string controller = "")
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            var currentAction = routeData.Values["action"]?.ToString();
            var currentController = routeData.Values["controller"]?.ToString();

            bool isActive = (!string.IsNullOrEmpty(action) && currentAction == action) &&
                            (!string.IsNullOrEmpty(controller) && currentController == controller);

            return isActive ? "active" : "";
        }

        public static string IsParentActive(this IHtmlHelper htmlHelper, string[] controller, params string[] actions)
        {
            var routeData = htmlHelper.ViewContext.RouteData;
            var currentAction = routeData.Values["action"]?.ToString();
            var currentController = routeData.Values["controller"]?.ToString();

            if (currentAction == "History" && controller[0] == "Client")
                return "active";

            bool isParentActive = controller.Contains(currentController) && actions.Contains(currentAction);

            return isParentActive ? "active" : "";
        }

        public static string AntiForgeryTokenForAjaxPost(this IHtmlHelper helper)
        {
            //https://stackoverflow.com/questions/6255344/how-can-i-use-jquery-to-post-json-data
            var antiForgeryInputTag = helper.AntiForgeryToken().ToString();
            var removedStart = antiForgeryInputTag.Replace(@"<input name=""__RequestVerificationToken"" type=""hidden"" value=""", "");
            var tokenValue = removedStart.Replace(@""" />", "");
            if (antiForgeryInputTag == removedStart || removedStart == tokenValue)
                throw new InvalidOperationException("Oops! The Html.AntiForgeryToken() method seems to return something I did not expect.");
            return new string(string.Format(@"{0}:""{1}""", "__RequestVerificationToken", tokenValue));
        }
    }
}
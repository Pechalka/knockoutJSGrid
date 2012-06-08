using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace KnockoutJSGrid
{
    public static class JSONHelpers
    {
        public static MvcHtmlString ToJSON(this HtmlHelper htmlHelper, object data)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return new MvcHtmlString(serializer.Serialize(data));
        }
    }
}
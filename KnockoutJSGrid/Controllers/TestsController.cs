using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace KnockoutJSGrid.Controllers
{
    public class TestsController : Controller
    {
        //
        // GET: /Tests/

        public ActionResult CustomeBinding()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Form()
        {
            var form = new TestViewModel
                           {
                               Id = Guid.NewGuid(),
                               Date = DateTime.Now,
                               Age = 12,
                               Text = "text"
                           };
            return View(form);
        }

        [HttpPost]
        public ActionResult Form(TestViewModel form)
        {
            if (!ModelState.IsValid)
                return View(form);

            if (form.Name == "v")
            {
                ModelState.AddModelError(string.Empty, "Имя v занито");

                return View(form);
            }
            return Redirect("Form");
        }
    }
}

public class TestViewModel
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Text { get; set; }
    [Required]
    public string Name { get; set; }

    [Range(18, Int32.MaxValue, ErrorMessage = "Пользователь должен быть совершеннолетним")]
    public int Age { get; set; }
}



public static class LabelExtensions
{
    public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
    {
        return html.LabelFor(expression, null, htmlAttributes);
    }

    public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes)
    {
        return html.LabelHelper(
            ModelMetadata.FromLambdaExpression(expression, html.ViewData),
            ExpressionHelper.GetExpressionText(expression),
            HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes),
            labelText);
    }

    private static MvcHtmlString LabelHelper(this HtmlHelper html, ModelMetadata metadata, string htmlFieldName, IDictionary<string, object> htmlAttributes, string labelText = null)
    {
        var str = labelText
            ?? (metadata.DisplayName
            ?? (metadata.PropertyName
            ?? htmlFieldName.Split(new[] { '.' }).Last()));

        if (string.IsNullOrEmpty(str))
            return MvcHtmlString.Empty;

        var tagBuilder = new TagBuilder("label");
        tagBuilder.MergeAttributes(htmlAttributes);
        tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
        tagBuilder.SetInnerText(str);

        return tagBuilder.ToMvcHtmlString(TagRenderMode.Normal);
    }

    private static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder, TagRenderMode renderMode)
    {
        return new MvcHtmlString(tagBuilder.ToString(renderMode));
    }

}


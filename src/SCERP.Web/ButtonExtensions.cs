using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SCERP.Web
{
    public static class ButtonExtensions
    {


        public static MvcHtmlString ReportViewer(this HtmlHelper helper, string src)
        {

            var builder = new TagBuilder("iframe");
            builder.MergeAttribute("src", src);
            builder.MergeAttribute("height", "620");
            builder.MergeAttribute("width", "1250");
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));

        }
        public static MvcHtmlString Button(this HtmlHelper html, object htmlAttributes)
        {
            return Button(html, "", "submit", new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Generate save button
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString SaveButton(this HtmlHelper html)
        {
            return SaveButton(html, new Dictionary<string, object>());
        }

        /// <summary>
        /// Generate save button
        /// </summary>
        /// <param name="html"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString SaveButton(this HtmlHelper html, object htmlAttributes)
        {
            return SaveButton(html, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Generate save button
        /// </summary>
        /// <param name="html"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString SaveButton(this HtmlHelper html, IDictionary<string, object> htmlAttributes)
        {
            if (!htmlAttributes.ContainsKey("title"))
            {
                htmlAttributes.Add("title", "Save");
            }
            if (!htmlAttributes.ContainsKey("class"))
            {
                htmlAttributes.Add("class", "save addButton");

            }
            return Button(html, "Save", "submit", htmlAttributes);
        }


        /// <summary>
        /// Generate delete button
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString DeleteButton(this HtmlHelper html)
        {
            return DeleteButton(html, new Dictionary<string, object>());
        }

        /// <summary>
        /// Generate delete button
        /// </summary>
        /// <param name="html"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString DeleteButton(this HtmlHelper html, object htmlAttributes)
        {
            return DeleteButton(html, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Generate delete button
        /// </summary>
        /// <param name="html"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString DeleteButton(this HtmlHelper html, IDictionary<string, object> htmlAttributes)
        {
            if (!htmlAttributes.ContainsKey("title"))
            {
                htmlAttributes.Add("title", "Delete");
            }
            if (!htmlAttributes.ContainsKey("class"))
            {
                htmlAttributes.Add("class", "delete");
            }
            return Button(html, "", "submit", htmlAttributes);
        }

        /// <summary>
        /// Generate edit button
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString EditButton(this HtmlHelper html)
        {
            return EditButton(html, new Dictionary<string, object>());
        }

        /// <summary>
        /// Generate edit button
        /// </summary>
        /// <param name="html"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString EditButton(this HtmlHelper html, object htmlAttributes)
        {
            return EditButton(html, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Generate edit button
        /// </summary>
        /// <param name="html"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString EditButton(this HtmlHelper html, IDictionary<string, object> htmlAttributes)
        {
            if (!htmlAttributes.ContainsKey("title"))
            {
                htmlAttributes.Add("title", "Edit");
            }
            if (!htmlAttributes.ContainsKey("class"))
            {
                htmlAttributes.Add("class", "edit");
            }
            return Button(html, "", "submit", htmlAttributes);
        }

        /// <summary>
        /// Generate search button
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static MvcHtmlString SearchButton(this HtmlHelper html)
        {
            return SearchButton(html, new Dictionary<string, object>());
        }

        /// <summary>
        /// Generate search button
        /// </summary>
        /// <param name="html"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString SearchButton(this HtmlHelper html, object htmlAttributes)
        {
            return SearchButton(html, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Generate search button
        /// </summary>
        /// <param name="html"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString SearchButton(this HtmlHelper html, IDictionary<string, object> htmlAttributes)
        {
            if (!htmlAttributes.ContainsKey("title"))
            {
                htmlAttributes.Add("title", "Search");
            }
            if (!htmlAttributes.ContainsKey("class"))
            {
                htmlAttributes.Add("class", "search");
            }
            return Button(html, "", "submit", htmlAttributes);
        }
        public static MvcHtmlString Button(this HtmlHelper html, string text, string type, IDictionary<string, object> htmlAttributes)
        {
            var button = new TagBuilder("button");

            button.MergeAttribute("type", type);

            button.MergeAttributes(htmlAttributes);

            button.InnerHtml = text;

            return new MvcHtmlString(button.ToString());
        }
    }
}
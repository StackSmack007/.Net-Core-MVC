namespace ChushkaApp.Views.TagHelpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;
    [HtmlTargetElement("h1")]
    public class H1TagHelper : TagHelper
    {
        public string AspColor { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AspColor != string.Empty)
            {
                output.PostContent.AppendHtml("<p>To be Or not to be!</p>");
                output.Attributes.SetAttribute("style", "color:" + AspColor);
            }
        }
    }
}
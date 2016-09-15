using System.Xml.Linq;

namespace Skybrud.Umbraco.Extensions {
    
    internal static class XElementExtensions {

        public static string GetAttributeValue(this XElement element, XName name) {
            XAttribute attr = element == null ? null : element.Attribute(name);
            return attr == null ? null : attr.Value;
        }

    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace Klepto
{
    public class HtmlSchemaHelper<T>
    {
        private string _HTMLResult;
        private HtmlDocument _HTMLDocument;

        public void Load(string url)
        {
            var a = new WebClient();
            a.Encoding = System.Text.Encoding.UTF8;
            _HTMLResult = a.DownloadString(url);

            _HTMLDocument = new HtmlDocument();
            _HTMLDocument.LoadHtml(_HTMLResult);
        }

        public bool Fetch(T instance, string selector, Expression<Func<T, object>> func)
        {
            var node = _HTMLDocument.DocumentNode.QuerySelector(selector);
            if (node != null) {
                var value = node.InnerText;
                value = value.Trim();
                SetPropertyValue(instance, func, value);
                return true;
            }
            return false;
        }

        public bool Fetch(T instance, string selector, Action<T, string> func)
        {
            var node = _HTMLDocument.DocumentNode.QuerySelector(selector);
            if (node != null) {
                var value = node.InnerText;
                value = value.Trim();
                try {
                    func(instance, value);
                }
                catch (Exception) {
                    return false;
                }
                return true;
            }

            return false;
        }

        public static void SetPropertyValue<TThing>(TThing target, Expression<Func<TThing, object>> memberLamda, object value)
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            if (memberSelectorExpression != null) {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null) {
                    property.SetValue(target, value, null);
                }
            }
        }
    }
}

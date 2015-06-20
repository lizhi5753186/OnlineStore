using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace OnlineStore.Web
{
    public class MvcSiteMap
    {
        private static readonly MvcSiteMap _instance = new MvcSiteMap();

        private static readonly XDocument Doc = XDocument.Load(HttpContext.Current.Server.MapPath(@"~/SiteMap.xml"));

        private UrlHelper _url = null;

        private string _currentUrl;

        public static MvcSiteMap Instance 
        {
            get { return _instance;}
        }

        private MvcSiteMap()
        {
        }

        public MvcHtmlString Navigator()
        {
            // 获得当前请求的路由信息
            _url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var routeUrl = _url.RouteUrl(HttpContext.Current.Request.RequestContext.RouteData.Values);
            if (routeUrl != null)
                _currentUrl = routeUrl.ToLower(); 

            // 从配置的站点Xml文件中找到当前请求的Url相同的节点
            var c = FindNode(Doc.Root);
            var temp = GetPath(c);

            return MvcHtmlString.Create(BuildPathString(temp));
        }

        // 从SitMap配置文件中找到当前请求匹配的节点
        private XElement FindNode(XElement node)
        {
            // 如果xml节点对应的url是否与当前请求的节点相同，如果相同则直接返回xml对应的节点
            // 如果不同开始递归子节点
            return IsUrlEqual(node) == true ? node : RecursiveNode(node);
        }

        // 判断xml节点对应的url是否与当前请求的url一样
        private bool IsUrlEqual(XElement c)
        {
            var a = GetNodeUrl(c).ToLower();
            return a == _currentUrl;
        }

        // 递归Xml节点
        private XElement RecursiveNode(XElement node)
        {
            foreach (var c in node.Elements())
            {
                if (IsUrlEqual(c) == true)
                {
                    return c;
                }
                else
                {
                    var x = RecursiveNode(c);
                    if (x != null)
                    {
                        return x;
                    }
                }
            }

            return null;
        }

        // 获得xml节点对应的请求url
        private string GetNodeUrl(XElement c)
        {
            return _url.Action(c.Attribute("action").Value, c.Attribute("controller").Value,
                new {area = c.Attribute("area").Value});
        }

        // 根据对应请求url对应的Xml节点获得其在Xml中的路径，即获得其父节点有什么
        // SiteMap.xml 中节点的父子节点一定要配置对
        private Stack<XElement> GetPath(XElement c)
        {
            var temp = new Stack<XElement>();
            while (c != null)
            {
                temp.Push(c);
                c = c.Parent;
            }
            return temp;
        }

        // 根据节点的路径来拼接带标签的字符串
        private string BuildPathString(Stack<XElement> m)
        {
            var sb = new StringBuilder();
            var tc = new TagBuilder("span");
            tc.SetInnerText(">");
            var sp = tc.ToString();
            var count = m.Count;
            for (var x = 1; x <= count; x++)
            {
                var c = m.Pop();
                TagBuilder tb;
                if (x == count)
                {
                    tb = new TagBuilder("span");
                }
                else
                {
                    tb = new TagBuilder("a");
                    tb.MergeAttribute("href", GetNodeUrl(c));
                }

                tb.SetInnerText(c.Attribute("title").Value);
                sb.Append(tb);
                sb.Append(sp);
            }

            return sb.ToString();
        }
    }
}
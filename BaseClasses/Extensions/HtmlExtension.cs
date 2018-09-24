using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace BaseClasses.Extensions
{
	public static class HtmlExtension
	{
		
//	<ul class="pagination">
//		<li class="paginate_button first disabled"><a href = "#" data-index="1">|<</a></li>
//	<li class="paginate_button previous disabled"><a href = "#" data-index="1"><</a></li>
//	<li class="paginate_button active"><a href = "#" data-index="2">1</a></li>
//	<li class="paginate_button "><a href = "#" data-index="3">2</a></li>
//	<li class="paginate_button "><a href = "#" data-index="4">3</a></li>
//	<li class="paginate_button next"><a href = "#" data-index="6">></a></li>
//	<li class="paginate_button last"><a href = "#" data-index="7">>|</a></li>
//	</ul>
		public static MvcHtmlString PageRender(this HtmlHelper html, int? pageIndex,int? pageSize,int total)
		{
			pageIndex = pageIndex ?? 1;
			pageSize = pageSize ?? 10;
			var totalPage = (total - 1) / (pageSize??10) + 1;
			if (totalPage <= 1)
			{
				return new MvcHtmlString("");
			}
			var htmlStr = new StringBuilder();
			htmlStr.Append("<ul class='pagination'>");
			var first = $"<li class='paginate_button first {(pageIndex <= 1 ? "disabled" : "")}'><a href = '{GetIndexPath(1, pageIndex, totalPage)}'>|<</a></li>";
			htmlStr.Append(first);
			var pre = $"<li class='paginate_button previous {(pageIndex <= 1 ? "disabled" : "")}''><a href = '{GetIndexPath(pageIndex-1, pageIndex, totalPage)}'><</a></li>";
			htmlStr.Append(pre);
			if (pageIndex > 1)
			{
				var preN = $"<li class='paginate_button previous {(pageIndex <= 1 ? "disabled" : "")}''><a href = '{GetIndexPath(pageIndex - 1, pageIndex, totalPage)}'>{pageIndex - 1}</a></li>";
				htmlStr.Append(preN);
			}
			var current = $"<li class='paginate_button active''><a href = '#'>{pageIndex}</a></li>";
			htmlStr.Append(current);
			if (pageIndex < totalPage)
			{
				var nextN = $"<li class='paginate_button '><a href = '{GetIndexPath(pageIndex + 1, pageIndex, totalPage)}'>{pageIndex+1}</a></li>";
				htmlStr.Append(nextN);
			}
			var next = $"<li class='paginate_button next {(pageIndex >= totalPage ? "disabled" : "")}''><a href = '{GetIndexPath(pageIndex + 1, pageIndex, totalPage)}'>></a></li>";
			htmlStr.Append(next);
			var last = $"<li class='paginate_button last {(pageIndex >= totalPage ? "disabled" : "")}'><a href = '{GetIndexPath(totalPage, pageIndex, totalPage)}' >>|</a></li>";
			htmlStr.Append(last);
			htmlStr.Append("</ul>");
			return new MvcHtmlString(htmlStr.ToString());
		}

		private static string GetIndexPath(int? pageIndex,int? currentPageIndex,int totalpage)
		{
			if (pageIndex < 1 || pageIndex > totalpage|| pageIndex== currentPageIndex)
			{
				return "javascript:void(0);";
			}
			var ps = HttpContext.Current.Request.Url.ToString();
			var paramList = new List<KeyValuePair<string, string>>();
			if (ps.Split('?').Length > 1)
			{
				var paramStr = ps.Split('?')[1];
				var paramItems = paramStr.Split('&');
				foreach (var paramItem in paramItems)
				{
					var paramItemKey = paramItem.Split('=')[0];
					if (paramItemKey.ToLower() != "pageindex" && paramItem.Split('=').Length > 1)
					{
						paramList.Add(new KeyValuePair<string, string>(paramItemKey, paramItem.Split('=')[1]));
					}
				}
			}
			paramList.Add(new KeyValuePair<string, string>("pageIndex", $"{pageIndex}"));
			return "?"+string.Join("&", paramList.Select(e => $"{e.Key}={e.Value}"));
		}

		public static MvcHtmlString VersionedJs(this HtmlHelper helper, string filename)
		{
			string version = GetVersion(helper, filename);
			return MvcHtmlString.Create("<script type=\"text/javascript\" src=\"" + filename + version + "\"></script>");
		}

		public static MvcHtmlString VersionedCss(this HtmlHelper helper, string filename)
		{
			string version = GetVersion(helper, filename);
			return MvcHtmlString.Create("<link rel=\"stylesheet\" href=\"" + filename + version + "\"></script>");
		}
		private static string GetVersion(this HtmlHelper helper, string filename)
		{
			var context = helper.ViewContext.RequestContext.HttpContext;

			if (context.Cache[filename] == null)
			{
				var physicalPath = context.Server.MapPath(filename);
				var version = "?v=" +
							  new System.IO.FileInfo(physicalPath).LastWriteTime
								  .ToString("yyyyMMddHHmmss");
				context.Cache.Add(physicalPath, version, null,
					DateTime.Now.AddMinutes(10), TimeSpan.Zero,
					CacheItemPriority.Normal, null);
				context.Cache[filename] = version;
				return version;
			}
			else
			{
				return context.Cache[filename] as string;
			}
		}
	}

}

using System;
using System.Xml.Linq;
namespace TEDRss
{
	public class TEDEntry
	{
		public string Title{get;private set;}
		public string Description{get;private set;}
		public string ImageUrl{get;private set;}
		public string VideoUrl{ get;private set;}
		public DateTime Date{ get;private  set;}
		public TimeSpan Duration{ get;private set;}

		private string GetElementValue(XElement elem,XName name)
		{
			if (elem == null || elem.Element (name) == null)
			{
				return String.Empty;
			}
			return elem.Element (name).Value;
		}

		private string GetElementAttribute(XElement elem,XName name,string attributeName)
		{
			if (elem == null || elem.Element (name) == null || elem.Element (name).Attribute (attributeName) == null)
			{
				return string.Empty;
			}
			return elem.Element (name).Attribute (attributeName).Value;
		}

		public TEDEntry(XElement entry)
		{
			Title = GetElementValue (entry, "title");
			Description = GetElementValue (entry,XName.Get("summary","http://www.itunes.com/dtds/podcast-1.0.dtd") );
			Date = DateTime.Parse (GetElementValue (entry, "pubDate"));
			Duration = TimeSpan.Parse (GetElementValue(entry,XName.Get("duration","http://www.itunes.com/dtds/podcast-1.0.dtd")));
			ImageUrl = GetElementAttribute (entry, XName.Get ("thumbnail", "http://search.yahoo.com/mrss/"),"url");
			VideoUrl = GetElementAttribute (entry, "enclosure", "url");

		}
	}
}


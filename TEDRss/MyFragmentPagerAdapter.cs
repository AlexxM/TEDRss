using System;
using Android.Support.V4.App;
using System.Collections.Generic;
namespace TEDRss
{
	public class MyFragmentPagerAdapter : FragmentPagerAdapter
	{
		List<TEDEntry> _listEntries;
		public MyFragmentPagerAdapter (FragmentManager fm,List<TEDEntry> lst) : base(fm)
		{
			_listEntries = lst;
		}

		public override Fragment GetItem (int position)
		{
			Fragment f = new RssEntryFragment (position,_listEntries[position]);
			return f;
		}

		public override int Count {
			get {
				return _listEntries.Count;
			}
		}
	}
}


using System;
using System.Xml.Linq;
using System.Collections.Generic;
using Android.Net;
using Android.App;
using System.Threading.Tasks;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.App;
namespace TEDRss
{
	public class RssFeedReader
	{

		private string _url;
		private MainActivity _ma;
		private XDocument _rssFeed;
		private List<TEDEntry> _list;

		public RssFeedReader (string url,MainActivity ma)
		{
			_url = url;
			_ma = ma;
		}
			
		public void FillEntriesCollection()
		{

			Task task = new Task (() => {


				_ma.ShowProgressDialog(_ma.GetString(Resource.String.getting_info));
				if(!CheckInternetConnection())
					throw new InvalidOperationException(_ma.GetString(Resource.String.error_internet_connection));
				_rssFeed = XDocument.Load(_url);
				ParseRss();
			});

			task.ContinueWith ((t) => {

				_ma.CloseProgressDialog();
				if(t.Exception!=null)
				{
					//нет соединения, информация не получена -> выход

					foreach(Exception ex in t.Exception.Flatten().InnerExceptions)
					{
						if(ex is InvalidOperationException)
						{
							_ma.ShowAlertDialog(_ma.GetString(Resource.String.error_internet_connection),(e,i)=>_ma.Finish());
							return;
						}

					}

					_ma.ShowAlertDialog(_ma.GetString(Resource.String.error_getting_info), (e, i) => _ma.Finish ());
				}
				else
				{
					_ma.SetPagerAdapter(_list);

				}
			});

			task.Start ();



		
		}

		private void ParseRss()
		{
			if (_rssFeed == null)
				return;

			_list = new List<TEDEntry> ();
			foreach(XElement element in _rssFeed.Descendants("item"))
			{
				TEDEntry entry = new TEDEntry (element);
				_list.Add (entry);

			}
		}

		private bool CheckInternetConnection()
		{

			ConnectivityManager cm = (ConnectivityManager)_ma.GetSystemService (Service.ConnectivityService);
			foreach(NetworkInfo ni in cm.GetAllNetworkInfo())
			{
				if (ni.GetState() == NetworkInfo.State.Connected)
				{
					return true;
				}

			}
			return false;

		}

	}




}


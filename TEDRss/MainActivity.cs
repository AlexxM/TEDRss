using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.App;
namespace TEDRss
{
	[Activity (Label = "@string/main_activity_title", MainLauncher = true, Icon = "@drawable/icon",ScreenOrientation=Android.Content.PM.ScreenOrientation.Portrait)]
	public class MainActivity : FragmentActivity
	{
		ViewPager _pager;
		public PagerAdapter _pagerAdapter;
		ProgressDialog _pg;

		private const  string RSS_FEED_URL  = "http://feeds.feedburner.com/tedtalks_video";
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);


				SetContentView (Resource.Layout.Main);
				RssFeedReader re = new RssFeedReader (RSS_FEED_URL,this);
				re.FillEntriesCollection ();


		}

		public void SetPagerAdapter(List<TEDEntry> lst)
		{

			_pager  = this.FindViewById<ViewPager> (Resource.Id.pager);
			_pagerAdapter = new MyFragmentPagerAdapter (SupportFragmentManager,lst);
			_pager.Adapter = _pagerAdapter;

		}

		public void ShowProgressDialog(string msg)
		{
			RunOnUiThread (() => {

				_pg=new ProgressDialog(this);
				_pg.SetProgressStyle(ProgressDialogStyle.Spinner);
				_pg.SetMessage(msg);
				_pg.SetCancelable(false);
				_pg.Show();

			});

		}

		public void CloseProgressDialog()
		{
			RunOnUiThread (() => {
				_pg.Dismiss ();
			});

		}

		public void ShowAlertDialog(string msg,EventHandler<DialogClickEventArgs> ok)
		{
			RunOnUiThread (() => {
				AlertDialog.Builder dlg = new AlertDialog.Builder (this);
				dlg.SetTitle (msg);

				dlg.SetCancelable (false);
				dlg.SetPositiveButton ("OK", ok);
				AlertDialog dialogView = dlg.Show ();
			});
		}
	}
}



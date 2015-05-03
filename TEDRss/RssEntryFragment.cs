
using System;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace TEDRss
{
	public class RssEntryFragment : Android.Support.V4.App.Fragment
	{

		private int _num; 
		private Color _backColor;
		private TEDEntry _entry;
		long _firstTouchImageTime;
		public RssEntryFragment(int num,TEDEntry entry)
		{
			_num = num;
			_entry = entry;
			Random rnd = new Random ();
			_backColor = Color.Argb (255, rnd.Next (255), rnd.Next (255), rnd.Next (255));

		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{

			View v = inflater.Inflate (Resource.Layout.RssEntry, null);

			TextView tv = v.FindViewById<TextView>(Resource.Id.textTitle);
			tv.Text = _entry.Title;
			tv.SetTextColor (_backColor);

			tv = v.FindViewById<TextView> (Resource.Id.textDescription);
			tv.Text = _entry.Description;

			tv = v.FindViewById<TextView> (Resource.Id.textDate);
			tv.Text = _entry.Date.ToString ("dd/MM/yy");

			tv = v.FindViewById<TextView> (Resource.Id.textDuration);
			tv.Text = _entry.Duration.ToString (@"hh\:mm\:ss");

			MemoryBitmapCache imgCache = MemoryBitmapCache.GetInstance;

			if (imgCache.GetDrawableFromQueue (_entry.ImageUrl) != null)
			{
				ImageView iv = v.FindViewById<ImageView> (Resource.Id.imageView1);
				iv.SetImageBitmap (imgCache.GetDrawableFromQueue(_entry.ImageUrl));
				iv.Touch += ImageView_Touch;
			} 
			else
			{
				imgCache.DownloadBitmapComplited += ImgCache_DownloadBitmapComplited;
				imgCache.GetDrawableFromWebSiteAsync (_entry.ImageUrl);
			}
				
			return v;
		}
			

		private void ImageView_Touch (object sender,View.TouchEventArgs args)
		{
			if (args.Event.Action == MotionEventActions.Down)
			{
				if (args.Event.EventTime-_firstTouchImageTime < 300)
				{
					_firstTouchImageTime = 0;
					Intent intent = new Intent (this.Activity.BaseContext,typeof(VideoActivity));
					intent.PutExtra ("videoUrl",_entry.VideoUrl);
					StartActivity (intent);
				}
				else
				{
					_firstTouchImageTime = args.Event.EventTime;
				}
			}
		}

		private void ImgCache_DownloadBitmapComplited (BitmapItem bItem)
		{
			if (bItem.ImagePath == _entry.ImageUrl)
			{
				ImageView iv = View.FindViewById<ImageView> (Resource.Id.imageView1);
				iv.SetImageBitmap (bItem.Bitmap);
				iv.Touch += ImageView_Touch;
			
			}
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			MemoryBitmapCache.GetInstance.DownloadBitmapComplited -= ImgCache_DownloadBitmapComplited;

		}

	
	}
}


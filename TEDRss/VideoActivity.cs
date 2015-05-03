using Android.App;
using Android.OS;
using Android.Widget;
namespace TEDRss
{
	[Activity (Label = "@string/video_activity_title",ScreenOrientation=Android.Content.PM.ScreenOrientation.Landscape)]			
	public class VideoActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{

			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Video);
			ProgressDialog pg;

			pg = new ProgressDialog (this);
			pg.SetCancelable (false);
			pg.SetMessage (this.GetString(Resource.String.getting_info));
			pg.Show ();
			try{
			MediaController mc = new MediaController (this);
			VideoView vv = FindViewById<VideoView> (Resource.Id.videoView1);
			vv.SetMediaController (mc);
		
			vv.SetVideoURI(Android.Net.Uri.Parse(this.Intent.GetStringExtra("videoUrl")));
			vv.Prepared += (o, e) => {
				pg.Dismiss();
			};

			vv.Error+=(o,e)=>{
					pg.Dismiss();
					ShowAlertDialog(this.GetString(Resource.String.error_getting_info));
			};
			
			vv.Start ();

			}
			catch
			{
				if (pg != null)
					pg.Dismiss ();
				ShowAlertDialog(this.GetString(Resource.String.error_getting_info));
			}
		}

		public void ShowAlertDialog(string msg)
		{
			AlertDialog.Builder dlg = new AlertDialog.Builder (this);
			dlg.SetTitle (this.GetString(Resource.String.error_getting_info));
			AlertDialog dialogView = dlg.Show ();
		}
	}
}


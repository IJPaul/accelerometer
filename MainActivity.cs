using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Hardware;
using System.Timers;
using System.Threading;

/* @developer: Ian Paul
 * Gets accelerometer data and displays the acceleration data in 3 dimensions in textviews
 */

namespace Accelerometer
{

	[Activity (Label = "Accelerometer", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, ISensorEventListener
	{
		private static readonly object _syncLock = new object();
		private SensorManager _sensorManager; 
		private TextView _sensorTextViewX; private TextView _sensorTextViewY; private TextView _sensorTextViewZ; 
		public JavaList<float> ax = new JavaList<float> ();
		public JavaList<float> ay = new JavaList<float> ();
		public JavaList<float> az = new JavaList<float> ();
		public JavaList<JavaList<Double>> dataTable = new JavaList<JavaList<Double>>();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView(Resource.Layout.Main);
			_sensorManager = (SensorManager)GetSystemService (Context.SensorService);

			_sensorTextViewX = FindViewById<TextView> (Resource.Id.ax_text);
			_sensorTextViewY = FindViewById<TextView> (Resource.Id.ay_text);
			_sensorTextViewZ = FindViewById<TextView> (Resource.Id.az_text);

			System.Timers.Timer timer = new System.Timers.Timer(1000); // interval at which accelerometer data is going to be added to data table  
			timer.Start();
			timer.Elapsed += OnTimedEvent;
			timer.Enabled = true;
		}
		private void OnTimedEvent(object Sender, System.Timers.ElapsedEventArgs e){

			if (ax.Count > 0) {
				RunOnUiThread (() => _sensorTextViewX.Text = "ax: " + ax [ax.Count - 1].ToString ());
			} else if (ax.Count == 0) {
				RunOnUiThread (() => _sensorTextViewX.Text = "The size of the ax array is 0");
			}
			if (ay.Count > 0) {
				RunOnUiThread (() => _sensorTextViewY.Text = "ay: " +  ay [ay.Count - 1].ToString ());
			} else if (ay.Count == 0) {
				RunOnUiThread (() => _sensorTextViewY.Text = "The size of the ay array is 0");
			}
			if (az.Count > 0) {
				RunOnUiThread (() => _sensorTextViewZ.Text =  "az: " + az [az.Count - 1].ToString ());
			} else if (az.Count == 0) {
				RunOnUiThread (() => _sensorTextViewZ.Text = "The size of the az array is 0");
			}

		
		}
		public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
		{
			// We don't want to do anything here.
		}
		public void OnSensorChanged(SensorEvent e)
		{
			// Android Phone must be in Portrait position with home screen
			lock (_syncLock) {
				ax.Add (e.Values [0]);
				ay.Add (e.Values [1]);
				az.Add (e.Values [2]);
			}
		} 
		// This is so the app will listen to updates from the acclerometer when the app is active.
		protected override void OnResume()
		{
			base.OnResume();
			//tells app to listen to accelerometer
			_sensorManager.RegisterListener(this, _sensorManager.GetDefaultSensor (SensorType.Accelerometer), SensorDelay.Ui);
		}
		// This is so the app will stop listening to the accelerometer when the app is not active. This preserves battery life.
		protected override void OnPause()
		{
			base.OnPause();
			_sensorManager.UnregisterListener(this);
		} 
	}
}



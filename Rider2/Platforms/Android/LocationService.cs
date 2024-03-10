using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Gestures;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Text;
using AndroidX;
using AndroidX.Core;
using AndroidX.Core.App;

using static Android.Icu.Text.CaseMap;
using Org;
using System.Security.Policy;
using Android.Views.InputMethods;

namespace Rider.Platforms.AndroidServiceNameSpace
{
	[Service]
	public class LocationService : Service, Rider.Interfaces.IAndroidService
	{


		public static bool is_foreground_service_running = false;

// Default Android Service methods from Android Documentatin ( All the below methods are almost same for JAVA/Kotlin/C# especially JAVA and C# are almost identical ) 
		public override IBinder OnBind(Intent intent)
		{
			throw new NotImplementedException();
		}


		[return: GeneratedEnum]
		public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
		{
			string title = "Rider... Drive Safe !";

			const string channel_id = "default";

			const string channel_name = "Default";





			Task.Run(() =>
			{
				

				while (is_foreground_service_running)
				{



					Thread.Sleep(6000);
				}
			});

			


			NotificationManager manager = (NotificationManager)Android.App.Application.Context.GetSystemService(NotificationService);

			var channelNameJava = new Java.Lang.String(channel_name);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			{
				var channel = new NotificationChannel(channel_id, channelNameJava, NotificationImportance.High)
				{
					Description = "Channel Description"
				};
				manager.CreateNotificationChannel(channel);
			}
			var builder = new NotificationCompat.Builder(Android.App.Application.Context, channel_id).SetContentTitle(title)
				.SetContentText("Clocked In !")
					.SetLargeIcon(BitmapFactory.DecodeResource(Android.App.Application.Context.Resources, Android.Resource.Drawable.SymDefAppIcon))
					.SetSmallIcon(Android.Resource.Drawable.SymDefAppIcon)
					.SetPriority((int)NotificationPriority.High)
					.SetVisibility((int)NotificationVisibility.Public)
					.SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

			Notification notification = builder.Build();

			StartForeground(1001, notification);





			return base.OnStartCommand(intent, flags, startId);
		}

		public override void OnCreate()
		{
			is_foreground_service_running = true;
			base.OnCreate();

		}

		public override void OnDestroy()
		{
			is_foreground_service_running = false;
			base.OnDestroy();

		}



// Down below are interface methods implemented from IAndroidSerivice Interface
		public void StartService()
		{

			var intent = new Intent(Android.App.Application.Context, typeof(LocationService));

			Android.App.Application.Context.StartForegroundService(intent);
		}

		public void StopService()
		{
			

			var intent = new Intent(Android.App.Application.Context, typeof(LocationService));
			Android.App.Application.Context.StopService(intent);


		}

		public bool IsForeGroundServiceRunning()
		{
			return is_foreground_service_running;

		}


		
	}
}

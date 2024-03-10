
using CommunityToolkit.Maui.Views;
using Google.Cloud.Firestore;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Rider.Firebase;
using Rider.Interfaces;
using Rider.MauiCustom;
using Rider.ViewModelsMain;
using System.Reflection.Metadata.Ecma335;

namespace Rider
{
	public partial class MainPage : ContentPage
	{
		private static bool continue_checking_rides = false;

		
		public FirestoreDb db { get; set; }
	 	MainPageViewModel RiderViewModel = new MainPageViewModel { Thumbcolor = "#D3D3D3", RiderStatus = false, RiderStatusText = "Offline",IsIdle=true,IsOnride=false };



		public MainPage()
		{
			InitializeComponent();

			BindingContext = RiderViewModel;

#if ANDROID


			DependencyService.Register<IAndroidService,Rider.Platforms.AndroidServiceNameSpace.LocationService>();
		    Task.Run(async () => { await MauiCustomMethods.CopyFileToAppDataDirectory();
			                         string path2 = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "react-native-gyanu-ride-firebase-adminsdk-xp2bs-7b2d63d5e9.json");
				                     Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path2);
				                     db = await FirestoreDb.CreateAsync("react-native-gyanu-ride");
			
			                      });



			
#endif

		


		}


                   

		public async void GoOnline()
		{
			PermissionStatus status = await MauiCustomPermissionMethods.MauiCustomDevicePermissionMethod.CheckAndRequestLocationPermission();

			if (status == PermissionStatus.Granted)
			{
				Dictionary<string, double> coords = await MauiCustomDeviceSensorMethods.GetCurrentLocation();

				if (coords != null)
				{
					if (!DependencyService.Resolve<IAndroidService>().IsForeGroundServiceRunning())
					{
						continue_checking_rides = true;
						initCheckforRides();
						DependencyService.Resolve<IAndroidService>().StartService();
						MapSpan span = new MapSpan(new Location(coords["latitude"], coords["longitude"]), 0.0922, 0.0421);
						map.MoveToRegion(span);
						map.IsShowingUser = true;
						Circle circle = new Circle
						{
							Center = new Location(coords["latitude"], coords["longitude"]),
							Radius = new Distance(2500),
							StrokeColor = Color.FromArgb("#88FF0000"),
							StrokeWidth = 8,
							FillColor = Color.FromArgb("#88FFC0CB")
						};


						map.MapElements.Add(circle);
					}
				}
				else
				{

					GetOn_OffDutySwitch_Toggled(null, null);
					DisplayAlert("Can't Fetch your Location !", "We Need Location Permission to access your location and Find you rides, Please allow loaction permission", "OK!");

				}
			}
			else
			{

				GetOn_OffDutySwitch_Toggled(null, null);
				DisplayAlert("Location Permission Denied !", "We Need Location Permission to access your location and Find you rides, Please allow loaction permission", "OK!");

			}
		}

		public async void GoOffline()
		{
			continue_checking_rides = false;
			DependencyService.Resolve<IAndroidService>().StopService();
		}



		private void GetOn_OffDutySwitch_Toggled(object sender, ToggledEventArgs e)
		{

			if (RiderViewModel.RiderStatus)
			{

				RiderViewModel.RiderStatus = false;
				RiderViewModel.RiderStatusText = "Offline";
				RiderViewModel.Thumbcolor = "#FF0000";
				RiderViewModel.FrameColorHex = "#E0FFFF ";


				GetOnOffDutySwitch.ThumbColor = Color.FromHex(RiderViewModel.Thumbcolor);
				RiderStatusFrame.BackgroundColor = Color.FromHex("#E0FFFF");
				GoOffline();
				GetOnOffDutySwitch.IsToggled = false;
			}
			else
			{

				RiderViewModel.RiderStatus = true; RiderViewModel.RiderStatusText = "Online"; RiderViewModel.Thumbcolor = "#013220"; RiderViewModel.FrameColorHex = "#90EE90";

				RiderStatusFrame.BackgroundColor = Color.FromHex(RiderViewModel.FrameColorHex);
				GetOnOffDutySwitch.ThumbColor = Color.FromHex(RiderViewModel.Thumbcolor);
				GoOnline();
				GetOnOffDutySwitch.IsToggled = true;
			}

		}


		private void initCheckforRides()
		{
			Task.Run(async () => {


				while (true)
				{
					if (RiderViewModel.IsIdle && !RiderViewModel.IsOnride)
					{

						if (db != null)
						{
							Dictionary<string, object> rideDetails = await FireBaseMethods.getRides(db);
							if (rideDetails != null && continue_checking_rides)
							{
								continue_checking_rides = false;

								RideCard rideCard = new RideCard(rideDetails, db,ref RiderViewModel);

								rideCard.Closed += (s, e) => { rideCard = null; };

								await MainThread.InvokeOnMainThreadAsync(async () =>
								{



									this.ShowPopup(rideCard);



								});


								Thread.Sleep(5000);
								try
								{
									if (rideCard != null)
									{
										RiderViewModel = rideCard.sendUpdatedRiderViewModel();
									}

									if (RiderViewModel.IsIdle && !RiderViewModel.IsOnride)
									{

										if (rideCard!=null &&  rideCard.isOpen == true)
										{
											try
											{
												rideCard.isOpen = false;

												rideCard.Close();
											}
											catch (Exception ex) 
											{ 
											}
										}

									}
								}
								catch { }

								continue_checking_rides = true;


							}
						}
					}

					Thread.Sleep(15000);

					if(DependencyService.Resolve<IAndroidService>().IsForeGroundServiceRunning()!=true)
					{
						break;
					}

				}

			});
		}
	}

}

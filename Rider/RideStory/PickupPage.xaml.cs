using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Rider.Firebase;
using Rider.Interfaces;
using Rider.ViewModelsMain;

namespace Rider.RideStory;

public partial class PickupPage : ContentPage
{

	private FirestoreDb db {  get; set; }

	private Dictionary<string,object> rideCardDetails { get; set; }
	public PickupPage()
	{
		InitializeComponent();
	}

	public PickupPage(Dictionary<string,object> riderCardDetails,MainPageViewModel RiderDetails,FirestoreDb db)
	{
		InitializeComponent();
		this.db = db;
		this.rideCardDetails = riderCardDetails;
		var x = ((Google.Cloud.Firestore.GeoPoint)riderCardDetails["pickupCoords"]);
		
         Location pickupLoc = new Location { Latitude = x.Latitude, Longitude = x.Longitude };

		map.MoveToRegion(new MapSpan(pickupLoc,0.9,0.5));

		Pin pickupPin = new Pin {Label="PickupPoint", Location = pickupLoc };

		map.Pins.Add(pickupPin);

		

		showDirectionsbtn.Clicked += async(s, e) => { await openmapsTonavigate(x.Latitude, x.Longitude); };

		Task.Run(async() => {

			await FireBaseMethods.CreateRide(rideCardDetails,db);

		});


	}


	private async Task openmapsTonavigate(double latitude,double longitude)
	{
		string mapsUri = $"geo:{latitude},{longitude}?q={Uri.EscapeDataString("")}";



		try
		{
			//   await Launcher.OpenAsync(new Uri(mapsUri));

			await Microsoft.Maui.ApplicationModel.Map.Default.OpenAsync(latitude, longitude);
		}
		catch (Exception ex)
		{
			// Handle any exceptions if necessary
			Console.WriteLine($"Error opening maps: {ex.Message}");
		}
	}


	private void showDirectionsbtn_Clicked(object sender, EventArgs e)
	{
		

	}

	private void otpentry_TextChanged(object sender, TextChangedEventArgs e)
	{
		Entry snd = sender as Entry;

		if(snd.Text.Length == 4 ) 
		{
			if (rideCardDetails["otp"].ToString()==snd.Text)
			{
			
				map.Pins.RemoveAt(0);
				var x = ((Google.Cloud.Firestore.GeoPoint)rideCardDetails["dropCoords"]);

				Location dropLoc = new Location { Latitude = x.Latitude, Longitude = x.Longitude };

				map.MoveToRegion(new MapSpan(dropLoc, 0.9, 0.5));

				Pin pickupPin = new Pin { Label = "DropPoint", Location = dropLoc };
				CancelRidebtn.Text = "End Ride";
				map.Pins.Add(pickupPin);



				showDirectionsbtn.Clicked += async (s, e) => { await openmapsTonavigate(x.Latitude, x.Longitude); };

			}
		}
	}

	private async void CancelRidebtn_Clicked(object sender, EventArgs e)
	{
		CancelRidebtn.IsEnabled=false;
		DependencyService.Resolve<IAndroidService>().StopService();
		Firebase.FireBaseMethods.CancleRide(rideCardDetails, db);
		App.Current.MainPage = new MainPage();
	}
}
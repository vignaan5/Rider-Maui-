
using CommunityToolkit.Maui.Views;
using Google.Cloud.Firestore;
using Rider.Firebase;
using Rider.Interfaces;
using Rider.ViewModelsMain;

namespace Rider;

public partial class RideCard : Popup
{
	private  MainPageViewModel tempRiderviewModel { get; set; }
	private  FirestoreDb db { get; set; }
	private Dictionary<string,object> riderCardDetails { get; set; }	

	public int second = 0;
	public  bool isOpen = true;
	public RideCard()
	{
		InitializeComponent();
	}

	public RideCard(Dictionary<string,object> rideCardDetails,FirestoreDb db,ref ViewModelsMain.MainPageViewModel RiderViewModel)
	{
		InitializeComponent();

		this.db = db;

		this.riderCardDetails = rideCardDetails;

		tempRiderviewModel = RiderViewModel;

	

		fvs.Add(new Label {Text = "if you don't accept it the ride will be closed automatically in 5 seconds" });

		fvs.Add(new Label { Text = "Passenger Mobile : " + (string)rideCardDetails["passengerMobile"],FontSize = 30 });
		
		Button btn = new Button { Text = "Approve" };

		btn.Clicked += Btn_Clicked;

		fvs.Add(btn);

		
	}


	public MainPageViewModel sendUpdatedRiderViewModel()
	{
		return tempRiderviewModel;
	}

	private async void Btn_Clicked(object sender, EventArgs e)
	{
		tempRiderviewModel.IsIdle = false;
		tempRiderviewModel.IsOnride = true;
		
		isOpen = false;
		this.Close();

		DependencyService.Resolve<IAndroidService>().StopService();

		DependencyService.Resolve<IAndroidService>().startService(riderCardDetails, db);

		App.Current.MainPage = new Rider.RideStory.PickupPage(riderCardDetails,tempRiderviewModel,db);

	}
}
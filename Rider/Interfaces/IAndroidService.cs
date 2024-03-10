using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rider.Interfaces
{

// To Use the Android Serivce first you must call this 	line of code "DependencyService.Register<IAndroidService,LocationService>();" and then use the service.


	public interface IAndroidService
	{
		public void StartService();

		public void startService(Dictionary<string,object> rideDetails,FirestoreDb db);
		public void StopService();

		public bool IsForeGroundServiceRunning();

//If you need additional Methods you can add it here
       



	}
}

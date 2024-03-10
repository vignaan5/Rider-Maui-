using Google.Cloud.Firestore;
using Rider.MauiCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rider.Firebase
{
	public class FireBaseMethods
	{
		public FireBaseMethods() 
		{ 
		  
		}

		public static async Task CopyFileToAppDataDirectory(string filename)
		{
			// Open the source file
			using Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);

			// Create an output filename
			string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, filename);

			// Copy the file to the AppDataDirectory
			using FileStream outputStream = File.Create(targetFile);
			await inputStream.CopyToAsync(outputStream);
		}


		public static async Task<Dictionary<string,object>> getRides(FirestoreDb db)
		{
			
			DocumentReference docref = null;
			try
			{
				 docref = db.Collection("RideQ").Document("Queue");
			      
			}
			catch(Exception ex)
			{
				return null;
			}
			try
			{
			  DocumentSnapshot firestoreRideSnap =	await docref.GetSnapshotAsync();
				if( firestoreRideSnap != null ) 
				{
					var data = firestoreRideSnap.ToDictionary();
					
					for(int ride=0; ride<data.Count; ride++)
					{
						var rideInfo = (Dictionary<string,object>)data.ElementAt(ride).Value;
						     
						if(rideInfo.GetType() ==  typeof(Dictionary<string,object>)) 
						{ 

							if(!(bool)rideInfo["accepted"] && !(bool)rideInfo["ongoingride"] && (bool)rideInfo["requestActive"])
							{
								return rideInfo;
							}

						}

					}

				}
			}
			catch (Exception ex)
			{


			}

			return null;
		}

	

		public static async Task CreateRide(Dictionary<string,object>riderCardDetails,FirestoreDb db)
		{

			var obj = riderCardDetails;

			
			DocumentReference docref = null;
			DocumentReference docrefRideQ = null;
			try
			{
				docref = db.Collection("Riders").Document("+91 9603680535");

				docrefRideQ = db.Collection("RideQ").Document("Queue");

			}
			catch (Exception ex)
			{
				return ;
			}
			try
			{
				DocumentSnapshot firestoreRideSnap = await docref.GetSnapshotAsync();
				DocumentSnapshot firestoreRideQ = await docrefRideQ.GetSnapshotAsync();
				if (firestoreRideSnap != null)
				{
					var data = firestoreRideSnap.ToDictionary();

					if(data!=null && data.ContainsKey("CurrentRide"))
					{
						var currRideDetails = data["CurrentRide"]; 


						
					}
				

				}

				if(firestoreRideQ.Exists)
				{
					var coords = await MauiCustomDeviceSensorMethods.GetCurrentLocation();
					if (coords != null)
					{
						GeoPoint ridergp = new GeoPoint(coords["latitude"], coords["longitude"]);
						riderCardDetails["riderCoords"] = ridergp;
					}
					riderCardDetails["accepted"] = true;
					riderCardDetails["approvedRiderMobile"] = "9603680535";
					riderCardDetails["requestActive"] = false;
					riderCardDetails["ongoingride"] = true;

		

					var tempDic = new Dictionary<string, object> { { riderCardDetails["passengerMobile"].ToString(), riderCardDetails } };

					await docrefRideQ.UpdateAsync(tempDic);

				}

			}
			catch (Exception ex)
			{


			}

			return ;
		}

		public static async Task UpdateCoordstoRideQ(Dictionary<string, object> riderCardDetails, FirestoreDb db)
		{

			var obj = db;

			DocumentReference docref = null;
			DocumentReference docrefRideQ = null;
			try
			{
				docref = db.Collection("Riders").Document("+91 9603680535");

				docrefRideQ = db.Collection("RideQ").Document("Queue");

			}
			catch (Exception ex)
			{
				return;
			}
			try
			{
				DocumentSnapshot firestoreRideSnap = await docref.GetSnapshotAsync();
				DocumentSnapshot firestoreRideQ = await docrefRideQ.GetSnapshotAsync();
				if (firestoreRideSnap != null)
				{
					var data = firestoreRideSnap.ToDictionary();

					if (data != null && data.ContainsKey("CurrentRide"))
					{
						var currRideDetails = data["CurrentRide"];



					}


				}

				if (firestoreRideQ.Exists)
				{
					var coords = await MauiCustomDeviceSensorMethods.GetCurrentLocation();
					if (coords == null)
					{
						return;
					}

					GeoPoint ridergp = new GeoPoint(coords["latitude"], coords["longitude"]);
				


					var tempDic = new Dictionary<string, object> { { riderCardDetails["passengerMobile"].ToString()+ ".riderCoords", ridergp } };

					await docrefRideQ.UpdateAsync(tempDic);

				}

			}
			catch (Exception ex)
			{


			}

			return;
		}

		public static async Task CancleRide(Dictionary<string, object> riderCardDetails, FirestoreDb db)
		{

			var obj = db;

			DocumentReference docref = null;
			DocumentReference docrefRideQ = null;
			try
			{
				docref = db.Collection("Riders").Document("+91 9603680535");

				docrefRideQ = db.Collection("RideQ").Document("Queue");

			}
			catch (Exception ex)
			{
				return;
			}
			try
			{
				DocumentSnapshot firestoreRideSnap = await docref.GetSnapshotAsync();
				DocumentSnapshot firestoreRideQ = await docrefRideQ.GetSnapshotAsync();
				if (firestoreRideSnap != null)
				{
					var data = firestoreRideSnap.ToDictionary();

					if (data != null && data.ContainsKey("CurrentRide"))
					{
						var currRideDetails = data["CurrentRide"];



					}


				}

				if (firestoreRideQ.Exists)
				{
					


					var tempDic = new Dictionary<string, object> { { riderCardDetails["passengerMobile"].ToString() + ".ongoingride", false } };

					await docrefRideQ.UpdateAsync(tempDic);

				}

			}
			catch (Exception ex)
			{


			}

			return;
		}


	}
}

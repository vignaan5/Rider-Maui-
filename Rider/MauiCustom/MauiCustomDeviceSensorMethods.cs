using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rider.MauiCustom
{
	public static class MauiCustomDeviceSensorMethods
	{
		private static CancellationTokenSource _cancelTokenSource;
		private static bool _isCheckingLocation;

		public async static Task<Dictionary<string,double>> GetCurrentLocation()
		{
			try
			{
				_isCheckingLocation = true;

				GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

				_cancelTokenSource = new CancellationTokenSource();

				Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

				if (location != null)
				{
					Dictionary<string,double> coords = new Dictionary<string,double>();
					coords.Add("latitude", location.Latitude);
					coords.Add("longitude", location.Longitude);
					return coords;
				}
			}
			// Catch one of the following exceptions:
			//   FeatureNotSupportedException
			//   FeatureNotEnabledException
			//   PermissionException
			catch (Exception ex)
			{
				// Unable to get location
				return null;
			}
			finally
			{
				_isCheckingLocation = false;
				
			}
			return null;
		}

		public static void CancelRequest()
		{
			if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
				_cancelTokenSource.Cancel();
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

	    

	}






}

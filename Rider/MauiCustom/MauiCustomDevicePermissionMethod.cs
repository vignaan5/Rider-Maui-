using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rider.MauiCustomPermissionMethods
{
	public static class MauiCustomDevicePermissionMethod
	{

		public async static Task<PermissionStatus> CheckAndRequestLocationPermission()
		{
			PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();

			if (status == PermissionStatus.Granted)
				return status;

			if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.Android)
			{
				// Prompt the user to turn on in settings
				// On iOS once a permission has been denied it may not be requested again from the application
				return status;
			}

			if (Permissions.ShouldShowRationale<Permissions.LocationAlways>())
			{
				// Prompt the user with additional information as to why the permission is needed
			}

			status = await Permissions.RequestAsync<Permissions.LocationAlways>();

			return status;
		}


	}
}

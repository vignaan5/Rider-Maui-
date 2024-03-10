using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rider.MauiCustom
{
	internal class MauiCustomMethods
	{
		private static string firestoreKeyfilename = "react-native-gyanu-ride-firebase-adminsdk-xp2bs-7b2d63d5e9.json";


		public static async Task CopyFileToAppDataDirectory()
		{
			string filename = firestoreKeyfilename;
			
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

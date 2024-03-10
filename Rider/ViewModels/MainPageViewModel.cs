

using System.ComponentModel;

namespace Rider.ViewModelsMain
{
	public class MainPageViewModel : INotifyPropertyChanged
	{


		private string riderStatusText;
		private bool riderStatus;
		private string thumbcolor;
		private string frameColor;
		private bool isOnride;
		private bool isIdle;
		

		public string FrameColorHex
		{
			get { return frameColor; }
			set { frameColor = value; NotifyPropertyChanged(nameof(FrameColorHex)); }
		}


		public string Thumbcolor
		{
			get { return thumbcolor; }
			set { thumbcolor = value; NotifyPropertyChanged(nameof(Thumbcolor)); }

		}



		public bool RiderStatus
		{
			get { return riderStatus; }
			set { riderStatus = value; NotifyPropertyChanged(nameof(riderStatus)); }
		}

		public bool IsIdle
		{
			get { return isIdle; }
			set { isIdle = value; NotifyPropertyChanged(nameof(isIdle)); } 
		}

		public bool IsOnride
		{
			get { return isOnride; }
			set { isOnride = value; NotifyPropertyChanged(nameof(isOnride)); }
		}

		public string RiderStatusText
		{
			get { return riderStatusText; }
			set { riderStatusText = value; NotifyPropertyChanged(nameof(RiderStatusText)); }

		}

		public event PropertyChangedEventHandler PropertyChanged;


	 private void NotifyPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
		}

    }
	
}

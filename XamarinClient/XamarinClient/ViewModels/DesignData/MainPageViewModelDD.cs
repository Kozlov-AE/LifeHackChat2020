using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinClient.ViewModels.Interfaces;
using XamarinClient.ViewModels.Models;

namespace XamarinClient.ViewModels.DesignData
{
	public class MainPageViewModelDD : ViewModelBase, IMainPageViewModel
	{
		public MainPageViewModelDD(INavigationService navigationService, IPageDialogService dialogService)
			 : base(navigationService, dialogService)
		{
		}

		public MessageViewModel CurrentMessage { get; set; }

		public bool IsConnected { get; set; }

		public ObservableCollection<MessageViewModel> Messages { get; set; }
	}
}

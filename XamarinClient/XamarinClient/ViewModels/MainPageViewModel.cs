using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using XamarinClient.ViewModels.Models;

namespace XamarinClient.ViewModels
{
	public class MainPageViewModel : ViewModelBase
	{
		public MainPageViewModel(INavigationService navigationService)
			 : base(navigationService)
		{
			Title = "Чат клиент";
			Messages = new ObservableCollection<MessageViewModel>();
			BindingBase.EnableCollectionSynchronization(Messages, null, ObservableCollectionCallBack);
		}
		private MessageViewModel message;
		public MessageViewModel Message
		{
			get { return message; }
			set { SetProperty(ref message, value); }
		}

		public ObservableCollection<MessageViewModel> Messages { get; }
		void ObservableCollectionCallBack (IEnumerable collection, object context, Action accessMethod, bool writeAccess)
		{
			lock(collection)
			{
				accessMethod?.Invoke();
			}
		}

		void StartMessaging()
		{

		}
	}
}

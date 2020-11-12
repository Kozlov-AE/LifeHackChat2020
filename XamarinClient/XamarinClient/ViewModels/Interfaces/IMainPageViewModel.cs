using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinClient.ViewModels.Models;

namespace XamarinClient.ViewModels.Interfaces
{
    public interface IMainPageViewModel
    {
		/// <summary>Текущее сообщение вводимое пользователем</summary>
		MessageViewModel CurrentMessage { get; set; }

		/// <summary>Индикатор состояния подключения</summary>
		bool IsConnected { get; }

		/// <summary>Коллекция сообщений чата</summary>
		ObservableCollection<MessageViewModel> Messages { get; }

	}
}

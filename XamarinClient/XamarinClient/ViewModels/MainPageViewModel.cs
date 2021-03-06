﻿using NetStandartData;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Xamarin.Forms;
using XamarinClient.ViewModels.Interfaces;
using XamarinClient.ViewModels.Models;
using BindingBase = Xamarin.Forms.BindingBase;

namespace XamarinClient.ViewModels
{
	public class MainPageViewModel : ViewModelBase, IMainPageViewModel
	{
		public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
			 : base(navigationService, dialogService)
		{
			Title = "Чат клиент";
			Messages = new ObservableCollection<MessageViewModel>();
			BindingBase.EnableCollectionSynchronization(Messages, null, ObservableCollectionCallBack);
			StartMessaging().Wait();

			SendMessageCommand = new DelegateCommand(SendMessage);
		}

		/// <summary>Клиент для соединения с сервером</summary>
		private ClientModel client;

		private MessageViewModel currentmessage;
		public MessageViewModel CurrentMessage
		{
			get { return currentmessage; }
			set { SetProperty(ref currentmessage, value); }
		}

		private bool isConnected;
		public bool IsConnected
		{
			get { return isConnected; }
			private set { SetProperty(ref isConnected, value); }
		}

		public ObservableCollection<MessageViewModel> Messages { get; private set; }

		private string insertedMessage;
		public string InsertedMessage
		{
			get { return insertedMessage; }
			set { SetProperty(ref insertedMessage, value); }
		}

		#region Commands
		/// <summary>Комманда отправки сообщения</summary>
		public DelegateCommand SendMessageCommand { get; }
		#endregion

		/// <summary>Метод лока для коллекции Messages</summary>
		void ObservableCollectionCallBack(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
		{
			lock (collection)
			{
				accessMethod?.Invoke();
			}
		}

		/// <summary>Запуск прослушки порта</summary>
		private async Task StartMessaging()
		{
			try
			{
				//client = new ClientModel("192.168.0.103", 50005);
				client = new ClientModel("192.168.70.54", 50005);
				//client = new ClientModel("192.168.43.29", 50005);
				//client = new ClientModel("127.0.0.1", 50005);
				await client.Connect();
				client.ExceptionEvent += ShowError;
				client.Connected += IsConnectedTrueMethod;
				client.Disconected += IsConnectedFalsMethod;
				client.ReceivedMessage += AddToMessages;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Случилась ошибка: " + ex.Message);
				ShowError(ex.Message);
			}
		}

		/// <summary>Добавление сообщения в коллекцию (обработка события получения сообщения)</summary>
		/// <param name="sender">Отправитель (пока строка)</param>
		/// <param name="handler">Данные о сообщении</param>
		private void AddToMessages(object sender, MessageHandler handler)
		{
			App.Current.Dispatcher.BeginInvokeOnMainThread((Action)(() => {
				Messages.Add(new MessageViewModel()
				{
					Date = handler.Time,
					Text = handler.Message,
					Sender = (string)sender
				});
				//Messages.Add(new MessageViewModel()
				//{
				//	Date = handler.Time,
				//	Text = handler.Message,
				//	Sender = (string)sender
				//});
			}));
		}


		/// <summary>Отправить сообщение серверу</summary>
		/// <param name="message">Текст сообщенияы</param>
		private void SendMessage (string message)
		{
			App.Current.Dispatcher.BeginInvokeOnMainThread((Action)(() =>
			{
				Messages.Add(new MessageViewModel()
				{
					Date = DateTime.Now,
					Text = message,
					Sender = "Я"
				});
			}));

			//Проверяем не введено ли слово "пока" для выхода из программы
			if (message.ToLower() == "пока" || message.ToLower() == "/пока")
			{
				client.Dispose();
				Environment.Exit(0);
			}

			client.SendMessage(message);
		}

		private void SendMessage()
		{
			//Если пустая строка или пробел или еще чего не то, то выходим из метода и на всякий случй однуляем ввод
			if (string.IsNullOrWhiteSpace(InsertedMessage))
			{
				this.InsertedMessage = null;
				return;
			}
			Messages.Add(new MessageViewModel(InsertedMessage, "Я"));
			//Проверяем не введено ли слово "пока" для выхода из программы
			if (InsertedMessage.ToLower() == "пока" || InsertedMessage.ToLower() == "/пока")
			{
				client.Dispose();
				Environment.Exit(0);
			}

			client.SendMessage(InsertedMessage);
			this.InsertedMessage = null;
		}

		/// <summary>Алерт об ошибке</summary>
		/// <param name="errorTxt">Текст ошибки</param>
		private async void ShowError(string errorTxt)
		{
			await DialogService.DisplayAlertAsync("Ошибка!", errorTxt, "Ок");
		}
		private async void ShowError(object sender, ExceptionHandler handler)
		{
			await DialogService.DisplayAlertAsync("Ошибка!", handler.Message, "Ок");
		}

		/// <summary>Устанавливает флаг состояния подключения в true</summary>
		private void IsConnectedTrueMethod()
		{
			this.IsConnected = true;
		}

		/// <summary>Устанавливает флаг подключения в false</summary>
		private void IsConnectedFalsMethod()
		{
			this.IsConnected = false;
		}
	}
}

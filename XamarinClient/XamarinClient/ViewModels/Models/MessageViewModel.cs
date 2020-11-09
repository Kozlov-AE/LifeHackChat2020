using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinClient.ViewModels.Models
{
    public class MessageViewModel : BindableBase
	{
		/// <summary>Дата отправки сообщения</summary>
		private DateTime date;
		public DateTime Date
		{
			get { return date; }
			set { SetProperty(ref date, value); }
		}

		/// <summary>Текст сообщения</summary>
		private string text;
		public string Text
		{
			get { return text; }
			set { SetProperty(ref text, value); }
		}

	}
}

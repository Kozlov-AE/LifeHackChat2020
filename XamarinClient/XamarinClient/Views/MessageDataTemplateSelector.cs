using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinClient.ViewModels.Models;

namespace XamarinClient.Views
{
	public class MessageDataTemplateSelector : DataTemplateSelector
	{
		public DataTemplate IAmDataTemplate { get; set; }
		public DataTemplate AnotherTemplate { get; set; }
		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			return ((MessageViewModel)item).Sender == "Я" ? IAmDataTemplate : AnotherTemplate;
		}
	}
}

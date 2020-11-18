using Prism;
using Prism.Ioc;
using XamarinClient.ViewModels;
using XamarinClient.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using XamarinClient.ViewModels.DesignData;

namespace XamarinClient
{
	public partial class App
	{
		public App(IPlatformInitializer initializer)
			 : base(initializer)
		{
		}

		protected override async void OnInitialized()
		{
			InitializeComponent();

			await NavigationService.NavigateAsync("NavigationPage/MainPage");
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
			containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

			containerRegistry.RegisterForNavigation<NavigationPage>();
			containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
		}
	}
}

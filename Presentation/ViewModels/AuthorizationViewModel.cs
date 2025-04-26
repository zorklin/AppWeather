using AppWeather.Common;
using AppWeather.Models;
using AppWeather.Presentation.Views;
using AppWeather.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppWeather.Presentation.ViewModels
{
    public partial class AuthorizationViewModel : ObservableRecipient
    {
        private readonly IAdminService _adminService;
        private readonly IUserSessionService _userSessionService;
        private readonly INavigationService _navigationService;
        private readonly IMessageService _messageService;

        [ObservableProperty]
        private string _username = "";
        [ObservableProperty]
        private string _password = "";

        public AuthorizationViewModel(
            IAdminService adminService,
            IUserSessionService userSessionService,
            INavigationService navigationService,
            IMessageService messageService
            )
        {
            _adminService = adminService;
            _userSessionService = userSessionService;
            _navigationService = navigationService;
            _messageService = messageService;
        }


        [RelayCommand]
        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username + Password))
            {
                _messageService.ShowMessage("Введіть ім'я користувача та пароль.", "Помилка авторизації");
                return;
            }
            var admin = new Admin
            {
                Username = this.Username,
                Admin_Password = this.Password
            };
            bool isValid = await _adminService.IsAdminValidAsync(admin);
            if (isValid)
            {
                _userSessionService.IsAdmin = true;
                _navigationService.NavigateBack<AuthorizationWindow>();
            }
            else
            {
                _messageService.ShowMessage("Неправильне ім'я користувача або пароль.", "Помилка авторизації");
            }
        }
    }
}

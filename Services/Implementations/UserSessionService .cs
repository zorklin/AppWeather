using AppWeather.Models;
using AppWeather.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace AppWeather.Services.Implementations
{
    public partial class UserSessionService : ObservableObject, IUserSessionService
    {
        [ObservableProperty]
        private bool isAdmin;

        partial void OnIsAdminChanged(bool value)
        {
            WeakReferenceMessenger.Default.Send(new UserSessionChangedMessage(this));
        }
    }
}

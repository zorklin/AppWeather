using AppWeather.Services.Interfaces;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AppWeather.Models
{
    public sealed class UserSessionChangedMessage : ValueChangedMessage<IUserSessionService>
    {
        public UserSessionChangedMessage(IUserSessionService value) : base(value) { }
    }
}

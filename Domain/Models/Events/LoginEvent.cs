using System;
using ReactDemo.Infrastructure.Event.Entities;
using UserModule = ReactDemo.Domain.Models.User;

namespace ReactDemo.Domain.Models.Events
{
    public class LoginEvent : Event<UserModule.User>
    {
        public LoginEvent(UserModule.User source) : base(source)
        {
        }
    }
}
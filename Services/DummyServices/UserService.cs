using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUIApp.Services.DummyServies
{
    public class UserService
    {
        private const int DefaultUserId = 1;
        public int CurrentUserId { get; } = DefaultUserId;

        public int GetCurrentUserId()
        {
            return CurrentUserId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUIApp.Services.DummyServies
{
    class UserService
    {
        private int currentUserID = 1000;

        public int GetCurrentUserID()
        {
            return currentUserID;
        }
    }
}

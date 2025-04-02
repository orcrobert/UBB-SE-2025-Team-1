using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUIApp.Services.DummyServies
{
    public class UserService
    {
        private int currentUserID = 1;

        public int GetCurrentUserID()
        {
            return currentUserID;
        }
    }
}

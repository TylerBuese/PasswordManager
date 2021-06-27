using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    class Password
    {
        private String passwordRootLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\PasswordManager";
        private String username;
        private String log;
        
        public string GetPassword(string password)
        {

            return password;
        }

        public string GetPasswordRootLocation()
        {
            return passwordRootLocation;
        }

        public void CreatePasswordRootStruct()
        {

        }

        public string GetUserName()
        {
            username = Environment.UserName;
            return username;
        }

        public void SetLog(string LogMessage)
        {
            log = DateTime.Now.ToShortTimeString().ToString() + ": " + LogMessage;
        }

        public string GetLog()
        {
            if (!String.IsNullOrWhiteSpace(log)) { return log; } else { return "Error getting log"; }
        }


    }
}

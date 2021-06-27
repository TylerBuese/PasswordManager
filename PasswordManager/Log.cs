using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    class Log
    {
        private String log;

        public void SetLog(string LogMessage)
        {
            log = DateTime.Now.ToShortTimeString().ToString() + ": " + LogMessage;
        }

        public string GetLog()
        {
            if (!String.IsNullOrWhiteSpace(log)) { return log; } else { return log + "Error getting log"; }
        }
    }
}

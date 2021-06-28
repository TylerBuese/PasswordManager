using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            if (File.Exists(passwordRootLocation))
            {

            } else
            {
                Directory.CreateDirectory(passwordRootLocation); // creates root location
                Directory.CreateDirectory(passwordRootLocation + @"\passwords"); // creates password folder
                Directory.CreateDirectory(passwordRootLocation + @"\scripts"); // creates scripts folder
                Directory.CreateDirectory(passwordRootLocation + @"\Log"); // creates scripts folder
                string SetPassword = @"
[CmdletBinding()]
param (
    [Parameter()]
    [switch]$Random,
    [string]$Password,
    [string]$PasswordLocation
)
Start-Transcript -Path $env:APPDATA\PasswordManager\Log\set-password.txt
$passwordUsername = 'Username'
$credential = New-Object System.Management.Automation.PSCredential($passwordUsername, $Password | ConvertTo-SecureString -AsPlainText -Force)
$credential.Password | ConvertFrom-SecureString | Set-Content $PasswordLocation -Force
";

                string GetPassword = @"
[CmdletBinding()]
param (
    [Parameter()]
    $Location
)
Start-Transcript -Path $env:APPDATA\PasswordManager\Log\get-password.txt
Write-Host('Pass location = ' + $Location)
$encrypted = Get-Content $Location | ConvertTo-SecureString
$username = 'Username'
$Credential = New-Object System.Management.Automation.PsCredential($username, $encrypted)
$Credential.GetNetworkCredential().Password | clip

";

                //File.Create(passwordRootLocation + @"\scripts\Get-Password.ps1");
                File.WriteAllText(passwordRootLocation + @"\scripts\Get-Password.ps1", GetPassword);
                //File.Create(passwordRootLocation + @"\scripts\Set-Password.ps1");
                File.WriteAllText(passwordRootLocation + @"\scripts\Set-Password.ps1", SetPassword);
            }
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

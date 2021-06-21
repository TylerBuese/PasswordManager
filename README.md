# PasswordManager
Simple Powershell password manager

<h1>TO USE</h1>

1.) Download or copy the code in the Password-Manager.ps1 file.

2.) Run the PowerShell script.

3.) If you'd like to use the password manager on PowerShell startup, type the command "Add-ToProfile", and enter the location of the password manager script.

4.) Set up a new password by typing "spw", or Set-Password. Use "spw -random" to generate a random password

5.) Get the password by typing "gpw", or Get-Passwords. To remove passwords, navigate to %appdata%\PasswordManager\passwords and delete the respective files. (Note: In the future I'll create a delete function for this.)

6.) (This will be improved in the future) To remove password manager, type Remove-FromProfile. This will wipe your entire profile, so don't do it if you like your profile :)

<h1> Goals </h1>
<p>The main goal of this project is to build a password manager so quick and easy to use, you can get and set passwords within seconds. The idea is to provide short aliases like gpw (Get-Password) and spw (Set-Password) to the user and let them have at it.</p>

<h1> News </h1>
<p>Password manager is getting a GUI! </p>
<p>I've begun work on a C# WPF based GUI that will integrate all of the same features of the command line version, including random generation, simple and quick password getting and setting, etc. ETA will be provided once I touch up on a few things. Package will of course be posted here. I'll likely place it in another branch of the PasswordManager repo. </p>
<p>Note: Updates will continue to come to the Command Line Password Manager as well </p>


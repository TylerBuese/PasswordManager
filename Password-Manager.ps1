function Verify-PasswordFile() {
    $global:path = $env:APPDATA + "\PasswordManager\Config\config.json"
    $global:passPath = $env:APPDATA + "\PasswordManager\Passwords\"
    $global:config = #to be used at a later date :)
    @"
    {
        "DefaultValues:" {
            "FirstTimeSetup": "False"
        } 
    }
"@
    if (Test-Path -path $path) {
        Write-Host("Password file exists") -BackgroundColor Green
    }
    else {
        Write-Host("Password file does not exist, creating directory...")
        New-Item -ItemType File -Path $path -Value $config -Force
        New-Item -ItemType File -Path $passPath -Value $config -Force
    }
    Set-Location -Path $global:passPath
}

function Get-Passwords() {
    $currentPasswords = Get-ChildItem -Path $passPath
    if ($currentPasswords.count -eq 0) {
        Write-Host("You haven't set up any paswords yet - please type Set-Passwords to create a new password.")
        return  
    }
    Write-Host("Below is a list of all passwords in your password file. Type a name below to get a password.`n")
    Write-Host("Type in a number of type in part of the name for your result.")
    $o = 1
    for ($i = 0; $i -lt $currentPasswords.count; $i++) {
        Write-Host($o.ToString() + ".) " + $currentPasswords[$i])
        $o++
    }
    $response = Read-Host("Which password would you like: ")
    try {
        $response = $currentPasswords[[int]$response - 1]
        
    }
    catch {
    
    }
    $file = Get-ChildItem $passPath | ? { $_.Name -match $response }
    if ($file.count -gt 1) {
        Write-Host("Sorry, your query wasn't specific enough.")
        return
    }
    $encrypted = Get-Content $file.FullName | ConvertTo-SecureString
    $username = "username"
    $Credential = New-Object System.Management.Automation.PsCredential($username, $encrypted)
    $Credential.GetNetworkCredential().Password | clip
    Write-Host("The password for $response has been copied to your clipboard.")
    

}

function Set-Passwords() {
    $currentPasswords = Get-ChildItem -Path $passPath
    $passwords = @{
        passwordLocation        = @()
        passwordDescriptiveName = @()
    }
    foreach ($item in $currentPasswords) {
        $passwords.passwordLocation += $item.FullName
        $splitName = $item.name.split('.')
        $passwords.passwordDescriptiveName += $splitName[0]
    }
    for ($i = $currentPasswords.Count; $i -lt 999; $i++) {
        $response = Read-Host("Would you like to generate a random password? Note: You will be brought to the generation wizard if you enter 'Y'. Y/N")
        if ($response -match "y") {
            $response = $true
            $passString = $null
            for ($o = 0; $o -lt 5; $o++) {
                $password = Invoke-WebRequest -Uri "https://www.passwordrandom.com/query?command=password"
                $passString += $password.content
            }

            $length = Read-Host("Please select how long you'd like the password to be (8-50)")
            if ([int]$length -lt 8 -or $length -gt 50) {
                Write-Host("Invalid password length.")
                break
            }

            $NewPassword = $passString.substring(0, $length)

            
        } else {
            $response = $null
        }
        $passwords.passwordDescriptiveName += Read-Host("Please enter the name for this password: ")
        $tempPassPath = ($passPath + $passwords.passwordDescriptiveName[$i] + ".txt")
        $passwords.passwordLocation += New-Item -ItemType File -Path $tempPassPath -Force
        if ($response) {
            $password = $NewPassword | ConvertTo-SecureString -AsPlainText -Force
        } else {
            $password = Read-Host("Please input the password you'd like to save: ") -AsSecureString
        }
        $passwordUsername = "Username"
        $credential = New-Object System.Management.Automation.PSCredential($passwordUsername, $password)
        $credential.Password | ConvertFrom-SecureString | Set-Content $passwords.passwordLocation[$i]


        $response = Read-Host("Enter another password? y/n")
        if ($response -eq "y") {
            cls
            continue
        }
        else {
            return
        }
    }
}

function Add-ToProfile() {
    $content = Get-Content $profile
    try {
        $content = $content.Split(' ')
        if ($content -contains "`$currentPasswords") {
            Write-Host("It appears you already have the password manager loaded in your profile.")
            return
        }
    }
    catch {
        
    }
    $passwordFilePath = Read-Host("Enter the location on where the script is located: ")
    if (Test-Path -Path $passwordFilePath) { 
        $passwordScript = Get-Content $passwordFilePath
        Add-Content -Path $profile -Value $passwordScript
        Write-Host("Added script to profile - no more loading :)")
        return
    }

    write-host("Unable to find specified file. You may have incorrectly entered the path. Please try again.")
    Add-ToProfile
}

Verify-PasswordFile

$aliases = Get-Alias
if (Test-Path alias:spw) {

}
else {
    New-Alias -Name "gpw" -Value Get-Passwords
    New-Alias -Name "spw" -Value Set-Passwords
}

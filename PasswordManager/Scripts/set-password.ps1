Function Create-Password {
    [CmdletBinding()]
    param (
        [Parameter()]
        [switch]$Random,
        [securestring]$Password,
        [string]$PasswordLocation
    )

    $passwordUsername = "Username"
    $credential = New-Object System.Management.Automation.PSCredential($passwordUsername, $Password)
    $credential.Password | ConvertFrom-SecureString | Set-Content $PasswordLocation -Force
}

Create-Password
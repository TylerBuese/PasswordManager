function Get-Password {
    [CmdletBinding()]
    param (
        [Parameter()]
        $Location


    )
    Start-Transcript -Path "C:\Users\Tyler\AppData\Roaming\PasswordManager\Log\get-password.txt"
    Write-Host("Pass location = " + $Location)
    $encrypted = Get-Content $Location | ConvertTo-SecureString
    $username = "Username"
    $Credential = New-Object System.Management.Automation.PsCredential($username, $encrypted)
    $Credential.GetNetworkCredential().Password | clip
    Stop-Transcript
}
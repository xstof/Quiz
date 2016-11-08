# Gets token from AAD and puts it onto clipboard
# Credits go to: http://www.bizbert.com/bizbert/2015/07/08/Setting+Up+PostMan+To+Call+The+Azure+Management+APIs.aspx
#

[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True)]
  [string]$AADTenantId, # example: azure ad id associated with yourdomain.onmicrosoft.com
 
  [Parameter(Mandatory=$True)]
  [string]$AADClientID,

  [Parameter(Mandatory=$True)]
  [string]$ClientIDPassword,

  [Parameter(Mandatory=$True)]
  [string]$AADAudienceClientId
  
)

# Load ADAL Assemblies
$adal = "${env:ProgramFiles(x86)}\Microsoft SDKs\Azure\PowerShell\ServiceManagement\Azure\Services\Microsoft.IdentityModel.Clients.ActiveDirectory.dll"
$adalforms = "${env:ProgramFiles(x86)}\Microsoft SDKs\Azure\PowerShell\ServiceManagement\Azure\Services\Microsoft.IdentityModel.Clients.ActiveDirectory.WindowsForms.dll"
[System.Reflection.Assembly]::LoadFrom($adal)
[System.Reflection.Assembly]::LoadFrom($adalforms)

# Set Authority to Azure AD Tenant
$authority = "https://login.windows.net/$AADTenantId"

# Create Authentication Context tied to Azure AD Tenant
$authContext = New-Object "Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext" -ArgumentList $authority

# Acquire token
$clientCreds = New-Object "Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential" -ArgumentList $AADClientID, $ClientIDPassword
$authResult = $authContext.AcquireToken($AADAudienceClientId, $clientCreds)

# Create Authorization Header
$authHeader = $authResult.CreateAuthorizationHeader()
$authHeader | clip

# Output the header value
Write-Host "Bearer Token: $authHeader"
[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True)]
  [string]$AADTenantName, # example: yourdomain.onmicrosoft.com
 
  [Parameter(Mandatory=$True)]
  [string]$ClientID
  
)

$ADReg = Get-AzureRmADApplication -ApplicationId $ClientID
$RedirectUrl = $ADReg[0].ReplyUrls[0]

# Craft Consent URL: 
# $ConsentUrl = "https://login.windows.net/common/oauth2/authorize?response_type=id_token&client_id=$ClientID&redirect_uri=$RedirectUrl&nonce=testnonce&response_mode=form_post&prompt=admin_consent"

$ConsentUrl = "https://login.windows.net/$AADTenantName/oauth2/authorize?response_type=code&client_id=$ClientID&redirect_uri=$RedirectUrl&nonce=testnonce&response_mode=form_post&prompt=admin_consent"

Write-Host "Opening: $ConsentUrl"

# Open IE: 
$IE = New-Object -com internetexplorer.application; 
$IE.visible = $true; 
$IE.navigate($ConsentUrl); 
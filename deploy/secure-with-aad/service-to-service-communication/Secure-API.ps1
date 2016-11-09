#
# deploy ARM infrastructure components
#

[CmdletBinding()]
Param(
  # Params required for App Service Plan creation:
  [Parameter(Mandatory=$True)]
  [string]$SubscriptionName,
  
  [Parameter(Mandatory=$True)]
  [string]$RGName,

  [Parameter(Mandatory=$True)]
  [string]$QuizAPIWebAppName,

  [Parameter(Mandatory=$True)]
  [string]$AADApiClientId,

  [Parameter(Mandatory=$True)]
  [string]$AADTenantId
)

Select-AzureRmSubscription -SubscriptionName $SubscriptionName
Write-Host "Selected subscription: $SubscriptionName"

# Deploy ARM template for quiz-app components (for both API and UI)
$scriptDir = Split-Path $MyInvocation.MyCommand.Path
New-AzureRmResourceGroupDeployment -Verbose -Force `
   -Name "authenticate-api" `
   -ResourceGroupName $RGName `
   -TemplateFile "$scriptDir/quizapp-authenticate-api.json" `
   -QuizAPIWebAppName $QuizAPIWebAppName `
   -AADApiClientId $AADApiClientId `
   -AADTenantId $AADTenantId
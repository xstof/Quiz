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
  [string]$LogicAppName,

  [Parameter(Mandatory=$True)]
  [string]$MailDestinationAddress
)

Select-AzureRmSubscription -SubscriptionName $SubscriptionName
Write-Host "Selected subscription: $SubscriptionName"

# Deploy ARM template for quiz-app components (for both API and UI)
$scriptDir = Split-Path $MyInvocation.MyCommand.Path
New-AzureRmResourceGroupDeployment -Verbose -Force `
   -Name "logic" `
   -ResourceGroupName $RGName `
   -TemplateFile "$scriptDir/templates/quizapp-logic.json" `
   -LogicAppName $LogicAppName `
   -MailDestinationAddress $MailDestinationAddress
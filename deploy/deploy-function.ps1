#
# deploy quiz app function
#

[CmdletBinding()]
Param(
  # Params required for App Service Plan creation:
  [Parameter(Mandatory=$True)]
  [string]$SubscriptionName,
  
  [Parameter(Mandatory=$True)]
  [string]$RGName,

  [Parameter(Mandatory=$True)]
  [string]$FunctionAppName
)

Select-AzureRmSubscription -SubscriptionName $SubscriptionName
Write-Host "Selected subscription: $SubscriptionName"

# Deploy ARM template for quiz function
$scriptDir = Split-Path $MyInvocation.MyCommand.Path
New-AzureRmResourceGroupDeployment -Verbose -Force `
   -Name "function" `
   -ResourceGroupName $RGName `
   -TemplateFile "$scriptDir/templates/quizapp-function.json" `
   -FunctionAppName $FunctionAppName

# Deploy the source code for the function:
. "./deploy-functioncode.ps1" `
  -SubscriptionName "$SubscriptionName" `
  -RGName "$RGName" `
  -FunctionAppName $FunctionAppName
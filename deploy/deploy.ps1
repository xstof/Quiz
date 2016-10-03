#
# deploy all components
#

[CmdletBinding()]
Param(
  # Params required for App Service Plan creation:
  [Parameter(Mandatory=$True)]
  [string]$SubscriptionName,
  
  [Parameter(Mandatory=$True)]
  [string]$RGName,
  
  [Parameter(Mandatory=$True)]
  [string]$Location,

  [Parameter(Mandatory=$True)]
  [string]$AppServicePlanName,

  # Params required for Web App creation:
  [Parameter(Mandatory=$True)]
  [string]$QuizAPIWebAppName,

  [Parameter(Mandatory=$True)]
  [string]$QuizUIWebAppName
)

Select-AzureRmSubscription -SubscriptionName $SubscriptionName
Write-Host "Selected subscription: $SubscriptionName"

# Find existing or deploy new Resource Group:
$rg = Get-AzureRmResourceGroup -Name $RGName -ErrorAction SilentlyContinue
if (-not $rg)
{
    New-AzureRmResourceGroup -Name "$RGName" -Location "$Location"
    echo "New resource group deployed: $RGName"   
}
else{ echo "Resource group found: $RGName"}

# Deploy ARM template for quiz-app components (for both API and UI)
$scriptDir = Split-Path $MyInvocation.MyCommand.Path 
New-AzureRmResourceGroupDeployment -Verbose -Force `
   -Name "all" `
   -ResourceGroupName $RGName `
   -TemplateFile "$scriptDir/templates/quizapp-all.json" `
   -AppServicePlanName $AppServicePlanName `
   -QuizAPIWebAppName $QuizAPIWebAppName `
   -QuizUIWebAppName $QuizUIWebAppName

# Deploy code onto API Web App:
. "./deploy-webappcode-api.ps1" `
  -SubscriptionName "$SubscriptionName" `
  -RGName "$RGName" `
  -WebAppName $QuizAPIWebAppName

# Deploy code onto UI Web App:
. "./deploy-webappcode-ui.ps1" `
  -SubscriptionName "$SubscriptionName" `
  -RGName "$RGName" `
  -WebAppName $QuizUIWebAppName
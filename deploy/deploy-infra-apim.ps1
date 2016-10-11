#
# deploy ARM APIM infrastructure components
#

[CmdletBinding()]
Param(
  # Params required for provisioning into the exsiting Resource group:
  [Parameter(Mandatory=$True)]
  [string]$SubscriptionName,
  
  [Parameter(Mandatory=$True)]
  [string]$RGName,
  
  [Parameter(Mandatory=$True)]
  [string]$Location,

 # Params required for API Management instance creation:
  [Parameter(Mandatory=$True)]
  [string]$ApimPublisherName,
 
  [Parameter(Mandatory=$True)]
  [string]$ApimPublisherEmail

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
   -Name "apim" `
   -ResourceGroupName $RGName `
   -TemplateFile "$scriptDir/templates/quizapp-apimonly.json" `
   -publisherName  $ApimPublisherName `
   -publisherEmail $ApimPublisherEmail 



Write-Host "Once provisioned, please make sure you enable REST Endpoint for Administration"


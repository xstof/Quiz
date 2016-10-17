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

 # Params required for ASE
  [Parameter(Mandatory=$True)]
  [string]$AseName,

  [Parameter(Mandatory=$True)]
  [string]$NewArmVnetName,

  [Parameter(Mandatory=$True)]
  [string]$SubnetName

  ## note - some of the parameters available in the ARM template are not used here for simplified deployment
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
   -Name "ase" `
   -ResourceGroupName $RGName `
   -TemplateFile "$scriptDir/templates/quizapp-ase-only.json" `
   -aseName  $AseName `
   -aseLocation $Location `
   -newArmVnetName $NewArmVnetName `
   -subnetName $SubnetName 





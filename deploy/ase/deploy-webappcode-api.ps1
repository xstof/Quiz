#
# Deploys code for the API web app onto the prior-deployed Azure infrastructure
#

[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True)]
  [string]$SubscriptionName,
  
  [Parameter(Mandatory=$True)]
  [string]$RGName,
  
  [Parameter(Mandatory=$True)]
  [string]$WebAppName
)

# Determine current working directory:
$invocation = (Get-Variable MyInvocation).Value
$directorypath = Split-Path $invocation.MyCommand.Path
$parentDirectoryPath = (Get-Item $directorypath).Parent.Parent.FullName

# Constants:
$webAppPublishingProfileFileName = $directorypath + "\quizapp-api.publishsettings"
echo "web publishing profile will be stored to: $webAppPublishingProfileFileName"

# Determine which directory to deploy:
 $sourceDirToBuild = $parentDirectoryPath + "\src\Quiz\Quiz.API\"
echo "source directory to build: $sourceDirToBuild"

# Build the quiz api:
Nuget.exe restore "$parentDirectoryPath\src\Quiz"
& "$(Get-Content env:windir)\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" `
 "$sourceDirToBuild\Quiz.API.csproj"  /p:DeployOnBuild=false /p:PublishProfile="quizapp-api" /p:VisualStudioVersion=14.0

# Select Subscription:
Get-AzureRmSubscription -SubscriptionName "$SubscriptionName" | Select-AzureRmSubscription
echo "Selected Azure Subscription"

# Fetch publishing profile for web app:
Get-AzureRmWebAppPublishingProfile -Name $WebAppName -OutputFile $webAppPublishingProfileFileName -ResourceGroupName $RGName
echo "Fetched Azure Web App Publishing Profile: quizapp-api.publishsettings"

# Parse values from .publishsettings file:
[Xml]$publishsettingsxml = Get-Content $webAppPublishingProfileFileName
$websiteName = $publishsettingsxml.publishData.publishProfile[0].msdeploySite
echo "web site name: $websiteName"

$username = $publishsettingsxml.publishData.publishProfile[0].userName
echo "user name: $username"

$password = $publishsettingsxml.publishData.publishProfile[0].userPWD
echo "password: $password"

$computername = $publishsettingsxml.publishData.publishProfile[0].publishUrl
echo "computer name: $computername"

# Deploy the web app api
$msdeploy = "C:\Program Files (x86)\IIS\Microsoft Web Deploy V3\msdeploy.exe"

$msdeploycommand = $("`"{0}`" -verb:sync -source:contentPath=`"{1}`" -dest:contentPath=`"{2}`",computerName=https://{3}/msdeploy.axd?site={4},userName={5},password={6},authType=Basic"   -f $msdeploy, $sourceDirToBuild, $websiteName, $computername, $websiteName, $username, $password)

cmd.exe /C "`"$msdeploycommand`"";
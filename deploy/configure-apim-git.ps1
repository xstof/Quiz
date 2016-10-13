
#
# Configure GIT for APIM via code
#

[CmdletBinding()]
Param(
  

 # Params required for API Management:
  [Parameter(Mandatory=$True)]
  [string]$ApimInstanceName,

  [Parameter(Mandatory=$True)]
  [string]$ApimUid,

  [Parameter(Mandatory=$True)]
  [string]$ApimKey,

  [Parameter(Mandatory=$True)]
  [string]$ApiHostUri,

  [Parameter(Mandatory=$false)]
  [string]$GitHubSourceUri = "https://github.com/katriendg/quiz-aapim-repo.git"

)


$scriptDir = Split-Path $MyInvocation.MyCommand.Path 

$apimVersion = "2014-02-14-preview"
$expiry = (Get-Date).AddDays(1).ToString("O",[System.Globalization.CultureInfo]::InvariantCulture)
$apimInstanceUri = [string]::Format("https://{0}.management.azure-api.net", $ApimInstanceName)
$GitApimCloneUri = [string]::Format("https://{0}.scm.azure-api.net/", $ApimInstanceName)

# Generate the access token for APIM
#======================================

[Byte[]]$keyBytes = [System.Text.Encoding]::UTF8.GetBytes($ApimKey)
$hmacsha = New-Object System.Security.Cryptography.HMACSHA512
$hmacsha.Key = $keyBytes
$message = $ApimUid + "`n" + $expiry;
$signature = $hmacsha.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($message))
$signature = [System.Convert]::ToBase64String($signature)

$apimAccessToken = [string]::Format([System.Globalization.CultureInfo]::InvariantCulture,"SharedAccessSignature uid={0}&ex={1}&sn={2}", $ApimUid, $expiry, $signature)

#echo $apimAccessToken

# Add the property for the dynamic backend
#======================================

$newPropertyName = "QuizApiBackend"
$header = @{
  "Authorization" = "$apimAccessToken"
} 

$propBody =  @{
  name = "QuizApiBackend"
  value = "$ApiHostUri"
  secret= "false"
} 
$json = $propBody | ConvertTo-Json


$callUri = "$apimInstanceUri/properties/$newPropertyName`?api-version=$apimVersion"

Invoke-RestMethod -Method Put -Uri $callUri -Body $json -Header $header -ContentType "application/json"


# Git clone from a good known repo and deploy to APIM
#======================================

cd ..\..\
mkdir $ApimInstanceName
cd $ApimInstanceName
git clone $GitHubSourceUri 
cd quiz-aapim-repo

# Push git to repo
git status
#git config credential.modalPrompt false
git remote add apim $GitApimCloneUri
#TODO add  creditial info programmatically

git status

git add --all
git commit -m "First initial commit to new APIM"

git push apim

git status

cd $scriptDir

echo "Back in original execution folder"

# Call the deploy from repo to APIM command - will take a few minutes until you see it in portal
# You can use the Operation ID to check status for async operations (postman)
#======================================


$header = @{
  "Authorization" = "$apimAccessToken"
} 

$propBody =  @{
  branch = "master"
  force = "true"
} 
$json = $propBody | ConvertTo-Json

$callUri = "$apimInstanceUri/tenant/configuration/deploy`?api-version=$apimVersion"

Invoke-RestMethod -Method POST -Uri $callUri -Body $json -Header $header -ContentType "application/json"

echo "Git deployed to API Management production instance"
echo "Please review the deployed version in APIM Publisher Portal for $ApimInstanceName"
echo "Finished ================================================================"





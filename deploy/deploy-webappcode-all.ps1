#
# Deploys code for both the web apps onto the prior-deployed Azure infrastructure
#

[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True)]
  [string]$SubscriptionName,
  
  [Parameter(Mandatory=$True)]
  [string]$RGName,
  
  # Params required for Web App creation:
  [Parameter(Mandatory=$True)]
  [string]$QuizAPIWebAppName,

  [Parameter(Mandatory=$True)]
  [string]$QuizUIWebAppName
)

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
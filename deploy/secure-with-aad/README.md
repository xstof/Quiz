# Secure the Quiz API with Azure Active Directory

## Create two AAD Applications
Do so in a tenant where you are admin.

- the first app (let's call this the "Client App") is the client to access the Quiz API (it could represent a web application that in trusted subsystem accesses the Quiz API)
- the second app is the app that represents the Quiz API itself

Make sure to request keys (client credentials) for both apps and record these for later use.

To do so in PowerShell, make sure to log in to the tenant associated with the AAD instance you want to register apps into:

    Login-AzureRmAccount -TenantId [YOUR-AAD-GUID-GOES-HERE]

Then you can register both apps:

    # Register Quiz API application
    New-AzureRmADApplication -DisplayName "quizapi" -IdentifierUris "http://[QUIZAPINAME-GOES-HERE].azurewebsites.net" `
    -HomePage "http://[QUIZAPINAME-GOES-HERE].azurewebsites.net" `
    -ReplyUrls "http://[QUIZAPINAME-GOES-HERE].azurewebsites.net" `
    -AvailableToOtherTenants $false
    
    # Register Quiz Client application
    New-AzureRmADApplication -DisplayName "quizapi" -IdentifierUris "http://[QUIZCLIENTNAME-GOES-HERE].azurewebsites.net" `
    -HomePage "http://[QUIZCLIENTNAME-GOES-HERE]" `
    -ReplyUrls "http://[QUIZCLIENTNAME-GOES-HERE]" `
    -AvailableToOtherTenants $false

View the list of registered applications:

     Get-AzureRmADApplication

Then create client credentials for both apps:

    New-AzureRmADAppCredential -ApplicationId [QUIZAPI-APPID-GOES-HERE] -Password [PASSWORD-GOES-HERE]
    New-AzureRmADAppCredential -ApplicationId [QUIZCLIENT-APPID-GOES-HERE] -Password [PASSWORD-GOES-HERE]

And a service principle for at least the Quiz API:

   New-AzureRmADServicePrincipal -ApplicationId [QUIZAPI-APPID-GOES-HERE]

## Configure App Service "EasyAuth"
Likely you'll want to use the Advanced tab, as you need an AAD where you are admin.

- Client ID: is the client ID for the Quiz API itself
- Issuer URL: looks like `https://sts.windows.net/[YOUR-AAD-GUID-GOES-HERE]/` 
- Allowed Token Audiences: add the valid redirect url for the Quiz API

There should be no need for the Client Secret.

## Add application roles to the Quiz API AAD application registration manifest
Edit the manifest and add the following to the `appRoles` entry in the manifest.

    {
        "allowedMemberTypes": ["Application"],
        "description": "Applications can administer quizzes",
        "displayName": "QuizMaker",
        "isEnabled": true,
        "value": "quizmaker",
        "id": "[NEW-GUID-GOES-HERE]"
    },
    {
        "allowedMemberTypes": ["Application"],
        "description": "Applications can take attempts on quizzes",
        "displayName": "QuizTaker",
        "isEnabled": true,
        "value": "quiztaker",
        "id": "[NEW-GUID-GOES-HERE]"
    }

## Configure the Quiz API to require user assignment to get tokens for it
In the classic portal, go to `Configure` and enable the `USER ASSIGNMENT REQUIRED TO ACCESS APP` toggle.  As of know, there needs to be explicit user / application assignment in order to get back a token for our Quiz API.  (Make sure to give consent to the Quiz API application from within the "Users" tab on the portal if the ability to toggle the switch is grayed out.)

## Make the Client App register to access the Quiz API
In the new portal, access the  `Required permissions` blade for our Client API and add the Quiz API.  Assign both roles to the Client App.

## Approve as an admin the permissions the Client App is requiring
Request Admin Consent through a link like this:

    https://login.windows.net/common/oauth2/authorize?response_type=id_token&client_id=[CLIENTID]]&redirect_uri=[CLIENTREDIRECTURL]]&nonce=testnonce&response_mode=form_post&prompt=admin_consent

Alternatively, use the utility provided in this branch: Initiate-Admin-Consent.ps1

    Initiate-Admin-Consent.ps1 -AADTenantName [YOUR-AAD-GUID-GOES-HERE] -ClientID [YOUR-CLIENT-APP-ID-GOES-HERE]

Then login with an administrative (global admin) account and approve the requested permissions for the application.  Upon approval, those permissions will be granted.  (In this case: permission for the client app to access the api with the roles specified.)

## Get a token and call the Quiz API with it
For this, you may use the PowerShell utility that comes in this branch: get-token.ps1

    .\get-token.ps1 -AADTenantName [YOUR-AAD-GUID-GOES-HERE] -AADClientID [YOUR-CLIENT-APP-ID-GOES-HERE] -ClientIDPassword [YOUR-CLIENT-PASSWORD-GOES-HERE] -AADAudienceClientId [APP-ID-FOR-RESOURCE-To-ACCESS-GOES-HERE]

    
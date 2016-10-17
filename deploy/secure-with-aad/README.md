# Secure the Quiz API with Azure Active Directory

## Create two AAD Applications
Do so in a tenant where you are admin.

- the first app (let's call this the "Client App") is the client to access the Quiz API (it could represent a web application that in trusted subsystem accesses the Quiz API)
- the second app is the app that represents the Quiz API itself

Make sure to request keys (client credentials) for both apps and record these for later use.

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
        "value": "quizmaker"
    },
    {
        "allowedMemberTypes": ["Application"],
        "description": "Applications can take attempts on quizzes",
        "displayName": "QuizTaker",
        "isEnabled": true,
        "value": "quiztaker"
    }

## Configure the Quiz API to require user assignment to get tokens for it
In the classic portal, go to `Configure` and enable the `USER ASSIGNMENT REQUIRED TO ACCESS APP` toggle.  As of know, there needs to be explicit user / application assignment in order to get back a token for our Quiz API.

## Make the Client App register to access the Quiz API
In the new portal, access the  `Required permissions` blade for our Client API and add the Quiz API.  Assign both roles to the Client App.

## Approve as an admin the permissions the Client App is requiring
Request Admin Consent through a link like this:

    https://login.windows.net/common/oauth2/authorize?response_type=id_token&client_id=[CLIENTID]]&redirect_uri=[CLIENTREDIRECTURL]]&nonce=testnonce&response_mode=form_post&prompt=admin_consent

## Get a token and call the Quiz API with it
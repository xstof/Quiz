# Registers applications in AAD for service-to-service communications (daemon scenario)
# This script includes the automation of adding roles on the api service principal
# and assigning such role requirement to the api client app registration.

[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True)]
  [string]$AADTenantName, # example: yourdomain.onmicrosoft.com
 
  [Parameter(Mandatory=$True)]
  [string]$QuizAPIName,

  [Parameter(Mandatory=$True)]
  [string]$ClientPassword
)

# Gets authorization token for use with Graph API
function GetAuthToken
{
    param
    (
        [Parameter(Mandatory=$true)]
        $TenantName
    )
    [System.Reflection.Assembly]::LoadFrom("${env:ProgramFiles(x86)}\Microsoft SDKs\Azure\PowerShell\ServiceManagement\Azure\Services\Microsoft.IdentityModel.Clients.ActiveDirectory.dll") | Out-Null
    [System.Reflection.Assembly]::LoadFrom("${env:ProgramFiles(x86)}\Microsoft SDKs\Azure\PowerShell\ServiceManagement\Azure\Services\Microsoft.IdentityModel.Clients.ActiveDirectory.WindowsForms.dll") | Out-Null
    $clientId = "1950a258-227b-4e31-a9cf-717495945fc2"
    $redirectUri = "urn:ietf:wg:oauth:2.0:oob"
    $resourceAppIdURI = "https://graph.windows.net"
    $authority = "https://login.windows.net/$TenantName"
    $authContext = New-Object "Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext" -ArgumentList $authority
    $authResult = $authContext.AcquireToken($resourceAppIdURI, $clientId, $redirectUri, "Auto")
    return $authResult
}

# Makes calls against the Graph API
function PostToGraphApi
{
    param
    (
        [Parameter(Mandatory=$true)]
        $TenantName,
        [Parameter(Mandatory=$true)]
        $AuthHeader,
        [Parameter(Mandatory=$true)]
        $EntityType,
        [Parameter(Mandatory=$true)]
        $Body
    )
    $bodyJson = ConvertTo-Json -InputObject $Body -Depth 100
    $uri = "https://graph.windows.net/$($TenantName)/$($EntityType)?api-version=1.6"
    $result = Invoke-RestMethod -Uri $uri -Headers $AuthHeader -Body $bodyJson -Method POST -Verbose
    return $result
}

$token = GetAuthToken -TenantName $AADTenantName
$tokenAsString = $token.CreateAuthorizationHeader()
Write-Host "Got token to use with Graph API: $tokenAsString"
$authHeader = @{
    "Content-Type"="application/json";
    "Authorization"=$token.CreateAuthorizationHeader()
}

$apiAADIdentifierUris = @("https://$QuizAPIName.azurewebsites.net")
$apiAADRedirectUris = @("https://$QuizAPIName.azurewebsites.net")
$apiAADKeyId = [System.Guid]::NewGuid().ToString()
$quizTakerRoleGuid = [System.Guid]::NewGuid().ToString()
$quizMakerRoleGuid = [System.Guid]::NewGuid().ToString()

# See https://msdn.microsoft.com/Library/Azure/Ad/Graph/api/entity-and-complex-type-reference#application-entity
$apiAADApplicationDefinition = @{
    "appRoles"=@(
        @{
            "allowedMemberTypes"=@("application");
            "displayName"="QuizTaker";
            "id"=$quizTakerRoleGuid;
            "isEnabled"=$True;
            "description"="Applications can take attempts on quizzes";
            "value"="quiztaker";
        },
        @{
            "allowedMemberTypes"=@("application");
            "displayName"="QuizMaker";
            "id"=$quizMakerRoleGuid;
            "isEnabled"=$True;
            "description"="Applications can administer quizzes";
            "value"="quizmaker";
        }
    )
    "displayName"=$QuizAPIName;
    "homepage"="https://$QuizAPIName.azurewebsites.net";
    "identifierUris"= $apiAADRedirectUris;
    "passwordCredentials"=@(
        @{
            "endDate"=[DateTime]::UtcNow.AddDays(365).ToString("u").Replace(" ", "T");
            "keyId"=$apiAADKeyId;
            "startDate"=[DateTime]::UtcNow.AddDays(-1).ToString("u").Replace(" ", "T");  
            "value"=$ClientPassword;
        }
    );
    "oauth2AllowImplicitFlow"=$false;
    "replyUrls"=$apiAADRedirectUris;
    "requiredResourceAccess"=@(
        @{
            "resourceAppId"="00000002-0000-0000-c000-000000000000"; # Azure Active Directory
            "resourceAccess"=@(
                @{
                    "id"="311a71cc-e848-46a1-bdf8-97ff7156d8e6"; # Sign in and read user profile
                    "type"="Scope"; # Delegated Permission (Note: use "Role" for Application Permission)
                }
            )
        }
    );
}

# Register API
$ApiAADApplication = PostToGraphApi -TenantName $AADTenantName -AuthHeader $authHeader -EntityType "applications" -Body $apiAADApplicationDefinition
$ApiAADApplicationId = $ApiAADApplication.appId
Write-Host "Registered api as an application in AAD"

# Create API Service Principal
New-AzureRmADServicePrincipal -ApplicationId $ApiAADApplicationId
Write-Host "Created ServicePrincipal for api"

# Register API Client
$clientAADIdentifierUris = @("https://$($QuizAPIName)Client")
$clientAADRedirectUris = @("https://$($QuizAPIName)Client")
$clientAADKeyId = [System.Guid]::NewGuid().ToString()

# See https://msdn.microsoft.com/Library/Azure/Ad/Graph/api/entity-and-complex-type-reference#application-entity
$clientAADApplicationDefinition = @{
    "displayName"="$($QuizAPIName)Client";
    "homepage"="https://$($QuizAPIName)Client";
    "identifierUris"= $clientAADIdentifierUris;
    "passwordCredentials"=@(
        @{
            "endDate"=[DateTime]::UtcNow.AddDays(365).ToString("u").Replace(" ", "T");
            "keyId"=$clientAADKeyId;
            "startDate"=[DateTime]::UtcNow.AddDays(-1).ToString("u").Replace(" ", "T");  
            "value"=$ClientPassword;
        }
    );
    "oauth2AllowImplicitFlow"=$false;
    "replyUrls"=$clientAADRedirectUris;
    "requiredResourceAccess"=@(
        @{
            "resourceAppId"="00000002-0000-0000-c000-000000000000"; # Azure Active Directory
            "resourceAccess"=@(
                @{
                    "id"="311a71cc-e848-46a1-bdf8-97ff7156d8e6"; # Sign in and read user profile
                    "type"="Scope"; # Delegated Permission (Note: use "Role" for Application Permission)
                }
            )
        },
        @{
            "resourceAppId"=$ApiAADApplicationId; # ID for API ServicePrincipal
            "resourceAccess"=@(
                @{
                    "id"=$quizMakerRoleGuid; # QuizMaker Role
                    "type"="Role"; # Delegated Permission (Note: use "Role" for Application Permission)
                },
                @{
                    "id"=$quizTakerRoleGuid; # QuizTaker Role
                    "type"="Role"; # Delegated Permission (Note: use "Role" for Application Permission)
                }
            )
        }
    );
}

$ClientAADApplication = PostToGraphApi -TenantName $AADTenantName -AuthHeader $authHeader -EntityType "applications" -Body $clientAADApplicationDefinition
$ClientAADApplicationId = $ClientAADApplication.appId
Write-Host "Registered api as an application in AAD"

Write-Host "Initiating admin consent flow now"
.\Initiate-Admin-Consent.ps1 -AADTenantName $AADTenantName -ClientID $ClientAADApplicationId

Write-Host "For testig purposes you might want to try get a token using the client credentials of the api client."
Write-Host "Try executing: .\Get-Token.ps1 -AADTenantId $AADTenantName -AADClientID $ClientAADApplicationId -ClientIDPassword $ClientPassword -AADAudienceClientId $ApiAADApplicationId"

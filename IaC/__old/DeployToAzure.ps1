#####################################################################################
#
# What does this script do?
# This script is used to kick off the process for creating the Azure resources that
# you will need for the workshop. The script itself does not have individual commands
# for each Azure resource, the script will get this information from the ARM template
# microservices.json, located in the provisioning folder. The student should not modify
# the settings in the ARM template.
#
# This script also creates an Azure Key Vault. We create the key vault from PowerShell
# instead of ARM because ARM will not provide a default access policy when it creates
# a key vault
#
# Lastly, this script calls the PullUserSecretInfo.ps1 to fill in User Secrets information
# If you would like to validate that your ARM template is formatted correctly without
# actually doing the deployment, change the $ValidateOnly variable to $true
#
# This script requires that you provide 3 pieces of information:
# 1. Your initials, minimum 5 characters. No special characters like hypens
# 2. Your subscription ID. You can get this from the Azure portal
# 3. The Azure region that you want your resources deployed in
# 4. The name of the resource group to put the resources in
# 
# SUGGESTIONS: 
# If you make any changes to the ARM template, always run this script with
# $ValidateOnly set to $true first
# If you are using your own subscription, set $IsCloudSlice = $false
#####################################################################################
# VARIABLES YOU NEED TO SET
#####################################################################################
# Your initials, use at least 5 characters
# No special characters like hypen or &
# Must start with a letter of the alphabet

$yourInitials = "microv2conf"
#$yourInitials = "your-initials"

# Subscription ID 
$subscriptionId = "c2c53b88-db9f-4780-8318-af2f14b5c1dd"
#$subscriptionId = "your-subscription-id"

# Azure region where you want the resources deployed
# You can retrieve location abbreviations from https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.documents.locationnames.eastus?view=azure-dotnet
$location = "eastus"
#$location = "your-regions"

# Resource group name
# The name of the resource group where your resources will be placed
$resourceGroupName = "microv2conf"
#$resourceGroupName = "your-resource-group"

# **********************************************************************************************
# Should we bounce this script execution?
# **********************************************************************************************
if (($yourInitials -eq '') -or `
    ($yourInitials.Length -le 4) -or `
    ($subscriptionId -eq '') -or `
    ($location -eq '') -or
    ($resourceGroupName -eq ''))
{
	Write-Host 'You must provide your Initials, Subscription ID and Azure region/location before executing' -foregroundcolor Yellow
	exit
}
#####################################################################################
# DO NOT CHANGE ANY OF THE VARIABLES BELOW
#####################################################################################
$ValidateOnly = $false
$IsCloudSlice = $false
$dirDivider = "\"
$armTemplateName = "microservices.json"
$pullSecretsName = "PullUserSecretInfo.ps1"

# this code sets the directory that this script is running in
# so that it knows where to go find the parameters and 
# template.json files. Parameters.json and template.json
# should be in the same directory as this script
$MyDir = $PSScriptRoot
$templateFilePath = "$MyDir$dirDivider$armTemplateName"
$secretsPath = "$MyDir$dirDivider$pullSecretsName"

# For the resource group, it needs to have a prefix name of rgServices
#$resourceGroupName = "rgServices-"

# Your Azure Key Vault name
$kvName = $yourInitials + "keyvault"
# Deployment name used by the ARM template
$deploymentName = $yourInitials + "deploy" + ((Get-Date).ToUniversalTime()).ToString('MMdd-HHmm')

# These parameters will be used with your ARM template
$paramObject = @{
    'yourInitials' = $yourInitials
    'location' = $location
}


# Function checks to see if the user is logged in as administrator
function Test-Administrator
{
    $user = [Security.Principal.WindowsIdentity]::GetCurrent()

    (New-Object Security.Principal.WindowsPrincipal $user).IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)  
}

# Did you open Powershell as and admin?
if(!(Test-Administrator))
{
    Write-Host "Please close PowerShell ISE and open PowerShell ISE with Adminstrator access"
    exit;
}


# If user not logged into Azure account, redirect to login screen
# If user not logged into Azure account, redirect to login screen
# Need to log in to both AzureRM and az login since we are 
# using az commands
if ([string]::IsNullOrEmpty($(Get-AzureRmContext).Account)) 
{
    Login-AzureRmAccount 
    #If more than one under your account
    Select-AzureRmSubscription -SubscriptionId $subscriptionId
    #Verify Current Subscription
    Get-AzureRmSubscription –SubscriptionId $subscriptionId
    $VerbosePreference = "SilentlyContinue"

    # Do an az login since we are also using az commands
    az login
    az account set --subscription $subscriptionId
}


# Function for validating ARM template
function Format-ValidationOutput {
    param ($ValidationOutput, [int] $Depth = 0)
    Set-StrictMode -Off
    return @($ValidationOutput | Where-Object { $_ -ne $null } | ForEach-Object { @('  ' * $Depth + ': ' + $_.Message) + @(Format-ValidationOutput @($_.Details) ($Depth + 1)) })
}

# Check to see if the resource group already exists, if it doesn't $checkforResourceGroup will be null
# For Cloud Slice, the resource group MUST already exist
# For non-Cloud Slice, if the rg exists, the script will use that rg, if not, it will create a new rg
$checkforResourceGroup = Get-AzureRmResourceGroup | Where ResourceGroupName -Like ($resourceGroupName)

# If you are using Cloud Slice, check to make sure the user has entered the correct resource group name
# because students cannot create their own resource groups
if($IsCloudSlice)
{

if($checkforResourceGroup -eq $null)
    {
    Write-Host "The resource group name you entered is incorrect. Please try again"
    exit;
    }
}
else
{
    # If you are not using Cloud Slice, then you can name your resource group anything you wish
    # New-AzureRmResourceGroup will check to see if the rg already exists and if it does it will
    # just put the resources in that rg, otherwise it will create a new rg
    if($checkforResourceGroup -eq $null)
    {
    Write-Host "Resource group '$resourceGroupName' does not exist. Creating a new resource group.";
    Write-Host "Creating resource group '$resourceGroupName' in location '$location'";
    New-AzureRmResourceGroup -Name $resourceGroupName -Location $location -Force
    }
}


# If you are just validating your ARM template
if ($ValidateOnly) 
{
    $ErrorMessages = Format-ValidationOutput (Test-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName `
                                       -TemplateFile $templateFilePath `
                                       -TemplateParameterObject $paramObject)
    if ($ErrorMessages) {
        Write-Output '', 'Validation returned the following errors:', @($ErrorMessages), '', 'Template is invalid.'
    }
    else {
        Write-Output '', 'Template is valid.'
    }
}
else 
{

    # We 'could' create the key vault in ARM, but ARM will not create an access policy
    # for the subscription owner without running other scripts, so we create the vault
    # here. An access policy will automatically be created for the subscription owner
    if (-not (Get-AzureRmKeyVault -VaultName $kvName -ResourceGroupName $resourceGroupName))
    {
     Write-Output "Creating Azure Key Vault"
    New-AzureRmKeyVault -VaultName $kvName `
        -ResourceGroupName $resourceGroupName `
        -Location $location `
        -sku standard `
        -WarningAction SilentlyContinue `
        -EnabledForDeployment `
        -EnabledForDiskEncryption `
        -EnabledForTemplateDeployment

        #Create an environment variable to hold the root key vault endpoint
        #This env variable is used so the app can access key vault secrets
        $kvEndpoint = "https://" + $kvName + ".vault.azure.net/"
        [System.Environment]::SetEnvironmentVariable('KEYVAULT_ENDPOINT',$kvEndpoint,[System.EnvironmentVariableTarget]::Machine)

    }
   
   # Deploy using the ARM template
    $myOutput =(New-AzureRmResourceGroupDeployment -Name $deploymentName `
                                       -ResourceGroupName $resourceGroupName `
                                       -TemplateFile $templateFilePath `
                                       -TemplateParameterObject $paramObject `
                                       -Force -Verbose `
                                       -ErrorVariable ErrorMessages).Outputs

     #Write-Output $myOutput.Values.Value

    if ($ErrorMessages) {
        Write-Output '', 'Template deployment returned the following errors:', @(@($ErrorMessages) | ForEach-Object { $_.Exception.Message.TrimEnd("`r`n") })
   }

   # Call the PS script that scrapes the Azure resources information, puts secrets in key vault and writes to the app user secrets
   $command = $MyDir + $dirDivider + "PullUserSecretInfo.ps1 -YourInitials " + $yourInitials + " -ResourceGroupName " + $resourceGroupName + " -subscriptionId " + $subscriptionId
   Invoke-Expression $command


 }


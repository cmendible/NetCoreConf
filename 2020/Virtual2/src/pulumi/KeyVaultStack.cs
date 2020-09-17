using Pulumi;
using Pulumi.Azure.Core;
using Pulumi.Azure.KeyVault;
using Pulumi.Azure.KeyVault.Inputs;

class KeyVaultStack : Stack
{
    public KeyVaultStack()
    {
        // Get current Subscription
        var currentSubscription = Output.Create(GetSubscription.InvokeAsync());
        var tenantId = currentSubscription.Apply(currentSubscription => currentSubscription.TenantId);

        // Get current Subscription
        var currentClient = Output.Create(GetClientConfig.InvokeAsync());
        var objectId = currentClient.Apply(currentClient => currentClient.ObjectId);

        var config = new Pulumi.Config();
        var resourceGroupName = config.Require("resource_group_name");
        var keyVaultName = config.Require("key_vault_name");

        // Create an Azure Resource Group
        var resourceGroup = new ResourceGroup(resourceGroupName);

        // Create an Azure Key Vault
        var keyVault = new KeyVault(keyVaultName, new KeyVaultArgs
        {
            ResourceGroupName = resourceGroup.Name,
            SkuName = "standard",
            TenantId = tenantId,
            AccessPolicies = {
                new KeyVaultAccessPolicyArgs
                {
                    TenantId = tenantId,
                    ObjectId = objectId,
                    SecretPermissions = { "list", "get" }
                }
            }
        });

        this.VaultUri = keyVault.VaultUri;
    }

    [Output]
    public Output<string> VaultUri { get; set; }
}

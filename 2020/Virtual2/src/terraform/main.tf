# Get current subscription
data "azurerm_subscription" "current" {}

# Get current client
data "azurerm_client_config" "current" {}

# DeployResource Group
resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = "West Europe"
}

# Deploy Key Vault
resource "azurerm_key_vault" "kv" {
  name                = var.key_vault_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  tenant_id           = data.azurerm_subscription.current.tenant_id

  sku_name = "standard"


  # Add read & list permissions to the calling client.
  access_policy {
    tenant_id = data.azurerm_subscription.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    key_permissions = []

    secret_permissions = [
      "get",
      "list",
    ]

    storage_permissions = []
  }
}

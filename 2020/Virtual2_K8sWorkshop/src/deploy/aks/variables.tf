variable resource_group_name {
  default = "netcoreconfv2"
}

variable managed_identity_name {
  default = "netcoreconfv2"
}

variable managed_identity_selector {
  default = "reads-vault"
}

variable location {
  default = "West Europe"
}

variable cluster_name {
  default = "netcoreconfv2-aks"
}

variable "dns_prefix" {
  default = "netcoreconfv2-aks"
}

variable "agent_count" {
  default = 3
}

variable key_vault_name {
  default = "netcoreconfv2-kv"
}

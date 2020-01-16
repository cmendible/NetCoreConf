using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace aspnet.secret.manager
{
    public class PrefixKeyVaultSecretManager : IKeyVaultSecretManager

    {
        private readonly string _prefix;

        public PrefixKeyVaultSecretManager(string prefix)
        {
            _prefix = $"{prefix}-";
        }

        public bool Load(SecretItem secret)
        {
            return secret.Identifier.Name.StartsWith(_prefix);
        }

        public string GetKey(SecretBundle secret)
        {
            return secret.SecretIdentifier.Name
                .Substring(_prefix.Length)
                .Replace("--", ConfigurationPath.KeyDelimiter);
        }
    }
}
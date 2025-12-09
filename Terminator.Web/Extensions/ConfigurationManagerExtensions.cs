namespace Terminator.Web.Extensions;

public static class ConfigurationManagerExtensions
{
    public static ConfigurationManager AddApiSettings(this ConfigurationManager config)
    {
        config.AddEnvironmentVariables("ASPNETCORE_");
        config.AddEnvironmentVariables("DOTNET_");
        config.AddEnvironmentVariables();

        string? secretsPath = config.GetValue<string>("SecretsPath");

        if (!string.IsNullOrEmpty(secretsPath))
        {
            config.AddKeyPerFile(directoryPath: secretsPath);
        }

        return config;
    }
}
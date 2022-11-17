using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TECHIS.CloudFile;
using TECHIS.Core;
using TECHIS.Secrets;


namespace Test.Cloud.AzureStorage
{
    public static class Connector
    {

        private static DefaultKeyVault _defaultSecretStore = new DefaultKeyVault("https://techis-proj-azurestorage.vault.azure.net/");
        internal static string ADControlledStorage = "https://techis4devad.blob.core.windows.net";
        internal static string ADControlledContainerUrl = "https://techis4devad.blob.core.windows.net/cloudfile";

        public static string GetContainerUri() => _defaultSecretStore.GetSecret("ContainerUri");

        /// <summary>
        /// Needs rights to create a container
        /// </summary>
        public static string StorageConnectionString => _defaultSecretStore.GetSecret("ConnectionString");

        private static IApplicationSettings applicationSettings = new InMemoryApplicationSettings((nameof( DefaultAppCredentialTokenFactory.DefaultAppTokenType), "VisualStudioCredential"));
        public static IApplicationSettings GetApplicationSettings()=> applicationSettings;


        public static ILogger<TCategory> GetLogger<TCategory>()
        {
            //new ServiceCollection().AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.AddConsole();
            //});
            //return new ServiceCollection().BuildServiceProvider(true)
            //    .GetService<ILoggerFactory>()
            //    .CreateLogger<TCategory>();
            var logger = new Mock<ILogger<TCategory>>();
            //logger.Setup(o => o.LogInformation(It.IsAny<string>()));
            return logger.Object;
        }
    }
}

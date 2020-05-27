﻿using System;

using KeyVault.CertificateRotation;
using KeyVault.CertificateRotation.Internal;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Management.Cdn;
using Microsoft.Azure.Management.FrontDoor;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Rest;

[assembly: FunctionsStartup(typeof(Startup))]

namespace KeyVault.CertificateRotation
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var subscriptionId = Environment.GetEnvironmentVariable("WEBSITE_OWNER_NAME").Split('+')[0];

            builder.Services.AddSingleton(provider =>
                new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback)));

            builder.Services.AddSingleton(provider => new CdnManagementClient(new TokenCredentials(new AppAuthenticationTokenProvider()))
            {
                SubscriptionId = subscriptionId
            });

            builder.Services.AddSingleton(provider => new FrontDoorManagementClient(new TokenCredentials(new AppAuthenticationTokenProvider()))
            {
                SubscriptionId = subscriptionId
            });
        }
    }
}

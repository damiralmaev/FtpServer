// <copyright file="ISafeCommunicationService.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace FubarDev.FtpServer.ConnectionHandlers
{
    public interface ISafeCommunicationService : IBasicCommunicationService, ICommunicationChannelService
    {
        [NotNull]
        Task ResetAsync(CancellationToken cancellationToken);

        [NotNull]
        Task EnableSslStreamAsync([NotNull] X509Certificate2 certificate, CancellationToken cancellationToken);
    }
}
// <copyright file="UnixDirectoryEntry.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;

using FubarDev.FtpServer.AccountManagement;

using JetBrains.Annotations;

using Mono.Unix;

namespace FubarDev.FtpServer.FileSystem.Unix
{
    internal class UnixDirectoryEntry : UnixFileSystemEntry, IUnixDirectoryEntry
    {
        public UnixDirectoryEntry(
            [NotNull] UnixDirectoryInfo info,
            IFtpUser user,
            UnixUserInfo userInfo,
            IUnixDirectoryEntry parent = null)
            : base(info)
        {
            IsRoot = parent == null;
            Info = info;

            if (parent == null)
            {
                // Root user
                IsDeletable = false;
            }
            else if (info.Parent == info)
            {
                // File system root
                IsDeletable = false;
            }
            else
            {
                IsDeletable = parent.GetEffectivePermissions(user, userInfo.UserId, userInfo.GroupId).Write;
            }
        }

        /// <summary>
        /// Gets the unix directory info.
        /// </summary>
        public UnixDirectoryInfo Info { get; }

        /// <inheritdoc />
        public bool IsRoot { get; }

        /// <inheritdoc />
        public bool IsDeletable { get; }
    }
}

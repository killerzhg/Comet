﻿using Comet.Common.Models;
using System.Collections.Generic;

namespace Comet.Client.Recovery
{
    /// <summary>
    /// Provides a common way to read stored accounts from applications.
    /// </summary>
    public interface IUsers
    {
        /// <summary>
        /// Reads the stored accounts of the application.
        /// </summary>
        /// <returns>A list of recovered accounts</returns>
        IEnumerable<SaveUser> ReadAccounts();

        /// <summary>
        /// The name of the application.
        /// </summary>
        string ApplicationName { get; }
    }
}

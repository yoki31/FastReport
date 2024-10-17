﻿using System;

namespace FastReport.Web.Cache
{
    /// <summary>
    /// Represents the cache where all webReports will be stored
    /// </summary>
    public interface IWebReportCache : IDisposable
    {
        void Add(WebReport webReport);

        void Touch(string id);

        WebReport Find(string id);

        void Remove(WebReport webReport);

    }
}
﻿namespace PVOutput.Net.Requests.Handler
{
    using System.Net.Http;

    public interface IHttpClientProvider
    {
        HttpClient GetHttpClient();
    }
}
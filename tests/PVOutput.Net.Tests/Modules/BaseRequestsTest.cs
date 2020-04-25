﻿using NUnit.Framework;
using PVOutput.Net.Responses;

namespace PVOutput.Net.Tests.Modules
{
    public class BaseRequestsTest
    {
        protected static void AssertStandardResponse<TResponseContentType>(PVOutputResponse<TResponseContentType> response)
        {
            Assert.Multiple(() =>
            {
                Assert.IsNull(response.Error);
                Assert.IsTrue(response.HasValue);
                Assert.IsTrue(response.IsSuccess);
                Assert.IsNotNull(response.Value);
            });
        }

        protected static void AssertStandardResponse<TResponseContentType>(PVOutputArrayResponse<TResponseContentType> response)
        {
            Assert.Multiple(() =>
            {
                Assert.IsNull(response.Error);
                Assert.IsTrue(response.HasValues);
                Assert.IsTrue(response.IsSuccess);
                Assert.IsNotNull(response.Values);
            });
        }

        protected static void AssertStandardResponse(PVOutputBasicResponse response)
        {
            Assert.Multiple(() =>
            {
                Assert.IsNull(response.Error);
                Assert.IsTrue(response.IsSuccess);
            });
        }
    }
}

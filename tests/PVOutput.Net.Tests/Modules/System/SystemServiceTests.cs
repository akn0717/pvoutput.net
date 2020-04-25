﻿using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using PVOutput.Net.Enums;
using PVOutput.Net.Objects.Factories;
using PVOutput.Net.Objects;
using PVOutput.Net.Tests.Utils;
using RichardSzalay.MockHttp;
using PVOutput.Net.Objects.Modules.Implementations;
using System.Collections.Generic;

namespace PVOutput.Net.Tests.Modules.System
{
    [TestFixture]
    public partial class SystemServiceTests : BaseRequestsTest
    {
        [Test]
        public async Task SystemService_GetOwnSystem_CallsCorrectUri()
        {
            PVOutputClient client = TestUtility.GetMockClient(out MockHttpMessageHandler testProvider);

            testProvider.ExpectUriFromBase(GETSYSTEM_URL)
                        .RespondPlainText(SYSTEM_RESPONSE_EXTENDED);

            var response = await client.System.GetOwnSystemAsync();
            testProvider.VerifyNoOutstandingExpectation();
            AssertStandardResponse(response);
        }

        [Test]
        public async Task SystemService_GetOtherSystem_CallsCorrectUri()
        {
            PVOutputClient client = TestUtility.GetMockClient(out MockHttpMessageHandler testProvider);

            testProvider.ExpectUriFromBase(GETSYSTEM_URL)
                        .WithQueryString("sid1=54321")
                        .RespondPlainText(SYSTEM_RESPONSE_EXTENDED);

            var response = await client.System.GetOtherSystemAsync(54321);
            testProvider.VerifyNoOutstandingExpectation();
            AssertStandardResponse(response);
        }

        [Test]
        public async Task SystemService_PostSystem_CallsCorrectUri()
        {
            PVOutputClient client = TestUtility.GetMockClient(out MockHttpMessageHandler testProvider);

            testProvider.ExpectUriFromBase(POSTSYSTEM_URL)
                        .WithExactQueryString("sid=54321&name=New%20name&v7l=New%20power&v7u=W")
                        .RespondPlainText("");

            var response = await client.System.PostSystem(54321, "New name", new List<IExtendedDataDefinition>() { new ExtendedDataDefinition() { Label = "New power", Unit = "W" } });
            testProvider.VerifyNoOutstandingExpectation();
            AssertStandardResponse(response);
        }

        /*
         * Deserialisation tests below
         */

        // TODO: not all aspects are being tested right now

        [Test]
        public async Task SystemReader_ForResponse_CreatesCorrectObject()
        {
            ISystem result = await TestUtility.ExecuteObjectReaderByTypeAsync<ISystem>(SYSTEM_RESPONSE_EXTENDED);

            Assert.AreEqual("Test System", result.SystemName);
            Assert.AreEqual(1, result.Donations);
            Assert.AreEqual(10.65, result.ImportDailyCharge);
            Assert.AreEqual("DC-2 Voltage", result.ExtendedDataConfig[1].Label);
            Assert.AreEqual(159, result.MonthlyGenerationEstimates[PVMonth.October]);
            Assert.AreEqual(0, result.MonthlyConsumptionEstimates[PVMonth.January]);
        }
    }
}

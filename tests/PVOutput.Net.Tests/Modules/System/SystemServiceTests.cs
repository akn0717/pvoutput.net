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
using System;
using System.Linq;

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

        [Test]
        public async Task SystemReader_ForResponse_CreatesCorrectObject()
        {
            ISystem result = await TestUtility.ExecuteObjectReaderByTypeAsync<ISystem>(SYSTEM_RESPONSE_EXTENDED);

            Assert.That(result.SystemName, Is.EqualTo("Test System"));
            Assert.That(result.SystemSize, Is.EqualTo(4125));
            Assert.That(result.Postcode, Is.EqualTo(1234));
            Assert.That(result.NumberOfPanels, Is.EqualTo(15));
            Assert.That(result.PanelPower, Is.EqualTo(275));
            Assert.That(result.PanelBrand, Is.EqualTo("JA Solar mono"));
            Assert.That(result.NumberOfInverters, Is.EqualTo(1));
            Assert.That(result.InverterPower, Is.EqualTo(5500));
            Assert.That(result.InverterBrand, Is.EqualTo("Fronius Primo 3.6-1"));
            Assert.That(result.Orientation, Is.EqualTo("E"));
            Assert.That(result.ArrayTilt, Is.EqualTo(53.1d));
            Assert.That(result.Shade, Is.EqualTo("None"));
            Assert.That(result.InstallDate, Is.EqualTo(new DateTime(2016, 8, 22)));
            Assert.That(result.Location.Latitude, Is.EqualTo(51.0d));
            Assert.That(result.Location.Longitude, Is.EqualTo(6.1d));
            Assert.That(result.StatusInterval, Is.EqualTo(5));
            Assert.That(result.SecondaryNumberOfPanels, Is.EqualTo(10));
            Assert.That(result.SecondaryPanelPower, Is.EqualTo(190));
            Assert.That(result.SecondaryOrientation, Is.EqualTo("W"));
            Assert.That(result.SecondaryArrayTilt, Is.EqualTo(33.5d));
            Assert.That(result.ExportTariff, Is.EqualTo(17.37d));
            Assert.That(result.ImportPeakTariff, Is.EqualTo(20.46d));
            Assert.That(result.ImportOffPeakTariff, Is.EqualTo(20.2d));
            Assert.That(result.ImportShoulderTariff, Is.EqualTo(25.4d));
            Assert.That(result.ImportHighShoulderTariff, Is.EqualTo(22.65));
            Assert.That(result.ImportDailyCharge, Is.EqualTo(40d));
            Assert.That(result.Teams, Has.Exactly(4).Items);
            Assert.That(result.Teams.First(), Is.EqualTo(12));
            Assert.That(result.Teams.Last(), Is.EqualTo(512));
            Assert.That(result.Donations, Is.EqualTo(1));
            Assert.That(result.ExtendedDataConfig, Has.Exactly(4).Items);
            Assert.That(result.ExtendedDataConfig.First().Label, Is.EqualTo("DC-1 Voltage"));
            Assert.That(result.ExtendedDataConfig.First().Unit, Is.EqualTo("V"));
            Assert.That(result.ExtendedDataConfig.Last().Label, Is.EqualTo("DC-1 Power (2x13x290Wp)"));
            Assert.That(result.ExtendedDataConfig.Last().Unit, Is.EqualTo("W"));
            Assert.That(result.MonthlyGenerationEstimates[PVMonth.October], Is.EqualTo(159));
            Assert.That(result.MonthlyGenerationEstimates[PVMonth.August], Is.EqualTo(354));            
            Assert.That(result.MonthlyConsumptionEstimates[PVMonth.February], Is.EqualTo(350));
            Assert.That(result.MonthlyConsumptionEstimates[PVMonth.August], Is.EqualTo(175));
        }

        [Test]
        public async Task SystemReader_ForResponseWithoutTeams_CreatesCorrectObject()
        {
            ISystem result = await TestUtility.ExecuteObjectReaderByTypeAsync<ISystem>(SYSTEM_RESPONSE_WITHOUT_TEAMS);
            Assert.That(result.SystemName, Is.EqualTo("Test System"));
            Assert.That(result.SystemSize, Is.EqualTo(4125));
            Assert.That(result.Postcode, Is.EqualTo(1234));
            Assert.That(result.NumberOfPanels, Is.EqualTo(15));
            Assert.That(result.PanelPower, Is.EqualTo(275));
            Assert.That(result.PanelBrand, Is.EqualTo("JA Solar mono"));
            Assert.That(result.NumberOfInverters, Is.EqualTo(1));
            Assert.That(result.InverterPower, Is.EqualTo(5500));
            Assert.That(result.InverterBrand, Is.EqualTo("Fronius Primo 3.6-1"));
            Assert.That(result.Orientation, Is.EqualTo("E"));
            Assert.That(result.ArrayTilt, Is.EqualTo(53.1d));
            Assert.That(result.Shade, Is.EqualTo("None"));
            Assert.That(result.InstallDate, Is.EqualTo(new DateTime(2016, 8, 22)));
            Assert.That(result.Location.Latitude, Is.EqualTo(51.0d));
            Assert.That(result.Location.Longitude, Is.EqualTo(6.1d));
            Assert.That(result.StatusInterval, Is.EqualTo(5));
            Assert.That(result.SecondaryNumberOfPanels, Is.EqualTo(10));
            Assert.That(result.SecondaryPanelPower, Is.EqualTo(190));
            Assert.That(result.SecondaryOrientation, Is.EqualTo("W"));
            Assert.That(result.SecondaryArrayTilt, Is.EqualTo(33.5d));
            Assert.That(result.ExportTariff, Is.EqualTo(17.37d));
            Assert.That(result.ImportPeakTariff, Is.EqualTo(20.46d));
            Assert.That(result.ImportOffPeakTariff, Is.EqualTo(20.2d));
            Assert.That(result.ImportShoulderTariff, Is.EqualTo(25.4d));
            Assert.That(result.ImportHighShoulderTariff, Is.EqualTo(22.65));
            Assert.That(result.ImportDailyCharge, Is.EqualTo(40d));
            Assert.That(result.Teams, Has.Count.Zero);
            Assert.That(result.Donations, Is.EqualTo(1));
            Assert.That(result.ExtendedDataConfig, Has.Exactly(4).Items);
            Assert.That(result.ExtendedDataConfig.First().Label, Is.EqualTo("DC-1 Voltage"));
            Assert.That(result.ExtendedDataConfig.First().Unit, Is.EqualTo("V"));
            Assert.That(result.ExtendedDataConfig.Last().Label, Is.EqualTo("DC-1 Power (2x13x290Wp)"));
            Assert.That(result.ExtendedDataConfig.Last().Unit, Is.EqualTo("W"));
            Assert.That(result.MonthlyGenerationEstimates[PVMonth.October], Is.EqualTo(159));
            Assert.That(result.MonthlyGenerationEstimates[PVMonth.August], Is.EqualTo(354));
            Assert.That(result.MonthlyConsumptionEstimates[PVMonth.February], Is.EqualTo(350));
            Assert.That(result.MonthlyConsumptionEstimates[PVMonth.August], Is.EqualTo(175));
        }

        [Test]
        public async Task SystemReader_ForResponseWithoutExtendedProperties_CreatesCorrectObject()
        {
            ISystem result = await TestUtility.ExecuteObjectReaderByTypeAsync<ISystem>(SYSTEM_RESPONSE_WITHOUT_EXTENDEDDATA);

            Assert.That(result.SystemName, Is.EqualTo("Test System"));
            Assert.That(result.SystemSize, Is.EqualTo(4125));
            Assert.That(result.Postcode, Is.EqualTo(1234));
            Assert.That(result.NumberOfPanels, Is.EqualTo(15));
            Assert.That(result.PanelPower, Is.EqualTo(275));
            Assert.That(result.PanelBrand, Is.EqualTo("JA Solar mono"));
            Assert.That(result.NumberOfInverters, Is.EqualTo(1));
            Assert.That(result.InverterPower, Is.EqualTo(5500));
            Assert.That(result.InverterBrand, Is.EqualTo("Fronius Primo 3.6-1"));
            Assert.That(result.Orientation, Is.EqualTo("E"));
            Assert.That(result.ArrayTilt, Is.EqualTo(53.1d));
            Assert.That(result.Shade, Is.EqualTo("None"));
            Assert.That(result.InstallDate, Is.EqualTo(new DateTime(2016, 8, 22)));
            Assert.That(result.Location.Latitude, Is.EqualTo(51.0d));
            Assert.That(result.Location.Longitude, Is.EqualTo(6.1d));
            Assert.That(result.StatusInterval, Is.EqualTo(5));
            Assert.That(result.SecondaryNumberOfPanels, Is.EqualTo(10));
            Assert.That(result.SecondaryPanelPower, Is.EqualTo(190));
            Assert.That(result.SecondaryOrientation, Is.EqualTo("W"));
            Assert.That(result.SecondaryArrayTilt, Is.EqualTo(33.5d));
            Assert.That(result.ExportTariff, Is.EqualTo(17.37d));
            Assert.That(result.ImportPeakTariff, Is.EqualTo(20.46d));
            Assert.That(result.ImportOffPeakTariff, Is.EqualTo(20.2d));
            Assert.That(result.ImportShoulderTariff, Is.EqualTo(25.4d));
            Assert.That(result.ImportHighShoulderTariff, Is.EqualTo(22.65));
            Assert.That(result.ImportDailyCharge, Is.EqualTo(40d));
            Assert.That(result.Teams, Has.Exactly(4).Items);
            Assert.That(result.Teams.First(), Is.EqualTo(12));
            Assert.That(result.Teams.Last(), Is.EqualTo(512));
            Assert.That(result.Donations, Is.EqualTo(1));
            Assert.That(result.ExtendedDataConfig, Has.Count.Zero);
            Assert.That(result.MonthlyGenerationEstimates[PVMonth.October], Is.EqualTo(159));
            Assert.That(result.MonthlyGenerationEstimates[PVMonth.August], Is.EqualTo(354));
            Assert.That(result.MonthlyConsumptionEstimates[PVMonth.February], Is.EqualTo(350));
            Assert.That(result.MonthlyConsumptionEstimates[PVMonth.August], Is.EqualTo(175));
        }

        [Test]
        public async Task SystemReader_ForResponseWithoutEstimates_CreatesCorrectObject()
        {
            ISystem result = await TestUtility.ExecuteObjectReaderByTypeAsync<ISystem>(SYSTEM_RESPONSE_WITHOUT_ESTIMATES);

            Assert.That(result.SystemName, Is.EqualTo("Test System"));
            Assert.That(result.SystemSize, Is.EqualTo(4125));
            Assert.That(result.Postcode, Is.EqualTo(1234));
            Assert.That(result.NumberOfPanels, Is.EqualTo(15));
            Assert.That(result.PanelPower, Is.EqualTo(275));
            Assert.That(result.PanelBrand, Is.EqualTo("JA Solar mono"));
            Assert.That(result.NumberOfInverters, Is.EqualTo(1));
            Assert.That(result.InverterPower, Is.EqualTo(5500));
            Assert.That(result.InverterBrand, Is.EqualTo("Fronius Primo 3.6-1"));
            Assert.That(result.Orientation, Is.EqualTo("E"));
            Assert.That(result.ArrayTilt, Is.EqualTo(53.1d));
            Assert.That(result.Shade, Is.EqualTo("None"));
            Assert.That(result.InstallDate, Is.EqualTo(new DateTime(2016, 8, 22)));
            Assert.That(result.Location.Latitude, Is.EqualTo(51.0d));
            Assert.That(result.Location.Longitude, Is.EqualTo(6.1d));
            Assert.That(result.StatusInterval, Is.EqualTo(5));
            Assert.That(result.SecondaryNumberOfPanels, Is.EqualTo(10));
            Assert.That(result.SecondaryPanelPower, Is.EqualTo(190));
            Assert.That(result.SecondaryOrientation, Is.EqualTo("W"));
            Assert.That(result.SecondaryArrayTilt, Is.EqualTo(33.5d));
            Assert.That(result.ExportTariff, Is.EqualTo(17.37d));
            Assert.That(result.ImportPeakTariff, Is.EqualTo(20.46d));
            Assert.That(result.ImportOffPeakTariff, Is.EqualTo(20.2d));
            Assert.That(result.ImportShoulderTariff, Is.EqualTo(25.4d));
            Assert.That(result.ImportHighShoulderTariff, Is.EqualTo(22.65));
            Assert.That(result.ImportDailyCharge, Is.EqualTo(40d));
            Assert.That(result.Teams, Has.Exactly(4).Items);
            Assert.That(result.Teams.First(), Is.EqualTo(12));
            Assert.That(result.Teams.Last(), Is.EqualTo(512));
            Assert.That(result.Donations, Is.EqualTo(1));
            Assert.That(result.ExtendedDataConfig, Has.Exactly(4).Items);
            Assert.That(result.ExtendedDataConfig.First().Label, Is.EqualTo("DC-1 Voltage"));
            Assert.That(result.ExtendedDataConfig.First().Unit, Is.EqualTo("V"));
            Assert.That(result.ExtendedDataConfig.Last().Label, Is.EqualTo("DC-1 Power (2x13x290Wp)"));
            Assert.That(result.ExtendedDataConfig.Last().Unit, Is.EqualTo("W"));

            Assert.That(result.MonthlyGenerationEstimates, Has.Count.Zero);
            Assert.That(result.MonthlyConsumptionEstimates, Has.Count.Zero);
        }
    }
}

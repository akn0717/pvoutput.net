# PVOutput.Net

> A .NET Core (Standard 2.0 compatible) wrapper library for API of the popular [PVOutput](https://pvoutput.org) service.
> PVOutput is a free service for sharing and comparing PV output data.

![GitHub last commit (master)](https://img.shields.io/github/last-commit/pyrocumulus/PVOutput.Net/master?label=last%20commit%20%28master%29)
[![NuGet Version](https://img.shields.io/nuget/v/PVOutput.Net.svg?logo=nuget)](https://www.nuget.org/packages/PVOutput.Net/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/PVOutput.Net.svg?logo=nuget)](https://www.nuget.org/packages/PVOutput.Net/)
[![fuget](https://www.fuget.org/packages/PVOutput.Net/badge.svg)](https://www.fuget.org/packages/PVOutput.Net)
![.NET Core](https://img.shields.io/github/workflow/status/pyrocumulus/PVOutput.Net/.NET%20Core/develop)

## Installation

Installation can be done through installation of the [NuGet package](https://www.nuget.org/packages/PVOutput.Net/).

## Usage

### Getting data out of PVOutput.org

```csharp
var client = new PVOutputClient(apiKey: "myPvOutputKey", ownedSystemId: 1);

// Request output for today
var outputResponse = await client.Output.GetOutputForDateAsync(DateTime.Today);
var output = outputResponse.Value;
Console.WriteLine($"Output for date {output.OutputDate.ToShortDateString()}, 
    {output.EnergyGenerated} Wh generated");

```

### Adding data to a system in PVOutput.org

```csharp
var client = new PVOutputClient(apiKey: "myPvOutputKey", ownedSystemId: 1);
var builder = new StatusPostBuilder<IStatusPost>();

// Build the status
var status = builder.SetTimeStamp(DateTime.Now)
                .SetGeneration(200, null)
                .Build();

// Push the status back to PVOutput
var response = await client.Status.AddStatusAsync(status);

```

### Using the client in an ASP.Net Core application

```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddPVOutputClient(options =>
        {
            options.ApiKey = "myPvOutputKey";
            options.OwnedSystemId = 1;
        });
    }
```

For more information on usage, please see the [documentation](https://pyrocumulus.github.io/pvoutput.net/).

## API Coverage

The library covers almost nearly all the official PVOutput API exposes. See [documentation](https://pyrocumulus.github.io/pvoutput.net/) for details.

## Building the project

As the whole solution has all that dotnet magic, you can just run:

```posh
dotnet build
```

to build the solution as a whole or a single project.

Running the Nunit tests can also be done from the cli, just run:

```posh
dotnet test
```

## Contribute

See [Contributing](CONTRIBUTING.md) for information on how to contribute to this project.

## License

This project's structure and Request handling have been seriously inspired (in part, copied even) by Henrik Fröhling's work on [Trakt.NET](https://github.com/henrikfroehling/Trakt.NET), when it was still called TraktApiSharp. While this project is licensed under the same license as Trakt.NET, I'd still like to make this absolutely clear.

MIT © Marcel Boersma

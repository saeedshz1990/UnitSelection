﻿using Microsoft.Extensions.Configuration;
using Xunit;

namespace UnitSelection.Specs.Infrastructure;

public class ConfigurationFixture
{
    public TestSettings Value { get; private set; }

    public ConfigurationFixture()
    {
        Value = GetSettings();
    }

    private TestSettings GetSettings()
    {
        var settings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettingsTest.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .AddCommandLine(Environment.GetCommandLineArgs())
            .Build();

        var testSettings = new TestSettings();
        settings.Bind(testSettings);
        return testSettings;
    }
}

public class TestSettings
{
    public string DbConnectionString { get; set; }
}

[CollectionDefinition(nameof(ConfigurationFixture), DisableParallelization = false)]
public class ConfigurationCollectionFixture : ICollectionFixture<ConfigurationFixture>
{
}
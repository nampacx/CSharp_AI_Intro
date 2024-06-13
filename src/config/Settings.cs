// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.DotNet.Interactive;
using InteractiveKernel = Microsoft.DotNet.Interactive.Kernel;

// ReSharper disable InconsistentNaming

public static class Settings
{
    private const string DefaultConfigFile = "config/settings.json";
    private const string TypeKey = "type";
    private const string ModelKey = "model";
    private const string EndpointKey = "endpoint";
    private const string SecretKey = "apikey";
    private const string AiSearchEndpoint = "aisearchendpoint";
    private const string AiSearchKey = "aisearchkey";

    private const bool StoreConfigOnFile = true;

    // Load settings from file
    public static (
        bool useAzureOpenAI,
        string model,
        string azureEndpoint,
        string apiKey,
        string aisearchendpoint,
        string aisearchkey
    ) LoadFromFile(string configFile = DefaultConfigFile)
    {
        if (!File.Exists(configFile))
        {
            Console.WriteLine("Configuration not found: " + configFile);
            Console.WriteLine(
                "\nPlease run the Setup Notebook (0-AI-settings.ipynb) to configure your AI backend first.\n"
            );
            throw new Exception(
                "Configuration not found, please setup the notebooks first using notebook 0-AI-settings.pynb"
            );
        }

        try
        {
            var config = JsonSerializer.Deserialize<Dictionary<string, string>>(
                File.ReadAllText(configFile)
            );
            bool useAzureOpenAI = config[TypeKey] == "azure";
            string model = config[ModelKey];
            string azureEndpoint = config[EndpointKey];
            string apiKey = config[SecretKey];
            string aisearchendpoint = config[AiSearchEndpoint];
            string aisearchkey = config[AiSearchKey];

            return (
                useAzureOpenAI,
                model,
                azureEndpoint,
                apiKey,
                aisearchendpoint,
                aisearchkey
            );
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong: " + e.Message);
            return (true, "", "", "", "", "");
        }
    }
}

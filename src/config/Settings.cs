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
    private const string TextToImageModelKey = "texttoimagemodel";

    private const bool StoreConfigOnFile = true;

    // Load settings from file
    public static (
        bool useAzureOpenAI,
        string model,
        string texttoimagemodel,
        string azureEndpoint,
        string apiKey,
        string aisearchendpoint,
        string aisearchkey
    ) LoadFromFile(string configFile = DefaultConfigFile)
    {
        if (!File.Exists(configFile))
        {
            var str = $"Configuration not found: {configFile}";
            Console.WriteLine(str);
          
            throw new FileNotFoundException(
                str
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
            string texttoimagemodel= config[TextToImageModelKey];

            return (
                useAzureOpenAI,
                model,
                texttoimagemodel,
                azureEndpoint,
                apiKey,
                aisearchendpoint,
                aisearchkey
            );
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong: " + e.Message);
            return (true, "","", "", "", "", "");
        }
    }
}

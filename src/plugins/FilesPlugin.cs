// Copyright (c) Microsoft. All rights reserved.

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.SemanticKernel;
using Kernel = Microsoft.SemanticKernel.Kernel;

internal sealed class FilesPlugin
{
    private const bool logging = false;

    [KernelFunction, Description("Reads all text from a file.")]
    public string ReadFileContent([Description("Path to the file")] string filePath)
    {
        if (filePath is null or "")
        {
            return $"Error reading file {filePath} Please provide a file path to read the content.";
        }

        return File.ReadAllText(filePath);
    }

    [KernelFunction, Description("Takes some string content and writes it to a given file path.")]
    public string WriteFileContent([Description("Path to the file")] string filePath, [Description("Content to write")] string content)
    {
        if (filePath is null or "")
        {
            return $"Error writing to file {filePath}. Please provide a valid file path.";
        }

        try
        {
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(filePath, content);
            return $"Successfully wrote to file {filePath}.";
        }
        catch (Exception ex)
        {
            return $"Error writing to file {filePath}: {ex.Message}";
        }
    }

    [KernelFunction, Description("List all files in a given directory.")]
    public string[] ListFilesForGivenDirectory([Description("Directory path")] string directoryPath)
    {
        ReportCalling();
        return Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
    }

    [KernelFunction, Description("List all files in the current directory.")]
    public string[] ListFilesInCurrentDirectory()
    {
        ReportCalling();
        return Directory.GetFiles(
            Directory.GetCurrentDirectory(),
            "*",
            SearchOption.AllDirectories
        );
    }

    private static void ReportCalling([CallerMemberName] string caller = "")
    {
        if (logging)
            Console.WriteLine($"Called by: {caller}");
    }
}

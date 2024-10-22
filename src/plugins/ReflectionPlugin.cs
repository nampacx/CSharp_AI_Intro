using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.SemanticKernel;
using System.Runtime.CompilerServices;

public sealed class ReflectionPlugin
{
    private const bool logging = true;

    private static void ReportCalling([CallerMemberName] string caller = "")
    {
        if (logging)
            Console.WriteLine($"Called by: {caller}");
    }

    [KernelFunction, Description("Find a type in an assembly given by a path")]
    public static string FindTypeInAssembly(
        [Description("The path to the assembly")] string assemblyPath,
        [Description("The name of the type to search for")] string typeName
    )
    {
        ReportCalling();

        try
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            var fileName = Path.GetFileNameWithoutExtension(assemblyPath);
            var types = assembly.GetTypes().Where(t => t.FullName.StartsWith(fileName));
            var type = types.Where(t => t.FullName.Contains(typeName)).FirstOrDefault();

            if (type == null)
            {
                return $"Type not found: {typeName}";
            }
            var info = AssemblyDocumentationHelper.GetXmlComments(type);

            return info;
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

        [KernelFunction, Description("Get member info of a method or property of a given type.")]
    public static string GetMemberInfo(
        [Description("The path to the assembly")] string assemblyPath,
        [Description("The name of the type to search for")] string typeName,
        [Description("The name of the member to search for")] string memberName
    )
    {
        ReportCalling();

        try
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            var fileName = Path.GetFileNameWithoutExtension(assemblyPath);
            var types = assembly.GetTypes().Where(t => t.FullName.StartsWith(fileName));
            var type = types.Where(t => t.FullName.Contains(typeName)).FirstOrDefault();

            if (type == null)
            {
                return $"Type not found: {typeName}";
            }

            var member = type.GetMember(memberName).FirstOrDefault();
            var info = AssemblyDocumentationHelper.GetXmlComments(member);

            return info;
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    [KernelFunction, Description("Returns information about the members (Methods and Properties) of a given type of a given assembly.")]
    public static string GetMembers(
        [Description("The path to the assembly")] string assemblyPath,
        [Description("The name of the type to search for")] string typeName
    )
    {
        ReportCalling();

        try
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            var fileName = Path.GetFileNameWithoutExtension(assemblyPath);
            var types = assembly.GetTypes().Where(t => t.FullName.StartsWith(fileName));
            var type = types.Where(t => t.FullName.Contains(typeName)).FirstOrDefault();

            if (type == null)
            {
                return $"Type not found: {typeName}";
            }

            var members = type.GetMembers().Select(m =>new { m.Name,MemberType = m.MemberType.ToString()});

            return JsonSerializer.Serialize(members);
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    [KernelFunction, Description("Searches for DLL files matching a given assembly name" )]
    public static string FindAssembly([Description("Name of a assembly")] string assemblyName )
    {
        ReportCalling();
        var nugetPath = GetNugetDirectory();
        var dirs = Directory.GetFiles(nugetPath, $"{assemblyName}*.dll", SearchOption.AllDirectories);

        return JsonSerializer.Serialize(dirs);
    }


    private static string GetNugetDirectory()
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        
        // Construct the path to the .nuget directory
        var nugetPath = Path.Combine(userProfile, ".nuget");
        return nugetPath;
    }
}
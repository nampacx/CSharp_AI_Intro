using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;

public static class AssemblyDocumentationHelper
{
    private static XDocument LoadXmlDocumentation(Assembly assembly)
    {
        var xmlPath = Path.ChangeExtension(assembly.Location, ".xml");
        if (!File.Exists(xmlPath))
            throw new FileNotFoundException("XML documentation file not found.", xmlPath);

        return XDocument.Load(xmlPath);
    }

    private static string GetXmlComments(XDocument xdoc, string memberName)
    {
        var memberElement = xdoc
            .Root.Element("members")
            .Elements("member")
            .FirstOrDefault(e => e.Attribute("name").Value == memberName);

        return GetJosonString(memberElement);
    }

    private static string GetJosonString(XElement memberElement)
    {
        var jsonObject = new
        {
            Name = memberElement.Attribute("name").Value,
            // Summary = memberElement.Element("summary").ToString().Trim("<summary>".ToArray<Char>()),
            Summary = GetSummary(memberElement.Element("summary")),
            Parameters = memberElement
                .Elements("param")
                .ToDictionary(param => param.Attribute("name").Value, param => param.Value.Trim())
        };

        return JsonSerializer.Serialize(jsonObject);
    }

    private static string GetSummary(XElement summary)
    {
        var seeElement = summary.Element("see");

        if (seeElement != null)
        {
            var crefValue = seeElement.Attribute("cref").Value;
            var typeName = crefValue.Substring(2); // Remove the "T:" prefix
            seeElement.ReplaceWith(typeName);
        }
        var sentences = summary.Value.Trim().Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        return string.Join(' ', sentences);
    }

    public static string GetXmlComments(MemberInfo member)
    {
        if (member is MethodInfo methodInfo)
        {
            return GetXmlCommentsForMethod(methodInfo);
        }
        else if (member is PropertyInfo propertyInfo)
        {
            return GetXmlCommentsForProperty(propertyInfo);
        }
        else
        {
            throw new ArgumentException("Unsupported member type", nameof(member));
        }
    }

    private static string GetXmlCommentsForMethod(MethodInfo methodInfo)
    {
        var assembly = methodInfo.Module.Assembly;
        var xdoc = LoadXmlDocumentation(assembly);
        var parameters = string.Join(
            ",",
            methodInfo.GetParameters().Select(p => p.ParameterType.FullName)
        );
        var memberName = $"M:{methodInfo.DeclaringType.FullName}.{methodInfo.Name}({parameters})";
        return GetXmlComments(xdoc, memberName);
    }

    private static string GetXmlCommentsForProperty(PropertyInfo propertyInfo)
    {
        var assembly = propertyInfo.Module.Assembly;
        var xdoc = LoadXmlDocumentation(assembly);
        var memberName = $"P:{propertyInfo.DeclaringType.FullName}.{propertyInfo.Name}";
        memberName.Display();
        return GetXmlComments(xdoc, memberName);
    }

    public static string GetXmlComments(Type type)
    {
        var assembly = type.Assembly;
        var xdoc = LoadXmlDocumentation(assembly);
        var typeName = $"T:{type.FullName}";
        return GetXmlComments(xdoc, typeName);
    }

    public static string ResolveSummary(XElement summaryElement)
    {
        summaryElement.Display();

        if (summaryElement == null)
            return "No summary found.";

        var summaryText = summaryElement.Value;

        foreach (var seeElement in summaryElement.Descendants("see"))
        {
            var cref = seeElement.Attribute("cref")?.Value;
            if (cref != null && cref.StartsWith("T:"))
            {
                var typeName = cref.Substring(2); // Remove the "T:" prefix
                summaryText = summaryText.Replace(seeElement.ToString(), typeName);
            }
        }

        return summaryText.Trim();
    }
}

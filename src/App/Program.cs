using App.Utils;
using System.Text.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

class Program
{
    const string valueToParse = "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)";
    public static void Main()
    {
        // parse the stucture into potenially nested dictionaries
        var parsedStructure = ParserUtils.ParseString(valueToParse);

        // Initialize a yaml serializer ;-) 
        var yamlSerializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance) // Use camel case
            .Build();

        Console.WriteLine($"""
			First Output:
			{ParserUtils.PrintStructure(parsedStructure, sort: false)}
			------------------------------------------
						
			Second Output (sorted alphabetically):
			{ParserUtils.PrintStructure(parsedStructure, sort: true)}
			------------------------------------------
						  
			Of course, now that it is parsed, serializing the output to other formats is straight forward. 
						  
			-- JSON: --
			{JsonSerializer.Serialize(parsedStructure, new JsonSerializerOptions { WriteIndented = true })}
						
			-- YAML: --
			{yamlSerializer.Serialize(parsedStructure)}
			""");
    }
}
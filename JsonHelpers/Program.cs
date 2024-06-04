using JsonHelpers;
using Newtonsoft.Json.Schema;

class Program
{
    static void Main(string[] args)
    {
        string filePath = @"D:\repos\JsonHelpers\JsonHelpers\data.json";


        // SerializeObject örneği
        var obj = new { Name = "John", Age = 30 };
        var json = JsonHelper.SerializeObject(obj);
        Console.WriteLine("Serialized Object: ");
        Console.WriteLine(json);

        // DeserializeObjectFromFile örneği
        var person = JsonHelper.DeserializeObjectFromFile<Person>(filePath);
        Console.WriteLine("\nDeserialized Object from File: ");
        Console.WriteLine($"Name: {person.Name}, Age: {person.Age}");

        // DeserializeObjectFromStream örneği
        using (var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
        {
            var deserializedObj = JsonHelper.DeserializeObjectFromStream<Person>(stream);
            Console.WriteLine("\nDeserialized Object from Stream: ");
            Console.WriteLine($"Name: {deserializedObj.Name}, Age: {deserializedObj.Age}");
        }

        // ValidateJsonSchema örneği
        var schemaJson = "{\"type\":\"object\",\"properties\":{\"Name\":{\"type\":\"string\"},\"Age\":{\"type\":\"integer\"}},\"required\":[\"Name\",\"Age\"]}";
        var schema = JSchema.Parse(schemaJson);
        var isValid = JsonHelper.ValidateJsonSchema(json, schema);
        Console.WriteLine($"\nIs JSON Valid according to schema? {isValid}");

        // MergeJsonObjects örneği
        var json1 = "{\"name\":\"John\"}";
        var json2 = "{\"age\":30}";
        var mergedJson = JsonHelper.MergeJsonObjects(json1, json2);
        Console.WriteLine("\nMerged JSON Objects: ");
        Console.WriteLine(mergedJson);

        // JsonToXml örneği
        var xml = JsonHelper.JsonToXml(json);
        Console.WriteLine("\nJSON to XML: ");
        Console.WriteLine(xml);

        // XmlToJson örneği
        var newJson = JsonHelper.XmlToJson(xml);
        Console.WriteLine("\nXML to JSON: ");
        Console.WriteLine(newJson);

        // PrettyPrintJson örneği
        Console.WriteLine("\nPretty Printed JSON: ");
        Console.WriteLine(JsonHelper.PrettyPrintJson(json));

        // MinifyJson örneği
        Console.WriteLine("\nMinified JSON: ");
        Console.WriteLine(JsonHelper.MinifyJson(json));

        // GetValueByKey örneği
        var value = JsonHelper.GetValueByKey(json, "Name");
        Console.WriteLine($"\nValue for key 'Name': {value}");

        // GetValueByKeyFromFile örneği
        var valueFromFile = JsonHelper.GetValueByKeyFromFile(filePath, "Name");
        Console.WriteLine($"\nValue for key 'Name' from file: {valueFromFile}");

        // UpdateValueByKey örneği
        var updatedJson = JsonHelper.UpdateValueByKey(json, "Age", 35);
        Console.WriteLine("\nUpdated JSON: ");
        Console.WriteLine(updatedJson);

        // UpdateValueByKeyInFile örneği
        JsonHelper.UpdateValueByKeyInFile(filePath, "Age", 35);

        // RemoveKey örneği
        var jsonWithoutKey = JsonHelper.RemoveKey(json, "Age");
        Console.WriteLine("\nJSON without key 'Age': ");
        Console.WriteLine(jsonWithoutKey);

        // RemoveKeyFromFile örneği
        JsonHelper.RemoveKeyFromFile(filePath, "Age");

        // JsonToDictionary örneği
        var dictionary = JsonHelper.JsonToDictionary(json);
        Console.WriteLine("\nDictionary from JSON: ");
        foreach (var kvp in dictionary)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }

        // DictionaryToJson örneği
        var newJsonFromDict = JsonHelper.DictionaryToJson(dictionary);
        Console.WriteLine("\nJSON from Dictionary: ");
        Console.WriteLine(newJsonFromDict);

        // DeepClone örneği
        var deepClonedObj = JsonHelper.DeepClone(obj);
        Console.WriteLine("\nDeep Cloned Object: ");
        Console.WriteLine($"Name: {deepClonedObj.Name}, Age: {deepClonedObj.Age}");

        // CompareJsonObjects örneği
        var json3 = "{\"name\":\"John\"}";
        var json4 = "{\"name\":\"John\"}";
        var areEqual = JsonHelper.CompareJsonObjects(json3, json4);
        Console.WriteLine($"\nAre JSON Objects Equal? {areEqual}");

        // FlattenJson ve UnflattenJson örneği
        var nestedJson = "{\"person\":{\"name\":\"John\",\"age\":30}}";
        var flattenedJson = JsonHelper.FlattenJson(nestedJson);
        Console.WriteLine("\nFlattened JSON: ");
        Console.WriteLine(flattenedJson);
        var unflattenedJson = JsonHelper.UnflattenJson(flattenedJson);
        Console.WriteLine("\nUnflattened JSON: ");
        Console.WriteLine(unflattenedJson);
    }
}

class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Address Address { get; set; }
    public List<string> Hobbies { get; set; }
    public Languages Languages { get; set; }
}

class Address
{
    public string City { get; set; }
    public string Country { get; set; }
}

class Languages
{
    public string Primary { get; set; }
    public string Secondary { get; set; }
}
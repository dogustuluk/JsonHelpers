using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace JsonHelpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using Newtonsoft.Json.Schema.Generation;

    public static class JsonHelper
    {
        #region Serialization
        /// <summary>
        /// nesneleri json'a dönüştürür.
        /// Örnek kullanım:
        /// <code>
        /// var obj = new { Name = "John", Age = 30 };
        /// var json = JsonHelper.SerializeObject(obj);
        /// </code>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// nesneleri json'a dönüştürür ve filePath yolundaki dosyaya yazar.
        /// <code>
        /// var obj = new { Name = "John", Age = 30 };
        /// JsonHelper.SerializeObjectToFile(obj, "data.json");
        /// </code>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        public static void SerializeObjectToFile(object obj, string filePath)
        {
            var json = SerializeObject(obj);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// nesneleri json'a dönüştürür ve ardından stream'e yazar
        /// <code>
        /// var obj = new { Name = "John", Age = 30 };
        /// using (var stream = new MemoryStream())
        ///{
        ///    JsonHelper.SerializeObjectToStream(obj, stream);
        ///}
        /// </code>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="stream"></param>
        public static void SerializeObjectToStream(object obj, Stream stream)
        {
            var json = SerializeObject(obj);
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(json);
            }
        }
        #endregion

        #region Deserialization
        /// <summary>
        /// json'u belirtilen türe dönüştürür
        /// <code>
        /// var json = "{\"Name\":\"John\",\"Age\":30}";
        /// var obj = JsonHelper.DeserializeObject<Person>(json);
        /// </code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// filePath ile gelen yoldaki dosyayı belirttiğimiz türe dönüştürür
        /// <code>
        /// var obj = JsonHelper.DeserializeObjectFromFile<Person>("data.json");
        /// </code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T DeserializeObjectFromFile<T>(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return DeserializeObject<T>(json);
        }

        /// <summary>
        /// json'u belirtilen stream'e dönüştürür.
        /// <code>
        /// using (var stream = new MemoryStream()) {
        /// var obj = JsonHelper.DeserializeObjectFromStream<Person>(stream);
        /// </code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T DeserializeObjectFromStream<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                return DeserializeObject<T>(json);
            }
        }
        #endregion

        #region Validation
        /// <summary>
        /// json verisinin geçerli olup olmadığını kontrol eder.
        /// <code>
        /// bool isValid = JsonHelper.IsValidJson(json);
        /// </code>
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static bool IsValidJson(string json)
        {
            try
            {
                JToken.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// json'un belirtilen şemaya uygun olup olmadığını doğrular.
        /// <code>
        /// var schema = new JSchema();
        /// bool isValid = JsonHelper.ValidateJsonSchema(json, schema);
        /// </code>
        /// </summary>
        /// <param name="json"></param>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static bool ValidateJsonSchema(string json, JSchema schema)
        {
            var jToken = JToken.Parse(json);
            return jToken.IsValid(schema);
        }
        #endregion

        #region Transformation
        /// <summary>
        /// iki json nesnesini birleştirir.
        /// <code>var mergedJson = JsonHelper.MergeJsonObjects(json1, json2);</code>
        /// </summary>
        /// <param name="json1"></param>
        /// <param name="json2"></param>
        /// <returns></returns>
        public static string MergeJsonObjects(string json1, string json2)
        {
            var jObject1 = JObject.Parse(json1);
            var jObject2 = JObject.Parse(json2);
            jObject1.Merge(jObject2, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union
            });
            return jObject1.ToString();
        }

        /// <summary>
        /// JSON'u XML'e dönüştürür.
        /// <code>var xml = JsonHelper.JsonToXml(json);</code>
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string JsonToXml(string json)
        {
            var doc = JsonConvert.DeserializeXmlNode(json, "Root");
            return doc.OuterXml;
        }

        /// <summary>
        /// jml verisini json'a dönüştürür
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string XmlToJson(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return JsonConvert.SerializeXmlNode(doc);
        }

        public static string PrettyPrintJson(string json)
        {
            var parsedJson = JToken.Parse(json);
            return parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// json verisindeki gereksiz boşlukları kaldırır.{"name":"John","age":30,"city":"New York"} gibi yapar.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string MinifyJson(string json)
        {
            var parsedJson = JToken.Parse(json);
            return parsedJson.ToString(Newtonsoft.Json.Formatting.None);
        }
        #endregion

        #region Helper
        /// <summary>
        /// belirtilen json içeriğindeki bir key'in değerini alır.
        /// <code>var jObject = JObject.Parse(json);
        ///return jObject[key]?.ToString();</code>
        /// </summary>
        /// <param name="json">JSON içeriği.</param>
        /// <param name="key">Anahtar.</param>
        /// <returns></returns>
        public static string GetValueByKey(string json, string key)
        {
            var jObject = JObject.Parse(json);
            return jObject[key]?.ToString();
        }

        /// <summary>
        /// belirtilen dosyadaki json içeriğindeki bir key'in değerini alır
        /// <code>    var json = File.ReadAllText(filePath);
        ///     return GetValueByKey(json, key);
        /// </code>
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueByKeyFromFile(string filePath, string key)
        {
            var json = File.ReadAllText(filePath);
            return GetValueByKey(json, key);
        }

        /// <summary>
        /// belirtilen json'ın key'ine karşılık gelen değeri günceller
        /// <code>
        /// var jObject = JObject.Parse(json);
        /// jObject[key] = value;
        /// return jObject.ToString();
        /// </code>
        /// </summary>
        /// <param name="json"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UpdateValueByKey(string json, string key, JToken value)
        {
            var jObject = JObject.Parse(json);
            jObject[key] = value;
            return jObject.ToString();
        }

        /// <summary>
        /// belirtilen dosyadaki json'ın ilgili anahtarının değerini günceller
        /// <code>
        /// var json = File.ReadAllText(filePath);
        /// var updatedJson = UpdateValueByKey(json, key, value);
        /// File.WriteAllText(filePath, updatedJson);
        /// </code>
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void UpdateValueByKeyInFile(string filePath, string key, JToken value)
        {
            var json = File.ReadAllText(filePath);
            var updatedJson = UpdateValueByKey(json, key, value);
            File.WriteAllText(filePath, updatedJson);
        }

        /// <summary>
        /// belirtilen json içindeki key'i siler
        /// <code>
        /// var jObject = JObject.Parse(json);
        /// jObject.Remove(key);
        /// return jObject.ToString();
        /// </code>
        /// </summary>
        /// <param name="json"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveKey(string json, string key)
        {
            var jObject = JObject.Parse(json);
            jObject.Remove(key);
            return jObject.ToString();
        }

        /// <summary>
        /// belirtilen dosyadaki json'ın ilgili anahtarını siler
        /// <code>
        /// var json = File.ReadAllText(filePath);
        /// var updatedJson = RemoveKey(json, key);
        /// File.WriteAllText(filePath, updatedJson);
        /// </code>
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="key"></param>
        public static void RemoveKeyFromFile(string filePath, string key)
        {
            var json = File.ReadAllText(filePath);
            var updatedJson = RemoveKey(json, key);
            File.WriteAllText(filePath, updatedJson);
        }
        #endregion

        #region Advanced
        /// <summary>
        /// json verisini dictionary'e dönüştürür.
        /// <code>
        /// return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        /// </code>
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonToDictionary(string json)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        /// <summary>
        /// verilen dictionary'i json'a dönüştürür
        /// <code>
        /// return JsonConvert.SerializeObject(dictionary, Newtonsoft.Json.Formatting.Indented);
        /// </code>
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string DictionaryToJson(Dictionary<string, object> dictionary)
        {
            return JsonConvert.SerializeObject(dictionary, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// bir nesnenin tam kopyasını oluşturur yani içinde bulunan alt nesnelerin referanslarını da kopyalar. İç içe geçmiş nesnelerde kullanılmalıdır. Bu metodun amacı ilgili nesneyi tam kopyalayarak orijinal nesnenin üzerinde yapılan değişikliklerden etkilenmemesidir. oluşturulduktan sonra iki farklı nesne varmış gibi bir davranış sergiler.
        /// <code>
        /// var json = SerializeObject(obj);
        /// return DeserializeObject<T>(json);
        /// </code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepClone<T>(T obj)
        {
            var json = SerializeObject(obj);
            return DeserializeObject<T>(json);
        }

        /// <summary>
        /// iki json ifadesini karşılaştırır ve birbirine eşit olup olmadığını bulur
        /// <code>
        /// var jObject1 = JObject.Parse(json1);
        /// var jObject2 = JObject.Parse(json2);
        /// return JToken.DeepEquals(jObject1, jObject2);</code>
        /// </summary>
        /// <param name="json1"></param>
        /// <param name="json2"></param>
        /// <returns></returns>
        public static bool CompareJsonObjects(string json1, string json2)
        {
            var jObject1 = JObject.Parse(json1);
            var jObject2 = JObject.Parse(json2);
            return JToken.DeepEquals(jObject1, jObject2);
        }

        /// <summary>
        /// json verisini düzleştirir.
        /// <code>
        /// var jObject = JObject.Parse(json);
        /// var flat = new JObject();
        /// Flatten(jObject, flat, "");
        /// return flat.ToString();
        /// </code>
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string FlattenJson(string json)
        {
            var jObject = JObject.Parse(json);
            var flat = new JObject();
            Flatten(jObject, flat, "");
            return flat.ToString();
        }

        /// <summary>
        /// düzleştirilen json verisini eski haline getirir
        /// <code>
        /// var flat = JObject.Parse(flatJson);
        /// var unflattened = new JObject();
        /// Unflatten(flat, unflattened);
        /// return unflattened.ToString();</code>
        /// </summary>
        /// <param name="flatJson"></param>
        /// <returns></returns>
        public static string UnflattenJson(string flatJson)
        {
            var flat = JObject.Parse(flatJson);
            var unflattened = new JObject();
            Unflatten(flat, unflattened);
            return unflattened.ToString();
        }


        private static void Flatten(JObject source, JObject target, string prefix)
        {
            foreach (var property in source.Properties())
            {
                var key = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";
                if (property.Value is JObject nestedObject)
                {
                    Flatten(nestedObject, target, key);
                }
                else
                {
                    target[key] = property.Value;
                }
            }
        }

        private static void Unflatten(JObject source, JObject target)
        {
            foreach (var property in source.Properties())
            {
                var keys = property.Name.Split('.');
                JObject current = target;
                for (int i = 0; i < keys.Length - 1; i++)
                {
                    var key = keys[i];
                    if (current[key] == null)
                    {
                        current[key] = new JObject();
                    }
                    current = (JObject)current[key];
                }
                current[keys[^1]] = property.Value;
            }
        }
        #endregion
    }

}

using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SuperMaxim.Core.Extensions
{
    public static class SerialisationExtensions
    {
        public static string ToJson<T>(this T graph)
        {
            var serializer = new DataContractJsonSerializer(graph.GetType());
            using var stream = new MemoryStream();
            serializer.WriteObject(stream, graph);
            var json = Encoding.Default.GetString(stream.GetBuffer());
            return json;
        }

        public static T FromJson<T>(this string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var data = Encoding.Default.GetBytes(json);
            using var stream = new MemoryStream(data);
            var result = serializer.ReadObject(stream);
            var graph = (T)result;
            return graph;
        }
    }
}

using System.Text;
using System.Web.Script.Serialization;

namespace BaseClasses
{
    public class JsonUtilities
    {
        public static string Serialize(object o)
        {
            return new JavaScriptSerializer().Serialize(o);
        }

        public static void Serialize(object o, StringBuilder output)
        {
            new JavaScriptSerializer().Serialize(o, output);
        }

        public static T Deserialize<T>(string input)
        {
            return new JavaScriptSerializer().Deserialize<T>(input);
        }

       
    }
}

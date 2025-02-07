using Microsoft.Windows.ApplicationModel.Resources;

namespace Kubix.Helpers
{
    public static class Stringer
    {
        public static string GetString(string mainString)
        {
            ResourceLoader loader = new ResourceLoader();

            mainString = loader.GetString(mainString);

            return string.Format(mainString);
        }

        public static string GetString(string mainString, string param1)
        {
            ResourceLoader loader = new ResourceLoader();

            mainString = loader.GetString(mainString);

            return string.Format(mainString, param1);
        }

        public static string GetString(string mainString, string param1, string param2)
        {
            ResourceLoader loader = new ResourceLoader();

            mainString = loader.GetString(mainString);

            return string.Format(mainString, param1, param2);
        }
    }
}

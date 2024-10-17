using Microsoft.Windows.ApplicationModel.Resources;

namespace KanBoard.Helpers
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
    }
}

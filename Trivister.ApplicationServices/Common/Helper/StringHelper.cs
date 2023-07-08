using System.Text;

namespace Trivister.ApplicationServices.Common.Helper;

public class StringHelper
{
    public static string ConvertArrayToString(string[] input)
    {
        return string.Concat(input);
    }
}
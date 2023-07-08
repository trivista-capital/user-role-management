namespace Trivister.ApplicationServices.Common.Helper;

public class EmailTemplateHelper
{
    public static string ExtractMailTemplate(string fileName)
    {
        var mailTemplate = Path.Combine(Directory.GetCurrentDirectory(), $"Email_Templates_Borrow_Ease/{fileName}");
        string[] lines = File.ReadAllLines(mailTemplate);
        var template = StringHelper.ConvertArrayToString(lines);
        return template;
    }
}
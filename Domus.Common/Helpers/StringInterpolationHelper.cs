using System.Text;

namespace Domus.Common.Helpers;

public static class StringInterpolationHelper
{
    private static StringBuilder? _builder;

    private static StringBuilder Builder
    {
        get
        {
            if (_builder is null)
            {
                _builder = new StringBuilder();
            }

            return _builder;
        }
    }

    public static String BuildAndClear()
    {
        var result = Builder.ToString();
        Builder.Clear();
        return result;
    }

    public static void AppendWithDefaultFormat(string content)
    {
        Builder.Append($" - {content}");
    }

    public static void AppendToStart(string content)
    {
        Builder.Clear();
        Builder.Append(content);
    }

    public static void Clear()
    {
        Builder.Clear();
    }

    public static void Append(string content)
    {
        Builder.Append(content);
    }

    public static string TrimSpaceString(this string content)
    {
        return content.Replace(" ", "");
    }

    public static string GenerateUniqueFileName(string fileName,int length)
    {
        if (length <= 0) throw new Exception("Invalid Length");
        var originalName = fileName.Split(".")[0];
        var extenstionName = fileName.Split(".")[1];
        var uniqueFileName = originalName + RandomPasswordHelper.GenerateRandomPassword(length) + "." + extenstionName;
        return uniqueFileName.TrimSpaceString();
    }
}

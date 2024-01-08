namespace Domus.Api.Helpers;

public static class EnumHelper
{
    public static T GetEnumValueFromString<T>(string valueName) where T : struct, Enum
    {
        var values = Enum.GetValues<T>();
        
        foreach (var value in values)
        {
            if (Equals(value.ToString(), valueName))
            {
                return value;
            }
        }

        throw new Exception();
    }
}

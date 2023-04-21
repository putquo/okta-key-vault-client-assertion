namespace IdentityBridge.Extensions;

public static class StringExtensions
{
  public static DateTime Parse(this string value)
  {
    return DateTimeOffset.FromUnixTimeSeconds(long.Parse(value)).DateTime;
  }
}

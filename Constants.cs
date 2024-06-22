using DotNetEnv;

public static class Constants
{
  public static readonly string VARIABLE_NOT_FOUND = "VARIABLE_NOT_FOUND";
  public static readonly string CONNECTION_STRING = Env.GetString("CONNECTION_STRING", VARIABLE_NOT_FOUND);
}
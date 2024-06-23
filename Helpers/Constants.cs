using DotNetEnv;

public static class Hidden
{
  public static readonly string VARIABLE_NOT_FOUND = "VARIABLE_NOT_FOUND";
  public static readonly string CONNECTION_STRING = Env.GetString("CONNECTION_STRING", VARIABLE_NOT_FOUND);
  public static readonly string SIGNING_KEY = Env.GetString("SIGNING_KEY", VARIABLE_NOT_FOUND);
  public static readonly string ISSUER = Env.GetString("ISSUER", VARIABLE_NOT_FOUND);
  public static readonly string AUDENCE = Env.GetString("AUDENCE", VARIABLE_NOT_FOUND);

}
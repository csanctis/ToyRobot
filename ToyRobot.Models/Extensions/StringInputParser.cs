namespace ToyRobot.Models.Extensions;

public static class StringInputParser
{
    private static string Sanitize(this string input)
    {
        return input.ToUpper().TrimStart().TrimEnd();
    }

    /// <summary>
    /// Splits the input string and returns the last parts, which are the arguments
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string[] GetCommandArguments(this string input)
    {
        // <COMMAND> <ARG1>,<ARG2>,<ARG3>
        var sanitized = input.Sanitize();
        if (sanitized.IndexOf(" ", StringComparison.Ordinal) > 0)
        {
            return input.Substring(input.IndexOf(" ", StringComparison.Ordinal)).Split(',');
        }

        return new []{""};
    }

    /// <summary>
    /// Splits the input string and returns just the first part, which is the command
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string GetCommand(this string input)
    {
        // <COMMAND> <ARG1>,<ARG2>,<ARG3>
        var sanitized = input.Sanitize();
        if (sanitized.IndexOf(" ", StringComparison.Ordinal) > 0)
        {
            return input.Substring(0,input.IndexOf(" ", StringComparison.Ordinal));
        }
        return sanitized;
    }
}
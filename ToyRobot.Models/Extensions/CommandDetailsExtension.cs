namespace ToyRobot.Models.Extensions;

public static class CommandDetailsExtension
{
    public static bool IsCommandValid(this CommandDetails commandDetails)
    {
        return commandDetails.Command != Command.INVALID &&
               commandDetails.Direction != Direction.INVALID;
    }
}
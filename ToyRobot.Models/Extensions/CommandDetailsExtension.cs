namespace ToyRobot.Models.Extensions;

public static class CommandDetailsExtension
{
    public static bool IsCommandValid(this CommandDetails commandDetails)
    {
        return commandDetails.Command != Command.INVALID &&
               commandDetails.Direction != Direction.INVALID;
    }

    public static bool IsDirectionValid(this CommandDetails commandDetails)
    {
        return commandDetails.Direction != Direction.INVALID &&
               commandDetails.Direction != Direction.EMPTY;
    }

}
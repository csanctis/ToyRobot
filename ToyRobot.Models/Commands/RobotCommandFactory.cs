using System.Drawing;
using ToyRobot.Models.Extensions;

namespace ToyRobot.Models.Commands;

public static class RobotCommandFactory
{
    public static CommandDetails ParseAndGenerateCommand(string input)
    {
        var commandDetail = new CommandDetails(input);

        if (string.IsNullOrEmpty(input)) return commandDetail;

        // Sanitizing input
        var arguments = input.GetCommandArguments();

        // PLACE command is the only one with multiple parameters
        if (arguments.Length > 1)
        {
            commandDetail = ValidatePlaceArguments(arguments);
        }
        else
        {
            commandDetail.Command = GetCommand(input.GetCommand());
        }
        return commandDetail;
    }

    private static CommandDetails ValidatePlaceArguments(string[]? arguments)
    {
        var commandDetail = new CommandDetails("")
        {
            Command = Command.PLACE
        };

        if (arguments.Length < 3) return commandDetail;
        
        commandDetail.Position = GetPlaceCoordinates(arguments);
        commandDetail.Direction = GetDirection(arguments[2]);

        return commandDetail;
    }

    private static Point GetPlaceCoordinates(string[]? arguments)
    {
        var success = int.TryParse(arguments[0], out int posX);
        var success2 = int.TryParse(arguments[1], out int posY);
        var newPoint = success && success2 ? new Point(posX, posY) : new Point(-1,-1);
        return newPoint;
    }

    private static Command GetCommand(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Command.INVALID;
        }

        var isValid = Enum.TryParse(input.ToUpper(), out Command validInput);
        return isValid ? validInput : Command.INVALID;
        
    }

    private static Direction GetDirection(string direction)
    {
        if (string.IsNullOrEmpty(direction))
        {
            return Direction.EMPTY;
        }
        else
        {
            var isValid = Enum.TryParse(direction.ToUpper(), out Direction validDirection);
            return !isValid ? Direction.INVALID : validDirection;
        }
    }
}
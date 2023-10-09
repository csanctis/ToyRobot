using System.Drawing;
using ToyRobot.Models.Extensions;

namespace ToyRobot.Models.Commands;

public static class RobotCommandFactory
{
    public enum Command
    {
        PLACE = 1,
        MOVE = 2,
        LEFT = 3,
        RIGHT = 4,
        REPORT = 5,
        INVALID = -1
    }

    public static Instruction ParseAndGenerateCommand(string input)
    {
        var instruction = new Instruction(input);

        if (string.IsNullOrEmpty(input)) return instruction;

        // Sanitizing input
        var arguments = input.GetCommandArguments();

        // PLACE command is the only one with multiple parameters
        if (arguments.Length > 1)
            instruction = ValidatePlaceArguments(arguments);
        else
            instruction.Command = GetCommand(input.GetCommand());
        return instruction;
    }

    private static Instruction ValidatePlaceArguments(string[] arguments)
    {
        var instruction = new Instruction("")
        {
            Command = Command.PLACE
        };

        if (arguments.Length < 3) return instruction;

        instruction.Position = GetPlaceCoordinates(arguments);
        instruction.Direction = GetDirection(arguments[2]);

        return instruction;
    }

    private static Point GetPlaceCoordinates(string[] arguments)
    {
        var success = int.TryParse(arguments[0], out var posX);
        var success2 = int.TryParse(arguments[1], out var posY);
        var newPoint = success && success2 ? new Point(posX, posY) : new Point(-1, -1);
        return newPoint;
    }

    private static Command GetCommand(string input)
    {
        if (string.IsNullOrEmpty(input)) return Command.INVALID;

        var isValid = Enum.TryParse(input.ToUpper(), out Command validInput);
        return isValid ? validInput : Command.INVALID;
    }

    private static Direction GetDirection(string direction)
    {
        if (string.IsNullOrEmpty(direction))
        {
            return Direction.EMPTY;
        }

        var isValid = Enum.TryParse(direction.ToUpper(), out Direction validDirection);
        return !isValid ? Direction.INVALID : validDirection;
    }
}
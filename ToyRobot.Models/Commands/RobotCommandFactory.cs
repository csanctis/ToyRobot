using System.Drawing;
using ToyRobot.Models.Extensions;

namespace ToyRobot.Models.Commands;

public static class RobotCommandFactory
{
    public static Instruction ParseAndGenerateCommand(string input)
    {
        var instruction = new Instruction(input);

        if (string.IsNullOrEmpty(input)) return instruction;

        // Sanitizing input
        var arguments = input.GetCommandArguments();

        // PLACE command is the only one with multiple parameters
        if (arguments.Length > 1)
        {
            instruction = ValidatePlaceArguments(arguments);
        }
        else
        {
            instruction.Command = GetCommand(input.GetCommand());
        }
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
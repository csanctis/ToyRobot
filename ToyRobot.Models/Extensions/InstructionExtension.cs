using ToyRobot.Models.Commands;

namespace ToyRobot.Models.Extensions;

public static class InstructionExtension
{
    public static bool IsCommandValid(this Instruction instruction)
    {
        return instruction.Command != RobotCommandFactory.Command.INVALID &&
               instruction.Direction != Direction.INVALID;
    }

    public static bool IsDirectionValid(this Instruction instruction)
    {
        return instruction.Direction != Direction.INVALID &&
               instruction.Direction != Direction.EMPTY;
    }
}
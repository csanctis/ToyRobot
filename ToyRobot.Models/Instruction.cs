using System.Drawing;
using ToyRobot.Models.Commands;

namespace ToyRobot.Models;

public enum Direction
{
    NORTH = 1,
    SOUTH = 2,
    EAST = 3,
    WEST = 4,
    INVALID = -1,
    EMPTY = 0
}

public class Instruction
{
    public Instruction(string input)
    {
        OriginalInput = input;
    }

    public string OriginalInput { get; set; }
    public Direction Direction { get; set; } = Direction.EMPTY;
    public RobotCommandFactory.Command Command { get; set; } = RobotCommandFactory.Command.INVALID;
    public Point Position { get; set; } = new(-1, -1);
}
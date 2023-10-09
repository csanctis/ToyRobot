using System.ComponentModel.DataAnnotations;
using System.Drawing;

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

public enum DirectionSymbol
{
    [Display(Name = "^")]
    NORTH = '^',
    [Display(Name = "v")]
    SOUTH = 'v',
    [Display(Name = ">")]
    EAST = '>',
    [Display(Name = "<")]
    WEST = '<',
    [Display(Name = "x")]
    INVALID = 'x'
}

public enum Command
{
    PLACE = 1,
    MOVE = 2,
    LEFT = 3,
    RIGHT = 4,
    REPORT = 5,
    INVALID = -1
}

public class Instruction
{
    public Instruction(string input)
    {
        OriginalInput = input;
    }

    public string OriginalInput { get; set; }
    public Direction Direction { get; set; } = Direction.EMPTY;
    public Command Command { get; set; } = Command.INVALID;
    public Point Position { get; set; } = new(-1, -1);
}
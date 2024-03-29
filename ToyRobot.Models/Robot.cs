using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text;
using ToyRobot.Models.Commands;
using ToyRobot.Models.Extensions;

namespace ToyRobot.Models;

public class Robot
{
    // This keeps track of all previous commands received by the robot
    private readonly List<Instruction> _instructionList = new();

    private readonly IRobotCommand _robotCommand;

    // Allowed surface for the robot to roam
    private readonly TableSurface? _surface;

    private Direction _lastDirection = Direction.EMPTY;

    // Position awareness of the robot
    private Point _lastPosition = new(-1, -1);

    public Robot(TableSurface surface)
    {
        _surface = surface;
        _robotCommand = new RobotCommand(this);
    }

    public Robot()
    {
        _robotCommand = new RobotCommand(this);
    }

    public void ResetRobot()
    {
        _lastDirection = Direction.EMPTY;
        _lastPosition = new Point(-1, -1);
    }

    public bool ExecuteCommand(Instruction instruction)
    {
        if (_robotCommand.ExecuteInstruction(instruction))
        {
            // Keep history of all commands
            _instructionList.Add(instruction);
            return true;
        }

        return false;
    }

    public bool SetDirection(Instruction instruction)
    {
        if (IsRobotOnTable() && instruction.IsDirectionValid())
        {
            _lastDirection = instruction.Direction;
            return true;
        }

        return false;
    }

    public string GetDirectionSymbol()
    {
        switch (_lastDirection)
        {
            case Direction.NORTH:
                return DirectionSymbol.NORTH.GetEnumDisplayName();

            case Direction.SOUTH:
                return DirectionSymbol.SOUTH.GetEnumDisplayName();

            case Direction.EAST:
                return DirectionSymbol.EAST.GetEnumDisplayName();

            case Direction.WEST:
                return DirectionSymbol.WEST.GetEnumDisplayName();

            default:
                return DirectionSymbol.INVALID.GetEnumDisplayName();
        }
    }

    public string ReportLocation()
    {
        Console.WriteLine($"{_lastPosition.X},{_lastPosition.Y},{_lastDirection}");
        return $"{_lastPosition.X},{_lastPosition.Y},{_lastDirection}";
    }

    public Point GetPosition()
    {
        return _lastPosition;
    }

    public bool SetPosition(int x, int y)
    {
        if (!IsValidMove(x, y))
            return false;

        _lastPosition.X = x;
        _lastPosition.Y = y;
        return true;
    }

    public Direction GetDirection()
    {
        return _lastDirection;
    }

    public bool IsRobotOnTable()
    {
        return _surface != null && _lastPosition is { X: >= 0, Y: >= 0 };
    }

    public bool IsRobotOnThisPosition(int x, int y)
    {
        return _lastPosition.X == x && _lastPosition.Y == y;
    }

    private bool IsValidMove(int x, int y)
    {
        return !(x < 0 || x >= _surface?.Rows) && !(y < 0 || y >= _surface?.Columns);
    }

    public string PrintAllPreviousCommand()
    {
        var sBuilder = new StringBuilder();
        foreach (var cd in _instructionList) sBuilder.AppendLine(cd.OriginalInput);

        return sBuilder.ToString();
    }
}

public enum DirectionSymbol
{
    [Display(Name = "^")] NORTH = '^',
    [Display(Name = "v")] SOUTH = 'v',
    [Display(Name = ">")] EAST = '>',
    [Display(Name = "<")] WEST = '<',
    [Display(Name = "x")] INVALID = 'x'
}
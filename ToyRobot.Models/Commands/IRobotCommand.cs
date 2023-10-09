namespace ToyRobot.Models.Commands;

public interface IRobotCommand
{
    public bool Move();
    public bool Rotate(Instruction instruction);
    public bool Place(Instruction instruction);
}

public class RobotCommand : IRobotCommand
{
    private readonly Robot _robot;

    public RobotCommand(Robot robot)
    {
        _robot = robot;
    }

    public bool Move()
    {
        if (!_robot.IsRobotOnTable()) return false;

        switch (_robot.GetDirection())
        {
            case Direction.NORTH:
                return _robot.SetPosition(_robot.GetPosition().X, _robot.GetPosition().Y + 1);

            case Direction.SOUTH:
                return _robot.SetPosition(_robot.GetPosition().X, _robot.GetPosition().Y - 1);

            case Direction.EAST:
                return _robot.SetPosition(_robot.GetPosition().X + 1, _robot.GetPosition().Y);

            case Direction.WEST:
                return _robot.SetPosition(_robot.GetPosition().X - 1, _robot.GetPosition().Y);

            default:
                return false;
        }
    }

    public bool Rotate(Instruction instruction)
    {
        switch (instruction.Command)
        {
            case Command.LEFT:
                RotateLeft(instruction);

                break;

            case Command.RIGHT:
                RotateRight(instruction);
                break;

            default:
                instruction.Direction = Direction.NORTH;
                break;
        }

        return _robot.SetDirection(instruction);
    }

    private static void RotateRight(Instruction instruction)
    {
        switch (instruction.Direction)
        {
            case Direction.NORTH:
                instruction.Direction = Direction.EAST;
                break;
            case Direction.SOUTH:
                instruction.Direction = Direction.WEST;
                break;
            case Direction.EAST:
                instruction.Direction = Direction.SOUTH;
                break;
            case Direction.WEST:
                instruction.Direction = Direction.NORTH;
                break;
        }
    }

    private static void RotateLeft(Instruction instruction)
    {
        switch (instruction.Direction)
        {
            case Direction.NORTH:
                instruction.Direction = Direction.WEST;
                break;
            case Direction.SOUTH:
                instruction.Direction = Direction.EAST;
                break;
            case Direction.EAST:
                instruction.Direction = Direction.NORTH;
                break;
            case Direction.WEST:
                instruction.Direction = Direction.SOUTH;
                break;
        }
    }

    public bool Place(Instruction instruction)
    {
        if (_robot.SetPosition(instruction.Position.X, instruction.Position.Y))
        {
            if (_robot.SetDirection(instruction)) return true;

            // If direction is wrong, reset position to out of table
            _robot.ResetRobot();
        }

        return false;
    }
}
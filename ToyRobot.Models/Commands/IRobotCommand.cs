namespace ToyRobot.Models.Commands;

public interface IRobotCommand
{
    public bool ExecuteInstruction(Instruction  instruction);
}

public class RobotCommand : IRobotCommand
{
    private readonly Robot _robot;

    public RobotCommand(Robot robot)
    {
        _robot = robot;
    }

    public bool ExecuteInstruction(Instruction instruction)
    {
        if (!_robot.IsRobotOnTable() && instruction.Command != RobotCommandFactory.Command.PLACE)
            return false;

        switch (instruction.Command)
        {
            case RobotCommandFactory.Command.PLACE:
                return Place(instruction);

            case RobotCommandFactory.Command.MOVE:
                return Move();

            case RobotCommandFactory.Command.LEFT:
            case RobotCommandFactory.Command.RIGHT:
                instruction.Direction = _robot.GetDirection();
                return Rotate(instruction);

            case RobotCommandFactory.Command.REPORT:
                _robot.ReportLocation();
                return true;

            case RobotCommandFactory.Command.INVALID:
            default:
                return false;
        }
    }

    private bool Move()
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

    private bool Rotate(Instruction instruction)
    {
        switch (instruction.Command)
        {
            case RobotCommandFactory.Command.LEFT:
                RotateLeft(instruction);

                break;

            case RobotCommandFactory.Command.RIGHT:
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

    private bool Place(Instruction instruction)
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
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
                if (instruction.Direction == Direction.NORTH)
                    instruction.Direction = Direction.WEST;

                if (instruction.Direction == Direction.SOUTH)
                    instruction.Direction = Direction.EAST;

                if (instruction.Direction == Direction.EAST)
                    instruction.Direction = Direction.NORTH;

                if (instruction.Direction == Direction.WEST)
                    instruction.Direction = Direction.SOUTH;
                break;

            case Command.RIGHT:
                if (instruction.Direction == Direction.NORTH)
                    instruction.Direction = Direction.EAST;

                if (instruction.Direction == Direction.SOUTH)
                    instruction.Direction = Direction.WEST;

                if (instruction.Direction == Direction.EAST)
                    instruction.Direction = Direction.SOUTH;

                if (instruction.Direction == Direction.WEST)
                    instruction.Direction = Direction.NORTH;
                break;

            default:
                instruction.Direction = Direction.NORTH;
                break;
        }
        return _robot.SetDirection(instruction);
    }

    public bool Place(Instruction instruction)
    {
        var posResult = _robot.SetPosition(instruction.Position.X, instruction.Position.Y);
        var dirResult = _robot.SetDirection(instruction);
        return posResult && dirResult;
    }
}
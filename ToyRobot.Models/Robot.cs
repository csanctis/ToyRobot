using System.Drawing;

namespace ToyRobot.Models
{
    public enum DIRECTION
    {
        NORTH = 1,
        SOUTH = 2,
        EAST = 3,
        WEST = 4,
        INVALID = -1,
        EMPTY = 0
    }

    public enum COMMAND
    {
        PLACE = 1,
        MOVE = 2,
        LEFT = 3,
        RIGHT = 4,
        REPORT = 5,
        INVALID = -1
    }

    public class Robot
    {
        // UI symbols to represent the robot orientation
        private string _northDirection => "^";

        private string _southDirection => "v";
        private string _eastDirection => ">";
        private string _westDirection => "<";
        private string _invalidDirection => "x";

        // Allowed surface for the robot to roam
        private TableSurface _surface;

        // Position awaress of the robot
        private Point _lastPosition = new Point(-1, -1);

        private DIRECTION _lastDirection = DIRECTION.EMPTY;

        // This keeps track of all previous commands received by the robot
        private List<CommandDetails> _commands = new List<CommandDetails>();

        public Robot(TableSurface surface)
        {
            _surface = surface;
        }

        private bool AssignCommand(CommandDetails command)
        {
            if (command.Command != COMMAND.INVALID)
            {
                _commands.Add(command);
                return true;
            }
            return false;
        }

        public void ExecuteLastCommand()
        {
            if (_commands.Any())
                ExecuteCommand(_commands.Last());
        }

        private void ExecuteCommand(CommandDetails command)
        {
            switch (command.Command)
            {
                case COMMAND.PLACE:
                    Place(command);
                    break;

                case COMMAND.MOVE:
                    Move();
                    break;

                case COMMAND.LEFT:
                case COMMAND.RIGHT:
                    _lastDirection = Rotate(command);
                    break;

                case COMMAND.REPORT:
                    Report();
                    break;

                default:
                    break;
            }
        }

        private bool Move()
        {
            switch (_lastDirection)
            {
                case DIRECTION.NORTH:
                    _lastPosition.Y = _lastPosition.Y + 1 < _surface.Rows ? _lastPosition.Y + 1 : _lastPosition.Y;
                    return ShiftPosition(_lastPosition, _lastDirection);

                case DIRECTION.SOUTH:
                    _lastPosition.Y = _lastPosition.Y - 1 >= 0 ? _lastPosition.Y - 1 : _lastPosition.Y;
                    return ShiftPosition(_lastPosition, _lastDirection);

                case DIRECTION.EAST:
                    _lastPosition.X = _lastPosition.X + 1 < _surface.Columns ? _lastPosition.X + 1 : _lastPosition.X;
                    return ShiftPosition(_lastPosition, _lastDirection);

                case DIRECTION.WEST:
                    _lastPosition.X = _lastPosition.X - 1 >= 0 ? _lastPosition.X - 1 : _lastPosition.X;
                    return ShiftPosition(_lastPosition, _lastDirection);

                default:
                    return false;
            }
        }

        private DIRECTION Rotate(CommandDetails command)
        {
            switch (_lastDirection)
            {
                case DIRECTION.NORTH:
                    if (command.Command == COMMAND.LEFT)
                        return DIRECTION.WEST;
                    if (command.Command == COMMAND.RIGHT)
                        return DIRECTION.EAST;
                    break;

                case DIRECTION.SOUTH:
                    if (command.Command == COMMAND.LEFT)
                        return DIRECTION.EAST;
                    if (command.Command == COMMAND.RIGHT)
                        return DIRECTION.WEST;
                    break;

                case DIRECTION.EAST:
                    if (command.Command == COMMAND.LEFT)
                        return DIRECTION.NORTH;
                    if (command.Command == COMMAND.RIGHT)
                        return DIRECTION.SOUTH;
                    break;

                case DIRECTION.WEST:
                    if (command.Command == COMMAND.LEFT)
                        return DIRECTION.SOUTH;
                    if (command.Command == COMMAND.RIGHT)
                        return DIRECTION.NORTH;
                    break;

                default:
                    return _lastDirection;
            }
            return _lastDirection;
        }

        public string GetDIRECTIONymbol()
        {
            switch (_lastDirection)
            {
                case DIRECTION.NORTH:
                    return _northDirection;

                case DIRECTION.SOUTH:
                    return _southDirection;

                case DIRECTION.EAST:
                    return _eastDirection;

                case DIRECTION.WEST:
                    return _westDirection;

                default:
                    return _invalidDirection;
            }
        }

        private bool Place(CommandDetails command)
        {
            if (command.Direction != DIRECTION.INVALID && command.Direction != DIRECTION.EMPTY)
                return ShiftPosition(command.Position, command.Direction);

            return false;
        }

        private bool ShiftPosition(Point position, DIRECTION direction)
        {
            var isValid = ValidatePosition(position.X, position.Y);
            if (isValid)
            {
                // Update position and get new symbol
                _lastPosition = position;
                if (direction != DIRECTION.EMPTY)
                    _lastDirection = direction;

                return true;
            }
            return false;
        }

        public string Report()
        {
            Console.WriteLine($"{_lastPosition.X},{_lastPosition.Y},{_lastDirection}");
            return $"{_lastPosition.X},{_lastPosition.Y},{_lastDirection}";
        }

        public Point GetLocation()
        {
            return _lastPosition;
        }

        public DIRECTION GetDirection()
        {
            return _lastDirection;
        }

        public bool isRobotOnTable()
        {
            return (_lastPosition.X >= 0 && _lastPosition.Y >= 0);
        }

        public CommandDetails ParseInputAndGenerageCommand(string input)
        {
            var commandDetail = new CommandDetails
            {
                OriginalInput = input,
                Command = COMMAND.INVALID,
                Direction = DIRECTION.EMPTY,
            };

            if (!String.IsNullOrEmpty(input))
            {
                // Sanitizing input
                input = input.ToUpper().TrimStart().TrimEnd();
                var arguments = input.Split(',');

                // PLACE command is the only one with multiple parameters
                if (arguments.Length > 1)
                {
                    var split = input.Substring(input.IndexOf(" ")).Split(',');

                    commandDetail = ValidateArguments(split);
                    // If number of arguments not as expected, return error.
                    if (commandDetail.Command != COMMAND.INVALID &&
                        commandDetail.Direction != DIRECTION.INVALID)
                    {
                        AssignCommand(commandDetail);
                    }
                }
                else
                {
                    commandDetail.Command = GetCommand(input);
                    AssignCommand(commandDetail);
                }
            }
            return commandDetail;
        }

        private CommandDetails ValidateArguments(string[] arguments)
        {
            var commandDetail = new CommandDetails
            {
                Command = COMMAND.INVALID,
                Direction = DIRECTION.EMPTY,
                Position = new Point(-1, -1)
            };

            if (arguments.Length >= 2)
            {
                commandDetail.Command = COMMAND.PLACE;
                var success = int.TryParse(arguments[0], out int posX);
                var success2 = int.TryParse(arguments[1], out int posY);
                if (arguments.Length > 2)
                {
                    commandDetail.Direction = GetDirection(arguments[2]);
                }
                else
                {
                    commandDetail.Direction = _lastDirection;
                }

                if (success && success2 && ValidatePosition(posX, posY))
                {
                    commandDetail.Position = new Point(posX, posY);
                }
                else
                {
                    commandDetail.Command = COMMAND.INVALID;
                }
            }

            return commandDetail;
        }

        private bool ValidatePosition(int x, int y)
        {
            return !(x < 0 || x >= _surface.Rows) && !(y < 0 || y >= _surface.Columns);
        }

        private COMMAND GetCommand(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return COMMAND.INVALID;
            }
            else
            {
                var isvalid = Enum.TryParse(input.ToUpper(), out COMMAND validInput);
                if (!isvalid)
                {
                    return COMMAND.INVALID;
                }
                return validInput;
            }
        }

        private DIRECTION GetDirection(string direction)
        {
            if (String.IsNullOrEmpty(direction))
            {
                return DIRECTION.EMPTY;
            }
            else
            {
                var isvalid = Enum.TryParse(direction.ToUpper(), out DIRECTION validDirection);
                if (!isvalid)
                {
                    return DIRECTION.INVALID;
                }
                return validDirection;
            }
        }
    }
}
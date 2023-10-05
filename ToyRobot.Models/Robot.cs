using System.Drawing;

namespace ToyRobot.Models
{
	public enum Direction
	{
		NORTH = 1,
		SOUTH = 2,
		EAST = 3,
		WEST = 4,
		INVALID = -1,
		EMPTY = 0
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

		private Direction _lastDirection = Direction.EMPTY;

		// This keeps track of all previous commands received by the robot
		private List<CommandDetails> _commands = new List<CommandDetails>();

		public Robot(TableSurface surface)
		{
			_surface = surface;
		}

		private bool AssignCommand(CommandDetails command)
		{
			if (command.Command == Command.INVALID) return false;
			_commands.Add(command);
			return true;
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
				case Command.PLACE:
					Place(command);
					break;

				case Command.MOVE:
					Move();
					break;

				case Command.LEFT:
				case Command.RIGHT:
					_lastDirection = Rotate(command);
					break;

				case Command.REPORT:
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
				case Direction.NORTH:
					_lastPosition.Y = _lastPosition.Y + 1 < _surface.Rows ? _lastPosition.Y + 1 : _lastPosition.Y;
					return ShiftPosition(_lastPosition, _lastDirection);

				case Direction.SOUTH:
					_lastPosition.Y = _lastPosition.Y - 1 >= 0 ? _lastPosition.Y - 1 : _lastPosition.Y;
					return ShiftPosition(_lastPosition, _lastDirection);

				case Direction.EAST:
					_lastPosition.X = _lastPosition.X + 1 < _surface.Columns ? _lastPosition.X + 1 : _lastPosition.X;
					return ShiftPosition(_lastPosition, _lastDirection);

				case Direction.WEST:
					_lastPosition.X = _lastPosition.X - 1 >= 0 ? _lastPosition.X - 1 : _lastPosition.X;
					return ShiftPosition(_lastPosition, _lastDirection);

				default:
					return false;
			}
		}

		private Direction Rotate(CommandDetails command)
		{
			switch (_lastDirection)
			{
				case Direction.NORTH:
					if (command.Command == Command.LEFT)
						return Direction.WEST;
					if (command.Command == Command.RIGHT)
						return Direction.EAST;
					break;

				case Direction.SOUTH:
					if (command.Command == Command.LEFT)
						return Direction.EAST;
					if (command.Command == Command.RIGHT)
						return Direction.WEST;
					break;

				case Direction.EAST:
					if (command.Command == Command.LEFT)
						return Direction.NORTH;
					if (command.Command == Command.RIGHT)
						return Direction.SOUTH;
					break;

				case Direction.WEST:
					if (command.Command == Command.LEFT)
						return Direction.SOUTH;
					if (command.Command == Command.RIGHT)
						return Direction.NORTH;
					break;

				default:
					return _lastDirection;
			}
			return _lastDirection;
		}

		public string GetDirectionSymbol()
		{
			switch (_lastDirection)
			{
				case Direction.NORTH:
					return _northDirection;

				case Direction.SOUTH:
					return _southDirection;

				case Direction.EAST:
					return _eastDirection;

				case Direction.WEST:
					return _westDirection;

				default:
					return _invalidDirection;
			}
		}

		private bool Place(CommandDetails command)
		{
			if (command.Direction != Direction.INVALID && command.Direction != Direction.EMPTY)
				return ShiftPosition(command.Position, command.Direction);

			return false;
		}

		private bool ShiftPosition(Point position, Direction direction)
		{
			var isValid = ValidatePosition(position.X, position.Y);
			if (!isValid) return false;
			
			// Update position and get new symbol
			_lastPosition = position;
			if (direction != Direction.EMPTY)
				_lastDirection = direction;

			return true;
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

		public Direction GetDirection()
		{
			return _lastDirection;
		}

		public bool isRobotOnTable()
		{
			return (_lastPosition.X >= 0 && _lastPosition.Y >= 0);
		}

		public CommandDetails ParseInputAndGenerateCommand(string input)
		{
			var commandDetail = new CommandDetails
			{
				OriginalInput = input,
				Command = Command.INVALID,
				Direction = Direction.EMPTY,
			};

			if (string.IsNullOrEmpty(input)) return commandDetail;
			
			// Sanitizing input
			input = input.ToUpper().TrimStart().TrimEnd();
			var arguments = input.Split(',');

			// PLACE command is the only one with multiple parameters
			if (arguments.Length > 1)
			{
				var split = input.Substring(input.IndexOf(" ", StringComparison.Ordinal)).Split(',');

				commandDetail = ValidateArguments(split);
				// If number of arguments not as expected, return error.
				if (commandDetail.Command != Command.INVALID &&
				    commandDetail.Direction != Direction.INVALID)
				{
					AssignCommand(commandDetail);
				}
			}
			else
			{
				commandDetail.Command = GetCommand(input);
				AssignCommand(commandDetail);
			}
			return commandDetail;
		}

		private CommandDetails ValidateArguments(string[] arguments)
		{
			var commandDetail = new CommandDetails
			{
				Command = Command.INVALID,
				Direction = Direction.EMPTY,
				Position = new Point(-1, -1)
			};

			if (arguments.Length >= 2)
			{
				commandDetail.Command = Command.PLACE;
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
					commandDetail.Command = Command.INVALID;
				}
			}

			return commandDetail;
		}

		private bool ValidatePosition(int x, int y)
		{
			return !(x < 0 || x >= _surface.Rows) && !(y < 0 || y >= _surface.Columns);
		}

		private static Command GetCommand(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return Command.INVALID;
			}
			else
			{
				var isValid = Enum.TryParse(input.ToUpper(), out Command validInput);
				return isValid ? validInput : Command.INVALID;
			}
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
}
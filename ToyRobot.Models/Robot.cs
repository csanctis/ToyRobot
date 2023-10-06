using System.Drawing;
using ToyRobot.Models.Extensions;

namespace ToyRobot.Models
{
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

		// Position awareness of the robot
		private Point _lastPosition = new Point(-1, -1);

		private Direction _lastDirection = Direction.EMPTY;

		// This keeps track of all previous commands received by the robot
		private List<CommandDetails> _commandsList = new();

		public Robot(TableSurface surface)
		{
			_surface = surface;
		}

		public void ExecuteCommand(CommandDetails command)
		{
			// Keep history of all commands
            _commandsList.Add(command);

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

                case Command.INVALID:
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

		public bool IsRobotOnTable()
		{
			return _lastPosition is { X: >= 0, Y: >= 0 };
		}

        private bool ValidatePosition(int x, int y)
		{
			return !(x < 0 || x >= _surface.Rows) && !(y < 0 || y >= _surface.Columns);
		}

	}
}
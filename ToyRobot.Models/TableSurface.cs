namespace ToyRobot.Models
{
	public class TableSurface
	{
		private string[,] _tableSurface;
		private int _rows;
		private int _columns;

		public int Rows => this._rows;
		public int Columns => this._columns;

		// UI symbols to represent the robot orientation
		private string _northDirection => "^";

		private string _southDirection => "v";
		private string _eastDirection => ">";
		private string _westDirection => "<";
		private string _invalidDirection => "x";

		private string _noDirection => "O";

		/// <summary>
		/// Creates a new Surface Table with the specified length
		/// </summary>
		/// <param name="x">number os rows in the table</param>
		/// <param name="y">number of columns in the table</param>
		public TableSurface(int x, int y)
		{
			_rows = x;
			_columns = y;
			_tableSurface = new string[x, y];
			InitTable();
		}

		public string[,] GetTableSurface()
		{
			return _tableSurface;
		}

		private void InitTable()
		{
			for (int column = _columns - 1; column >= 0; column--)
			{
				for (int row = 0; row < _rows; row++)
				{
					_tableSurface[row, column] = _noDirection;
				}
			}
		}

		public string GetRobotPosition(Robot robot)
		{
			return GetDIRECTIONymbol(robot.GetDirection());
		}

		private string GetDIRECTIONymbol(DIRECTION direction)
		{
			switch (direction)
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
	}
}
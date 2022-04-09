using System.Text;

namespace ToyRobot.Models
{
	public class TableSurface
	{
		private string[,] _tableSurface;
		private int _rows;
		private int _columns;

		public int Rows => this._rows;
		public int Columns => this._columns;

		private Dictionary<Guid, Robot> _robotOnTable = new Dictionary<Guid, Robot>();

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
			for (int row = 0; row < _rows; row++)
			{
				for (int column = 0; column < _columns; column++)
				{
					_tableSurface[row, column] = _noDirection;
				}
			}
		}

		private bool IsRobotInThisLocation(Guid robotId, int x, int y)
		{
			var robot = GetRobotData(robotId);
			return robot != null && robot.GetLocation().X == x && robot.GetLocation().Y == y;
		}

		public string PrintTable()
		{
			StringBuilder sb = new StringBuilder();
			for (int row = _rows - 1; row >= 0; row--)
			{
				sb.AppendLine("");
				for (int column = 0; column <= _columns - 1; column++)
				{
					_tableSurface[row, column] = GetAllRobotsDirectionSymbol(column, row);
					sb.Append($"{_tableSurface[row, column]}");
					sb.Append("   ");
				}
			}
			return sb.ToString();
		}

		private string GetAllRobotsDirectionSymbol(int column, int row)
		{
			StringBuilder sbDirections = new StringBuilder();
			// Check if any robot on the table in at this location
			foreach (var robot in _robotOnTable)
			{
				var existingRobot = robot.Value;

				if (existingRobot != null &&
					existingRobot.isRobotOnTable() &&
					existingRobot.GetLocation().X == column
					&& existingRobot.GetLocation().Y == row)
				{
					sbDirections.Append(existingRobot.GetDIRECTIONymbol());
				}
			}

			if (sbDirections.Length == 0)
				return _noDirection;
			if (sbDirections.Length == 1)
				return sbDirections.ToString();
			if (sbDirections.Length > 1)
				return "@";

			return _noDirection;
		}

		public void AddRobotToTable(Robot robot)
		{
			_robotOnTable.Add(robot.GetID, robot);
		}

		public Robot GetRobotData(Guid id)
		{
			var exists = _robotOnTable.TryGetValue(id, out Robot existingRobot);
			if (exists) return existingRobot;
			return null;
		}
	}
}
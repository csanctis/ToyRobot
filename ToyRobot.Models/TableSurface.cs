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

		private Robot _robotOnTable;

		private static string _noDirection => "O";

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

		public string PrintTable()
		{
			var sb = new StringBuilder();
			for (int row = _rows - 1; row >= 0; row--)
			{
				sb.AppendLine("");
				for (int column = 0; column <= _columns - 1; column++)
				{
					// X is the column and Y is the row.
					_tableSurface[row, column] = GetDirectionSymbol(column, row);
					sb.Append($"{_tableSurface[row, column]}");
					sb.Append("   ");
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// X is the column and Y is the row.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		private string GetDirectionSymbol(int x, int y)
		{
			if (_robotOnTable.isRobotOnTable() &&
			    _robotOnTable.GetLocation().X == x
			    && _robotOnTable.GetLocation().Y == y)
			{
				return _robotOnTable.GetDirectionSymbol();
			}
			return _noDirection;
		}

		public void AddRobotToTable(Robot robot)
		{
			_robotOnTable = robot;
		}
	}
}
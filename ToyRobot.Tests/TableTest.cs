using Serilog;
using ToyRobot.Models;
using Xunit;

namespace ToyRobot.Tests
{
	public class TableTest
	{
		private ILogger _logger;

		private void GetLogger()
		{
			if (_logger == null)
			{
				_logger = new LoggerConfiguration()
			  .WriteTo.Debug()
			  .CreateLogger();
			}
		}

		[Fact]
		public void CreateTableSurfaceTest()
		{
			GetLogger();
			var surface = new TableSurface(6, 6);
			Assert.Equal(6, surface.Rows);
			Assert.Equal(6, surface.Columns);
			Robot robot = new Robot(surface);
			surface.AddRobotToTable(robot);

			var commandParsed = robot.ParseInputAndGenerageCommand("PLACE 5,2,NORTH");
			Assert.True(commandParsed.Command == COMMAND.PLACE);

			robot.ExecuteLastCommand();

			var print = surface.PrintTable();

			string expected =
				@"" + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   ^   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   ";

			Assert.Equal(expected, print);
		}
	}
}
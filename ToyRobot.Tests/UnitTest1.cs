using System;
using ToyRobot.Models;
using Xunit;

namespace ToyRobot.Tests
{
	public class UnitTest1
	{
		[Fact]
		public void CreateTableSurfaceTest()
		{
			var surface = new TableSurface(6, 6);
			Assert.Equal(6, surface.Rows);
			Assert.Equal(6, surface.Columns);
		}

		[Theory]
		[InlineData("LEFT", COMMAND.LEFT)]
		[InlineData("LeFT", COMMAND.LEFT)]
		[InlineData("left", COMMAND.LEFT)]
		[InlineData("RIGHT", COMMAND.RIGHT)]
		[InlineData("Right", COMMAND.RIGHT)]
		[InlineData("right", COMMAND.RIGHT)]
		[InlineData("REPORT", COMMAND.REPORT)]
		[InlineData("REPORt", COMMAND.REPORT)]
		[InlineData("report", COMMAND.REPORT)]
		[InlineData("MOVE", COMMAND.MOVE)]
		[InlineData("Move", COMMAND.MOVE)]
		[InlineData("move", COMMAND.MOVE)]
		[InlineData("", COMMAND.INVALID)]
		[InlineData(null, COMMAND.INVALID)]
		public void CommandsTest(string command, COMMAND expected)
		{
			var surface = new TableSurface(6, 6);
			var robot = new Robot(surface);
			var commandParsed = robot.ParseInputAndGenerageCommand(command);
			Assert.True(commandParsed.Command == expected);
		}

		[Fact]
		public void ParseCommandTest()
		{
			var surface = new TableSurface(6, 6);
			var robot = new Robot(surface);
			var commandParsed = robot.ParseInputAndGenerageCommand("PLACE 0,0,NORTH");
			Assert.True(commandParsed.Command == COMMAND.PLACE);

			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.NORTH, robot.GetDirection());
			Assert.Equal(0, robot.GetLocation().X);
			Assert.Equal(0, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerageCommand("MOVE");
			Assert.True(commandParsed.Command == COMMAND.MOVE);

			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.NORTH, robot.GetDirection());
			Assert.Equal(0, robot.GetLocation().X);
			Assert.Equal(1, robot.GetLocation().Y);
			Assert.Equal("0,1,NORTH", robot.Report());
		}

		[Fact]
		public void ParseInvalidCommandTest()
		{
			var surface = new TableSurface(6, 6);
			var robot = new Robot(surface);
			var commandParsed = robot.ParseInputAndGenerageCommand("PLACE 1,1,-1");
			Assert.True(commandParsed.Command == COMMAND.PLACE);

			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.EMPTY, robot.GetDirection());
			Assert.Equal(-1, robot.GetLocation().X);
			Assert.Equal(-1, robot.GetLocation().Y);

			Assert.Equal("-1,-1,EMPTY", robot.Report());
		}

		[Fact]
		public void ParseCommand2Test()
		{
			var surface = new TableSurface(6, 6);
			var robot = new Robot(surface);
			var commandParsed = robot.ParseInputAndGenerageCommand("PLACE 1,2,EAST");
			Assert.True(commandParsed.Command == COMMAND.PLACE);
			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.EAST, robot.GetDirection());
			Assert.Equal(1, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerageCommand("MOVE");
			Assert.True(commandParsed.Command == COMMAND.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.EAST, robot.GetDirection());
			Assert.Equal(2, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerageCommand("MOVE");
			Assert.True(commandParsed.Command == COMMAND.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.EAST, robot.GetDirection());
			Assert.Equal(3, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerageCommand("LEFT");
			Assert.True(commandParsed.Command == COMMAND.LEFT);
			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.NORTH, robot.GetDirection());
			Assert.Equal(3, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerageCommand("MOVE");
			Assert.True(commandParsed.Command == COMMAND.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.NORTH, robot.GetDirection());
			Assert.Equal(3, robot.GetLocation().X);
			Assert.Equal(3, robot.GetLocation().Y);
			Assert.Equal("3,3,NORTH", robot.Report());
		}

		[Fact]
		public void ParseCommand3Test()
		{
			var surface = new TableSurface(6, 6);
			var robot = new Robot(surface);
			var commandParsed = robot.ParseInputAndGenerageCommand("PLACE 1,2,EAST");
			Assert.True(commandParsed.Command == COMMAND.PLACE);
			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.EAST, robot.GetDirection());
			Assert.Equal(1, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerageCommand("MOVE");
			Assert.True(commandParsed.Command == COMMAND.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.EAST, robot.GetDirection());
			Assert.Equal(2, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerageCommand("LEFT");
			Assert.True(commandParsed.Command == COMMAND.LEFT);
			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.NORTH, robot.GetDirection());
			Assert.Equal(2, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerageCommand("MOVE");
			Assert.True(commandParsed.Command == COMMAND.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.NORTH, robot.GetDirection());
			Assert.Equal(2, robot.GetLocation().X);
			Assert.Equal(3, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerageCommand("PLACE 3,1");
			Assert.True(commandParsed.Command == COMMAND.PLACE);
			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.NORTH, robot.GetDirection());
			Assert.Equal(3, robot.GetLocation().X);
			Assert.Equal(1, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerageCommand("MOVE");
			Assert.True(commandParsed.Command == COMMAND.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(DIRECTION.NORTH, robot.GetDirection());
			Assert.Equal(3, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			Assert.Equal("3,2,NORTH", robot.Report());
		}
	}
}
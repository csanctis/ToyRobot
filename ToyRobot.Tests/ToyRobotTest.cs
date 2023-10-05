using System;
using ToyRobot.Models;
using Xunit;

namespace ToyRobot.Tests
{
	public class ToyRobotTest
	{
		[Theory]
		[InlineData("LEFT", Command.LEFT)]
		[InlineData("LeFT", Command.LEFT)]
		[InlineData("left", Command.LEFT)]
		[InlineData("RIGHT", Command.RIGHT)]
		[InlineData("Right", Command.RIGHT)]
		[InlineData("right", Command.RIGHT)]
		[InlineData("REPORT", Command.REPORT)]
		[InlineData("REPORt", Command.REPORT)]
		[InlineData("report", Command.REPORT)]
		[InlineData("MOVE", Command.MOVE)]
		[InlineData("Move", Command.MOVE)]
		[InlineData("move", Command.MOVE)]
		[InlineData("", Command.INVALID)]
		[InlineData(null, Command.INVALID)]
		public void CommandsTest(string command, Command expected)
		{
			var surface = new TableSurface(6, 6);
			var robot = new Robot(surface);
			var commandParsed = robot.ParseInputAndGenerateCommand(command);
			Assert.True(commandParsed.Command == expected);
		}

		[Fact]
		public void ParseInvalidCommandTest()
		{
			var surface = new TableSurface(6, 6);
			var robot = new Robot(surface);
			var commandParsed = robot.ParseInputAndGenerateCommand("PLACE 1,1,-1");
			Assert.True(commandParsed.Command == Command.PLACE);

			robot.ExecuteLastCommand();

			Assert.Equal(Direction.EMPTY, robot.GetDirection());
			Assert.Equal(-1, robot.GetLocation().X);
			Assert.Equal(-1, robot.GetLocation().Y);

			Assert.Equal("-1,-1,EMPTY", robot.Report());
		}

		[Fact]
		public void NavigationTest1()
		{
			var surface = new TableSurface(6, 6);
			var robot = new Robot(surface);
			var commandParsed = robot.ParseInputAndGenerateCommand("PLACE 0,0,NORTH");
			Assert.True(commandParsed.Command == Command.PLACE);

			robot.ExecuteLastCommand();

			Assert.Equal(Direction.NORTH, robot.GetDirection());
			Assert.Equal(0, robot.GetLocation().X);
			Assert.Equal(0, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerateCommand("MOVE");
			Assert.True(commandParsed.Command == Command.MOVE);

			robot.ExecuteLastCommand();

			Assert.Equal(Direction.NORTH, robot.GetDirection());
			Assert.Equal(0, robot.GetLocation().X);
			Assert.Equal(1, robot.GetLocation().Y);
			Assert.Equal("0,1,NORTH", robot.Report());
		}

		[Fact]
		public void NavigationTest2()
		{
			var surface = new TableSurface(6, 6);
			var robot = new Robot(surface);
			var commandParsed = robot.ParseInputAndGenerateCommand("PLACE 1,2,EAST");
			Assert.True(commandParsed.Command == Command.PLACE);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.EAST, robot.GetDirection());
			Assert.Equal(1, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerateCommand("MOVE");
			Assert.True(commandParsed.Command == Command.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.EAST, robot.GetDirection());
			Assert.Equal(2, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerateCommand("MOVE");
			Assert.True(commandParsed.Command == Command.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.EAST, robot.GetDirection());
			Assert.Equal(3, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerateCommand("LEFT");
			Assert.True(commandParsed.Command == Command.LEFT);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.NORTH, robot.GetDirection());
			Assert.Equal(3, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerateCommand("MOVE");
			Assert.True(commandParsed.Command == Command.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.NORTH, robot.GetDirection());
			Assert.Equal(3, robot.GetLocation().X);
			Assert.Equal(3, robot.GetLocation().Y);
			Assert.Equal("3,3,NORTH", robot.Report());
		}

		[Fact]
		public void NavigationTest3()
		{
			var surface = new TableSurface(6, 6);
			var robot = new Robot(surface);
			var commandParsed = robot.ParseInputAndGenerateCommand("PLACE 1,2,EAST");
			Assert.True(commandParsed.Command == Command.PLACE);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.EAST, robot.GetDirection());
			Assert.Equal(1, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerateCommand("MOVE");
			Assert.True(commandParsed.Command == Command.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.EAST, robot.GetDirection());
			Assert.Equal(2, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerateCommand("LEFT");
			Assert.True(commandParsed.Command == Command.LEFT);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.NORTH, robot.GetDirection());
			Assert.Equal(2, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerateCommand("MOVE");
			Assert.True(commandParsed.Command == Command.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.NORTH, robot.GetDirection());
			Assert.Equal(2, robot.GetLocation().X);
			Assert.Equal(3, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerateCommand("PLACE 3,1");
			Assert.True(commandParsed.Command == Command.PLACE);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.NORTH, robot.GetDirection());
			Assert.Equal(3, robot.GetLocation().X);
			Assert.Equal(1, robot.GetLocation().Y);

			commandParsed = robot.ParseInputAndGenerateCommand("MOVE");
			Assert.True(commandParsed.Command == Command.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.NORTH, robot.GetDirection());
			Assert.Equal(3, robot.GetLocation().X);
			Assert.Equal(2, robot.GetLocation().Y);

			Assert.Equal("3,2,NORTH", robot.Report());
		}

		[Fact]
		public void PushRobotOutOfTableTest()
		{
			var surface = new TableSurface(6, 6);
			var robot = new Robot(surface);
			var commandParsed = robot.ParseInputAndGenerateCommand("PLACE 0,0,EAST");
			Assert.True(commandParsed.Command == Command.PLACE);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.EAST, robot.GetDirection());
			Assert.Equal(0, robot.GetLocation().X);
			Assert.Equal(0, robot.GetLocation().Y);

			// push robot to edge of table
			for (int i = 0; i < surface.Columns - 1; i++)
			{
				commandParsed = robot.ParseInputAndGenerateCommand("MOVE");
				Assert.True(commandParsed.Command == Command.MOVE);
				robot.ExecuteLastCommand();

				Assert.Equal(Direction.EAST, robot.GetDirection());
				Assert.Equal(i + 1, robot.GetLocation().X);
				Assert.Equal(0, robot.GetLocation().Y);
			}

			// try to push robot our of table
			commandParsed = robot.ParseInputAndGenerateCommand("MOVE");
			Assert.True(commandParsed.Command == Command.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.EAST, robot.GetDirection());
			Assert.Equal(surface.Columns - 1, robot.GetLocation().X);
			Assert.Equal(0, robot.GetLocation().Y);
			Assert.Equal("5,0,EAST", robot.Report());

			commandParsed = robot.ParseInputAndGenerateCommand("MOVE");
			Assert.True(commandParsed.Command == Command.MOVE);
			robot.ExecuteLastCommand();

			Assert.Equal(Direction.EAST, robot.GetDirection());
			Assert.Equal(surface.Columns - 1, robot.GetLocation().X);
			Assert.Equal(0, robot.GetLocation().Y);
			Assert.Equal("5,0,EAST", robot.Report());
		}
	}
}
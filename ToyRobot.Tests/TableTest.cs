﻿using ToyRobot.Models;
using Xunit;

namespace ToyRobot.Tests
{
	public class TableTest
	{
		[Fact]
		public void DisplayRobotPositionCorrectly_1()
		{
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
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   ^   " + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   ";

			Assert.Equal(expected, print);
		}

		[Fact]
		public void DisplayRobotPositionCorrectly_2()
		{
			var surface = new TableSurface(6, 6);
			Assert.Equal(6, surface.Rows);
			Assert.Equal(6, surface.Columns);
			Robot robot = new Robot(surface);
			surface.AddRobotToTable(robot);

			var commandParsed = robot.ParseInputAndGenerageCommand("PLACE 0,0,SOUTH");
			Assert.True(commandParsed.Command == COMMAND.PLACE);

			robot.ExecuteLastCommand();

			var print = surface.PrintTable();

			string expected =
				@"" + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"v   O   O   O   O   O   ";

			Assert.Equal(expected, print);
		}

		[Fact]
		public void DisplayRobotPositionCorrectly_3()
		{
			var surface = new TableSurface(6, 6);
			Assert.Equal(6, surface.Rows);
			Assert.Equal(6, surface.Columns);
			Robot robot = new Robot(surface);
			surface.AddRobotToTable(robot);

			var commandParsed = robot.ParseInputAndGenerageCommand("PLACE 5,5,WEST");
			Assert.True(commandParsed.Command == COMMAND.PLACE);

			robot.ExecuteLastCommand();

			var print = surface.PrintTable();

			string expected =
				@"" + System.Environment.NewLine +
				"O   O   O   O   O   <   " + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   " + System.Environment.NewLine +
				"O   O   O   O   O   O   ";

			Assert.Equal(expected, print);
		}
	}
}
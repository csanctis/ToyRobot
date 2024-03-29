using ToyRobot.Models;
using Xunit;
using static ToyRobot.Models.Commands.RobotCommandFactory;

namespace ToyRobot.Tests;

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
        var commandParsed = ParseAndGenerateCommand(command);
        Assert.True(commandParsed.Command == expected);
    }

    [Fact]
    public void ParseInvalidCommandRobotNotOnTableTest()
    {
        var surface = new TableSurface(6, 6);
        var robot = new Robot(surface);
        var commandParsed = ParseAndGenerateCommand("LEFT");
        Assert.True(commandParsed.Command == Command.LEFT);

        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.EMPTY, robot.GetDirection());
        Assert.Equal(-1, robot.GetPosition().X);
        Assert.Equal(-1, robot.GetPosition().Y);

        Assert.Equal("-1,-1,EMPTY", robot.ReportLocation());
    }

    [Fact]
    public void ParseInvalidCommandTest0()
    {
        var surface = new TableSurface(6, 6);
        var robot = new Robot(surface);
        var commandParsed = ParseAndGenerateCommand("PLACE 6,1,north");
        Assert.True(commandParsed.Command == Command.PLACE);

        var result = robot.ExecuteCommand(commandParsed);
        Assert.False(result);

        Assert.Equal(Direction.EMPTY, robot.GetDirection());
        Assert.Equal(-1, robot.GetPosition().X);
        Assert.Equal(-1, robot.GetPosition().Y);

        Assert.Equal("-1,-1,EMPTY", robot.ReportLocation());
    }

    [Fact]
    public void ParseInvalidCommandTest1()
    {
        var surface = new TableSurface(6, 6);
        var robot = new Robot(surface);
        var commandParsed = ParseAndGenerateCommand("PLACE 1,1,-1");
        Assert.True(commandParsed.Command == Command.PLACE);

        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.EMPTY, robot.GetDirection());
        Assert.Equal(-1, robot.GetPosition().X);
        Assert.Equal(-1, robot.GetPosition().Y);

        Assert.Equal("-1,-1,EMPTY", robot.ReportLocation());
    }

    [Fact]
    public void NavigationTest0()
    {
        var surface = new TableSurface(6, 6);
        var robot = new Robot(surface);
        var commandParsed = ParseAndGenerateCommand("place 1,1,east");
        Assert.True(commandParsed.Command == Command.PLACE);

        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.EAST, robot.GetDirection());
        Assert.Equal(1, robot.GetPosition().X);
        Assert.Equal(1, robot.GetPosition().Y);
        Assert.Equal("1,1,EAST", robot.ReportLocation());
    }

    [Fact]
    public void NavigationTest1()
    {
        var surface = new TableSurface(6, 6);
        var robot = new Robot(surface);
        var commandParsed = ParseAndGenerateCommand("PLACE 0,0,NORTH");
        Assert.True(commandParsed.Command == Command.PLACE);

        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.NORTH, robot.GetDirection());
        Assert.Equal(0, robot.GetPosition().X);
        Assert.Equal(0, robot.GetPosition().Y);

        commandParsed = ParseAndGenerateCommand("MOVE");
        Assert.True(commandParsed.Command == Command.MOVE);

        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.NORTH, robot.GetDirection());
        Assert.Equal(0, robot.GetPosition().X);
        Assert.Equal(1, robot.GetPosition().Y);
        Assert.Equal("0,1,NORTH", robot.ReportLocation());
    }

    [Fact]
    public void NavigationTest2()
    {
        var surface = new TableSurface(6, 6);
        var robot = new Robot(surface);
        var commandParsed = ParseAndGenerateCommand("PLACE 1,2,EAST");
        Assert.True(commandParsed.Command == Command.PLACE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.EAST, robot.GetDirection());
        Assert.Equal(1, robot.GetPosition().X);
        Assert.Equal(2, robot.GetPosition().Y);

        commandParsed = ParseAndGenerateCommand("MOVE");
        Assert.True(commandParsed.Command == Command.MOVE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.EAST, robot.GetDirection());
        Assert.Equal(2, robot.GetPosition().X);
        Assert.Equal(2, robot.GetPosition().Y);

        commandParsed = ParseAndGenerateCommand("MOVE");
        Assert.True(commandParsed.Command == Command.MOVE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.EAST, robot.GetDirection());
        Assert.Equal(3, robot.GetPosition().X);
        Assert.Equal(2, robot.GetPosition().Y);

        commandParsed = ParseAndGenerateCommand("LEFT");
        Assert.True(commandParsed.Command == Command.LEFT);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.NORTH, robot.GetDirection());
        Assert.Equal(3, robot.GetPosition().X);
        Assert.Equal(2, robot.GetPosition().Y);

        commandParsed = ParseAndGenerateCommand("MOVE");
        Assert.True(commandParsed.Command == Command.MOVE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.NORTH, robot.GetDirection());
        Assert.Equal(3, robot.GetPosition().X);
        Assert.Equal(3, robot.GetPosition().Y);
        Assert.Equal("3,3,NORTH", robot.ReportLocation());
    }

    [Fact]
    public void NavigationTest3()
    {
        var surface = new TableSurface(6, 6);
        var robot = new Robot(surface);
        var commandParsed = ParseAndGenerateCommand("PLACE 1,2,EAST");
        Assert.True(commandParsed.Command == Command.PLACE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.EAST, robot.GetDirection());
        Assert.Equal(1, robot.GetPosition().X);
        Assert.Equal(2, robot.GetPosition().Y);

        commandParsed = ParseAndGenerateCommand("MOVE");
        Assert.True(commandParsed.Command == Command.MOVE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.EAST, robot.GetDirection());
        Assert.Equal(2, robot.GetPosition().X);
        Assert.Equal(2, robot.GetPosition().Y);

        commandParsed = ParseAndGenerateCommand("LEFT");
        Assert.True(commandParsed.Command == Command.LEFT);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.NORTH, robot.GetDirection());
        Assert.Equal(2, robot.GetPosition().X);
        Assert.Equal(2, robot.GetPosition().Y);

        commandParsed = ParseAndGenerateCommand("MOVE");
        Assert.True(commandParsed.Command == Command.MOVE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.NORTH, robot.GetDirection());
        Assert.Equal(2, robot.GetPosition().X);
        Assert.Equal(3, robot.GetPosition().Y);

        commandParsed = ParseAndGenerateCommand("PLACE 3,1, NORTH");
        Assert.True(commandParsed.Command == Command.PLACE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.NORTH, robot.GetDirection());
        Assert.Equal(3, robot.GetPosition().X);
        Assert.Equal(1, robot.GetPosition().Y);

        commandParsed = ParseAndGenerateCommand("MOVE");
        Assert.True(commandParsed.Command == Command.MOVE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.NORTH, robot.GetDirection());
        Assert.Equal(3, robot.GetPosition().X);
        Assert.Equal(2, robot.GetPosition().Y);

        Assert.Equal("3,2,NORTH", robot.ReportLocation());
    }

    [Fact]
    public void NavigationTest4()
    {
        var surface = new TableSurface(6, 6);
        var robot = new Robot(surface);
        var commandParsed = ParseAndGenerateCommand("PLACE 5,5,SOUTH");
        Assert.True(commandParsed.Command == Command.PLACE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.SOUTH, robot.GetDirection());
        Assert.Equal(5, robot.GetPosition().X);
        Assert.Equal(5, robot.GetPosition().Y);

        commandParsed = ParseAndGenerateCommand("MOVE");
        Assert.True(commandParsed.Command == Command.MOVE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.SOUTH, robot.GetDirection());
        Assert.Equal(5, robot.GetPosition().X);
        Assert.Equal(4, robot.GetPosition().Y);

        commandParsed = ParseAndGenerateCommand("MOVE");
        Assert.True(commandParsed.Command == Command.MOVE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.SOUTH, robot.GetDirection());
        Assert.Equal(5, robot.GetPosition().X);
        Assert.Equal(3, robot.GetPosition().Y);

        commandParsed = ParseAndGenerateCommand("RIGHT");
        Assert.True(commandParsed.Command == Command.RIGHT);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.WEST, robot.GetDirection());
        Assert.Equal(5, robot.GetPosition().X);
        Assert.Equal(3, robot.GetPosition().Y);
    }

    [Fact]
    public void PushRobotOutOfTableTest()
    {
        var surface = new TableSurface(6, 6);
        var robot = new Robot(surface);
        var commandParsed = ParseAndGenerateCommand("PLACE 0,0,EAST");
        Assert.True(commandParsed.Command == Command.PLACE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.EAST, robot.GetDirection());
        Assert.Equal(0, robot.GetPosition().X);
        Assert.Equal(0, robot.GetPosition().Y);

        // push robot to edge of table
        for (var i = 0; i < surface.Columns - 1; i++)
        {
            commandParsed = ParseAndGenerateCommand("MOVE");
            Assert.True(commandParsed.Command == Command.MOVE);
            robot.ExecuteCommand(commandParsed);

            Assert.Equal(Direction.EAST, robot.GetDirection());
            Assert.Equal(i + 1, robot.GetPosition().X);
            Assert.Equal(0, robot.GetPosition().Y);
        }

        // try to push robot out of table
        commandParsed = ParseAndGenerateCommand("MOVE");
        Assert.True(commandParsed.Command == Command.MOVE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.EAST, robot.GetDirection());
        Assert.Equal(surface.Columns - 1, robot.GetPosition().X);
        Assert.Equal(0, robot.GetPosition().Y);
        Assert.Equal("5,0,EAST", robot.ReportLocation());

        commandParsed = ParseAndGenerateCommand("MOVE");
        Assert.True(commandParsed.Command == Command.MOVE);
        robot.ExecuteCommand(commandParsed);

        Assert.Equal(Direction.EAST, robot.GetDirection());
        Assert.Equal(surface.Columns - 1, robot.GetPosition().X);
        Assert.Equal(0, robot.GetPosition().Y);
        Assert.Equal("5,0,EAST", robot.ReportLocation());
    }
}
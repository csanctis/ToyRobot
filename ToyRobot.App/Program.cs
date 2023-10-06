using ToyRobot.Models;
using ToyRobot.Models.Commands;
using ToyRobot.Models.Extensions;

namespace ToyRobot
{
	internal class Program
	{
		private static readonly CancellationTokenSource _cancelTokenSrc = new CancellationTokenSource();
		private static TableSurface _tableSurface = new TableSurface(6, 6);
		private static Robot _robot = new Robot(_tableSurface);

		private static void Main(string[] args)
		{
			// CTL + C is the built-in cancellation for console apps;
			Console.CancelKeyPress += Console_CancelKeyPress;
			CancellationToken cancelToken = _cancelTokenSrc.Token;

			PrintInstructions();
			_tableSurface.AddRobotToTable(_robot);

			try
			{
				// thread that listens for keyboard input until cancelled
				Task.Run(ListenForInput, cancelToken);
				// continue listening until cancel signal is sent
				cancelToken.WaitHandle.WaitOne();
				cancelToken.ThrowIfCancellationRequested();
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("Operation Canceled.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}
		}

		private static void PrintInstructions()
		{
			Console.ForegroundColor = ConsoleColor.White;
			//toy robot moving on a 6 x 6 square tabletop
			Write(null, "==================================================================================");
			Write(null, "    This is simulation of a toy robot moving on a 6 x 6 square tabletop.");
			Write(null, "    Valid Commands:");
			Write(ConsoleColor.Green, "* PLACE X,Y, <DIRECTION>");
			Write(null, "*	  (Position X, Y can be between 0,0 and 5,5)");
			Write(null, "*	  (Directions can be NORTH, SOUTH, EAST or WEST)");
			Write(null, "*	  (Example: PLACE 1,2,NORTH)");
			Write(ConsoleColor.Green,"* MOVE");
			Write(null, "*      (Moves the toy robot one unit forward in the direction it is currently facing)");
			Write(ConsoleColor.Green, "* LEFT");
			Write(null, "*      (Rotates the robot 90 degrees LEFT in the specified direction without changing the position of the robot)");
			Write(ConsoleColor.Green, "* RIGHT");
			Write(null, "*      (Rotates the robot 90 degrees RIGHT in the specified direction without changing the position of the robot)");
			Write(ConsoleColor.Green, "* REPORT ");
			Write(null, "*      Announces the X,Y and orientation of the robot.");
			Write(ConsoleColor.Green, "* Press CTL+C to Terminate");
			Write();
			Write(null, "==================================================================================");
		}

		private static void ListenForInput()
		{
			while (true)
			{
				string userInput = Console.ReadLine();
				if (string.IsNullOrWhiteSpace(userInput)) continue;

                var commandDetails = RobotCommandFactory.ParseAndGenerateCommand(userInput);
				
				// TODO Get rid of this from Robot
				if (commandDetails.IsCommandValid())
				{
					_robot.ExecuteCommand(commandDetails);
					Console.WriteLine(_tableSurface.PrintTable());
				}
				else
				{
					Write(ConsoleColor.Red, "Invalid Command");
					PrintInstructions();
				}
			}
		}

		private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			// we want to cancel the default behavior so we can send the cancellation signal
			// to our background threads and not just terminate here
			e.Cancel = true;
			Console.WriteLine("Cancelling...");
			_cancelTokenSrc.Cancel();
		}
		
		private static void Write(params object[] oo)
		{
			foreach(var o in oo)
				if(o == null)
					Console.ResetColor();
				else if(o is ConsoleColor)
					Console.ForegroundColor = (ConsoleColor)o;
				else
					Console.WriteLine(o.ToString());
		}
	}
}
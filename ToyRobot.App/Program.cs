using ToyRobot.Models;

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
				Task.Run(() => ListenForInput(), cancelToken);
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
			Console.WriteLine("==================================================================================");
			Console.WriteLine("    This is simulation of a toy robot moving on a 6 x 6 square tabletop.");
			Console.WriteLine("    Valid Commands:");
			Console.WriteLine("* PLACE X,Y, <DIRECTION>");
			Console.WriteLine("*	  (Position X, Y can be between 0,0 and 5,5)");
			Console.WriteLine("*	  (Directions can be NORTH, SOUTH, EAST or WEST)");
			Console.WriteLine("*	  (Example: PLACE 1,2,NORTH)");
			Console.WriteLine("* MOVE");
			Console.WriteLine("*      (Moves the toy robot one unit forward in the direction it is currently facing)");
			Console.WriteLine("* LEFT");
			Console.WriteLine("*      (Rotates the robot 90 degrees LEFT in the specified direction without changing the position of the robot)");
			Console.WriteLine("* RIGHT");
			Console.WriteLine("*      (Rotates the robot 90 degrees RIGHT in the specified direction without changing the position of the robot)");
			Console.WriteLine("* REPORT ");
			Console.WriteLine("*      Announces the X,Y and orientation of the robot.");
			Console.WriteLine("* Press CTL+C to Terminate");
			Console.WriteLine();
			Console.WriteLine("==================================================================================");
		}

		private static void ListenForInput()
		{
			while (true)
			{
				string userInput = Console.ReadLine();
				if (!String.IsNullOrWhiteSpace(userInput))
				{
					var command = _robot.ParseInputAndGenerageCommand(userInput);
					if (command.IsValid)
					{
						_robot.ExecuteLastCommand();
						Console.WriteLine(_tableSurface.PrintTable());
					}
					else
					{
						Console.WriteLine("Invalid Command");
						PrintInstructions();
					}
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
	}
}
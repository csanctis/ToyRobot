using System.Drawing;

namespace ToyRobot.Models
{
	public class CommandDetails
	{
		public string OriginalInput { get; set; }
		public Direction Direction { get; set; }
		public Command Command { get; set; }
		public Point Position { get; set; }

		public bool IsValid => Command != Command.INVALID;
	}
}
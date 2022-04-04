using System.Drawing;

namespace ToyRobot.Models
{
	public class CommandDetails
	{
		public string OriginalInput { get; set; }
		public DIRECTION Direction { get; set; }
		public COMMAND Command { get; set; }
		public Point Position { get; set; }

		public bool IsValid => Command != COMMAND.INVALID;
	}
}
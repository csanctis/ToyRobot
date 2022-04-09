using ToyRobot.Models;

namespace ToyRobot.Services
{
	public interface IRobotCommandService
	{
		public Robot InstantiateRobot();

		public TableSurface InstantiateTableSurface();
	}

	public class RobotCommandService : IRobotCommandService
	{
		public Robot InstantiateRobot()
		{
			throw new NotImplementedException();
		}

		public TableSurface InstantiateTableSurface()
		{
			throw new NotImplementedException();
		}
	}
}
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ToyRobot.Interface
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();
		}

		private void initiateGrid()
		{
			StackPanel sp = null;
			TextBlock tb = null;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					//<StackPanel Grid.Row="0" Grid.Column="0" VerticalAlignment="Center" HorizontalAlignment="Center" BorderBrush="#FF020202">
					//	<TextBlock Foreground="White" FontSize="50" Text="O" Name="text00" />
					//</StackPanel>
					sp = new StackPanel();
					sp.VerticalAlignment = VerticalAlignment.Center;
					sp.HorizontalAlignment = HorizontalAlignment.Center;

					tb = new TextBlock();
					tb.Text = string.Format("Row {0} Column {1}", i, j);
					tb.FontSize = 50;
					sp.Children.Add(tb);
					TableSurfaceGrid.Children.Add(sp);
				}
			}
		}
	}
}
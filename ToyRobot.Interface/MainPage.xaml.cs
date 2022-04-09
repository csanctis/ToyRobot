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
			InitializeComponent();
			initiateGrid();
		}

		private void initiateGrid()
		{
			StackPanel sp = null;
			TextBlock tb = null;

			for (int row = 6 - 1; row >= 0; row--)
			{
				for (int column = 0; column <= 6 - 1; column++)
				{
					// X is the column and Y is the row.
					//_tableSurface[row, column] = GetDirectionSymbol(column, row);

					//<StackPanel Grid.Row="0" Grid.Column="0" VerticalAlignment="Center" HorizontalAlignment="Center">
					//	<TextBlock Foreground="White" FontSize="50" Text="O" Name="text00" />
					//</StackPanel>
					sp = new StackPanel();
					sp.VerticalAlignment = VerticalAlignment.Center;
					sp.HorizontalAlignment = HorizontalAlignment.Center;

					tb = new TextBlock();
					tb.Text = $"{column}{row}";
					tb.FontSize = 50;

					sp.Children.Add(tb);

					Grid.SetColumn(sp, column);
					Grid.SetRow(sp, row);
					TableSurfaceGrid.Children.Add(sp);
				}
			}
		}
	}
}
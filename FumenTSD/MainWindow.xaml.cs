using FumenTSD.Windows;
using System.Windows;

namespace FumenTSD
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void BtnFieldEdit_Click(object sender, RoutedEventArgs e)
		{
			(new EditField()).ShowDialog();
		}
	}
}
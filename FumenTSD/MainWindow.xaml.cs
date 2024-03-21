using Fumen;
using FumenTSD.UserControls;
using FumenTSD.Windows;
using Microsoft.Win32;
using System.IO;
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
			Field.Name.Text = "盤面";
			FieldHint.Name.Text = "ヒント用盤面";
		}

		private void BtnFieldEdit_Click(object sender, RoutedEventArgs e)
		{
			var editFieldWindow = new EditField(Field, FieldHint);
			editFieldWindow.ShowDialog();
			if (editFieldWindow.ApplyFlag)
			{
				Field.CopyAll(editFieldWindow.Field);
				FieldHint.CopyAll(editFieldWindow.FieldHint);
			}
		}

		private void BtnLoad_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();

			if (dialog.ShowDialog() == true)
			{
				StreamReader reader = new StreamReader(dialog.FileName);
				var data = FumenTSDFormat.Decode(reader)!;
				//TxtboxMenuTitle.Text = data.Title;
				//TxtboxDeveloper.Text=data.Title

			}
		}

		private void BtnOutput_Click(object sender, RoutedEventArgs e)
		{
			FumenTSDFormat data = new FumenTSDFormat();
			data.MenuTitle = TxtboxMenuTitle.Text;
			data.GameTitle = TxtboxInGameTitle.Text;
			try
			{
				data.Clears = new Clears(
				int.Parse(TSMS.Text == string.Empty ? "0" : TSMS.Text),
				int.Parse(TSMD.Text == string.Empty ? "0" : TSMD.Text),
				int.Parse(TSS.Text == string.Empty ? "0" : TSS.Text),
				int.Parse(TSD.Text == string.Empty ? "0" : TSD.Text),
				int.Parse(TST.Text == string.Empty ? "0" : TST.Text),
				int.Parse(Tetris.Text == string.Empty ? "0" : Tetris.Text),
				int.Parse(PC.Text == string.Empty ? "0" : PC.Text),
				int.Parse(REN.Text == string.Empty ? "0" : REN.Text)
				);
			}
			catch
			{
				MessageBox.Show("クリア条件のフォーマットが適切ではありません。");
			}
			data.Developer = TxtboxDeveloper.Text;
			data.Difficulty = ComboboxDifficulty.SelectedIndex + 1;
			data.Comment = TxtboxComment.Text;
			data.Hold = Field.Hold;
			data.Current = Field.Current;
			data.Nexts = Field.Next.ToArray();
			data.Board = Field.Board.ToArray();
			data.BoardHint = FieldHint.Board.ToArray();

			var dialog = new SaveFileDialog();
			dialog.Filter = "テキストファイル(*.txt)|*.txt";
			if (dialog.ShowDialog() == true)
			{
				StreamWriter writer = new StreamWriter(dialog.FileName);
				writer.WriteLine(data.Encode());
				writer.Flush();
				writer.Close();
			}


		}
	}
}
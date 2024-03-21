using Fumen;
using FumenTSD.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Fumen.Constants;
using Decoder = Fumen.Decoder;

namespace FumenTSD.Windows
{
	/// <summary>
	/// Interaction logic for EditField.xaml
	/// </summary>
	public partial class EditField : Window
	{
		public bool ApplyFlag { get; private set; } = false;
		public static readonly char[] MINOS = new char[] { 'S', 'Z', 'L', 'J', 'T', 'O', 'I', };

		public EditField(Field field, Field fieldHint)
		{
			InitializeComponent();
			Field.SetBoard(field.Board);
			FieldHint.SetBoard(fieldHint.Board);
			Field.SetHold(field.Hold);
			FieldHint.SetHold(fieldHint.Hold);
			Field.SetCurrent(field.Current);
			FieldHint.SetCurrent(fieldHint.Current);

			Field.Name.Text = "盤面";
			FieldHint.Name.Text = "ヒント用盤面";
			Field.Editable = true;
			FieldHint.Editable = true;

			CreateMinoButtons(FieldContainer, "Field");
			CreateMinoButtons(NextContainer, "Next");
			CreateMinoButtons(HoldContainer, "Hold");
			CreateMinoButtons(CurrentContainer, "Current");
		}


		private void CreateMinoButtons(UniformGrid container, string name)
		{
			foreach (var mino in MINOS)
			{
				Button button = new Button();
				switch (name)
				{
					case "Next":
						button.Click += ModifyNext_Click;
						break;
					case "Hold":
						button.Click += ModifyHold_Click;
						break;
					case "Current":
						button.Click += ModifyCurrent_Click;
						break;
				}
				button.Name = name + mino.ToString();

				var img = Util.CreateMinoBitmapFrame(mino, 50);
				var brush = new ImageBrush(img);
				brush.Stretch = Stretch.Uniform;
				container.Children.Add(button);
				button.Background = brush;
			}
		}

		private void ModifyNext_Click(object sender, RoutedEventArgs e)
		{
			var field = RadioBtnAppyToField.IsChecked == true ? Field : FieldHint;
			char value = char.Parse(((Button)sender).Name.Substring(((Button)sender).Name.Length - 1, 1));
			if (value == '_')
				field.RemoveLatestNext();
			else
				field.AddNext(value);
		}

		private void ModifyHold_Click(object sender, RoutedEventArgs e)
		{
			var field = RadioBtnAppyToField.IsChecked == true ? Field : FieldHint;
			char value = char.Parse(((Button)sender).Name.Substring(((Button)sender).Name.Length - 1, 1));
			field.SetHold(value);
		}

		private void ModifyCurrent_Click(object sender, RoutedEventArgs e)
		{
			var field = RadioBtnAppyToField.IsChecked == true ? Field : FieldHint;
			char value = char.Parse(((Button)sender).Name.Substring(((Button)sender).Name.Length - 1, 1));
			field.SetCurrent(value);
		}

		private void BtnLoadFumen_Click(object sender, RoutedEventArgs e)
		{
			var result = MessageBox.Show("現在編集中のフィールドを上書きします。", "確認", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

			if (result == MessageBoxResult.Cancel)
				return;

			//	try
			//{
			var decoder = new Decoder(TxtBoxFumenURL.Text);
			int pageIndex;
			pageIndex = int.Parse(TxtBoxFumenPage.Text) - 1;
			var fumenField = decoder.Pages[pageIndex];

			var field = RadioBtnAppyToField.IsChecked == true ? Field : FieldHint;

			//下から8段目のインデックス
			int fumenDiff = 10 * 15;
			for (int i = 0; i < Field.FIELD_WIDTH * Field.FIELD_HEIGHT; i++)
				field.SetBoard(i, fumenField.Field[fumenDiff + i].ConvertFumenKind());

			field.SetHold('_');
			//ネクストミノを算出
			List<char> Nexts = new List<char>();
			for (int i = pageIndex; i < decoder.Pages.Count; i++)
			{
				if (decoder.Pages[i].MinoFlags.CurrentMino.Kind != Constants.BlockKind.Empty)
					Nexts.Add(decoder.Pages[i].MinoFlags.CurrentMino.Kind.ConvertFumenKind());
			}

			field.SetNext(Nexts.ToArray());
			//	}
			//catch (Exception)
			//	{
			//	MessageBox.Show("譜面の読み込みに失敗しました。");
			//	return;
			//	}
		}

		private void BtnApply_Click(object sender, RoutedEventArgs e)
		{
			ApplyFlag = true;
			Close();
		}

		private void FieldSelect(object sender, MouseButtonEventArgs e)
		{

		}

	}
}

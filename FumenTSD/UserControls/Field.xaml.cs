using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FumenTSD.UserControls
{
	/// <summary>
	/// Interaction logic for Field.xaml
	/// </summary>
	public partial class Field : UserControl
	{
		private List<Button> _boardElements = new List<Button>();
		public Field()
		{
			InitializeComponent();
			Hold.Title.Text = "HOLD";
			Next.Title.Text = "NEXT";
			InitializeBoard();
		}

		private void InitializeBoard()
		{
			var fieldParent = FieldParent;

			for (int rowIndex = 0; rowIndex < VisualTreeHelper.GetChildrenCount(fieldParent); rowIndex++)
			{
				UniformGrid? grid = VisualTreeHelper.GetChild(fieldParent, rowIndex) as UniformGrid;
				if (grid != null)
				{
					for (int columnIndex = 0; columnIndex < VisualTreeHelper.GetChildrenCount(grid); columnIndex++)
					{
						Button? button = VisualTreeHelper.GetChild(grid, columnIndex) as Button;
						if (button != null)
						{
							_boardElements.Add(button);
						}
					}
				}
			}
			//TODO: 直接文字列で指定しない
			_boardElements.ForEach(block => { block.Style = (Style)FindResource("BlockEmpty"); });
		}
	}
}

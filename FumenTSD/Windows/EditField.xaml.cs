using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FumenTSD.Windows
{
	/// <summary>
	/// Interaction logic for EditField.xaml
	/// </summary>
	public partial class EditField : Window
	{
		public EditField()
		{
			InitializeComponent();
			Field.Name.Text = "盤面";
			FieldHint.Name.Text = "ヒント用盤面";
		}
	}
}

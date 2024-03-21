using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
		private List<char> _board = new List<char>();
		private List<char> _next = new List<char>();
		private char _hold;
		private char _current;
		public IReadOnlyList<char> Board => _board.AsReadOnly();
		public IReadOnlyList<char> Next => _next.AsReadOnly();
		public char Hold => _hold;
		public char Current => _current;

		public bool Editable = false;
		public const int FIELD_WIDTH = 10;
		public const int FIELD_HEIGHT = 8;
		public Field()
		{
			InitializeComponent();
			ControlHold.Title.Text = "Hold";
			ControlCurrent.Title.Text = "Current";
			ControlNext.Title.Text = "NEXT";
			InitializeBoard();
			InitializeBoardEvent();
		}

		private void InitializeBoardEvent()
		{
			foreach (var button in _boardElements)
			{
				button.MouseEnter += Button_MouseEnter;
				button.Click += Button_Click;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if (!Editable)
				return;

			var name = (sender as Button).Name;
			var num = int.Parse(name.Substring(5));
			SetBoard(num, 'L');
		}

		private void Button_MouseEnter(object sender, MouseEventArgs e)
		{
			if (!Editable)
				return;

			if (e.RightButton == MouseButtonState.Pressed)
			{
				var name = (sender as Button).Name;
				var num = int.Parse(name.Substring(5));
				SetBoard(num, 'L');
			}

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
							_board.Add('_');
							button.Name = "Index" + (rowIndex * FIELD_WIDTH + columnIndex).ToString();
							SetBoard(rowIndex * FIELD_WIDTH + columnIndex, _board[^1]);
						}
					}
				}
			}

			_hold = '_';
			_current = '_';

		}

		public void CopyAll(Field field)
		{
			_board = new List<char>(field.Board);
			_next = new List<char>(field.Next);
			_hold = field.Hold;
			_current = field.Current;

			RefreshNext();
			RefreshHold();
			RefreshBoard();
			RefreshCurrent();
		}

		public void SetBoard(int index, char value)
		{
			_board[index] = value;
			_boardElements[index].Style = (Style)FindResource(GetBlockStyleNameFromCharKind(value));
		}

		public void SetBoard(IReadOnlyList<char> board)
		{
			_board.Clear();
			_board = new List<char>(board);
		}

		public void SetHold(char value)
		{
			_hold = value;

			RefreshHold();
		}

		public void SetCurrent(char value)
		{
			_current = value;

			RefreshCurrent();
		}

		public void SetNext(char[] values)
		{
			_next.Clear();
			_next.AddRange(values);

			RefreshNext();
		}

		public void AddNext(char value)
		{
			_next.Add(value);
			RefreshNext();
		}

		public void RemoveLatestNext()
		{
			if (_next.Count <= 0)
				return;
			_next.RemoveAt(_next.Count - 1);
			RefreshNext();
		}

		private void RefreshNext()
		{
			ControlNext.ClearMinos();
			foreach (char value in _next)
				ControlNext.AddMino(value);
		}
		private void RefreshBoard()
		{
			for (int i = 0; i < _boardElements.Count; i++)
			{
				_boardElements[i].Style = (Style)FindResource(GetBlockStyleNameFromCharKind(_board[i]));
			}
		}



		private void RefreshHold()
		{
			ControlHold.ClearMinos();
			if (_hold == '_')
				return;
			ControlHold.AddMino(_hold);
		}


		private void RefreshCurrent()
		{
			ControlCurrent.ClearMinos();
			if (_current == '_')
				return;
			ControlCurrent.AddMino(_current);
		}

		private static string GetBlockStyleNameFromCharKind(char kind)
		{
			switch (kind)
			{
				case 'L':
					return "BlockL";
				case 'J':
					return "BlockJ";
				case 'S':
					return "BlockS";
				case 'Z':
					return "BlockZ";
				case 'T':
					return "BlockT";
				case 'O':
					return "BlockO";
				case 'I':
					return "BlockI";
				case 'X':
					return "BlockGarbage";
				case '_':
					return "BlockEmpty";
				default:
					throw new ArgumentException();
			}
		}
	}
}

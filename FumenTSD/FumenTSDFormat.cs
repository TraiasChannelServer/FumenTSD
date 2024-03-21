using FumenTSD.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;
using System.Windows;

namespace FumenTSD
{
	public struct Clears
	{
		public Clears(int tsm, int tsmd, int tss, int tsd, int tst, int tetris, int pc, int ren)
		{
			TSM = tsm;
			TSMD = tsmd;
			TSS = tss;
			TSD = tsd;
			TST = tst;
			Tetris = tetris;
			PC = pc;
			REN = ren;
		}

		public void ApplyAt(int index, int value)
		{

		}

		public int TSM;
		public int TSMD;
		public int TSS;
		public int TSD;
		public int TST;
		public int Tetris;
		public int PC;
		public int REN;
	}


	class FumenTSDFormat
	{
		public const string Version = "1.0.0";
		public string Developer;
		public int Difficulty;
		public string MenuTitle;
		public string GameTitle;
		public string Comment;
		public Clears Clears;
		public char Current;
		public char Hold;
		public char[] Nexts;
		public char[] Board;
		public char[] BoardHint;
		public string Encode()
		{
			string output = string.Empty;
			StringBuilder builder = new StringBuilder();

			//line 1
			builder.Append(MenuTitle);
			builder.Append(",\n");
			//line 2
			builder.Append(GameTitle);
			builder.Append(",\n");
			//line 3
			builder.Append(Clears.TSM == 0 ? "*" : Clears.TSM);
			builder.Append(",");
			builder.Append(Clears.TSMD == 0 ? "*" : Clears.TSMD);
			builder.Append(",");
			builder.Append(Clears.TSS == 0 ? "*" : Clears.TSS);
			builder.Append(",");
			builder.Append(Clears.TSD == 0 ? "*" : Clears.TSD);
			builder.Append(",");
			builder.Append(Clears.TST == 0 ? "*" : Clears.TST);
			builder.Append(",");
			builder.Append(Clears.Tetris == 0 ? "*" : Clears.Tetris);
			builder.Append(",");
			builder.Append(Clears.PC == 0 ? "*" : Clears.PC);
			builder.Append(",");
			builder.Append(Clears.REN == 0 ? "*" : Clears.REN);
			builder.Append(",");
			builder.Append("\n");
			//line 4
			builder.Append(Current.ToString());
			builder.Append(",");
			builder.Append(Hold.ToString());
			builder.Append(",");
			foreach (var next in Nexts)
			{
				builder.Append(next.ToString());
				builder.Append(",");
			}
			builder.Append("\n");
			//line board
			for (int y = 0; y < Field.FIELD_HEIGHT; y++)
			{
				for (int x = 0; x < Field.FIELD_WIDTH; x++)
				{
					builder.Append(Board[x + y * Field.FIELD_WIDTH].ToString());
				}
				builder.Append(",");
				builder.Append("\n");
			}
			//line boardHint
			for (int y = 0; y < Field.FIELD_HEIGHT; y++)
			{chrome://vivaldi-webui/startpage?section=Speed-dials&background-color=#2f2f2f
				for (int x = 0; x < Field.FIELD_WIDTH; x++)
				{
					builder.Append(BoardHint[x + y * Field.FIELD_WIDTH].ToString());
				}
				builder.Append(",");
				builder.Append("\n");
			}

			//line difficulty
			builder.Append(Difficulty);
			builder.Append(",\n");

			//line developer
			builder.Append(Developer);
			builder.Append(",\n");

			//line comment
			builder.Append(Comment);


			return builder.ToString();
		}

		public static FumenTSDFormat? Decode(StreamReader reader)
		{
			int lineParsed = 1;
			FumenTSDFormat data = new FumenTSDFormat();
			try
			{
				{
					string input = reader.ReadLine()!;
					data.MenuTitle = input.Substring(0, input.Length - 1);
					lineParsed++;
				}

				{
					string input = reader.ReadLine()!;
					data.GameTitle = input.Substring(0, input.Length - 1);
					lineParsed++;
				}

				{
					string input = reader.ReadLine()!;
					var values = input.Split(',');

					int counter = 0;
					foreach (var value in values)
					{
						int valueInt = value == "*" ? 0 : int.Parse(value);
						data.Clears.ApplyAt(counter, valueInt);
						counter++;
					}
					lineParsed++;
				}


				{
					string input = reader.ReadLine()!;
					var values = input.Split(',');
					List<char> nexts = new List<char>();

					int counter = 0;
					foreach (var value in values)
					{
						if (counter == 0)
						{
							//Current
							data.Current = char.Parse(value);
						}
						else if (counter == 1)
						{
							//Hold
							data.Hold = char.Parse(value);
						}
						else
						{
							//Next
							nexts.Add(char.Parse(value));
						}


						counter++;
					}
					data.Nexts = nexts.ToArray();
					lineParsed++;
				}


				for (int y = 0; y < Field.FIELD_HEIGHT; y++)
				{
					string input = reader.ReadLine()!;
					List<char> board = new List<char>();
					foreach (char value in input.Substring(0, Field.FIELD_WIDTH))
					{
						board.Add(value);
					}
					data.Board = board.ToArray();

					lineParsed++;
				}

				for (int y = 0; y < Field.FIELD_HEIGHT; y++)
				{
					string input = reader.ReadLine()!;
					List<char> board = new List<char>();
					foreach (char value in input.Substring(0, Field.FIELD_WIDTH))
					{
						board.Add(value);
					}
					data.BoardHint = board.ToArray();

					lineParsed++;
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(lineParsed + "行目で読み込みに失敗しました。");
				return null;
			}

			return data;
		}

		private static void DecodeInner(ref int parseLine, ref FumenTSDFormat data, StreamReader reader)
		{
			//if()

			switch (parseLine)
			{
				case 0:

					break;
			}

			parseLine++;
		}
	}
}

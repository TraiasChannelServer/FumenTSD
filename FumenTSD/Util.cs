using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FumenTSD
{
	public static class Util
	{
		public struct Vector2
		{
			public Vector2(float x, float y)
			{
				this.x = x;
				this.y = y;
			}

			public float x;
			public float y;
		}

		private static readonly Dictionary<char, Vector2[]> MINO_POSITIONS = new(){
		{'J',new Vector2[] {new Vector2(0+0.5f,0+0.5f),new Vector2(0+0.5f,1+0.5f),new Vector2(1+0.5f,1+0.5f),new Vector2(2+0.5f,1+0.5f)}},
		{'T',new Vector2[] {new Vector2(1+0.5f,0+0.5f),new Vector2(0+0.5f,1+0.5f),new Vector2(1+0.5f,1+0.5f),new Vector2(2+0.5f,1+0.5f)}},
		{'L',new Vector2[] {new Vector2(2+0.5f,0+0.5f),new Vector2(0+0.5f,1+0.5f),new Vector2(1+0.5f,1+0.5f),new Vector2(2+0.5f,1+0.5f)}},
		{'S',new Vector2[] {new Vector2(1+0.5f,0+0.5f),new Vector2(0+0.5f,1+0.5f),new Vector2(1+0.5f,1+0.5f),new Vector2(2+0.5f,0+0.5f)}},
		{'Z',new Vector2[] {new Vector2(1+0.5f,0+0.5f),new Vector2(2+0.5f,1+0.5f),new Vector2(1+0.5f,1+0.5f),new Vector2(0+0.5f,0+0.5f)}},
		{'O',new Vector2[] {new Vector2(1,0+0.5f),new Vector2(2,0+0.5f),new Vector2(1,1+0.5f),new Vector2(2,1+0.5f)}},
		{'I',new Vector2[] {new Vector2(0,1),new Vector2(1,1),new Vector2(2,1),new Vector2(3,1)}},
		};

		private static readonly Dictionary<char, Brush> MINO_COLORS = new(){
		{'_',Brushes.White},
		{'X',Brushes.Gray},
		{'J',Brushes.Blue},
		{'T',Brushes.Purple},
		{'L',Brushes.Orange},
		{'S',Brushes.Green},
		{'Z',Brushes.Red},
		{'O',Brushes.Yellow},
		{'I',Brushes.LightBlue},
		};



		public static BitmapFrame CreateMinoBitmapFrame(char minoType, int blockSize)
		{
			using (Stream stream = new MemoryStream())
			{
				var bitmap = CreateMinoBitmap(minoType, blockSize);
				bitmap.Save(stream, ImageFormat.Png);
				stream.Seek(0, SeekOrigin.Begin);
				return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
			}
		}

		private static Bitmap CreateMinoBitmap(char minoType, int blockSize)
		{
			// 画像のサイズを設定
			int width = blockSize * 4;
			int height = blockSize * 3;

			// Bitmapを作成
			Bitmap bitmap = new Bitmap(width, height);

			// Graphicsオブジェクトを作成
			Graphics g = Graphics.FromImage(bitmap);

			// ブロックの色とサイズを設定
			Brush brush = MINO_COLORS[minoType];


			// 各ブロックに四角形を描画
			foreach (var position in MINO_POSITIONS[minoType])
			{
				g.FillRectangle(brush, position.x * blockSize, position.y * blockSize, blockSize, blockSize);
			}

			return bitmap;
			// Bitmapを保存
			//bitmap.Save("output.png");

			// リソースを解放
			//TODO:
			//g.Dispose();
			//bitmap.Dispose();
		}

		public static char ConvertFumenKind(this Fumen.Constants.BlockKind kind)
		{
			switch (kind)
			{
				case Fumen.Constants.BlockKind.Empty: return '_';
				case Fumen.Constants.BlockKind.I: return 'I';
				case Fumen.Constants.BlockKind.O: return 'O';
				case Fumen.Constants.BlockKind.T: return 'T';
				case Fumen.Constants.BlockKind.L: return 'L';
				case Fumen.Constants.BlockKind.J: return 'J';
				case Fumen.Constants.BlockKind.S: return 'S';
				case Fumen.Constants.BlockKind.Z: return 'Z';
				case Fumen.Constants.BlockKind.Garbage: return 'X';

				default:
					throw new ArgumentException();
			}
		}
	};



}

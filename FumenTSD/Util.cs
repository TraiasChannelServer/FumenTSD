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

		private static readonly Vector2[][] MINO_POSITIONS ={
		new    Vector2[] {new Vector2(0+0.5f,0+0.5f),new Vector2(0+0.5f,1+0.5f),new Vector2(1+0.5f,1+0.5f),new Vector2(2+0.5f,1+0.5f)},
		new    Vector2[]    {new Vector2(2+0.5f,0+0.5f),new Vector2(0+0.5f,1+0.5f),new Vector2(1+0.5f,1+0.5f),new Vector2(2+0.5f,1+0.5f)},
		new    Vector2[]    {new Vector2(1+0.5f,0+0.5f),new Vector2(0+0.5f,1+0.5f),new Vector2(1+0.5f,1+0.5f),new Vector2(2+0.5f,1+0.5f)},
		};





		public static BitmapFrame CreateMinoBitmapFrame(int minoType)
		{
			using (Stream stream = new MemoryStream())
			{
				var bitmap = CreateMinoBitmap(minoType);
				bitmap.Save(stream, ImageFormat.Png);
				stream.Seek(0, SeekOrigin.Begin);
				return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
			}
		}

		private static Bitmap CreateMinoBitmap(int minoType)
		{
			// 画像のサイズを設定
			int width = 10 * 4;
			int height = 10 * 3;

			// Bitmapを作成
			Bitmap bitmap = new Bitmap(width, height);

			// Graphicsオブジェクトを作成
			Graphics g = Graphics.FromImage(bitmap);

			// ブロックの色とサイズを設定
			Brush brush = Brushes.Green;
			int blockSize = 10;


			// 各ブロックに四角形を描画
			foreach (var position in MINO_POSITIONS[0])
			{
				g.FillRectangle(brush, position.x * blockSize, position.y * blockSize, blockSize, blockSize);
			}

			return bitmap;
			// Bitmapを保存
			//bitmap.Save("output.png");

			// リソースを解放
			//g.Dispose();
			//bitmap.Dispose();
		}
	};

}

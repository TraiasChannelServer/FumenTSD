using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Brushes = System.Drawing.Brushes;
using Image = System.Windows.Controls.Image;

namespace FumenTSD.UserControls
{
	/// <summary>
	/// Interaction logic for MinoViewer.xaml
	/// </summary>
	public partial class MinoViewer : UserControl
	{
		private List<BitmapFrame> _minoImages = new List<BitmapFrame>();

		public MinoViewer()
		{
			InitializeComponent();

			AddMino();
		}

		private void InitializeMinoImages()
		{

		}

		public void AddMino()
		{
			Image image = new Image();
			using (Stream stream = new MemoryStream())
			{
				var bitmap = CreateCustomBitmap();
				bitmap.Save(stream, ImageFormat.Png);
				stream.Seek(0, SeekOrigin.Begin);
				image.Source = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
			}

			image.Height = 20;
			MinoContainer.Children.Add(image);
		}
		private Bitmap CreateCustomBitmap()
		{
			// 200x100サイズのBitmap画像を作成
			Bitmap bitmap = new Bitmap(200, 100);

			// Bitmap画像のGraphicsオブジェクトを作成
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				// 全体を青で塗りつぶす
				g.FillRectangle(Brushes.Blue, g.VisibleClipBounds);

				// 赤い円を描画する
				g.DrawEllipse(Pens.Red, 50, 20, 100, 60);
			}


			return bitmap;
		}
		public void ClearMinos()
		{
			MinoContainer.Children.Clear();
		}

	}
}

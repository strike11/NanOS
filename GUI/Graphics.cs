using System;
using System.Collections.Generic;
using Cosmos.System.Graphics;
using System.Drawing;
using System = Cosmos.System;
using Cosmos.System;
using IL2CPU.API.Attribs;

namespace NanOS.GUI.Graphics
{
	public class Graphics
	{
		public static Bitmap wallpaper;
		public static Bitmap poweroffimg;
		//Стандартные обои
		[ManifestResourceStream(ResourceName = "NanOS.wallpaper1.bmp")]
		static byte[] wallpaperbyte;
		[ManifestResourceStream(ResourceName = "NanOS.poweroff.bmp")]
		static byte[] powerofficon;
		public Canvas canvas;
		public MouseState prevmouseState;
		private Pen mousepen;
		private List<Tuple<Cosmos.System.Graphics.Point, Color>> savedPixels;
		private UInt32 pX, pY;
		public Graphics()
        {
			Pen pen = new Pen(Color.Red);
			wallpaper = new Bitmap(wallpaperbyte);
			poweroffimg = new Bitmap(powerofficon);
			canvas = FullScreenCanvas.GetFullScreenCanvas(new Mode(1920, 1080, ColorDepth.ColorDepth32));
			canvas.Clear(Color.FromArgb(41, 51, 64));
			canvas.DrawImage(wallpaper, 0, 0);
			canvas.DrawFilledRectangle(pen, 0, 0, 1920, 40);
			canvas.DrawImage(poweroffimg, 1880, 8);
			this.prevmouseState = MouseState.None;

			MouseManager.ScreenHeight = 1080;
			MouseManager.ScreenWidth = 1920;

        }

		public void HadleGUIImput()
        {
			if(this.pX!= MouseManager.X && this.pY != MouseManager.Y)
            {
				if(MouseManager.X<2||MouseManager.Y<2||MouseManager.X>(MouseManager.ScreenWidth-2)||MouseManager.Y>(MouseManager.ScreenHeight-2))
                {
					return;

					this.pX = MouseManager.X;
					this.pY = MouseManager.Y;

					this.pX = 3;
					this.pY = 3;
					this.savedPixels = new List<Tuple<Cosmos.System.Graphics.Point, Color>>();
					Cosmos.System.Graphics.Point[] points = new Cosmos.System.Graphics.Point[]
					{
						new Cosmos.System.Graphics.Point((Int32)MouseManager.X,(Int32)MouseManager.Y),
						new Cosmos.System.Graphics.Point((Int32)MouseManager.X+1,(Int32)MouseManager.Y+1),
						new Cosmos.System.Graphics.Point((Int32)MouseManager.X-1,(Int32)MouseManager.Y-1),
						new Cosmos.System.Graphics.Point((Int32)MouseManager.X,(Int32)MouseManager.Y+1),
						new Cosmos.System.Graphics.Point((Int32)MouseManager.X,(Int32)MouseManager.Y-1)
					};

					foreach(Tuple<Cosmos.System.Graphics.Point, Color> pixelData in this.savedPixels)        
						this.canvas.DrawPoint(new Pen(pixelData.Item2), pixelData.Item1);

					this.savedPixels.Clear();


					foreach(Cosmos.System.Graphics.Point p in points)
                    {
						this.savedPixels.Add(new Tuple<Cosmos.System.Graphics.Point, Color>(p, this.canvas.GetPointColor(p.X, p.Y)));
						this.canvas.DrawPoint(this.mousepen,p);
                    }

				}

            }

            if (MouseManager.MouseState == MouseState.Left&&this.prevmouseState!=MouseState.Left)
				Cosmos.System.PCSpeaker.Beep();

			this.prevmouseState = MouseManager.MouseState;
        }


	}

}


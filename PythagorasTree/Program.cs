using System;
using System.Drawing;

namespace PythagorasTree
{
	class Program
	{
		static void Main(string[] args)
		{
			using Bitmap bitmap = new(1000, 1000);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.Clear(Color.White);
				using Pen leavesPen = new(Color.Black, 2);
				using Pen trunkPen = new(Color.Brown, 2);
				graphics.DrawTreeRecursive(Brushes.Brown, Brushes.Green, (bitmap.Width / 2) - 80, bitmap.Height, (bitmap.Width / 2) + 80, bitmap.Height, false, 1);
			}
			bitmap.Save("test.png");
		}
	}

	//Has to be public so other assemblies can use it
	public static class GraphicsExtension
	{
		/// <summary>
		/// Recursively draws pythagora's tree
		/// </summary>
		/// <param name="trunk">Color of the trunk</param>
		/// <param name="leaves">Color of the leaves</param>
		/// <param name="originX">X start position</param>
		/// <param name="originY">Y start position</param>
		/// <param name="endX">X end position</param>
		/// <param name="endY">Y end position</param>
		/// <param name="stroke">Whether the tree should have a black stroke</param>
		/// <param name="steps">The amount of steps. Higher step values take more time</param>
		public static void DrawTreeRecursive(this Graphics graphics, Brush trunk, Brush leaves, float originX, float originY, float endX, float endY, bool stroke, int steps)
		{
			float a = endX - originX;
			float b = endY - originY;

			PointF side1 = new(endX + b, endY - a);

			//this is some dumb calculation i came up with but it works and it makes sense somewhat so just deal with it ¯\_(ツ)_/¯
			PointF vertex = new(originX + (a * 0.5f) + 1.5f * b, endY - (b * 0.5f) - 1.5f * a);

			PointF side2 = new(originX + b, originY - a);

			PointF[] polyPoints = new PointF[]
			{
				//Base
				new PointF(originX, originY),
				new PointF(endX, endY),

				//side 1
				side1,

				//oblique side common point
				vertex,

				//side 2
				side2
			};

			if (steps > 0)
			{
				graphics.DrawTreeRecursive(Brushes.Brown, Brushes.Green, side2.X, side2.Y, vertex.X, vertex.Y, stroke, steps - 1);
				graphics.DrawTreeRecursive(Brushes.Brown, Brushes.Green, vertex.X, vertex.Y, side1.X, side1.Y, stroke, steps - 1);
				graphics.FillPolygon(trunk, polyPoints);
			}
			else
				graphics.FillPolygon(leaves, polyPoints);

			if (stroke)
				graphics.DrawPolygon(Pens.Black, polyPoints);
		}
	}
}

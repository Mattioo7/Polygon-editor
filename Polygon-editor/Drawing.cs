using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_editor
{
	public partial class Form1 : Form
	{
		private void drawConstraintNumberLength(Vertex a, Vertex b, int drawnNumber, Brush brush)
		{
			Point midPoint = new Point();
			midPoint.Y = (a.p.Y + b.p.Y) / 2;
			midPoint.X = (a.p.X + b.p.X) / 2;

			using (Graphics g = Graphics.FromImage(drawArea))
			{
				g.FillRectangle(brush, midPoint.X - RADIUS_OF_MARKER + 2, midPoint.Y - RADIUS_OF_MARKER + 2, (RADIUS_OF_MARKER - 2) * 2, (RADIUS_OF_MARKER - 2) * 2);
				g.DrawString(drawnNumber.ToString(),
					new Font("Ink Free", RADIUS_OF_MARKER, FontStyle.Bold),
					new SolidBrush(Color.Yellow),
					midPoint.X, midPoint.Y,
					new StringFormat()
					{ Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

			}
		}

		private void drawConstraintNumberParallel(Vertex a, Vertex b, int drawnNumber, Brush brush)
		{
			Point midPoint = new Point();
			midPoint.Y = (a.p.Y + b.p.Y) / 2;
			midPoint.X = (a.p.X + b.p.X) / 2;

			using (Graphics g = Graphics.FromImage(drawArea))
			{
				g.FillEllipse(brush, midPoint.X - RADIUS_OF_MARKER + 2, midPoint.Y - RADIUS_OF_MARKER + 2, (RADIUS_OF_MARKER - 2) * 2, (RADIUS_OF_MARKER - 2) * 2);
				g.DrawString(drawnNumber.ToString(),
					new Font("Ink Free", RADIUS_OF_MARKER, FontStyle.Bold),
					new SolidBrush(Color.Yellow),
					midPoint.X, midPoint.Y,
					new StringFormat()
					{ Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

			}
		}

		private void drawLine(Point a, Point b, Color color)
		{
			if (this.radioButton_bresenham.Checked)
			{
				drawLineBresenham(a.X, a.Y, b.X, b.Y, Color.Brown);
			}
			else
			{
				using (Graphics g = Graphics.FromImage(this.drawArea))
				{
					g.DrawLine(pen, a, b);
				}
			}
		}

		private void drawLineBresenham(int x, int y, int x2, int y2, Color color)
		{
			//http://tech-algorithm.com/articles/drawing-line-using-bresenham-algorithm/
			int w = x2 - x;
			int h = y2 - y;
			int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
			if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
			if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
			if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;

			int longest = Math.Abs(w);
			int shortest = Math.Abs(h);
			if (!(longest > shortest))
			{
				longest = Math.Abs(h);
				shortest = Math.Abs(w);
				if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
				dx2 = 0;
			}
			int numerator = longest >> 1;
			for (int i = 0; i <= longest; i++)
			{
				this.drawArea.SetPixel(x, y, color);
				numerator += shortest;
				if (!(numerator < longest))
				{
					numerator -= longest;
					x += dx1;
					y += dy1;
				}
				else
				{
					x += dx2;
					y += dy2;
				}
			}
		}

		private void reDraw()
		{
			/*foreach (Parallel con in parallelConstraints)
			{
				if (!con.isValid())
				{
					con.fix(1);
				}
			}*/

			using (Graphics g = Graphics.FromImage(this.drawArea))
			{
				g.Clear(Color.AliceBlue);
				foreach (Polygon poly in this.polygons)
				{
					if (poly == polygons.Last())
					{
						if (numberOfVerticesInNewPolygon == 0)
						{
							for (int i = 0; i < poly.vertices.Count; ++i)
							{
								drawLine(poly.vertices[i].p, poly.vertices[(poly.vertices.Count + i - 1) % poly.vertices.Count].p, Color.Black);
							}
						}
						else
						{
							for (int i = 1; i < poly.vertices.Count; ++i)
							{
								drawLine(poly.vertices[i].p, poly.vertices[(poly.vertices.Count + i - 1) % poly.vertices.Count].p, Color.Black);
							}
						}
					}
					else
					{
						for (int i = 0; i < poly.vertices.Count; ++i)
						{
							drawLine(poly.vertices[i].p, poly.vertices[(poly.vertices.Count + i - 1) % poly.vertices.Count].p, Color.Black);
						}
					}

					for (int i = 0; i < poly.vertices.Count; ++i)
					{
						g.FillEllipse(Brushes.Black, poly.vertices[i].p.X - RADIUS + 2, poly.vertices[i].p.Y - RADIUS + 2, (RADIUS - 2) * 2, (RADIUS - 2) * 2);
					}
				}
			}
			for (int i = 0; i < sameLenghtConstraints.Count; ++i)
			{
				drawConstraintNumberLength(sameLenghtConstraints[i].a, sameLenghtConstraints[i].b, i + 1, Brushes.Blue);
			}
			for (int i = 0; i < parallelConstraints.Count; ++i)
			{
				drawConstraintNumberParallel(parallelConstraints[i].a, parallelConstraints[i].b, i + 1, Brushes.Green);
				drawConstraintNumberParallel(parallelConstraints[i].c, parallelConstraints[i].d, i + 1, Brushes.Green);
			}
			this.pictureBox_workingArea.Refresh();
		}
	}
}

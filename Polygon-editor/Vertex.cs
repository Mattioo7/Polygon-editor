using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_editor
{
	internal class Vertex
	{
		public Point p;
		public Brush brush;

		public Vertex(Point point)
		{
			p = point;
			brush = Brushes.Black;
		}
		public Vertex(Point point, Brush br)
		{
			p = point;
			brush = br;
		}
	}
}

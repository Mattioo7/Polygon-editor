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
		//public int constraint_id;
		//public ConstraintType following_edge_constrain;

		public Vertex(Point point)
		{
			p = point;
			brush = Brushes.Black;
			//following_edge_constrain = ConstraintType.NONE;
		}
		public Vertex(Point point, Brush br)
		{
			p = point;
			brush = br;
			//following_edge_constrain = ConstraintType.NONE;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_editor
{
	public partial class Form1 : Form
	{
		private (Polygon?, int) findPolygonByVertex(Vertex vert)
		{
			Polygon? polygon = null;
			int index = -1;

			foreach (Polygon poly in this.polygons)
			{
				if (polygon != null)
				{
					break;
				}

				for (int i = 0; i < poly.vertices.Count; ++i)
				{
					if (poly.vertices[i] == vert)
					{
						polygon = poly;
						index = i;
						break;
					}
				}
			}

			return (polygon, index);
		}

		private Polygon? findPolygonByMouse(MouseEventArgs e)
		{
			Polygon? polygon = null;

			foreach (Polygon poly in this.polygons)
			{
				if (polygon != null)
				{
					break;
				}

				foreach (Vertex v in poly.vertices)
				{
					int yDiff = Math.Abs(v.p.Y - e.Y);
					int xDiff = Math.Abs(v.p.X - e.X);

					if (yDiff * yDiff + xDiff * xDiff < 4 * (RADIUS + 1) * (RADIUS + 1))
					{
						polygon = poly;
						break;
					}
				}
			}

			return polygon;
		}

		private Vertex? findVertex(MouseEventArgs e)
		{
			Vertex? foundVertex = null;

			foreach (Polygon poly in this.polygons)
			{
				if (foundVertex != null)
				{
					break;
				}

				foreach (Vertex v in poly.vertices)
				{
					int yDiff = Math.Abs(v.p.Y - e.Y);
					int xDiff = Math.Abs(v.p.X - e.X);

					if (yDiff * yDiff + xDiff * xDiff < 4 * (RADIUS + 1) * (RADIUS + 1))
					{
						foundVertex = v;
						break;
					}
				}
			}

			return foundVertex;
		}

		private (int?, int?, Polygon?) findEdge(MouseEventArgs e)
		{
			(int?, int?, Polygon?) edge = (null, null, null);


			foreach (Polygon poly in this.polygons)
			{
				if (edge != (null, null, null))
				{
					break;
				}

				for (int i = 0; i < poly.vertices.Count; ++i)
				{
					Vertex prev = poly.vertices[(poly.vertices.Count + i - 1) % poly.vertices.Count];
					Vertex next = poly.vertices[i];

					double AB = Math.Sqrt(Math.Pow(prev.p.X - next.p.X, 2) + Math.Pow(prev.p.Y - next.p.Y, 2));
					double AP = Math.Sqrt(Math.Pow(prev.p.X - e.X, 2) + Math.Pow(prev.p.Y - e.Y, 2));
					double PB = Math.Sqrt(Math.Pow(next.p.X - e.X, 2) + Math.Pow(next.p.Y - e.Y, 2));

					if (AP == 0 || PB == 0 || (AP + PB) < (AB + 0.5))
					{
						edge = ((poly.vertices.Count + i - 1) % poly.vertices.Count, i, poly);
						break;
					}
				}
			}

			return edge;
		}
	}
}

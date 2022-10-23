using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_editor
{
	public partial class Form1 : Form
	{
		private void addPolygon(MouseEventArgs e)
		{
			if (numberOfVerticesInNewPolygon == 0)
			{
				polygons.Add(new Polygon());
				polygons.Last().vertices.Add(new Vertex(new Point(e.X, e.Y)));
				numberOfVerticesInNewPolygon++;

				using (Graphics g = Graphics.FromImage(this.drawArea))
				{
					g.FillEllipse(Brushes.Black, e.X - RADIUS + 2, e.Y - RADIUS + 2, (RADIUS - 2) * 2, (RADIUS - 2) * 2);
				}
			}
			else
			{
				int yDiff_withFirst = Math.Abs(polygons.Last().vertices[0].p.Y - e.Y);
				int xDiff_withFirst = Math.Abs(polygons.Last().vertices[0].p.X - e.X);

				if (yDiff_withFirst * yDiff_withFirst + xDiff_withFirst * xDiff_withFirst < 4 * (RADIUS + 1) * (RADIUS + 1))
				{
					numberOfVerticesInNewPolygon = 0;
					using (Graphics g = Graphics.FromImage(this.drawArea))
					{
						drawLine(polygons.Last().vertices.Last().p, polygons.Last().vertices[0].p, Color.Black);
					}
				}
				else
				{
					bool flag_iClickOnOtherVert = false;

					foreach (var v in polygons.Last().vertices)
					{
						int yDiff = Math.Abs(v.p.Y - e.Y);
						int xDiff = Math.Abs(v.p.X - e.X);

						if (yDiff * yDiff + xDiff * xDiff < 4 * (RADIUS + 1) * (RADIUS + 1))
						{
							flag_iClickOnOtherVert = true;
							break;
						}
					}

					if (flag_iClickOnOtherVert == false)
					{
						using (Graphics g = Graphics.FromImage(this.drawArea))
						{
							numberOfVerticesInNewPolygon++;
							polygons.Last().vertices.Add(new Vertex(new Point(e.X, e.Y)));
							g.FillEllipse(Brushes.Black, e.X - RADIUS + 2, e.Y - RADIUS + 2, (RADIUS - 2) * 2, (RADIUS - 2) * 2);
							drawLine(polygons.Last().vertices.Last().p, polygons.Last().vertices[polygons.Last().vertices.Count - 2].p, Color.Black);
						}
					}

				}

			}
			this.pictureBox_workingArea.Refresh();
		}

		private void deletePolygon(MouseEventArgs e)
		{
			(int? a, int? b, Polygon? poly) edge = findEdge(e);

			if (edge.poly != null)
			{
				deletePolygon(edge.poly);
				reDraw();
				return;
			}

			foreach (Polygon poly in this.polygons)
			{
				foreach (Vertex v in poly.vertices)
				{
					int yDiff = Math.Abs(v.p.Y - e.Y);
					int xDiff = Math.Abs(v.p.X - e.X);

					if (yDiff * yDiff + xDiff * xDiff < 4 * (RADIUS + 1) * (RADIUS + 1))
					{
						deletePolygon(poly);
						return;
					}
				}
			}

			reDraw();
		}

		private void deletePolygon(Polygon poly)
		{
			foreach (Vertex vertex in poly.vertices)
			{
				List<Constraint> constraints = doesVertexHasConstraints(vertex);

				foreach (Constraint constraint in constraints)
				{
					if (constraint is SameLenght)
					{
						sameLenghtConstraints.Remove((SameLenght)constraint);
					}
					else
					{
						parallelConstraints.Remove((Parallel)constraint);
					}
				}
			}

			this.polygons.Remove(poly);
		}

		private void moveVertex(MouseEventArgs e)
		{
			pressedVertex = findVertex(e);

			if (pressedVertex != null)
			{
				mouseDown = true;

				mousePosition.X = pressedVertex.p.X;
				mousePosition.Y = pressedVertex.p.Y;
			}
		}

		private void deleteVertex(MouseEventArgs e)
		{
			Vertex? vertex = findVertex(e);

			if (vertex != null)
			{
				foreach (Polygon poly in this.polygons)
				{
					if (poly.vertices.Contains(vertex))
					{
						int idx = poly.vertices.IndexOf(vertex);
						Vertex a = poly.vertices[(idx - 1 + poly.vertices.Count) % poly.vertices.Count];
						Vertex c = poly.vertices[(idx + 1) % poly.vertices.Count];
						Constraint? constraint;

						constraint = doesEdgeHasConstraint(a, vertex);
						if (constraint != null)
						{
							if (constraint is SameLenght)
							{
								sameLenghtConstraints.Remove((SameLenght)constraint);
							}
							else
							{
								parallelConstraints.Remove((Parallel)constraint);
							}
						}
						constraint = doesEdgeHasConstraint(vertex, c);
						if (constraint != null)
						{
							if (constraint is SameLenght)
							{
								sameLenghtConstraints.Remove((SameLenght)constraint);
							}
							else
							{
								parallelConstraints.Remove((Parallel)constraint);
							}
						}

						poly.vertices.Remove(vertex);

						if (poly.vertices.Count <= 2)
						{
							deletePolygon(poly);
						}

						break;
					}
				}
				reDraw();
			}
		}

		private void addVertexOnTheEdge(MouseEventArgs e)
		{
			(int? a, int? b, Polygon? poly) edge = findEdge(e);

			if (edge == (null, null, null))
			{
				return;
			}

			Vertex a = edge.poly.vertices[(int)edge.a];
			Vertex b = edge.poly.vertices[(int)edge.b];

			Constraint? constraint = doesEdgeHasConstraint(a, b);
			if (constraint != null)
			{
				if (constraint is SameLenght)
				{
					sameLenghtConstraints.Remove((SameLenght)constraint);
				}
				else
				{
					parallelConstraints.Remove((Parallel)constraint);
				}
			}

			Vertex newVertex = new Vertex(new Point(e.X, e.Y));
			edge.poly.vertices.Insert((int)edge.b, newVertex);
			reDraw();
		}

		private void moveEdge(MouseEventArgs e)
		{
			(int? a, int? b, Polygon? poly) edge = findEdge(e);

			if (edge != (null, null, null))
			{
				pressedEdge = (edge.poly.vertices[(int)edge.a], edge.poly.vertices[(int)edge.b], edge.poly);

				mouseDown = true;
				mousePosition.X = e.X;
				mousePosition.Y = e.Y;
			}
		}

		private void movePolygon(MouseEventArgs e)
		{
			pressedVertex = findVertex(e);
			pressedPolygon = findPolygonByMouse(e);

			if (pressedPolygon == null)
			{
				(int? idxA, int? idxB, Polygon? poly) = findEdge(e);
				pressedPolygon = poly;
			}

			if (this.radioButton_movePolygon.Checked && pressedPolygon != null)
			{
				mouseDown = true;
				mousePosition.X = e.X;
				mousePosition.Y = e.Y;
			}
		}

		private void addSameLengthConstraint(MouseEventArgs e)
		{
			(int? a, int? b, Polygon? poly) edge = findEdge(e);

			if (edge == (null, null, null))
			{
				return;
			}

			Vertex a = edge.poly.vertices[(int)edge.a];
			Vertex b = edge.poly.vertices[(int)edge.b];
			if (doesEdgeHasConstraint(a, b) != null)
			{
				return;
			}

			pressedEdge = (a, b, edge.poly);

			sameLenghtConstraints.Add(new SameLenght(a, b));

			drawConstraintNumberLength(a, b, sameLenghtConstraints.Count, Brushes.Blue);
			this.pictureBox_workingArea.Refresh();
		}

		private void addParallelEdgesConstraint(MouseEventArgs e)
		{
			if (parallelEdges[0] == (null, null, null))
			{
				parallelEdges[0] = findEdge(e);

				return;
			}
			else
			{
				if (doesEdgeHasConstraint(parallelEdges[0].poly.vertices[(int)parallelEdges[0].idxA], parallelEdges[0].poly.vertices[(int)parallelEdges[0].idxB]) != null)
				{
					parallelEdges[0] = (null, null, null);
					parallelEdges[1] = (null, null, null);
					return;
				}

				parallelEdges[1] = findEdge(e);

				if (parallelEdges[1] == parallelEdges[0])
				{
					parallelEdges[1] = (null, null, null);
					return;
				}

				if (parallelEdges[0].poly == parallelEdges[1].poly && parallelEdges[0].poly.vertices.Count == 3)
				{
					MessageBox.Show("Invalid constraint", "error", MessageBoxButtons.OK);

					parallelEdges[0] = (null, null, null);
					parallelEdges[1] = (null, null, null);
					return;
				}

			}

			if (parallelEdges[1] != (null, null, null))
			{
				if (doesEdgeHasConstraint(parallelEdges[1].poly.vertices[(int)parallelEdges[1].idxA], parallelEdges[1].poly.vertices[(int)parallelEdges[1].idxB]) != null)
				{
					parallelEdges[0] = (null, null, null);
					parallelEdges[1] = (null, null, null);
					return;
				}

				Vertex a = parallelEdges[0].poly.vertices[(int)parallelEdges[0].idxA];
				Vertex b = parallelEdges[0].poly.vertices[(int)parallelEdges[0].idxB];
				Vertex c = parallelEdges[1].poly.vertices[(int)parallelEdges[1].idxA];
				Vertex d = parallelEdges[1].poly.vertices[(int)parallelEdges[1].idxB];
				parallelConstraints.Add(new Parallel(a, b, c, d, this));

				if (!parallelConstraints.Last().isValid())
				{
					parallelConstraints.Last().fix(1);
					reDraw();
				}

				foreach (Parallel parallel in parallelConstraints)
				{
					if (parallel.a == d)
					{
						parallel.fix(1);
					}
					else if (parallel.b == d)
					{
						parallel.fix(2);
					}
					else if (parallel.c == d)
					{
						parallel.fix(3);
					}
					else if (parallel.d == d)
					{
						parallel.fix(4);
					}
				}

				parallelEdges[0] = (null, null, null);
				parallelEdges[1] = (null, null, null);

				drawConstraintNumberParallel(a, b, parallelConstraints.Count, Brushes.Green);
				drawConstraintNumberParallel(c, d, parallelConstraints.Count, Brushes.Green);
				this.pictureBox_workingArea.Refresh();
			}
		}

		private void deleteConstraint(MouseEventArgs e)
		{
			(int? a, int? b, Polygon? poly) edge = findEdge(e);

			if (edge == (null, null, null))
			{
				return;
			}

			Vertex a = edge.poly.vertices[(int)edge.a];
			Vertex b = edge.poly.vertices[(int)edge.b];

			Constraint? constraint = doesEdgeHasConstraint(a, b);
			if (constraint == null)
			{
				return;
			}

			if (constraint is SameLenght)
			{
				sameLenghtConstraints.Remove((SameLenght)constraint);
			}
			else
			{
				parallelConstraints.Remove((Parallel)constraint);
			}

			reDraw();
		}

	}
}

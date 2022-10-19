using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Polygon_editor
{
	public partial class Form1 : Form
	{
		private Bitmap drawArea;
		private Pen pen;
		private int numberOfVerticesInNewPolygon;
		private List<Polygon> polygons;
		private List<SameLenght> sameLenghtConstraints;
		private List<Parallel> parallelConstraints;
		private const int RADIUS = 6;
		private const int RADIUS_OF_MARKER = 16;
		private bool mouseDown;
		private Vertex? pressedVertex;
		private (Vertex?, Vertex?) pressedEdge;
		private (int? idxA, int? idxB, Polygon? poly)[] parallelEdges;
		private Polygon? pressedPolygon;
		private Point mousePosition;

		public Form1()
		{
			InitializeComponent();

			initializeEnviroment();
		}

		private void initializeEnviroment()
		{
			drawArea = new Bitmap(this.pictureBox_workingArea.Size.Width, this.pictureBox_workingArea.Size.Height);
			this.pictureBox_workingArea.Image = drawArea;
			using (Graphics g = Graphics.FromImage(drawArea))
			{
				g.Clear(Color.AliceBlue);
			}

			pen = new Pen(Color.Black, 1);

			numberOfVerticesInNewPolygon = 0;
			polygons = new List<Polygon>();
			parallelConstraints = new List<Parallel>();
			sameLenghtConstraints = new List<SameLenght>();
			mouseDown = false;
			pressedVertex = null;
			pressedPolygon = null;

			parallelEdges = new (int?, int?, Polygon?)[2];
		}

		private void clearVariables()
		{
			numberOfVerticesInNewPolygon = 0;
			mouseDown = false;
			pressedVertex = null;
			pressedEdge = (null, null);
			parallelEdges[0] = (null, null, null);
			parallelEdges[1] = (null, null, null);
			pressedPolygon = null;
			mousePosition = new Point(0, 0);
		}

		private void pictureBox_workingArea_MouseDown(object sender, MouseEventArgs e)
		{
			if (this.radioButton_addPolygon.Checked)
			{
				this.label1.Text = "add polygon";
				addPolygon(e);
			}
			else if (this.radioButton_deletePolygon.Checked)
			{
				this.label1.Text = "delete polygon";
				deletePolygon(e);
			}
			else if (this.radioButton_moveVertex.Checked)
			{
				this.label1.Text = "move vertex";
				moveVertex(e);
			}
			else if (this.radioButton_deleteVertex.Checked)
			{
				this.label1.Text = "delete vertex";
				deleteVertex(e);
			}
			else if (this.radioButton_edgeVertex.Checked)
			{
				this.label1.Text = "edge vertex";
				addVertexOnTheEdge(e);
			}
			else if (this.radioButton_moveEdge.Checked)
			{
				this.label1.Text = "move edge";
				moveEdge(e);
			}
			else if (this.radioButton_movePolygon.Checked)
			{
				this.label1.Text = "move polygon";
				movePolygon(e);
			}
			else if (this.radioButton_sameLength.Checked)
			{
				this.label1.Text = "same length";
				addSameLengthConstraint(e);
			}
			else if (this.radioButton_parallel.Checked)
			{
				this.label1.Text = "parallel";
				addParallelEdgesConstraint(e);
			}
			else if (this.radioButton_deleteConstraint.Checked)
			{
				this.label1.Text = "delete constraint";
				deleteConstraint(e);
			}

		}

		private void pictureBox_workingArea_MouseUp(object sender, MouseEventArgs e)
		{
			if (mouseDown == true)
			{
				reDraw();
			}

			mouseDown = false;
		}

		private void addPolygon(MouseEventArgs e)
		{
			// klikam i sprawdzam czy to pierwszy

			// pierwszy
				// dodaje wierzcho³ek

			// kolejny
				// sprawdzam czy klikam na pierwszy
					// zamykam
				// inny ni¿ pierwszy 
					// nie dodaje
				// zaden 
					// dodaje


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
					// jesli klikam na pierwszy to domykam
					numberOfVerticesInNewPolygon = 0;
					using (Graphics g = Graphics.FromImage(this.drawArea))
					{
						drawLine(polygons.Last().vertices.Last().p, polygons.Last().vertices[0].p, g, Brushes.Black);
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
							drawLine(polygons.Last().vertices.Last().p, polygons.Last().vertices[polygons.Last().vertices.Count - 2].p, g, Brushes.Black);
						}
					}

				}

			}
			this.pictureBox_workingArea.Refresh();
		}

		private void deletePolygon(MouseEventArgs e)
		{
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
			mouseDown = true;

			mousePosition.X = e.X;
			mousePosition.Y = e.Y;
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
				this.label1.Text = "MISS!";
				return;
			}

			this.label1.Text = "EDGE!";

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
				pressedEdge = (edge.poly.vertices[(int)edge.a], edge.poly.vertices[(int)edge.b]);

				mouseDown = true;
				mousePosition.X = e.X;
				mousePosition.Y = e.Y;
			}
		}

		private void movePolygon(MouseEventArgs e)
		{
			pressedVertex = findVertex(e);
			pressedPolygon = findPolygonByMouse(e);

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

			pressedEdge = (a, b);

			sameLenghtConstraints.Add(new SameLenght(a, b));

			drawConstraintNumber(a, b, sameLenghtConstraints.Count, Brushes.Gray);
			this.pictureBox_workingArea.Refresh();

			this.label1.Text = "sameLen for: " + a.ToString() + " " + b.ToString();
		}

		private void addParallelEdgesConstraint(MouseEventArgs e)
		{
			if (parallelEdges[0] == (null, null, null))
			{
				parallelEdges[0] = findEdge(e);

				this.label1.Text = "edge1";

				return;
			}
			else
			{
				parallelEdges[1] = findEdge(e);

				if (parallelEdges[1] == parallelEdges[0])
				{
					parallelEdges[1] = (null, null, null);
					return;
				}

				if (parallelEdges[0].poly == parallelEdges[1].poly && parallelEdges[0].poly.vertices.Count == 3)
				{
					//put error
					MessageBox.Show("Invalid constraint", "error", MessageBoxButtons.OK);

					parallelEdges[0] = (null, null, null);
					parallelEdges[1] = (null, null, null);
					this.label1.Text = "null";
					return;
				}

				this.label1.Text = "edge2";
			}

			if (parallelEdges[1] != (null, null, null))
			{
				Vertex a = parallelEdges[0].poly.vertices[(int)parallelEdges[0].idxA];
				Vertex b = parallelEdges[0].poly.vertices[(int)parallelEdges[0].idxB];
				Vertex c = parallelEdges[1].poly.vertices[(int)parallelEdges[1].idxA];
				Vertex d = parallelEdges[1].poly.vertices[(int)parallelEdges[1].idxB];
				parallelConstraints.Add(new Parallel(a, b, c, d));

				if (!parallelConstraints.Last().isValid())
				{
					parallelConstraints.Last().fix(1);
					reDraw();
					this.label1.Text = "reDraw";
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

				drawConstraintNumber(a, b, parallelConstraints.Count, Brushes.Green);
				drawConstraintNumber(c, d, parallelConstraints.Count, Brushes.Green);
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

		private void drawConstraintNumber(Vertex a, Vertex b, int drawnNumber, Brush brush)
		{
			// tutaj dodaæ, aby grapich by³o podawane z góry

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

		private void drawLine(Point a, Point b, Graphics e, Brush br)
		{
			if (this.radioButton_bresenham.Checked)
			{
				drawLineBresenham(a.X, a.Y, b.X, b.Y, e, br);
			}
			else
			{
				using (Graphics g = Graphics.FromImage(this.drawArea))
				{
					g.DrawLine(pen, a, b);
				}
			}
		}

		private void drawLineBresenham(int x, int y, int x2, int y2, Graphics e, Brush b)
		{
			//http://tech-algorithm.com/articles/drawing-line-using-bresenham-algorithm/
			int w = x2 - x;
			int h = y2 - y;
			int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
			if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
			if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
			if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
			try
			{
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
					drawPixel(x, y, e, b);
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
			catch (Exception ex)
			{
				return;
			}
		}

		private void drawPixel(int x, int y, Graphics g, Brush b)
		{
			g.FillRectangle(b, x, y, 1, 1);
		}

		private void reDraw()
		{
			using (Graphics g = Graphics.FromImage(this.drawArea))
			{
				g.Clear(Color.AliceBlue);
				foreach (Polygon poly in this.polygons)
				{
					for (int i = 0; i < poly.vertices.Count; ++i)
					{
						drawLine(poly.vertices[i].p, poly.vertices[(poly.vertices.Count + i - 1) % poly.vertices.Count].p, g, Brushes.Black);
						g.FillEllipse(Brushes.Black, poly.vertices[i].p.X - RADIUS + 2, poly.vertices[i].p.Y - RADIUS + 2, (RADIUS - 2) * 2, (RADIUS - 2) * 2);
					}
				}
			}
			for (int i = 0; i < sameLenghtConstraints.Count; ++i)
			{
				// drawSameLength

				drawConstraintNumber(sameLenghtConstraints[i].a, sameLenghtConstraints[i].b, i + 1, Brushes.Gray);
			}
			for (int i = 0; i < parallelConstraints.Count; ++i)
			{
				drawConstraintNumber(parallelConstraints[i].a, parallelConstraints[i].b, i + 1, Brushes.Green);
				drawConstraintNumber(parallelConstraints[i].c, parallelConstraints[i].d, i + 1, Brushes.Green);
			}
			this.pictureBox_workingArea.Refresh();
		}

		private void button_clear_Click(object sender, EventArgs e)
		{
			initializeEnviroment(); 
			this.pictureBox_workingArea.Refresh();
		}

		private void pictureBox_workingArea_MouseMove(object sender, MouseEventArgs e)
		{
			// jeœli mouseDown oraz wybrany jest VertexMove to poruszamy wierzcho³kiem, któy jest klikniêty
			if (this.radioButton_moveVertex.Checked && mouseDown == true && pressedVertex != null)
			{
				pressedVertex.p.X = e.X;
				pressedVertex.p.Y = e.Y;

				this.label1.Text = "cords: " + e.X.ToString() + ", " + e.Y.ToString();

				// jeœli wierzcho³ek koñczy krawêdŸ, która ma fixed d³ugoœæ to ruszamy ca³¹ krawêdŸ
				// mam nadziejê, ¿e bêdzie to propagowaæ
				HashSet<Vertex> verticesToMove = fixedLengthVerticesList(pressedVertex);

				foreach (Vertex vertex in verticesToMove)
				{
					// one sie nie usuwaja tylko s¹ w jednym miejscu
					vertex.p.X += e.X - mousePosition.X;
					vertex.p.Y += e.Y - mousePosition.Y;
				};

				mousePosition.X = e.X;
				mousePosition.Y = e.Y;

				List<Constraint> constraints = doesVertexHasConstraints(pressedVertex);

				foreach (Constraint constraint in constraints)
				{
					if (constraint is Parallel)
					{
						Parallel parallel = (Parallel)constraint;

						if (pressedVertex == parallel.a)
						{
							parallel.fix(1);
						}
						else if (pressedVertex == parallel.b)
						{
							parallel.fix(2);
						}
						else if (pressedVertex == parallel.c)
						{
							parallel.fix(3);
						}
						else if (pressedVertex == parallel.d)
						{
							parallel.fix(4);
						}
					}
				}

				reDraw();
			}
			else if (this.radioButton_movePolygon.Checked && mouseDown == true && pressedPolygon != null)
			{
				foreach (Vertex v in pressedPolygon.vertices)
				{
					v.p.X += e.X - mousePosition.X;
					v.p.Y += e.Y - mousePosition.Y;
				}
				
				mousePosition.X = e.X;
				mousePosition.Y = e.Y;

				reDraw();
			}
			else if (this.radioButton_moveEdge.Checked && mouseDown == true && pressedEdge != (null, null))
			{
				HashSet<Vertex> verticesToMove = fixedLengthVerticesList(pressedEdge.Item1, pressedEdge.Item2);

				foreach (Vertex vertex in verticesToMove)
				{
					vertex.p.X += e.X - mousePosition.X;
					vertex.p.Y += e.Y - mousePosition.Y;
				}

				mousePosition.X = e.X;
				mousePosition.Y = e.Y;

				// fix constraints?
				List<Constraint> constraints = doesVertexHasConstraints(pressedEdge.Item1);

				foreach (Constraint constraint in constraints)
				{
					if (constraint is Parallel)
					{
						Parallel parallel = (Parallel)constraint;

						if (pressedEdge.Item1 == parallel.a)
						{
							parallel.fix(1);
						}
						else if (pressedEdge.Item1 == parallel.b)
						{
							parallel.fix(2);
						}
						else if (pressedEdge.Item1 == parallel.c)
						{
							parallel.fix(3);
						}
						else if (pressedEdge.Item1 == parallel.d)
						{
							parallel.fix(4);
						}
					}
				}

				constraints = doesVertexHasConstraints(pressedEdge.Item2);

				foreach (Constraint constraint in constraints)
				{
					if (constraint is Parallel)
					{
						Parallel parallel = (Parallel)constraint;

						if (pressedEdge.Item2 == parallel.a)
						{
							parallel.fix(1);
						}
						else if (pressedEdge.Item2 == parallel.b)
						{
							parallel.fix(2);
						}
						else if (pressedEdge.Item2 == parallel.c)
						{
							parallel.fix(3);
						}
						else if (pressedEdge.Item2 == parallel.d)
						{
							parallel.fix(4);
						}
					}
				}

				reDraw();
			}

			/*if (mouseDown == true)
			{
				foreach (Parallel con in parallelConstraints)
				{
					if (!con.isValid())
					{
						con.fix(1, this.label1);
						reDraw();
					}
				}
			}*/
		}

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

					if (AP == 0 || PB == 0 || (AP + PB) < (AB + 0.5) )
					{
						edge = ((poly.vertices.Count + i - 1) % poly.vertices.Count, i, poly);
						break;
					}
				}
			}

			return edge;
		}

		private void radioButton_addPolygon_CheckedChanged(object sender, EventArgs e)
		{
			if (!radioButton_addPolygon.Checked && numberOfVerticesInNewPolygon != 0)
			{
				deletePolygon(polygons.Last());
				numberOfVerticesInNewPolygon = 0;
				reDraw();
			}
		}

		private Constraint? doesEdgeHasConstraint(Vertex a, Vertex b)
		{
			foreach (SameLenght constraint in sameLenghtConstraints)
			{
				if ((a == constraint.a && b == constraint.b) || (b == constraint.a && a == constraint.b))
				{
					return constraint;
				}
			}

			foreach (Parallel constraint in parallelConstraints)
			{
				if ((a == constraint.a && b == constraint.b) || (b == constraint.a && a == constraint.b))
				{
					return constraint;
				}
				else if ((a == constraint.c && b == constraint.d) || (b == constraint.c && a == constraint.d))
				{
					return constraint;
				}
			}

			return null;
		}

		private List<Constraint> doesVertexHasConstraints(Vertex a)
		{
			List<Constraint> constraintsForVertex = new List<Constraint>();

			foreach (SameLenght constraint in sameLenghtConstraints)
			{
				if (constraint.containsVertex(a))
				{
					constraintsForVertex.Add(constraint);
				}
			}

			foreach (Parallel constraint in parallelConstraints)
			{
				if (constraint.containsVertex(a))
				{
					constraintsForVertex.Add(constraint);
				}
			}

			return constraintsForVertex;
		}

		private List<SameLenght> doesVertexHasSameLengthConstraint(Vertex a)
		{
			List<SameLenght> sameLenghtsConstraintsForVertex = new List<SameLenght>();

			foreach (SameLenght constraint in sameLenghtConstraints)
			{
				if (constraint.containsVertex(a))
				{
					sameLenghtsConstraintsForVertex.Add(constraint);
				}
			}

			return sameLenghtsConstraintsForVertex;
		}

		private List<Parallel> doesVertexHasParallelConstraint(Vertex a)
		{
			List<Parallel> parallelConstraintsForVertex = new List<Parallel>();

			foreach (Parallel constraint in parallelConstraints)
			{
				if (constraint.containsVertex(a))
				{
					parallelConstraintsForVertex.Add(constraint);
				}
			}

			return parallelConstraintsForVertex;
		}

		private HashSet<Vertex> fixedLengthVerticesList(Vertex a)
		{
			HashSet<Vertex> resultList = new HashSet<Vertex>();
			HashSet<Vertex> resultListCopy;
			resultList.Add(a);
			bool wasAdded;

			do
			{
				wasAdded = false;

				foreach (SameLenght sameLenghtConstraint in sameLenghtConstraints)
				{
					resultListCopy = new HashSet<Vertex>(resultList);
					foreach (Vertex v in resultList)
					{
						if (sameLenghtConstraint.containsVertex(v))
						{
							if (!resultListCopy.Contains(sameLenghtConstraint.a) || !resultListCopy.Contains(sameLenghtConstraint.b))
							{
								wasAdded = true;
							}
							resultListCopy.Add(sameLenghtConstraint.a);
							resultListCopy.Add(sameLenghtConstraint.b);
						}
					}
					resultList = resultListCopy;
				}

			} while (wasAdded == true);

			this.radioButton_addPolygon.Text = "Add polygon (poly: " + polygons.Count + ", v in last: " + polygons.Last().vertices.Count;

			return resultList;
		}

		private HashSet<Vertex> fixedLengthVerticesList(Vertex a, Vertex b)
		{
			HashSet<Vertex> resultList = new HashSet<Vertex>();
			HashSet<Vertex> resultListCopy;
			resultList.Add(a);
			resultList.Add(b);
			bool wasAdded;

			do
			{
				wasAdded = false;

				foreach (SameLenght sameLenghtConstraint in sameLenghtConstraints)
				{
					resultListCopy = new HashSet<Vertex>(resultList);
					foreach (Vertex v in resultList)
					{
						if (sameLenghtConstraint.containsVertex(v))
						{
							if (!resultListCopy.Contains(sameLenghtConstraint.a) || !resultListCopy.Contains(sameLenghtConstraint.b))
							{
								wasAdded = true;
							}
							resultListCopy.Add(sameLenghtConstraint.a);
							resultListCopy.Add(sameLenghtConstraint.b);
						}
					}
					resultList = resultListCopy;
				}

			} while (wasAdded == true);

			this.radioButton_addPolygon.Text = "Add polygon (poly: " + polygons.Count + ", v in last: " + polygons.Last().vertices.Count;

			return resultList;
		}

		private void Form1_SizeChanged(object sender, EventArgs e)
		{
			var newsize = tableLayoutPanel_main.GetControlFromPosition(0, 0).Size;
			drawArea = new Bitmap(newsize.Width, newsize.Height);
			pictureBox_workingArea.Image = drawArea;
			using (Graphics g = Graphics.FromImage(drawArea))
			{
				g.Clear(Color.LightBlue);
			}

			reDraw();

			pictureBox_workingArea.Refresh();
		}

		private void radioButton_defaultDrawing_CheckedChanged(object sender, EventArgs e)
		{
			reDraw();
			this.label1.Text = "Changed drawing mode to";

			if (this.radioButton_defaultDrawing.Checked == true)
			{
				this.label1.Text += " default";
			}
			else
			{
				this.label1.Text += " bresenham";
			}
		}
	}
}
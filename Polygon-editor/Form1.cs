using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.VisualStyles;

namespace Polygon_editor
{
	public partial class Form1 : Form
	{
		private Bitmap drawArea;
		private Pen pen;
		private int numberOfVerticesInNewPolygon;
		private List<Polygon> polygons;
		private List<Parallel> parallelConstraints;
		private List<SameLenght> sameLenghtConstraints;
		private const int RADIUS = 6;
		private bool mouseDown;
		private Vertex? pressedVertex;
		private (Vertex?, Vertex?) pressedEdge;
		private (int? idxA, int? idxB, Polygon? poly)[] parallelEdges;
		private Polygon? pressedPolygon;
		private Point mousePosition;

		public Form1()
		{
			InitializeComponent();

			// zrobi� funkcj� wyczy�� wszystkie zmienne
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
				sameLength(e);
			}
			else if (this.radioButton_parallel.Checked)
			{
				this.label1.Text = "parallel";
				parallel(e);
			}
			
		}

		private void pictureBox_workingArea_MouseUp(object sender, MouseEventArgs e)
		{
			mouseDown = false;	
		}

		private void addPolygon(MouseEventArgs e)
		{
			// klikam i sprawdzam czy to pierwszy

			// pierwszy
				// dodaje wierzcho�ek

			// kolejny
				// sprawdzam czy klikam na pierwszy
					// zamykam
				// inny ni� pierwszy 
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
						PutLine(polygons.Last().vertices.Last().p, polygons.Last().vertices[0].p, g, Brushes.Black);
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
							PutLine(polygons.Last().vertices.Last().p, polygons.Last().vertices[polygons.Last().vertices.Count - 2].p, g, Brushes.Black);
						}
					}

				}

			}
			this.pictureBox_workingArea.Refresh();
		}

		private void deletePolygon(MouseEventArgs e)
		{
			// na razie trzeba klikn�� na wierzcho�ek

			Polygon? polyToRemove = null;

			foreach (Polygon poly in this.polygons)
			{
				foreach (Vertex v in poly.vertices)
				{
					int yDiff = Math.Abs(v.p.Y - e.Y);
					int xDiff = Math.Abs(v.p.X - e.X);

					if (yDiff * yDiff + xDiff * xDiff < 4 * (RADIUS + 1) * (RADIUS + 1))
					{
						polyToRemove = poly;
					}
				}
			}

			if (polyToRemove != null)
			{
				this.polygons.Remove(polyToRemove);
			}

			reDraw();	// nie przesadza� z tymi reDraw()
		}

		private void deletePolygon(Polygon poly)
		{
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

			if (edge != (null, null, null))
			{
				this.label1.Text = "EDGE!";

				Vertex newVertex = new Vertex(new Point(e.X, e.Y));
				edge.poly.vertices.Insert((int)edge.b, newVertex);
				reDraw();
			}
			else
			{
				this.label1.Text = "MISS!";
			}

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

		private void sameLength(MouseEventArgs e)
		{
			// je�li jest zabkolowana d�ugo�� to poruszamy oboma wierzcho�kami w trakcie moveVertex oraz moveEdge r�wnie�
			// potrzebuj� iteracji po wierzcho�kach danego wielok�ta

			(int? a, int? b, Polygon? poly) edge = findEdge(e);

			if (edge != (null, null, null))
			{
				Vertex a = edge.poly.vertices[(int)edge.a];
				Vertex b = edge.poly.vertices[(int)edge.b];

				pressedEdge = (a,b);

				sameLenghtConstraints.Add(new SameLenght(a, b));
				this.label1.Text = "sameLen for: " + a.ToString() + " " + b.ToString();
			}

		}

		private void parallel(MouseEventArgs e)
		{
			if (parallelEdges[0] == (null, null, null))
			{
				parallelEdges[0] = findEdge(e);

				this.label1.Text = "edge1";
			}
			else
			{
				parallelEdges[1] = findEdge(e);

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
					parallelConstraints.Last().fix(2);
					reDraw();
					this.label1.Text = "reDraw";
				}

				parallelEdges[0] = (null, null, null);
				parallelEdges[1] = (null, null, null);
			}
		}

		private void PutLine(Point a, Point b, Graphics e, Brush br)
		{
			if (this.radioButton_bresenham.Checked)
			{
				BresenhamLine(a.X, a.Y, b.X, b.Y, e, br);
			}
			else
			{
				using (Graphics g = Graphics.FromImage(this.drawArea))
				{
					g.DrawLine(pen, a, b);
				}
			}
		}

		private void BresenhamLine(int x, int y, int x2, int y2, Graphics e, Brush b)
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
					PutPixel(x, y, e, b);
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

		private void PutPixel(int x, int y, Graphics e, Brush b)
		{
			e.FillRectangle(b, x, y, 1, 1);
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
						PutLine(poly.vertices[i].p, poly.vertices[(poly.vertices.Count + i - 1) % poly.vertices.Count].p, g, Brushes.Black);
						g.FillEllipse(Brushes.Black, poly.vertices[i].p.X - RADIUS + 2, poly.vertices[i].p.Y - RADIUS + 2, (RADIUS - 2) * 2, (RADIUS - 2) * 2);
					}
				}
			}
			this.pictureBox_workingArea.Refresh();
		}

		private void button_clear_Click(object sender, EventArgs e)
		{
			numberOfVerticesInNewPolygon = 0;
			polygons.Clear();

			using (Graphics g = Graphics.FromImage(this.drawArea))
			{
				g.Clear(Color.AliceBlue);
			}
			this.pictureBox_workingArea.Refresh();
		}

		private void pictureBox_workingArea_MouseMove(object sender, MouseEventArgs e)
		{
			// je�li mouseDown oraz wybrany jest VertexMove to poruszamy wierzcho�kiem, kt�y jest klikni�ty
			if (this.radioButton_moveVertex.Checked && mouseDown == true && pressedVertex != null)
			{
				pressedVertex.p.X = e.X;
				pressedVertex.p.Y = e.Y;

				this.label1.Text = "cords: " + e.X.ToString() + ", " + e.Y.ToString();

				// je�li wierzcho�ek ko�czy kraw�d�, kt�ra ma fixed d�ugo�� to ruszamy ca�� kraw�d�
				// mam nadziej�, �e b�dzie to propagowa�
				HashSet<Vertex> verticesToMove = fixedLengthVerticesList(pressedVertex);

				foreach (Vertex vertex in verticesToMove)
				{
					// one sie nie usuwaja tylko s� w jednym miejscu
					vertex.p.X += e.X - mousePosition.X;
					vertex.p.Y += e.Y - mousePosition.Y
				};

				mousePosition.X = e.X;
				mousePosition.Y = e.Y;

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

				reDraw();
			}

			if (mouseDown == true)
			{
				foreach(Parallel con in parallelConstraints)
				{
					if (!con.isValid())
					{
						con.fix(1);
						reDraw();
					}
				}
			}
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

		private (Constraint?, int) doesEdgeHasConstraint(Vertex a, Vertex b)
		{
			/*foreach (Constraint constraint in constraints)
			{
				if ((a == constraint.a && b == constraint.b) || (b == constraint.a && a == constraint.b))
				{
					return (constraint, 1); 
				}
				else if ((a == constraint.c && b == constraint.d) || (b == constraint.c && a == constraint.d))
				{
					return (constraint, 2); 
				}
			}*/

			return (null, -1);
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
	}
}
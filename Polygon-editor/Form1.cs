using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
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
		private (Vertex? a, Vertex? b, Polygon? poly) pressedEdge;
		private (int? idxA, int? idxB, Polygon? poly)[] parallelEdges;
		private Polygon? pressedPolygon;
		private Point mousePosition;
		private bool movePolygonMode;

		public Form1()
		{
			InitializeComponent();

			defaultScene();
		}

		private void pictureBox_workingArea_MouseDown(object sender, MouseEventArgs e)
		{
			if (this.radioButton_addPolygon.Checked)
			{
				addPolygon(e);
			}
			else if (this.radioButton_deletePolygon.Checked)
			{
				deletePolygon(e);
			}
			else if (this.radioButton_moveVertex.Checked)
			{
				moveVertex(e);
			}
			else if (this.radioButton_deleteVertex.Checked)
			{
				deleteVertex(e);
			}
			else if (this.radioButton_edgeVertex.Checked)
			{
				addVertexOnTheEdge(e);
			}
			else if (this.radioButton_moveEdge.Checked)
			{
				moveEdge(e);
			}
			else if (this.radioButton_movePolygon.Checked)
			{
				movePolygon(e);
			}
			else if (this.radioButton_sameLength.Checked)
			{
				addSameLengthConstraint(e);
			}
			else if (this.radioButton_parallel.Checked)
			{
				addParallelEdgesConstraint(e);
			}
			else if (this.radioButton_deleteConstraint.Checked)
			{
				deleteConstraint(e);
			}

			/*Vertex? v = findVertex(e);

			foreach (Parallel con in parallelConstraints)
			{
				if (!con.isValid())
				{
					if (v != null && con.containsVertex(v))
					{
						if (v == con.a)
						{
							con.fix(1);
						}
						else if (v == con.b)
						{
							con.fix(2);
						}
						else if (v == con.c)
						{
							con.fix(3);
						}
						else if (v == con.d)
						{
							con.fix(4);
						}
					}
					reDraw();
				}
			}*/
		}

		private void pictureBox_workingArea_MouseUp(object sender, MouseEventArgs e)
		{
			if (mouseDown == true)
			{
				reDraw();
			}

			mouseDown = false;
		}

		public void pictureBox_workingArea_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.radioButton_moveVertex.Checked && mouseDown == true && pressedVertex != null && movePolygonMode == false)
			{
				pressedVertex.p.X = e.X;
				pressedVertex.p.Y = e.Y;

				HashSet<Vertex> verticesToMove = fixedLengthVerticesList(pressedVertex);
				List<Constraint> constraints = doesVertexHasConstraints(pressedVertex);

				if (verticesToMove.Count > 1)
				{
					foreach (Vertex vertex in verticesToMove)
					{
						List<Constraint> vertexConstraints = doesVertexHasConstraints(vertex);

						foreach (Constraint constraint in vertexConstraints)
						{
							if (constraint is Parallel)
							{
								/*MessageBox.Show("Can't resolve constraints", "error", MessageBoxButtons.OK);
								mouseDown = false;
								return;*/

								/*mousePosition.X = e.X;
								mousePosition.Y = e.Y;*/
								pressedPolygon = findPolygonByMouse(e);
								movePolygonMode = true;
								return;
							}
						}
					}
				}

				foreach (Vertex vertex in verticesToMove)
				{
					vertex.p.X += e.X - mousePosition.X;
					vertex.p.Y += e.Y - mousePosition.Y;
				}

				mousePosition.X = e.X;
				mousePosition.Y = e.Y;


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

				foreach (Parallel con in parallelConstraints)
				{
					if (!con.isValid())
					{
						con.fix(1);
					}
				}

				reDraw();
			}
			if ((this.radioButton_movePolygon.Checked && mouseDown == true && pressedPolygon != null) || movePolygonMode == true)
			{
				foreach (Vertex v in pressedPolygon.vertices)
				{
					v.p.X += e.X - mousePosition.X;
					v.p.Y += e.Y - mousePosition.Y;
				}

				mousePosition.X = e.X;
				mousePosition.Y = e.Y;

				movePolygonMode = false;

				reDraw();
			}
			if (this.radioButton_moveEdge.Checked && mouseDown == true && pressedEdge != (null, null, null))
			{
				HashSet<Vertex> verticesToMove = fixedLengthVerticesList(pressedEdge.Item1, pressedEdge.Item2);

				foreach (Vertex vertex in verticesToMove)
				{
					vertex.p.X += e.X - mousePosition.X;
					vertex.p.Y += e.Y - mousePosition.Y;
				}

				mousePosition.X = e.X;
				mousePosition.Y = e.Y;

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

		private void pictureBox_workingArea_MouseClick(object sender, MouseEventArgs e)
		{
			Vertex? v = findVertex(e);
			
			foreach (Parallel con in parallelConstraints)
			{
				if (!con.isValid())
				{
					if (v != null && con.containsVertex(v))
					{
						if (v == con.a)
						{
							con.fix(1);
						}
						else if (v == con.b)
						{
							con.fix(2);
						}
						else if (v == con.c)
						{
							con.fix(3);
						}
						else if (v == con.d)
						{
							con.fix(4);
						}
					}
					reDraw();
				}
			}
		}

		private void button_clear_Click(object sender, EventArgs e)
		{
			initializeEnviroment();
			this.pictureBox_workingArea.Refresh();
		}

		private void button_defaultScene_Click(object sender, EventArgs e)
		{
			defaultScene();
		}
	}
}
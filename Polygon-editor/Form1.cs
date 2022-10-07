using System.Drawing;
using System.Windows.Forms.VisualStyles;
using static Polygon_editor.EditRadioButton;

namespace Polygon_editor
{
	public partial class Form1 : Form
	{
		private Bitmap drawArea;
		private Pen pen;
		private EditRadioButtonEnum radioButtonEdit;
		private int numberOfVerticesInNewPolygon;
		private List<Polygon> polygons;
		private int RADIUS = 6;

		public Form1()
		{
			InitializeComponent();

			drawArea = new Bitmap(this.pictureBox_workingArea.Size.Width, this.pictureBox_workingArea.Size.Height);
			this.pictureBox_workingArea.Image = drawArea;
			using (Graphics g = Graphics.FromImage(drawArea))
			{
				g.Clear(Color.AliceBlue);
			}

			pen = new Pen(Color.Black, 3);

			radioButtonEdit = EditRadioButtonEnum.ADD_POLYGON;
			this.label1.Text = EditRadioButtonEnum.ADD_POLYGON.ToString();
			numberOfVerticesInNewPolygon = 0;
			polygons = new List<Polygon>();
		}

		private void pictureBox_workingArea_MouseDown(object sender, MouseEventArgs e)
		{
			if (radioButtonEdit == EditRadioButtonEnum.ADD_POLYGON)
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
							DrawLine(polygons.Last().vertices.Last().p, polygons.Last().vertices[0].p, g, Brushes.Black);
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
								DrawLine(polygons.Last().vertices.Last().p, polygons.Last().vertices[polygons.Last().vertices.Count - 2].p, g, Brushes.Black);
							}
						}

					}

				}
				this.pictureBox_workingArea.Refresh();
			}


		}

		private void radioButton_addPolygon_CheckedChanged(object sender, EventArgs e)
		{
			radioButtonEdit = EditRadioButtonEnum.ADD_POLYGON;
			this.label1.Text = EditRadioButtonEnum.ADD_POLYGON.ToString();
		}

		private void radioButton_removePolygon_CheckedChanged(object sender, EventArgs e)
		{
			radioButtonEdit = EditRadioButtonEnum.REMOVE_POLYGON;
			this.label1.Text = EditRadioButtonEnum.REMOVE_POLYGON.ToString();
		}

		private void radioButton_editPolygon_CheckedChanged(object sender, EventArgs e)
		{
			radioButtonEdit = EditRadioButtonEnum.EDIT_POLYGON;
			this.label1.Text = EditRadioButtonEnum.EDIT_POLYGON.ToString();
		}

		private void radioButton_moveVertex_CheckedChanged(object sender, EventArgs e)
		{
			radioButtonEdit = EditRadioButtonEnum.MOVE_VERTEX;
			this.label1.Text = EditRadioButtonEnum.MOVE_VERTEX.ToString();
		}
		private void radioButton_deleteVertex_CheckedChanged(object sender, EventArgs e)
		{
			radioButtonEdit = EditRadioButtonEnum.DELETE_VERTEX;
			this.label1.Text = EditRadioButtonEnum.DELETE_VERTEX.ToString();
		}

		private void radioButton_edgeVertex_CheckedChanged(object sender, EventArgs e)
		{
			radioButtonEdit = EditRadioButtonEnum.EDGE_VERTEX;
			this.label1.Text = EditRadioButtonEnum.EDGE_VERTEX.ToString();
		}

		private void radioButton_moveEdge_CheckedChanged(object sender, EventArgs e)
		{
			radioButtonEdit = EditRadioButtonEnum.MOVE_EDGE;
			this.label1.Text = EditRadioButtonEnum.MOVE_EDGE.ToString();
		}

		private void radioButton_movePolygon_CheckedChanged(object sender, EventArgs e)
		{
			radioButtonEdit = EditRadioButtonEnum.MOVE_POLYGON;
			this.label1.Text = EditRadioButtonEnum.MOVE_POLYGON.ToString();
		}


		private void DrawLine(Point a, Point b, Graphics e, Brush br)
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



		// przyda siê metoda do zrobienia redraw ca³oœci
	}
}
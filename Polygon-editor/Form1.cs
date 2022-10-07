using System.Drawing;
using static Polygon_editor.EditRadioButton;

namespace Polygon_editor
{
	public partial class Form1 : Form
	{
		private Bitmap drawArea;
		private EditRadioButtonEnum radioButtonEdit;
		private int numberOfVerticesInNewPolygon;
		private List<Polygon> polygons;
		private int RADIUS = 4;

		public Form1()
		{
			InitializeComponent();

			drawArea = new Bitmap(this.pictureBox_workingArea.Size.Width, this.pictureBox_workingArea.Size.Height);
			this.pictureBox_workingArea.Image = drawArea;
			using (Graphics g = Graphics.FromImage(drawArea))
			{
				g.Clear(Color.AliceBlue);
			}

			radioButtonEdit = EditRadioButtonEnum.ADD_POLYGON;
			this.label1.Text = EditRadioButtonEnum.ADD_POLYGON.ToString();
			numberOfVerticesInNewPolygon = 0;
			polygons = new List<Polygon>();
		}

		private void pictureBox_workingArea_MouseDown(object sender, MouseEventArgs e)
		{
			if (radioButtonEdit == EditRadioButtonEnum.ADD_POLYGON)
			{
				if (numberOfVerticesInNewPolygon == 0)
				{
					polygons.Add(new Polygon());
				}
				polygons.Last().vertices.Add(new Vertex(new Point(e.X, e.Y)));

				using (Graphics g = Graphics.FromImage(this.drawArea))
				{
					g.FillEllipse(Brushes.Black, e.X - RADIUS + 2, e.Y - RADIUS + 2, (RADIUS - 2) * 2, (RADIUS - 2) * 2);
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

	}
}
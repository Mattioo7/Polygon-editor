using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_editor
{
	public partial class Form1 : Form
	{
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
			pressedEdge = (null, null, null);
			parallelEdges[0] = (null, null, null);
			parallelEdges[1] = (null, null, null);
			pressedPolygon = null;
			mousePosition = new Point(0, 0);
		}

		private void defaultScene()
		{
			initializeEnviroment();

			Vertex v1 = new Vertex(new Point(175, 100));
			Vertex v2 = new Vertex(new Point(100, 250));
			Vertex v3 = new Vertex(new Point(340, 240));
			Vertex v4 = new Vertex(new Point(250, 100));

			Vertex v5 = new Vertex(new Point(580, 80));
			Vertex v6 = new Vertex(new Point(480, 130));
			Vertex v7 = new Vertex(new Point(480, 240));
			Vertex v8 = new Vertex(new Point(580, 250));
			Vertex v9 = new Vertex(new Point(660, 250));
			Vertex v10 = new Vertex(new Point(660, 160));

			Vertex v11 = new Vertex(new Point(130, 490));
			Vertex v12 = new Vertex(new Point(240, 380));
			Vertex v13 = new Vertex(new Point(280, 490));

			Vertex v14 = new Vertex(new Point(390, 490));
			Vertex v15 = new Vertex(new Point(400, 370));
			Vertex v16 = new Vertex(new Point(530, 380));
			Vertex v17 = new Vertex(new Point(530, 490));

			polygons.Add(new Polygon());
			polygons[0].vertices.Add(v1);
			polygons[0].vertices.Add(v2);
			polygons[0].vertices.Add(v3);
			polygons[0].vertices.Add(v4);

			polygons.Add(new Polygon());
			polygons[1].vertices.Add(v5);
			polygons[1].vertices.Add(v6);
			polygons[1].vertices.Add(v7);
			polygons[1].vertices.Add(v8);
			polygons[1].vertices.Add(v9);
			polygons[1].vertices.Add(v10);

			polygons.Add(new Polygon());
			polygons[2].vertices.Add(v11);
			polygons[2].vertices.Add(v12);
			polygons[2].vertices.Add(v13);

			polygons.Add(new Polygon());
			polygons[3].vertices.Add(v14);
			polygons[3].vertices.Add(v15);
			polygons[3].vertices.Add(v16);
			polygons[3].vertices.Add(v17);

			parallelConstraints.Add(new Parallel(v4, v1, v2, v3, this));
			parallelConstraints.Add(new Parallel(v9, v8, v6, v5, this));
			parallelConstraints.Add(new Parallel(v11, v13, v14, v17, this));

			foreach (Parallel parallel in parallelConstraints)
			{
				if (!parallel.isValid())
				{
					parallel.fix(1);
				}
			}

			sameLenghtConstraints.Add(new SameLenght(v6, v7));
			sameLenghtConstraints.Add(new SameLenght(v15, v16));

			reDraw();
		}
	}
}

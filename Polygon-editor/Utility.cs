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
			pressedEdge = (null, null);
			parallelEdges[0] = (null, null, null);
			parallelEdges[1] = (null, null, null);
			pressedPolygon = null;
			mousePosition = new Point(0, 0);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_editor
{
	public partial class Form1 : Form
	{
		private void radioButton_addPolygon_CheckedChanged(object sender, EventArgs e)
		{
			if (!radioButton_addPolygon.Checked && numberOfVerticesInNewPolygon != 0)
			{
				deletePolygon(polygons.Last());
				clearVariables();
				reDraw();
			}
		}

		private void radioButton_edgeVertex_CheckedChanged(object sender, EventArgs e)
		{
			clearVariables();
		}

		private void radioButton_moveVertex_CheckedChanged(object sender, EventArgs e)
		{
			clearVariables();
		}

		private void radioButton_moveEdge_CheckedChanged(object sender, EventArgs e)
		{
			clearVariables();
		}

		private void radioButton_movePolygon_CheckedChanged(object sender, EventArgs e)
		{
			clearVariables();
		}

		private void radioButton_deleteVertex_CheckedChanged(object sender, EventArgs e)
		{
			clearVariables();
		}

		private void radioButton_deletePolygon_CheckedChanged(object sender, EventArgs e)
		{
			clearVariables();
		}

		private void radioButton_sameLength_CheckedChanged(object sender, EventArgs e)
		{
			clearVariables();
		}

		private void radioButton_parallel_CheckedChanged(object sender, EventArgs e)
		{
			clearVariables();
		}

		private void radioButton_deleteConstraint_CheckedChanged(object sender, EventArgs e)
		{
			clearVariables();
		}

		private void radioButton_defaultDrawing_CheckedChanged(object sender, EventArgs e)
		{
			reDraw();
		}
	}
}

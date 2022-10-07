using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_editor
{
	internal class Polygon
	{
		public List<Vertex> vertices;
		public List<Constraint> constraints;

		public Polygon()
		{
			vertices = new List<Vertex>();
			constraints = new List<Constraint>();
		}


	}
}

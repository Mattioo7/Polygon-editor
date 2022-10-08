using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Polygon_editor
{
	internal abstract class Constraint
	{
		public Vertex a, b, c, d;

		public abstract void fix(int edgeNr);

		public abstract bool isValid();
	}

	internal class SameLenght : Constraint
	{
		public double firstSegmentLength()
		{
			return Math.Sqrt(Math.Pow(a.p.X - b.p.X, 2) + Math.Pow(a.p.Y - b.p.Y, 2));
		}

		public double secondSegmentLength()
		{
			return Math.Sqrt(Math.Pow(a.p.X - b.p.X, 2) + Math.Pow(a.p.Y - b.p.Y, 2));
		}

		public override bool isValid()
		{
			return firstSegmentLength() == secondSegmentLength();
		}

		public override void fix(int edgeNr)
		{
			Vertex vertex1;
			Vertex vertex2;
			double desiredLength;
			double brokenLength;

			if (edgeNr == 1)
			{
				vertex1 = a;
				vertex2 = b;
				desiredLength = secondSegmentLength();
				brokenLength = firstSegmentLength();
			}
			else
			{
				vertex1 = c;
				vertex2 = d;
				desiredLength = firstSegmentLength();
				brokenLength = secondSegmentLength();
			}

			double alfa = desiredLength / brokenLength;
			double diffrence = desiredLength - brokenLength;
			Point v = new Point(d.p.X - b.p.X, d.p.Y - b.p.Y);

			v.X = (int)Math.Round(v.X * alfa);
			v.Y = (int)Math.Round(v.Y * alfa);
			if (v.X < 20000 && v.Y < 20000)
				d.p.X = b.p.X + v.X;
			d.p.Y = b.p.Y + v.Y;
		
		}
	}

	internal class Parallel : Constraint
	{

	}

}

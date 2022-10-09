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

		public double SegmentLength(Point p, Point v)
		{
			double diffX = (v.X - p.X);
			double diffY = (v.Y - p.Y);

			return Math.Sqrt(diffX * diffX + diffY * diffY);
		}
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
		public Parallel(Vertex a, Vertex b, Vertex c, Vertex d)
		{
			this.a = a;
			this.b = b;
			this.c = c;
			this.d = d;
		}
		
		public override void fix(int edgeNr)
		{
			double len = SegmentLength(a.p, b.p);
			double len2 = SegmentLength(c.p, d.p);
			double wsp = len2 / len;
			if (len >= 0.01)
			{
				Point v = new Point(b.p.X - a.p.X, b.p.Y - a.p.Y);

				v.X = (int)(Math.Round(v.X * wsp));
				v.Y = (int)(Math.Round(v.Y * wsp));
				if (Angle(a.p, b.p, c.p, d.p) > 0)
				{
					d.p.X = c.p.X + v.X;
					d.p.Y = c.p.Y + v.Y;
				}
				else
				{
					d.p.X = c.p.X - v.X;
					d.p.Y = c.p.Y - v.Y;
				}
			}
		}

		public override bool isValid()
		{
			if ((a.p.X == b.p.X) && (c.p.X == d.p.X))
			{
				return true;
			}

			if ((a.p.X == b.p.X) || (a.p.X == b.p.X))
			{
				return false;
			}

			double a1 = (b.p.Y - a.p.Y);
			double a2 = (b.p.X - a.p.X);
			double a3 = (d.p.Y - c.p.Y);
			double a4 = (d.p.X - c.p.X);


			double m1 = (b.p.Y - a.p.Y) * 1.0 / (b.p.X - a.p.X);
			double m2 = (d.p.Y - c.p.Y) * 1.0 / (d.p.X - c.p.X);

			double aa = Math.Abs(m2 - m1);

			if (Math.Abs(m2-m1) < 0.0001)
			{
				return true;
			}

			return false;
		}
		public double Angle(Point p, Point v, Point p2, Point v2)
		{
			Point diff1 = new Point(v.X - p.X, v.Y - p.Y);
			Point diff2 = new Point(v2.X - p2.X, v2.Y - p2.Y);

			return Iloczyn(diff1, diff2) / (Math.Sqrt(Iloczyn(diff1, diff1)) * Math.Sqrt(Iloczyn(diff2, diff2)));
		}
		public double Iloczyn(Point diff1, Point diff2)
		{
			return diff1.X * diff2.X + diff1.Y * diff2.Y;
		}

	}

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Polygon_editor
{
	public partial class Form1 : Form
	{
		internal abstract class Constraint
		{
			public Vertex a, b;

			public abstract void fix(int edgeNr);

			public abstract bool isValid();

			public double segmentLength(Point p, Point v)
			{
				double diffX = (v.X - p.X);
				double diffY = (v.Y - p.Y);

				return Math.Sqrt(diffX * diffX + diffY * diffY);
			}
		}

		internal class SameLenght : Constraint
		{
			public SameLenght(Vertex a, Vertex b)
			{
				this.a = a;
				this.b = b;
			}

			public override void fix(int edgeNr)
			{
				throw new NotImplementedException();
			}

			public override bool isValid()
			{
				throw new NotImplementedException();
			}

			public bool containsVertex(Vertex v)
			{
				return v == a || v == b;
			}
		}

		internal class Parallel : Constraint
		{
			Form1 form1;
			public Vertex c, d;

			public Parallel(Vertex a, Vertex b, Vertex c, Vertex d, Form1 form1)
			{
				this.a = a;
				this.b = b;
				this.c = c;
				this.d = d;
				this.form1 = form1;
			}

			/*public void fix(int vertexNumber, Label label)
			{
				double len = segmentLength(a.p, b.p);
				double len2 = segmentLength(c.p, d.p);
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
						label.Text = "dodatni";
					}
					else
					{
						d.p.X = c.p.X - v.X;
						d.p.Y = c.p.Y - v.Y;
						label.Text = "ujemny";
					}
				}


			}*/

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

				if (Math.Abs(m2 - m1) < 0.0001)
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

			public bool containsVertex(Vertex v)
			{
				return v == a || v == b || v == c || v == d;
			}

			public override void fix(int nr)
			{
				if (nr == 2)
				{
					// b <-> a
					// c <-> d
					Vertex temp = b;
					b = a;
					a = temp;

					temp = c;
					c = d;
					d = temp;
				}
				else if (nr == 3)
				{
					// a <-> c
					// b <-> d
					Vertex temp = a;
					a = c;
					c = temp;

					temp = b;
					b = d;
					d = temp;
				}
				else if (nr == 4)
				{
					// a <-> d
					// b <-> c
					Vertex temp = a;
					a = d;
					d = temp;

					temp = b;
					b = c;
					c = temp;
				}

				double len = segmentLength(a.p, b.p);
				double len2 = segmentLength(c.p, d.p);
				double wsp = len2 / len;
				if (len >= 0.01)
				{
					// v to wektor z b do a
					Point v = new Point(b.p.X - a.p.X, b.p.Y - a.p.Y);

					// tutaj nadajemy mu długość aby cd było wsp razy dłuższe, tak jak wcześniej
					v.X = (int)(Math.Round(v.X * wsp));
					v.Y = (int)(Math.Round(v.Y * wsp));

					// szukam czy d nie należy do jakiś constraintów
					HashSet<Vertex> verticesToMove = form1.fixedLengthVerticesList(d);
					verticesToMove.Remove(d);
				
					if (Angle(a.p, b.p, c.p, d.p) > 0)
					{
						foreach (Vertex vertex in verticesToMove)
						{
							vertex.p.X += -(d.p.X - (c.p.X + v.X));
							vertex.p.Y += -(d.p.Y - (c.p.Y + v.Y));
						};

						d.p.X = c.p.X + v.X;
						d.p.Y = c.p.Y + v.Y;
					}
					else
					{
						foreach (Vertex vertex in verticesToMove)
						{
							vertex.p.X += -(d.p.X - (c.p.X - v.X));
							vertex.p.Y += -(d.p.Y - (c.p.Y - v.Y));
						};

						d.p.X = c.p.X - v.X;
						d.p.Y = c.p.Y - v.Y;
					}
				}
			}
		}
	}

	/*internal abstract class Constraint
	{
		public Vertex a, b;

		public abstract void fix(int edgeNr);

		public abstract bool isValid();

		public double segmentLength(Point p, Point v)
		{
			double diffX = (v.X - p.X);
			double diffY = (v.Y - p.Y);

			return Math.Sqrt(diffX * diffX + diffY * diffY);
		}
	}

	internal class SameLenght : Constraint
	{
		public SameLenght(Vertex a, Vertex b)
		{
			this.a = a;
			this.b = b;
		}

		public override void fix(int edgeNr)
		{
			throw new NotImplementedException();
		}

		public override bool isValid()
		{
			throw new NotImplementedException();
		}

		public bool containsVertex(Vertex v)
		{
			return v == a || v == b;
		}
	}

	internal class Parallel : Constraint
	{
		public Vertex c, d;

		public Parallel(Vertex a, Vertex b, Vertex c, Vertex d)
		{
			this.a = a;
			this.b = b;
			this.c = c;
			this.d = d;
		}
		
		public void fix(int vertexNumber, Label label)
		{
			double len = segmentLength(a.p, b.p);
			double len2 = segmentLength(c.p, d.p);
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
					label.Text = "dodatni";
				}
				else
				{
					d.p.X = c.p.X - v.X;
					d.p.Y = c.p.Y - v.Y;
					label.Text = "ujemny";
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

		public bool containsVertex(Vertex v)
		{
			return v == a || v == b || v == c || v == d;
		}

		public override void fix(int nr)
		{
			if (nr == 2)
			{
				// b <-> a
				// c <-> d
				Vertex temp = b;
				b = a;
				a = temp;

				temp = c;
				c = d;
				d = temp;
			}
			else if (nr == 3)
			{
				// a <-> c
				// b <-> d
				Vertex temp = a;
				a = c;
				c = temp;

				temp = b;
				b = d;
				d = temp;
			}
			else if (nr == 4)
			{
				// a <-> d
				// b <-> c
				Vertex temp = a;
				a = d;
				d = temp;

				temp = b;
				b = c;
				c = temp;
			}

			double len = segmentLength(a.p, b.p);
			double len2 = segmentLength(c.p, d.p);
			double wsp = len2 / len;
			if (len >= 0.01)
			{
				Point v = new Point(b.p.X - a.p.X, b.p.Y - a.p.Y);

				v.X = (int)(Math.Round(v.X * wsp));
				v.Y = (int)(Math.Round(v.Y * wsp));

				// szukam czy d nie należy do jakiś constraintów
				HashSet<Vertex> verticesToMove = fixedLengthVerticesList(pressedVertex);

				foreach (Vertex vertex in verticesToMove)
				{
					// one sie nie usuwaja tylko są w jednym miejscu
					vertex.p.X += e.X - mousePosition.X;
					vertex.p.Y += e.Y - mousePosition.Y;
				};

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
	}*/

}

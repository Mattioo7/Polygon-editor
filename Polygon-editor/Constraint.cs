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

			public Constraint(Vertex a, Vertex b)
			{
				this.a = a;
				this.b = b;
			}

			public double segmentLength(Point p, Point v)
			{
				double diffX = (v.X - p.X);
				double diffY = (v.Y - p.Y);

				return Math.Sqrt(diffX * diffX + diffY * diffY);
			}

			public abstract bool containsVertex(Vertex v);
		}

		internal class SameLenght : Constraint
		{
			public SameLenght(Vertex a, Vertex b) : base(a, b) { }

			public override bool containsVertex(Vertex v)
			{
				return v == a || v == b;
			}
		}

		internal class Parallel : Constraint
		{
			Form1 form1;
			public Vertex c, d;

			public Parallel(Vertex a, Vertex b, Vertex c, Vertex d, Form1 form1) : base(a, b)
			{
				this.c = c;
				this.d = d;
				this.form1 = form1;
			}

			public bool isValid()
			{
				if ((a.p.X == b.p.X) && (c.p.X == d.p.X))
				{
					return true;
				}

				if ((a.p.X == b.p.X) || (a.p.X == b.p.X))
				{
					return false;
				}

				double var1 = (b.p.Y - a.p.Y) * 1.0 / (b.p.X - a.p.X);
				double var2 = (d.p.Y - c.p.Y) * 1.0 / (d.p.X - c.p.X);

				if (Math.Abs(var2 - var1) < 0.0001)
				{
					return true;
				}

				return false;
			}
			public double Angle(Point a, Point b, Point c, Point d)
			{
				Point v1 = new Point(b.X - a.X, b.Y - a.Y);
				Point v2 = new Point(d.X - c.X, d.Y - c.Y);

				return vectorProduct(v1, v2) / (Math.Sqrt(vectorProduct(v1, v1)) * Math.Sqrt(vectorProduct(v2, v2)));
			}
			public double vectorProduct(Point v1, Point v2)
			{
				return v1.X * v2.X + v1.Y * v2.Y;
			}

			public override bool containsVertex(Vertex v)
			{
				return v == a || v == b || v == c || v == d;
			}

			public void fix(int nr)
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
							if (vertex == d)
							{
								MessageBox.Show("Can't resolve constraints", "error", MessageBoxButtons.OKCancel);
								return;
							}

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
							if (vertex == d)
							{
								MessageBox.Show("Can't resolve constraints", "error", MessageBoxButtons.OK);
								return;
							}

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

}

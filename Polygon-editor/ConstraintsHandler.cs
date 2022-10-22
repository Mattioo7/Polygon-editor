using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_editor
{
	public partial class Form1 : Form
	{
		private Constraint? doesEdgeHasConstraint(Vertex a, Vertex b)
		{
			foreach (SameLenght constraint in sameLenghtConstraints)
			{
				if ((a == constraint.a && b == constraint.b) || (b == constraint.a && a == constraint.b))
				{
					return constraint;
				}
			}

			foreach (Parallel constraint in parallelConstraints)
			{
				if ((a == constraint.a && b == constraint.b) || (b == constraint.a && a == constraint.b))
				{
					return constraint;
				}
				else if ((a == constraint.c && b == constraint.d) || (b == constraint.c && a == constraint.d))
				{
					return constraint;
				}
			}

			return null;
		}

		private List<Constraint> doesVertexHasConstraints(Vertex a)
		{
			List<Constraint> constraintsForVertex = new List<Constraint>();

			foreach (SameLenght constraint in sameLenghtConstraints)
			{
				if (constraint.containsVertex(a))
				{
					constraintsForVertex.Add(constraint);
				}
			}

			foreach (Parallel constraint in parallelConstraints)
			{
				if (constraint.containsVertex(a))
				{
					constraintsForVertex.Add(constraint);
				}
			}

			return constraintsForVertex;
		}

		internal List<SameLenght> doesVertexHasSameLengthConstraint(Vertex a)
		{
			List<SameLenght> sameLenghtsConstraintsForVertex = new List<SameLenght>();

			foreach (SameLenght constraint in sameLenghtConstraints)
			{
				if (constraint.containsVertex(a))
				{
					sameLenghtsConstraintsForVertex.Add(constraint);
				}
			}

			return sameLenghtsConstraintsForVertex;
		}

		private List<Parallel> doesVertexHasParallelConstraint(Vertex a)
		{
			List<Parallel> parallelConstraintsForVertex = new List<Parallel>();

			foreach (Parallel constraint in parallelConstraints)
			{
				if (constraint.containsVertex(a))
				{
					parallelConstraintsForVertex.Add(constraint);
				}
			}

			return parallelConstraintsForVertex;
		}

		private HashSet<Vertex> fixedLengthVerticesList(Vertex a)
		{
			HashSet<Vertex> resultList = new HashSet<Vertex>();
			HashSet<Vertex> resultListCopy;
			resultList.Add(a);
			bool wasAdded;

			do
			{
				wasAdded = false;

				foreach (SameLenght sameLenghtConstraint in sameLenghtConstraints)
				{
					resultListCopy = new HashSet<Vertex>(resultList);
					foreach (Vertex v in resultList)
					{
						if (sameLenghtConstraint.containsVertex(v))
						{
							if (!resultListCopy.Contains(sameLenghtConstraint.a) || !resultListCopy.Contains(sameLenghtConstraint.b))
							{
								wasAdded = true;
							}
							resultListCopy.Add(sameLenghtConstraint.a);
							resultListCopy.Add(sameLenghtConstraint.b);
						}
					}
					resultList = resultListCopy;
				}

			} while (wasAdded == true);

			return resultList;
		}

		private HashSet<Vertex> fixedLengthVerticesList(Vertex a, Vertex b)
		{
			HashSet<Vertex> resultList = new HashSet<Vertex>();
			HashSet<Vertex> resultListCopy;
			resultList.Add(a);
			resultList.Add(b);
			bool wasAdded;

			do
			{
				wasAdded = false;

				foreach (SameLenght sameLenghtConstraint in sameLenghtConstraints)
				{
					resultListCopy = new HashSet<Vertex>(resultList);
					foreach (Vertex v in resultList)
					{
						if (sameLenghtConstraint.containsVertex(v))
						{
							if (!resultListCopy.Contains(sameLenghtConstraint.a) || !resultListCopy.Contains(sameLenghtConstraint.b))
							{
								wasAdded = true;
							}
							resultListCopy.Add(sameLenghtConstraint.a);
							resultListCopy.Add(sameLenghtConstraint.b);
						}
					}
					resultList = resultListCopy;
				}

			} while (wasAdded == true);

			return resultList;
		}
	}
}

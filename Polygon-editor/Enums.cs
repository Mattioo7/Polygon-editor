using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_editor
{
    internal class EditRadioButton
    {
        public enum EditRadioButtonEnum
        {
            ADD_POLYGON,
            REMOVE_POLYGON,
            EDIT_POLYGON,
            MOVE_VERTEX,
            DELETE_VERTEX,
            EDGE_VERTEX,
            MOVE_EDGE,
            MOVE_POLYGON
        }

		public enum ConstraintTypes
		{
			SAME_LENGTH,
            PARALLEL_EDGES,
            NONE
		}
	}
}

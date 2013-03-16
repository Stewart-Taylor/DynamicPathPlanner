/*      PathNode Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This class is used to store path data
 *
 * Last Updated: 16/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPathPlanner.Code
{
    class PathNode
    {
        public int x;
        public int y;
        public PathNode parent;
        public PathNode child;
    }

}

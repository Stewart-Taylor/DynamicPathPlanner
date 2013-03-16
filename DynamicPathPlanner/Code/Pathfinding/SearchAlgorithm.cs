/*      SearchAlgorithm Class
 *	    AUTHOR: STEWART TAYLOR
 *------------------------------------
 * This abstract provides a base for search algorithms
 * 
 * Last Updated: 16/03/2013
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPathPlanner.Code
{
    abstract class SearchAlgorithm
    {
        protected List<PathNode> pathNodes = new List<PathNode>();

        protected int[,] grid;
        protected int startX;
        protected int startY;
        protected int targetX;
        protected int targetY;


        public SearchAlgorithm(int[,] grid, int startX, int startY, int targetX, int targetY)
        {

        }

        public List<PathNode> getPath()
        {
            return pathNodes;
        }


    }
}

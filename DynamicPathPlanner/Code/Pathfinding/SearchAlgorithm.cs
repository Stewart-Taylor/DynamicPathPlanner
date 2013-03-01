using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicPathPlanner.Code
{
    abstract class SearchAlgorithm
    {
        protected List<PathNode> pathNodes = new List<PathNode>();

        protected double[,] grid;
        protected int startX;
        protected int startY;
        protected int targetX;
        protected int targetY;


        public SearchAlgorithm(double[,] grid, int startX, int startY, int targetX, int targetY)
        {

        }

        public List<PathNode> getPath()
        {
            return pathNodes;
        }


    }
}

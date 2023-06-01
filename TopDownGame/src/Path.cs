using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SFML.Graphics;

namespace TopDownGame
{
    class Path
    {
        // Points alogn the path
        public Vector2[] Points { get; set; }
        // The index of the target point
        private int targetIndex;
        // Whether the path repeats to be a cycle
        private bool repeating;

        // Gets the position of the target along the path
        public Vector2 Target { get { return Points[targetIndex]; } }

        public Path(int pathCount, bool repeating = true)
        {
            Points = new Vector2[pathCount];
            targetIndex = 0;
            this.repeating = repeating;
        }

        /// <summary>
        /// sets the targets position to the next point along the path
        /// </summary>
        public void IncrementPath()
        {
            targetIndex++;
            if (targetIndex >= Points.Length)
                targetIndex = repeating ? 0 : Points.Length - 1;
        }

        /// <summary>
        /// Draws each point along the path
        /// </summary>
        public void Render()
        {
            foreach (Vector2 point in Points)
            {
                RectangleShape rect = new RectangleShape(new Vector2(20, 20));
                rect.FillColor = new Color(66, 105, 245);
                rect.Position = point - (new Vector2(10, 10));
                Window.Draw(rect);
            }
        }

        /// <summary>
        /// The Node in the graph search
        /// </summary>
        private class Node
        {
            public Vector2 Pos { get; }
            public float Distance { get; }
            public float Cost { get; }
            public Node Previous { get; }

            public string Dir { get; }

            public Node(Vector2 pos, Vector2 endPos, Node previous = null, string dir = "")
            {
                Pos = pos;
                Previous = previous;
                Dir = dir;
                Distance = Heuristic(endPos);
                Cost = PathCost();
            }

            public List<Vector2> Path()
            {
                List<Vector2> path = new List<Vector2> { Pos };
                Node focus = this;
                Node last = this;
                while (focus.Previous != null)
                {
                    if (focus.Dir != last.Dir)
                        path.Insert(0, focus.Pos);
                    last = focus;
                    focus = focus.Previous;
                }
                return path;
            }

            public float PathCost()
            {
                float cost = 0;
                Node focus = this;
                while (focus.Previous != null)
                {
                    cost += (float)Vector2.Distance(focus.Pos, focus.Previous.Pos);
                    focus = focus.Previous;
                }
                return cost;
            }

            public float Heuristic(Vector2 endPos)
            {
                return (float)Vector2.Distance(Pos, endPos);
            }
        }

        /// <summary>
        /// Checks if the path to the next target is clear 
        /// </summary>
        /// <param name="startPos">The position to look at the target</param>
        /// <param name="tileMap"></param>
        /// <returns></returns>
        public bool IsPathClear(Vector2 startPos, TileMap tileMap)
        {
            // is path to node clear 
            CollisionLine line = new CollisionLine(startPos, Target, tileMap);
            return (!line.IsColliding(tileMap.CollisionShape));
        }

        /// <summary>
        /// Creates a Path using the A* algorithm
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="tileMap"></param>
        public Path(Vector2 startPos, Vector2 endPos, TileMap tileMap)
        {
            targetIndex = 0;
            repeating = false;

            // is path to node clear 
            CollisionLine line = new CollisionLine(startPos, endPos, tileMap);
            if (!line.IsColliding(tileMap.CollisionShape))
            {
                Points = new Vector2[1] { endPos };
                return;
            }

            List<Node> frontier = new List<Node>();
            List<Node> checkedNodes = new List<Node>();
            Node initialNode = new Node(startPos, endPos);
            frontier.Add(initialNode);
            checkedNodes.Add(initialNode);

            while (frontier.Count != 0)
            {
                // get the fist item in the fronteir
                Node focus = frontier.OrderBy(f => f.Distance + f.Cost).ToList().First();
                frontier.Remove(focus);

                // if selection is at goal
                if (focus.Pos == endPos)
                {
                    Points = focus.Path().ToArray();
                    return;
                }

                // get the children
                int x = (int)Math.Floor(focus.Pos.X / tileMap.tileSize);
                int y = (int)Math.Floor(focus.Pos.Y / tileMap.tileSize);
                // expand along the grid 
                if (tileMap[y, x + 1] != 'w')
                {
                    Node node = new Node(new Vector2((x + 1) * tileMap.tileSize + tileMap.tileSize / 2, y * tileMap.tileSize + tileMap.tileSize / 2), endPos, focus, "r");
                    if (checkedNodes.All(n => n.Pos != node.Pos || n.Cost + n.Distance > n.Cost + n.Distance))
                    {
                        frontier.Add(node);
                        checkedNodes.Add(node);
                    }  
                }
                if (tileMap[y, x - 1] != 'w')
                {
                    Node node = new Node(new Vector2((x - 1) * tileMap.tileSize + tileMap.tileSize / 2, y * tileMap.tileSize + tileMap.tileSize / 2), endPos, focus, "l");
                    if (checkedNodes.All(n => n.Pos != node.Pos || n.Cost + n.Distance > n.Cost + n.Distance))
                    {
                        frontier.Add(node);
                        checkedNodes.Add(node);
                    }
                }
                if (tileMap[y + 1, x] != 'w')
                {
                    Node node = new Node(new Vector2(x * tileMap.tileSize + tileMap.tileSize / 2, (y + 1) * tileMap.tileSize + tileMap.tileSize / 2), endPos, focus, "d");
                    if (checkedNodes.All(n => n.Pos != node.Pos || n.Cost + n.Distance > n.Cost + n.Distance))
                    {
                        frontier.Add(node);
                        checkedNodes.Add(node);
                    }
                }
                if (tileMap[y - 1, x] != 'w')
                {
                    Node node = new Node(new Vector2(x * tileMap.tileSize + tileMap.tileSize / 2, (y - 1) * tileMap.tileSize + tileMap.tileSize / 2), endPos, focus, "u");
                    if (checkedNodes.All(n => n.Pos != node.Pos || n.Cost + n.Distance > n.Cost + n.Distance))
                    {
                        frontier.Add(node);
                        checkedNodes.Add(node);
                    }
                }

                // expand along the diagonal
                if (tileMap[y + 1, x + 1] != 'w' && tileMap[y, x + 1] != 'w' && tileMap[y + 1, x] != 'w')
                {
                    Node node = new Node(new Vector2((x + 1) * tileMap.tileSize + tileMap.tileSize / 2, (y + 1) * tileMap.tileSize + tileMap.tileSize / 2), endPos, focus, "dr");
                    if (checkedNodes.All(n => n.Pos != node.Pos || n.Cost + n.Distance > n.Cost + n.Distance))
                    {
                        frontier.Add(node);
                        checkedNodes.Add(node);
                    }
                }
                if (tileMap[y - 1, x - 1] != 'w' && tileMap[y, x - 1] != 'w' && tileMap[y - 1, x] != 'w')
                {
                    Node node = new Node(new Vector2((x - 1) * tileMap.tileSize + tileMap.tileSize / 2, (y - 1) * tileMap.tileSize + tileMap.tileSize / 2), endPos, focus, "ul");
                    if (checkedNodes.All(n => n.Pos != node.Pos || n.Cost + n.Distance > n.Cost + n.Distance))
                    {
                        frontier.Add(node);
                        checkedNodes.Add(node);
                    }
                }
                if (tileMap[y + 1, x - 1] != 'w' && tileMap[y, x - 1] != 'w' && tileMap[y + 1, x] != 'w')
                {
                    Node node = new Node(new Vector2((x - 1) * tileMap.tileSize + tileMap.tileSize / 2, (y + 1) * tileMap.tileSize + tileMap.tileSize / 2), endPos, focus, "dl");
                    if (checkedNodes.All(n => n.Pos != node.Pos || n.Cost + n.Distance > n.Cost + n.Distance))
                    {
                        frontier.Add(node);
                        checkedNodes.Add(node);
                    }
                }
                if (tileMap[y - 1, x + 1] != 'w' && tileMap[y, x + 1] != 'w' && tileMap[y - 1, x] != 'w')
                {
                    Node node = new Node(new Vector2((x + 1) * tileMap.tileSize + tileMap.tileSize / 2, (y - 1) * tileMap.tileSize + tileMap.tileSize / 2), endPos, focus, "ur");
                    if (checkedNodes.All(n => n.Pos != node.Pos || n.Cost + n.Distance > n.Cost + n.Distance))
                    {
                        frontier.Add(node);
                        checkedNodes.Add(node);
                    }
                }
            }

            Points = new Vector2[1] { startPos };
        }
    }
}

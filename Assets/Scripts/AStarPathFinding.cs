using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding
{
    public class Node
    {
        public Vector3Int position;
        public bool walkable;

        public int gCost;
        public int hCost;
        public int fCost => gCost + hCost;

        public Node parent;

        public Node(Vector3Int pos, bool walkable)
        {
            position = pos;
            this.walkable = walkable;
            Reset();
        }

        public void Reset()
        {
            gCost = int.MaxValue;
            hCost = 0;
            parent = null;
        }
    }

    // gridMap: position -> node
    public static List<Vector3Int> FindPath(Dictionary<Vector3Int, Node> gridMap, Vector3Int startPos, Vector3Int targetPos)
    {
        if (!gridMap.ContainsKey(startPos) || !gridMap.ContainsKey(targetPos))
            return null;

        foreach (var kv in gridMap)
            kv.Value.Reset();

        Node startNode = gridMap[startPos];
        Node targetNode = gridMap[targetPos];

        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);

        var openList = new List<Node> { startNode };
        var closedSet = new HashSet<Node>();

        while (openList.Count > 0)
        {
            Node current = GetLowestFCostNode(openList);

            if (current == targetNode)
                return ReconstructPath(current);

            openList.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in GetNeighbors(gridMap, current))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                int tentativeG = current.gCost + GetDistance(current, neighbor);

                if (tentativeG < neighbor.gCost)
                {
                    neighbor.gCost = tentativeG;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = current;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        return null; // no path
    }

    static List<Vector3Int> ReconstructPath(Node end)
    {
        var path = new List<Vector3Int>();
        Node cur = end;
        while (cur != null)
        {
            path.Add(cur.position);
            cur = cur.parent;
        }
        path.Reverse();
        return path;
    }

    static Node GetLowestFCostNode(List<Node> list)
    {
        Node best = list[0];
        for (int i = 1; i < list.Count; i++)
        { 
            if (list[i].fCost < best.fCost || (list[i].fCost == best.fCost && list[i].hCost < best.hCost))
                best = list[i];
        }
        return best;
    }

    static IEnumerable<Node> GetNeighbors(Dictionary<Vector3Int, Node> gridMap, Node node)
    {
        Vector3Int[] dirs = new Vector3Int[]
        {
            new Vector3Int(1,0,0),
            new Vector3Int(-1,0,0),
            new Vector3Int(0,1,0),
            new Vector3Int(0,-1,0),
        };

        foreach (var d in dirs)
        {
            var np = node.position + d;
            if (gridMap.TryGetValue(np, out var neighbor))
                yield return neighbor;
        }
    }

    static int GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.position.x - b.position.x);
        int dy = Mathf.Abs(a.position.y - b.position.y);
        return dx + dy;
    }
}

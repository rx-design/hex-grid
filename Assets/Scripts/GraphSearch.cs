using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GraphSearch
{
    // Breadth-first search
    public static SearchResult GetRange(HexGrid hexGrid, Vector3Int startPoint, int movementPoints)
    {
        var visitedNodes = new Dictionary<Vector3Int, Vector3Int?>();
        var costSoFar = new Dictionary<Vector3Int, int>();
        var nodesToVisitQueue = new Queue<Vector3Int>();

        nodesToVisitQueue.Enqueue(startPoint);
        costSoFar.Add(startPoint, 0);
        visitedNodes.Add(startPoint, null);

        while (nodesToVisitQueue.Count > 0)
        {
            var currentNode = nodesToVisitQueue.Dequeue();

            foreach (var neighbourPosition in hexGrid.GetNeighboursFor(currentNode))
            {
                if (hexGrid.GetTileAt(neighbourPosition).IsObstacle())
                {
                    continue;
                }

                var nodeCost = hexGrid.GetTileAt(neighbourPosition).GetCost();
                var currentCost = costSoFar[currentNode];
                var newCost = currentCost + nodeCost;

                if (newCost > movementPoints) continue;
                if (!visitedNodes.ContainsKey(neighbourPosition))
                {
                    visitedNodes[neighbourPosition] = currentNode;
                    costSoFar[neighbourPosition] = newCost;
                    nodesToVisitQueue.Enqueue(neighbourPosition);
                }
                else if (costSoFar[neighbourPosition] > newCost)
                {
                    costSoFar[neighbourPosition] = newCost;
                    visitedNodes[neighbourPosition] = currentNode;
                }
            }
        }

        return new SearchResult
        {
            VisitedNodesDict = visitedNodes
        };
    }

    public static List<Vector3Int> GeneratePath(Vector3Int current,
        IReadOnlyDictionary<Vector3Int, Vector3Int?> visitedNodesDict)
    {
        var path = new List<Vector3Int> { current };

        while (visitedNodesDict[current] != null)
        {
            path.Add(visitedNodesDict[current].Value);
            current = visitedNodesDict[current].Value;
        }

        path.Reverse();

        return path.Skip(1).ToList();
    }
}

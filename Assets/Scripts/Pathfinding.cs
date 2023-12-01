using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    public static List<Vector2> FindPath(Vector2 start, Vector2 end)
    {
       
        Dictionary<Vector2, float> gScore = new Dictionary<Vector2, float>();
        Dictionary<Vector2, float> fScore = new Dictionary<Vector2, float>();
        HashSet<Vector2> openSet = new HashSet<Vector2>();
        HashSet<Vector2> closedSet = new HashSet<Vector2>();
        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();

        openSet.Add(start);
        gScore[start] = 0;
        fScore[start] = HeuristicCostEstimate(start, end);

        while (openSet.Count > 0)
        {
            Vector2 current = GetLowestFScore(openSet, fScore,gScore);

            if (current == end)
                return ReconstructPath(cameFrom, end);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Vector2 neighbor in GetNeighbors(current, GridManager.Instance.tiles))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float tentativeGScore = gScore[current] + 1; // Assuming uniform cost for simplicity

                if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, end) + Random.Range(0.01f, 0.1f);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        // No path found
        return new List<Vector2>();
    }

    private static float HeuristicCostEstimate(Vector2 start, Vector2 end)
    {
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
    }

    private static Vector2 GetLowestFScore(HashSet<Vector2> openSet, Dictionary<Vector2, float> fScore , Dictionary<Vector2, float> gScore)
    {
        Vector2 lowest = openSet.First();
        foreach (Vector2 node in openSet)
        {
            if (fScore[node] < fScore[lowest])
                lowest = node;
            else if (fScore[node] == fScore[lowest] && gScore[node] < gScore[lowest])
                lowest = node;
        }
        return lowest;
    }

    private static List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
    {
        List<Vector2> path = new List<Vector2> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }

    private static IEnumerable<Vector2> GetNeighbors(Vector2 center, Dictionary<Vector2, Tile> tiles)
    {
        int x = (int)center.x;
        int y = (int)center.y;

        if (IsValidNeighbor(x + 1, y, tiles)) yield return new Vector2(x + 1, y);
        if (IsValidNeighbor(x - 1, y, tiles)) yield return new Vector2(x - 1, y);
        if (IsValidNeighbor(x, y + 1, tiles)) yield return new Vector2(x, y + 1);
        if (IsValidNeighbor(x, y - 1, tiles)) yield return new Vector2(x, y - 1);
    }

    private static bool IsValidNeighbor(int x, int y, Dictionary<Vector2, Tile> tiles)
    {
        Vector2 neighbor = new Vector2(x, y);

        return tiles.ContainsKey(neighbor) && !tiles[neighbor].hasBuilding;
    }
}


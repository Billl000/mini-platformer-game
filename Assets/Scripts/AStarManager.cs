using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AStarManager : MonoBehaviour
{
    public static AStarManager instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Node> GeneratePath(Node start, Node end)
    {
        List<Node> openSet = new List<Node>();

        foreach (Node node in FindObjectsOfType<Node>())
        {
            node.gScore = float.MaxValue;
        }

        start.gScore = 0;
        start.hScore = Vector2.Distance(start.transform.position, end.transform.position);
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            int lowestF = default;

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FScore() < openSet[lowestF].FScore())
                {
                    lowestF = i;
                }
            }

            Node curr = openSet[lowestF];
            openSet.Remove(curr);

            if (curr == end) {
                List<Node> path = new List<Node>();

                path.Insert(0, end);

                while (curr != start)
                {             
                    curr = curr.cameFrom;
                    path.Add(curr);
                }

                path.Reverse();
                return path;
            }

            foreach (Node node in curr.connections)
            {
                float heldGScore = curr.gScore + Vector2.Distance(curr.transform.position, node.transform.position);

                if (heldGScore < node.gScore)
                {
                    node.cameFrom = curr;
                    node.gScore = heldGScore;
                    node.hScore = Vector2.Distance(node.transform.position, end.transform.position);

                    if (!openSet.Contains(node))
                    {
                        openSet.Add(node);
                    }
                }
            }
        }

        return null;
    }

    public Node FindNearestNode(Vector2 position)
    {
        Node foundNode = null;
        float minDistance = float.MaxValue;

        foreach(Node node in NodesInScene())
        {
            float currDist = Vector2.Distance(position, node.transform.position);
            if (currDist < minDistance)
            {
                minDistance = currDist;
                foundNode = node;
            }
        }
        return foundNode;
    }

    public Node FindFurthestNode(Vector2 position) 
    {
        Node foundNode = null;
        float maxDistance = 0;

        foreach(Node node in NodesInScene())
        {
            float currDist = Vector2.Distance(position, node.transform.position);
            if (currDist > maxDistance)
            {
                maxDistance = currDist;
                foundNode = node;
            }
        }
        return foundNode; 
    }

    public Node[] NodesInScene()
    {
        return FindObjectsByType<Node>(sortMode:FindObjectsSortMode.InstanceID);
    }
}


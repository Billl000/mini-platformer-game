using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{
    [SerializeField] public Node nodePrefab;
    [SerializeField] public List<Node> nodeList;

    [SerializeField] public Transform startPoint;
    [SerializeField] public Transform endPoint;

    [SerializeField] public float distanceBetweenNodes = 4;

    private void Awake()
    {
        
    }
    public void DrawNodes()
    {
        for (float x = startPoint.transform.position.x; x < endPoint.transform.position.x; x+= distanceBetweenNodes)
        {
            for (float y = startPoint.transform.position.x; y < endPoint.transform.position.x; y+= distanceBetweenNodes)
            {
                Node node = Instantiate(nodePrefab, new Vector2(x, y), Quaternion.identity);
                nodeList.Add(node);
            }
        }
    }

    public void ConnectNode()
    {
        foreach (Node node in nodeList)
        {
            //check nearby nodes for connection
            //float dist = Physics2D.OverlapCircle(node.transform.position, distanceBetweenNodes, LayerMask.GetMask("Node"));

        }
    }
    public void RemoveEmptyNode()
    {

    }
    
}

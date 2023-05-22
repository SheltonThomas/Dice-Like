using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    private Node[] connectedNodes = new Node[0];

    public Node[] GetNodes() 
    {
        return connectedNodes; 
    }

    public void AddNode(Node newNode)
    {
        Node[] tempConnectedNodes = connectedNodes;
        connectedNodes = new Node[connectedNodes.Length + 1];
        
        for(int i = 0; i < connectedNodes.Length; i++)
        {
            connectedNodes[i] = tempConnectedNodes[i];
        }
        connectedNodes[connectedNodes.Length - 1] = newNode;
    }
}

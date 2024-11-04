using System.Collections.Generic;
using UnityEngine;

public class Pathnode : MonoBehaviour
{
    // List for all the pathnodes that are connected to this one.
    public List<GameObject> connections = new List<GameObject>();

    public bool nodeActive;

    // Add a connection if it is not already in the list.
    public void AddConnection(GameObject node)
    {
        if (!connections.Contains(node))
        {
            connections.Add(node);
        }        
    }

    // Clear the list of connections.
    public void ClearConnections()
    {
        connections.Clear();
    }

    // This will allow your Pathnodes to be seen in the Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);

        foreach (GameObject node in connections)
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}

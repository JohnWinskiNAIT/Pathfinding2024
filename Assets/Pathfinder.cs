using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] GameObject startNode, targetNode, currentNode, destinationNode;
    [SerializeField] List<GameObject> previousNode;
    [SerializeField] int previousNumber = 5;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startNode.transform.position;
        currentNode = startNode;
        targetNode = currentNode;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targetNode.transform.position) < 0.2f)
        {
            previousNode.Add(currentNode);
            if (previousNode.Count > previousNumber)
            {
                previousNode.RemoveAt(0);
            }

            currentNode = targetNode;
            if (currentNode.GetComponent<Pathnode>().connections.Count > 0)
            {
                bool foundNode = false;
                int timesThrough = 0;
                float closeDistance = 10000;
                GameObject closestNode = null;
                
                while(!foundNode && timesThrough < 100)
                {
                    //int index = Random.Range(0, currentNode.GetComponent<Pathnode>().connections.Count);
                    for (int i = 0; i < currentNode.GetComponent<Pathnode>().connections.Count; i++)
                    {
                        if (!previousNode.Contains(currentNode.GetComponent<Pathnode>().connections[i]) && currentNode.GetComponent<Pathnode>().connections[i].GetComponent<Pathnode>().nodeActive)
                        {
                            if (Vector3.Distance(currentNode.GetComponent<Pathnode>().connections[i].transform.position, destinationNode.transform.position) < closeDistance)
                            {                            
                                closeDistance = Vector3.Distance(currentNode.GetComponent<Pathnode>().connections[i].transform.position, destinationNode.transform.position);
                                closestNode = currentNode.GetComponent<Pathnode>().connections[i];
                            }
                        }
                    }

                    if (closestNode != null)
                    {
                        targetNode = closestNode;
                        foundNode = true;
                    }
                    else
                    {
                        previousNode.Clear();
                    }
                   
               
                    timesThrough++;
                }                
            }
        }
        else
        {
            transform.Translate((targetNode.transform.position - transform.position).normalized * Time.deltaTime * 3.0f);
        }
    }
}

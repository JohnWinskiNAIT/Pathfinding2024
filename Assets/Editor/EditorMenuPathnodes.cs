using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorMenuPathnodes : MonoBehaviour
{
    static GameObject node;
    static GameObject[] spawner;
    static GameObject[] allSpheres;

    static Transform selectedObject;

    static RaycastHit hit;
    static RaycastHit hit2;

    static float nodeDistance = 3.0f;
    static float pathDistance = 3.0f;

    [MenuItem("Grid Generation/Spawn Nodes", priority = 0)]

    static void SpawnNodes()
    {        
        Vector3 floorExtents;

        node = Resources.Load("Pathnode") as GameObject;
        
        selectedObject = Selection.transforms[0];
        floorExtents = selectedObject.GetComponent<BoxCollider>().bounds.extents - new Vector3(2, 0, 2);

        int numberOfNodes = 0;

        for (float i = floorExtents.x * -2; i <= 0; i += nodeDistance)
        {
            for (float j = floorExtents.z * -2; j <= 0; j += nodeDistance)
            {
                if (Physics.SphereCast(selectedObject.position + floorExtents + selectedObject.right * i + selectedObject.forward * j, 0.5f, Vector3.down, out hit))
                {
                    if (Physics.Raycast(selectedObject.position + floorExtents + selectedObject.right * i + selectedObject.forward * j, Vector3.down, out hit2))
                    {
                        if (hit.transform == hit2.transform)
                        {
                            GameObject spawnedNode = Instantiate(node, hit.point, Quaternion.identity);
                            spawnedNode.transform.parent = selectedObject;
                            spawnedNode.name = "Pathnode" + (++numberOfNodes).ToString();
                            spawnedNode.GetComponent<Pathnode>().nodeActive = true;
                        }
                    }
                }
            }
        }
    }

    [MenuItem("Grid Generation/Spawn Nodes", true)]
    static bool SpawnNodesValidate()
    {
        bool valid = true;
        
        if (Selection.transforms.Length != 1 || Selection.transforms[0].tag != "SpawnTrigger")
        {
            valid = false;
        }

        return valid;
    }

    [MenuItem("Grid Generation/Clear Trigger Nodes", priority = 0)]

    static void ClearTriggerNodes()
    {
        selectedObject = Selection.transforms[0];

        for (int i = 0; i < selectedObject.childCount;)
        {
            DestroyImmediate(selectedObject.GetChild(0).gameObject);
        }
    }

    [MenuItem("Grid Generation/Clear All Nodes", priority = 0)]

    static void ClearAllNodes()
    {
        allSpheres = GameObject.FindGameObjectsWithTag("Pathnode");

        foreach (GameObject target in allSpheres)
        {
            DestroyImmediate(target);
        }
    }

    [MenuItem("Grid Generation/Build Paths", priority = 50)]
    static void BuildPaths()
    {
        allSpheres = GameObject.FindGameObjectsWithTag("Pathnode");

        foreach (GameObject node in allSpheres)
        {
            node.GetComponent<Pathnode>().ClearConnections();
        }

        for (int i = 0; i < allSpheres.Length; i++)
        {
            for (int j = 0; j < allSpheres.Length; j++)
            {
                if (Vector3.Distance(allSpheres[i].transform.position, allSpheres[j].transform.position) <= pathDistance &&
                    i != j &&
                    !Physics.SphereCast(allSpheres[i].transform.position + new Vector3(0, 0.5f, 0), 0.4f, allSpheres[j].transform.position - allSpheres[i].transform.position, out hit, pathDistance))
                {
                    allSpheres[i].GetComponent<Pathnode>().AddConnection(allSpheres[j]);
                }
            }
        }
    }

    [MenuItem("Grid Generation/Clear Paths", priority = 51)]
    static void ClearPaths()
    {
        allSpheres = GameObject.FindGameObjectsWithTag("Pathnode");

        foreach (GameObject node in allSpheres)
        {
            node.GetComponent<Pathnode>().ClearConnections();
        }
    }
}

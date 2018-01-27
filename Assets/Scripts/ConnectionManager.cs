using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour {

    class PConnections
    {
        public PlayerController pc;
        public List<GameObject> connectedObjects;
        public bool connectedToWall = false;

        public PConnections(PlayerController pc, List<GameObject> connectedObjects)
        {
            this.pc = pc;
            this.connectedObjects = connectedObjects;
        }
        
    }
    PConnections[] pControllers = new PConnections[3];
	// Use this for initialization
	void Start () {
        PlayerController[] pController = FindObjectsOfType<PlayerController>();
        for(int i = 0; i < pController.Length; i++)
        {
            pControllers[i] = new PConnections(pController[i], new List<GameObject>());
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Disconnect(PlayerController pController, GameObject connectedBody)
    {
        PConnections updatedConnected = pControllers[FindPConIndex(pController)];
        updatedConnected.connectedObjects.Remove(connectedBody);
        if (connectedBody)
        {
            if (connectedBody.CompareTag("Player"))
            {
                pControllers[FindPConIndex(connectedBody.transform.parent.GetComponent<PlayerController>())].connectedObjects.Remove(pController.leftHinge.gameObject);
            }
            if (connectedBody)
            {
                CheckWalls();
            }
        }


    }

    public void Connect(PlayerController pController, GameObject connectedBody)
    {
        PConnections updatedConnection = null;
        updatedConnection = pControllers[FindPConIndex(pController)];
        updatedConnection.connectedObjects.Add(connectedBody);
        if (connectedBody.CompareTag("Player"))
        {
            pControllers[FindPConIndex(connectedBody.transform.parent.GetComponent<PlayerController>())].connectedObjects.Add(pController.leftHinge.gameObject);
        }
        if (updatedConnection != null)
        {
            CheckWalls();
        }
    }

    bool CheckConnectedToWall(PConnections pCon)
    {
        bool wall = false;
        foreach(GameObject go in pCon.connectedObjects)
        {
            if (!go.CompareTag("Player"))
            {
                pCon.connectedToWall = true;
                wall = true;
                return wall;
            }
        }
        pCon.connectedToWall = false;
        return wall;
    }

    void CheckIfOthersConnectedToWall(PConnections pCon)
    {
        //Debug.Log(pCon.connectedToWall);
        if (pCon.connectedToWall == false)
        {
            foreach(GameObject go in pCon.connectedObjects)
            {
                //Debug.Log("not player: " + go.name);
                if (go.CompareTag("Player"))
                {
                    //Debug.Log("Is player: " + go.transform.parent.name);
                    if (pControllers[FindPConIndex(go.transform.parent.GetComponent<PlayerController>())].connectedToWall)
                    {
                        //Debug.Log(go.transform.parent.name);
                        pCon.connectedToWall = true;
                    }
                }
            }
        }
        //Debug.Log(pCon.connectedToWall);
    }



    public void CheckWalls()
    {
        for (int i = 0; i < pControllers.Length; i++)
        {
            CheckConnectedToWall(pControllers[i]);
        }
        for (int i = 0; i < pControllers.Length; i++)
        {
            CheckIfOthersConnectedToWall(pControllers[i]);
        }

        for (int i = 0; i < pControllers.Length; i++)
        {
            if (pControllers[i].connectedToWall)
            {
                pControllers[i].pc.rb2d.mass = .5f;
            }
            else
            {
                pControllers[i].pc.rb2d.mass = 200f;
            }
        }
    }

    public int FindPConIndex(PlayerController playerControl)
    {
        for(int i = 0; i < pControllers.Length; i++)
        {
            if (pControllers[i].pc == playerControl)
            {
                return i;
            }
        }
        return 0;
    }
}

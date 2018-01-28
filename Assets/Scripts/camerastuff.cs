using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerastuff : MonoBehaviour
{
    GameObject[] players;
    float camSpeed = 5;
    Vector3 finalLookAt;
    float camDist = 20;
    public Vector3 angles = Vector3.zero;
    float minX = Mathf.Infinity;
    float maxX = -Mathf.Infinity;
    float minY = Mathf.Infinity;
    float maxY = -Mathf.Infinity;
    Vector2 cameraBuffer = new Vector2(10, 10);
    float camSize;
    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Players");
    }

    // Update is called once per frame
    void Update()
    {
        CalculateBounds();
        CalculateCameraPosAndSize();
    }

    void CalculateBounds()
    {
        minX = Mathf.Infinity;
        maxX = -Mathf.Infinity;
        minY = Mathf.Infinity;
        maxY = -Mathf.Infinity;
        foreach (GameObject player in players)
        {
            Vector3 tempPlayer = player.transform.position;
            //X Bounds
            if (tempPlayer.x < minX)
                minX = tempPlayer.x;
            if (tempPlayer.x > maxX)
                maxX = tempPlayer.x;
            //Y Bounds
            if (tempPlayer.y < minY)
                minY = tempPlayer.y;
            if (tempPlayer.y > maxY)
                maxY = tempPlayer.y;
        }
    }

    void CalculateCameraPosAndSize()
    {
        Vector3 cameraCenter = Vector3.zero;
        Vector3 pos;
        foreach (GameObject player in players)
        {
            cameraCenter += player.transform.position;
        }
        Vector3 finalCameraCenter = cameraCenter / players.Length;
        //Rotates and Positions camera around a point
        Quaternion rot;
        rot = Quaternion.Euler(angles);
        pos = rot * new Vector3(0f, 0f, -camDist) + finalCameraCenter;
        //transform.rotation = rot;
        transform.position = Vector3.Lerp(transform.position, pos, camSpeed * Time.deltaTime);
        //finalLookAt = Vector3.Lerp(finalLookAt, finalCameraCenter, camSpeed * Time.deltaTime);
        //transform.LookAt(finalLookAt);
        //Size
        float sizeX = maxX - minX + cameraBuffer.x;
        float sizeY = maxY - minY + cameraBuffer.y;
        camSize = (sizeX > sizeY ? sizeX : sizeY);
        Vector3 cameraDist = Camera.main.transform.position;
        cameraDist.z = -camSize;
        Camera.main.transform.position = cameraDist;
    }
}

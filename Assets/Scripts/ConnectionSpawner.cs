using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionSpawner : MonoBehaviour {
    public static ConnectionSpawner instance;
    public GameObject connectionPrefab;
    [Range(5, 20)]
    public int rows;
    [Range(5, 20)]
    public int columns;
    public Vector3 topRight;
    public Vector3 bottomLeft;
    // Use this for initialization
    void Start() {
        instance = this;
        float horizontalSpacing = Mathf.Abs((bottomLeft.x - topRight.x) / (columns - 1));
        float verticalSpacing = Mathf.Abs((bottomLeft.y - topRight.y) / (rows - 1));
        for (int i = 0; i < rows; ++i) {
            for (int j = 0; j < columns; ++j) {
                GameObject go = Instantiate(connectionPrefab, bottomLeft + Vector3.right * j * horizontalSpacing + Vector3.up * i * verticalSpacing, Quaternion.identity);
                go.name = "Connection " + i + " " + j;
                go.transform.SetParent(transform);
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerSource : MonoBehaviour {

    bool powered = true;

    void LateUpdate() {

        // check if can read a PowerUser
        if (powered) {
            // look down line for poweruser

        }


    }
}

public class PowerUser : MonoBehaviour {


    public void Power() {

    }

    public void Update() {
        // set thing to false
    }

}

public enum CType {
    WALL,
    PLAYER,
    USER,
    SOURCE,
    NULL
}


public class Wire : MonoBehaviour {

    public CType leftType = CType.NULL;
    public CType rightType = CType.NULL;

    public Wire leftWire = null;
    public Wire rightWire = null;

    public bool connectedToWall = false;
    public bool powered = false;

    // Use this for initialization
    void Start() {
        wires.Add(this);
    }

    // Update is called once per frame
    void Update() {

    }

    static List<Wire> wires = new List<Wire>();

    public void Disconnect(bool rightArm) {
        if (rightArm) {
            rightType = CType.NULL;
            rightWire = null;
        } else {
            leftType = CType.NULL;
            leftWire = null;
        }

        UpdateAllWires();
    }

    public void Connect(bool rightArm, Collider2D col) {
        CType type = CType.WALL; // default to wall
        if (col.CompareTag("Player")) {
            type = CType.PLAYER;
            Wire w = col.transform.parent.GetComponent<Wire>();
            if (w) {
                if (rightArm) {
                    rightWire = w;
                } else {
                    leftWire = w;
                }
            }
        } else if (col.CompareTag("User")) {
            type = CType.USER;
        } else if (col.CompareTag("Source")) {
            type = CType.SOURCE;
        } else {
            // bad?
        }

        if (rightArm) {
            rightType = type;
        } else {
            leftType = type;
        }

        UpdateAllWires();
    }

    void UpdateConnection() {
        connectedToWall = leftType == CType.WALL || rightType == CType.WALL;
        powered = leftType == CType.SOURCE || rightType == CType.SOURCE;
    }

    public static void UpdateAllWires() {
        for (int i = 0; i < wires.Count; ++i) {
            wires[i].UpdateConnection();
        }

        // propogate twice
        for (int i = 0; i < wires.Count; ++i) {
            wires[i].Propogate();
        }
        for (int i = 0; i < wires.Count; ++i) {
            wires[i].Propogate();
        }

    }

    // send out your states
    public void Propogate() {
        if (leftWire) {
            leftWire.connectedToWall |= connectedToWall;
            connectedToWall |= leftWire.connectedToWall;
            leftWire.powered |= powered;
            powered |= leftWire.powered;
        }
        if (rightWire) {
            rightWire.connectedToWall |= connectedToWall;
            connectedToWall |= rightWire.connectedToWall;
            rightWire.powered |= powered;
            powered |= rightWire.powered;
        }
    }

}

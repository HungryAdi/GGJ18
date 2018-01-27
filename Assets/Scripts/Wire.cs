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

public class PowerUser: MonoBehaviour {


    public void Power() {

    }

    public void Update() {
        // set thing to false
    }

}



public class Wire : MonoBehaviour {

    public Wire leftWire = null;
    public Wire rightWire = null;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    // tells wire that its connected
    void SetConnected(bool right, Collider2D col) {
        //if (col.CompareTag("Source")) {
        //    if (right) {
        //        rightPower = true;
        //    } else {
        //        leftPower = true;
        //    }
        //}
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBuilder : MonoBehaviour {

    public GameObject jointPrefab;
    public float angle = 60.0f;

    Rigidbody2D rigid;
    PlayerController pc;


    // Use this for initialization
    void Start() {
        rigid = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerController>();

        int c = 10;
        pc.leftWire = BuildJoints(c);
        pc.rightWire = BuildJoints(c);


    }

    Rigidbody2D BuildJoints(int count) {
        Rigidbody2D[] rbs = new Rigidbody2D[count];
        for (int i = 0; i < count; ++i) {
            GameObject go = Instantiate(jointPrefab, transform.position + new Vector3(i * 0.5f, 0, 0), Quaternion.identity, transform);
            go.name = "Joint" + i;
            HingeJoint2D joint = go.GetComponent<HingeJoint2D>();
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            rbs[i] = rb;
            if (i == 0) {
                joint.connectedBody = rigid;
            } else {
                joint.limits = new JointAngleLimits2D { min = -angle, max = angle };
                joint.connectedBody = rbs[i - 1];
            }

            if (i == count - 1) {
                return rb;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update() {

    }
}

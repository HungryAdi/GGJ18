using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wireconnectionscript : MonoBehaviour {

    // Use this for initialization
    public GameObject jointPrefab;
    float angle = 60;
    public Rigidbody2D one;
    public Rigidbody2D two;
	void Start () {
        BuildJointsBetweenTwoObjects(one, two);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void BuildJointsBetweenTwoObjects(Rigidbody2D first, Rigidbody2D second)
    {
        float dist = Vector2.Distance(first.transform.position, second.transform.position);
        Rigidbody2D[] rbs = new Rigidbody2D[(int)dist];
        for (int i = 0; i < (int)dist; i++)
        {
            Vector3 d = (second.transform.position - first.transform.position);
            GameObject go = Instantiate(jointPrefab, transform.position + ((d/d.magnitude) * (i)), Quaternion.identity, transform);
            go.name = "Joint" + i;
            HingeJoint2D joint = go.GetComponent<HingeJoint2D>();
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            rbs[i] = rb;
            if (i == 0)
            {
                joint.connectedBody = first;
            }
            else
            {
                joint.limits = new JointAngleLimits2D { min = -angle, max = angle };
                joint.connectedBody = rbs[i - 1];
            }
        }
    }
}

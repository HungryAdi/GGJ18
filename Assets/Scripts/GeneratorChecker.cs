using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorChecker : MonoBehaviour {

    public LightUp lightup;

    public List<GeneratorTip> tips;

    // Use this for initialization
    void Start() {
        StartCoroutine(CheckMyTips());
    }

    WaitForSeconds wait = new WaitForSeconds(0.2f);
    IEnumerator CheckMyTips() {

        while (true) {
            bool allPowered = true;

            for (int i = 0; i < tips.Count; ++i) {
                if (!tips[i].powered) {
                    allPowered = false;
                    break;
                }
            }
            if (allPowered) {
                lightup.litFam = true;
                for(int i = 0; i < tips.Count; ++i) { 
                    tips[i].freeze = true;
                }
                break;
            }

            yield return wait;
        }

    }

}

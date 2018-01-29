using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPlayer : MonoBehaviour {

    public AudioSource music;
    public AudioSource generator;

    public SongPlayer instance = null;

    // Use this for initialization
    void Start() {
        if (instance != null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            DestroyImmediate(gameObject);
        }
    }

    public void PlayGeneratorSound() {
        generator.Play();
        StartCoroutine(ReduceMusicVolume());
    }


    IEnumerator ReduceMusicVolume() {
        float time = 8.0f;
        generator.volume = 1.0f;
        float startVolume = 0.1f;
        float endVolume = 1.0f;
        float t = 0.0f;
        while (t < time) {
            music.volume = Mathf.Lerp(startVolume, endVolume, t / time);
            generator.volume = Mathf.Lerp(8.0f, 0.0f, t / time);
            t += Time.deltaTime;
            yield return null;
        }

    }
}

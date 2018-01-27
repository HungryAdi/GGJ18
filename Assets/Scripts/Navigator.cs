using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour {
    public static Navigator instance;
    public GameObject mainMenuCanvas;
    public GameObject inGameMenuCanvas;
	// Use this for initialization
	void Start () {
        if (!instance) {
            DontDestroyOnLoad(gameObject);
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu")) {
            ToggleInGameMenu(!inGameMenuCanvas.activeSelf);
        }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        inGameMenuCanvas.SetActive(false);
        if(scene.name == "Main") {
            mainMenuCanvas.SetActive(false);
        }
        if(scene.name == "MainMenu") {
            mainMenuCanvas.SetActive(true);
        }
    }

    public void ToggleInGameMenu(bool activate) {
        inGameMenuCanvas.SetActive(activate);
        Time.timeScale = activate ? 0.001f : 1f;
    }

    public void LoadLevel(string level) {
        if(level == "Quit") {
            Application.Quit();
        } else {
            SceneManager.LoadScene(level);
        }
    }
}

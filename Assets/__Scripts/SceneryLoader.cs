using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneryLoader : MonoBehaviour
{

    // TODO: make this pick levels more smarter
    public string levelName;

    // Start is called before the first frame update
    void Start()
    {

        // is a Level already loaded? if so, use it
        for (int n = 0; n < SceneManager.sceneCount; ++n)
        {
            Scene scene = SceneManager.GetSceneAt(n);
            if (scene.name.StartsWith("Level")) 
            {
                levelName = scene.name;
                Debug.LogFormat("Scene {0} {1} already loaded", scene.name, scene.buildIndex) ;
                if (scene.buildIndex < 0) Debug.LogError(scene.name+" is not in Build Settings. It will not ship.");
                return;
            }
        }

        // SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        LoadLevel(levelName);
    }

    public void LoadLevel(string sceneName)
    {
        if (levelName != null)
        {            
            Scene scene = SceneManager.GetSceneByName(levelName);
            if (scene != null && scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(levelName);
            }
        }
        levelName = sceneName;
        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadLevel("Level_1");
        }
    }


}

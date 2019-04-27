using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneryLoader : MonoBehaviour
{

    // TODO: make this pick levels more smarter
    public string levelName;

   Scene loadedLevel;

    // Start is called before the first frame update
    void Start()
    {

        // is a Level already loaded?
        for (int n = 0; n < SceneManager.sceneCount; ++n)
        {
            Scene scene = SceneManager.GetSceneAt(n);
            if (scene.name.StartsWith("Level")) 
            {
                loadedLevel = scene;
                Debug.LogFormat("Scene {0} {1} already loaded", loadedLevel.name, loadedLevel.buildIndex) ;
                if (scene.buildIndex < 0) Debug.LogError(loadedLevel.name+" is not in Build Settings. It will not ship.");
                return;
            }
        }

        loadedLevel = SceneManager.GetSceneByName(levelName);
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
    }

    public void LoadLevel(string sceneName)
    {
        if (loadedLevel != null)
        {
            // SceneManager.UnnloadSceneAsync(loadedLevel.name);
        }
    }


}

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
        Scene scene = SceneManager.GetSceneByName(levelName);
        if (scene.isLoaded)
        {      
            Debug.LogFormat("Scene {0} already loaded", levelName);
            return;
        }

        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
    }

}

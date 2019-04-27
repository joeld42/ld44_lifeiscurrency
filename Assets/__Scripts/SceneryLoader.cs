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
        Scene currentLevel = SceneManager.GetSceneByName(levelName);
        if (currentLevel == null)
        {           
            SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        } else
        {
            Debug.LogFormat("Scene {0} already loaded", levelName);
        }
    }

}

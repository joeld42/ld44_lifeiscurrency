using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneryLoader : MonoBehaviour
{

    // TODO: make this pick levels more smarter
    public string levelName;

    public List<AudioClip> backgroundTracks;

    // Start is called before the first frame update
    void Start()
    {

        PlayRandomBackgroundMusic();

        // is a Level already loaded? if so, use it
        for (int n = 0; n < SceneManager.sceneCount; ++n)
        {
            Scene scene = SceneManager.GetSceneAt(n);
            if (scene != null && scene.isLoaded && scene.name.StartsWith("Level")) 
            {
                Debug.LogFormat("Scene [{0}] {1} buildIndex:{2} already loaded", n, scene.name, scene.buildIndex, this) ;
                if (scene.buildIndex < 0) Debug.LogError(scene.name+" is not in Build Settings. It will not ship.");
                levelName = scene.name;
                return;
            }
        }

        // SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        LoadLevel(levelName);

       
    }

    public void PlayRandomBackgroundMusic()
    {
        // Choose a background track at random
        if ((backgroundTracks != null) && (backgroundTracks.Count > 0))
        {
            int trackIndex = UnityEngine.Random.Range(0, backgroundTracks.Count);
            AudioSource sauce = GetComponent<AudioSource>();
            if (sauce != null)
            {
                sauce.clip = backgroundTracks[trackIndex];
                sauce.Play();
                Debug.LogFormat("Playing BG music {0}, {1}", trackIndex, sauce.clip.name);
            }

        }
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
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadLevel("Level_1");
        }
    }


}

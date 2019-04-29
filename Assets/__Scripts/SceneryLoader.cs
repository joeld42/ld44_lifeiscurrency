using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneryLoader : MonoBehaviour
{
    public string startingLevel;
    public List<AudioClip> backgroundTracks;

    // Start is called before the first frame update
    void Start()
    {

        PlayRandomBackgroundMusic();


        string levelToLoad = startingLevel;
        if (GameGlobals.instance.nextLevelToLoad.Length > 0)
        {
            levelToLoad = GameGlobals.instance.nextLevelToLoad;
        }
        LoadLevel(levelToLoad);

       
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
    
        // Unload any loaded levels
        for (int n = 0; n < SceneManager.sceneCount; ++n)
        {
            Scene scene = SceneManager.GetSceneAt(n);
            if (scene != null && scene.isLoaded && scene.name.StartsWith("Level"))
            {
                Debug.LogFormat("Unloading level {0}", scene.name);
                SceneManager.UnloadSceneAsync( scene );
            }
        }
        Debug.Log("Loading scenery level {0}");
        SceneManager.LoadScene( sceneName, LoadSceneMode.Additive);
    }

    void Update()
    {
    }


}

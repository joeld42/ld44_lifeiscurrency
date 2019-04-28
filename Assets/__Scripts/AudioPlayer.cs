using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use this class when you need to play a sound for an object that is being destroyed.
[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    private static AudioSource m_audioSource;

    void Awake() {
        m_audioSource = GetComponent<AudioSource>();
    }

    public static void PlayClip(AudioClip clip, float startTime = 0) {
        // TODO: to set the time offset, the clip must live on the audio source
        // however, setting the clip will kill the previous clip. So perhaps the 
        // we should create a new audio source on every request? Or just trim the
        // audio clips so we don't need to set the offset. Passing in an audio
        // source would also allow the caller to configure the source as desired.
        if (startTime != 0) {
            m_audioSource.clip = clip;
            m_audioSource.time = startTime;
            m_audioSource.Play();
        } else {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }


}

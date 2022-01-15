using _Utilities;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : Singleton<AudioSystem> {

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource effectSource;

    /*
    // Will not be implemented
    [SerializeField]
    private AudioMixer audioMixer;
    */

    [SerializeField]
    private AudioClip[] audioClips;

    void Start() {
        /*musicSource.clip = audioClips.Last();
        musicSource.Play();
        musicSource.loop = true;*/
    }

    public void PlayPop() {
        if (effectSource.isPlaying)
            return;
        effectSource.clip = audioClips[0];
        effectSource.Play();
    }

    public void MuteMusic() {
        // Will not be implemented
    }
    public void MuteEffects() {
        // Will not be implemented
    }
}

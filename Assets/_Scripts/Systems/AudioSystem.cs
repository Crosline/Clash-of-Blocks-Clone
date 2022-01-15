using _Utilities;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : Singleton<AudioSystem> {

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource effectSource;

    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private AudioClip[] audioClips;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}

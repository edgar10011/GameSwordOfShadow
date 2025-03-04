using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("------------------ Audio source ------------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;
    [Header("------------------ Audio Clip ------------------")]
    public AudioClip song_1;
    public AudioClip player_walk;
    public AudioClip player_run;
    public AudioClip hurt1;
    public AudioClip hurt2;
    public AudioClip hurt3;
    private AudioClip[] hurtSounds;
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        hurtSounds = new AudioClip[] { hurt1, hurt2, hurt3 };
    }

    private void Start()
    {
        musicSource.clip = song_1;
        musicSource.Play();
    }

    public void PlaySFXLoop(AudioClip clip)
    {
        if (SFXSource.clip != clip)
        {
            SFXSource.clip = clip;
            SFXSource.loop = true;
            SFXSource.Play();
        }
    }

    public void StopSFX()
    {
        SFXSource.Stop();
        SFXSource.clip = null;
    }


    public bool IsPlaying(AudioClip clip)
    {
        return SFXSource.isPlaying && SFXSource.clip == clip;
    }

    public void PlayRandomHurtSound()
    {
        if (hurtSounds.Length > 0)
        {
            AudioClip randomHurtSound = hurtSounds[Random.Range(0, hurtSounds.Length)];

            GameObject tempAudio = new GameObject("TempAudio");
            AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
            tempSource.clip = randomHurtSound;
            tempSource.Play();

            Destroy(tempAudio, randomHurtSound.length);
        }
    }
}


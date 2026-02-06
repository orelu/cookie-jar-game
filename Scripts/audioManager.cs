using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    public AudioClip musicClip;
    public AudioClip altClip1;
    public AudioClip altClip2;
    public AudioClip altClip3;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        AudioManager.Instance.SetMusicVolume(0.125f); 
        AudioManager.Instance.SetSfxVolume(1.25f);    

    }

    void Start()
    {
        StartCoroutine(MusicLoop());
    }

    IEnumerator MusicLoop()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            if (musicClip != null)
            {
                musicSource.clip = musicClip;
                musicSource.loop = false;
                musicSource.Play();
                yield return new WaitForSeconds(musicClip.length);
            }

            yield return new WaitForSeconds(30f);
        }
    }

    public void PlayAlt1()
    {
        if (altClip1 != null)
            sfxSource.PlayOneShot(altClip1);
    }

    public void PlayAlt2()
    {
        if (altClip2 != null)
            sfxSource.PlayOneShot(altClip2);
    }

    public void PlayAlt3()
    {
        if (altClip2 != null)
            sfxSource.PlayOneShot(altClip3);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }

    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp01(volume);
    }
}

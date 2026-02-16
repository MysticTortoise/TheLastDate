using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    private static MusicHandler instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        transform.parent = null;
        DontDestroyOnLoad(this.gameObject);
    }

    public static void SetMusic(AudioClip clip)
    {
        FindAnyObjectByType<MusicHandler>().GetComponent<AudioSource>().clip = clip;
        FindAnyObjectByType<MusicHandler>().GetComponent<AudioSource>().Play();
    }

}
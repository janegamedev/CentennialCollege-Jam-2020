using Scriptables;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GridManagerVariable currentGrid;
    public static AudioManager Instance;
    public AudioSource source, music;
    public AudioClip winMusic;

    private void Awake()
    {
        Instance = this;
    }
    
    public void CallSfx(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public void OnRoomUpdated()
    {
        music.Stop();
        music.clip = currentGrid.value.setup.music;
        music.Play();
    }

    public void OnGameWin()
    {
        music.Stop();
        music.clip = winMusic;
        music.Play();
    }
}

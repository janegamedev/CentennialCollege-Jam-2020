using Scriptables;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GridManagerVariable currentGrid;
    public static AudioManager Instance;
    public AudioSource source, music;

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
        music.clip = currentGrid.value.setup.music;
    }
}

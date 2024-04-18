
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource ambienceSource;
    List<AudioClip> playingClips = new List<AudioClip>();

    public void PlayAmbience(AudioClip clip)
    {
        ambienceSource.clip = clip;
        ambienceSource.Play();
    }

    // Ayný seslerin ayný anda çalmasýný engelleyecek þekilde tasarlandý.
    public void PlaySFX(AudioClip clip)
    {
        foreach (var item in playingClips)
        {
            if (item == clip)
                return;
        }

        playingClips.Add(clip);
        sfxSource.PlayOneShot(clip);
        StartCoroutine(StopPlaying(clip));
        
            
    }

    IEnumerator StopPlaying(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        playingClips.Remove(clip);
    }



}

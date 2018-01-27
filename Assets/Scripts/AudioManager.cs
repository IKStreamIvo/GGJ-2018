using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;

    public AudioClip[] sounds;

    public enum Sound
    {
        TakeDamage1, TakeDamage2, TakeDamage3,
        EnemyExplode, FortExplode, 
        PlayerExplode,
        EnemyLaser, FortLaser,
        PlayerLaser1, PlayerLaser2,
        ItemPickup
    }

    private void Awake()
    {
        instance = this;
    }

    public void PlaySound(Sound sound)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = sounds[(int)sound];
        StartCoroutine(PlayClip(source));
    }

    IEnumerator PlayClip(AudioSource audio)
    {
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        Destroy(audio);
    }
}

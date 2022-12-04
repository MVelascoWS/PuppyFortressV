using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource music;
    public AudioSource sfx;

    private bool muteMusic = false;
    private bool muteSFX = false;


    void Awake()
    {
        instance = this;     
    }
    // Use this for initialization
    void Start ()
    {
        //if (PlayerPrefs.HasKey("Sound"))
        //    muteSFX = PlayerPrefs.GetInt("Sound") == 1 ? false : true;
        //if (PlayerPrefs.HasKey("Music"))
        //    muteMusic = PlayerPrefs.GetInt("Music") == 1 ? false : true;

        //music.volume = muteMusic ? 0 : 0.15f;
        //sfx.volume = muteSFX ? 0 : 1f;        
    }
	
	public void PlaySound(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }

    public void SoundStatus(bool status)
    {
       /* muteSFX = !status;
        PlayerPrefs.SetInt("Sound", status ? 1 : 0);   */     
    }

    public void MusicStatus(bool status)
    {
        /*muteMusic = !status;
        PlayerPrefs.SetInt("Music", status ? 1 : 0);*/
    }

    
}

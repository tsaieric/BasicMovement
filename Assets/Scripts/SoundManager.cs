using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager> {

    public AudioClip[] clips;
    private AudioSource aSource;
    
	// Use this for initialization
	void Start () {
        aSource = this.GetComponent<AudioSource>();
	}
	
    public void PlayGunShot()
    {
        aSource.PlayOneShot(clips[0]);
    }
    public void PlayLaserShot()
    {
        aSource.PlayOneShot(clips[8]);
    }
    public void PlayGrenade()
    {
        aSource.PlayOneShot(clips[7]);
    }

}

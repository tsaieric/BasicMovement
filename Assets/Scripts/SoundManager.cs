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
	public void PlayerHurt()
	{	
		aSource.PlayOneShot(clip[10]);
	}
	public void PlayerDie()
	{	
		aSource.PlayOneShot (clip [9]);
	}
	public void PlayRadar()
	{	
		aSource.PlayOneShot (clip [12]);
	}
	public void DogAttack()
	{
		aSource.PlayOneShot (clip [4]);
	}
	public void DogHurt()
	{
		aSource.PlayOneShot (clip [6]);
	}
	public void ZombieAttack()
	{
		aSource.PlayOneShot (clip [13]);
	}
	public void ZombieDie()
	{
		aSource.PlayOneShot (clip [14]);
	}
	public void BigZombieRoar()
	{
		aSource.PlayOneShot (clip [1]);
	}
	public void NightCome()
	{
		aSource.PlayOneShot (clip [11]);
	}
	public void DayCome()
	{
		aSource.PlayOneShot (clip [3]);
	}
}

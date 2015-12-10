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
		aSource.PlayOneShot(clips[10]);
	}
	public void PlayerDie()
	{	
		aSource.PlayOneShot (clips [9]);
	}
	public void PlayRadar()
	{	
		aSource.PlayOneShot (clips [12]);
	}
	public void DogAttack()
	{
		aSource.PlayOneShot (clips [4]);
	}
	public void DogHurt()
	{
		aSource.PlayOneShot (clips [6]);
	}
	public void ZombieAttack()
	{
		aSource.PlayOneShot (clips [13]);
	}
	public void ZombieDie()
	{
		aSource.PlayOneShot (clips [14]);
	}
	public void BigZombieRoar()
	{
		aSource.PlayOneShot (clips [1]);
	}
	public void NightCome()
	{
		aSource.PlayOneShot (clips [11]);
	}
	public void DayCome()
	{
		aSource.PlayOneShot (clips [3]);
	}
}

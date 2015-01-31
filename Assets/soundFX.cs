using UnityEngine;
using System.Collections;

public class soundFX : MonoBehaviour {
	
	public AudioSource footstepConcreteSound;
	public AudioSource footstepStoneSound;
	public AudioSource footstepStoneSound2;
	public AudioSource footstepStoneSound3;
	public AudioSource knifeMissSound;
	public AudioSource knifeMissSound2;
	public AudioSource knifeHitPlayerSound;
	public AudioSource woopSound;
	public AudioSource deathScreamSound;
	
	public static string SFX_KNIFE_MISS = "M";
	public static string SFX_KNIFE_HIT_PLAYER = "P";
	public static string SFX_WOOP = "W";
	public static string SFX_DIE = "D";
	
	bool leftFoot = false;
	AudioSource footstepStone1;
	AudioSource footstepStone2;
	
	void Awake(){
		int r = Random.Range(0, 3);
		if (r == 0){
			Debug.Log("Stone 1");
			footstepStone1 = footstepStoneSound;
		} else if (r == 1){
			Debug.Log("Stone 1");
			footstepStone1 = footstepStoneSound2;
		} else {
			Debug.Log("Stone 1");
			footstepStone1 = footstepStoneSound3;
		}
		r = Random.Range(0, 3);
		if (r == 0){
			Debug.Log("Stone 2");
			footstepStone2 = footstepStoneSound;
		} else if (r == 1){
			Debug.Log("Stone 2");
			footstepStone2 = footstepStoneSound2;
		} else {
			Debug.Log("Stone 2");
			footstepStone2 = footstepStoneSound3;
		}
	}
	
	[RPC]
	public void PlaySound(string clip){
		if (clip == SFX_KNIFE_MISS){
			if (Random.Range(0, 2) == 0){
				knifeMissSound.Play();
			} else {
				knifeMissSound2.Play();
			}
		} else if (clip == SFX_KNIFE_HIT_PLAYER){
			knifeHitPlayerSound.Play();
		} else if (clip == SFX_WOOP){
			woopSound.Play();
		} else if (clip == SFX_DIE){
			deathScreamSound.Play();
		}
	}
	
	[RPC]
	public void PlayWalk(string clip){
		AudioSource sound;
		if (leftFoot){
			sound = footstepStone1;
		} else {
			sound = footstepStone2;
		}
		if (!sound.isPlaying){
			if (!leftFoot){
				Debug.Log("Playing right foot");
				sound = footstepStone1;
			} else {
				Debug.Log("Playing left foot");
				sound = footstepStone2;
			}
			sound.Play();
			leftFoot = !leftFoot;
		}
	}
}

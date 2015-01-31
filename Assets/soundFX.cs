using UnityEngine;
using System.Collections;

public class soundFX : MonoBehaviour {
	
	public AudioSource knifeMissSound;
	public AudioSource knifeHitPlayerSound;
	public AudioSource footstepConcreteSound;
	
	public static string SFX_KNIFE_MISS = "M";
	public static string SFX_KNIFE_HIT_PLAYER = "P";

	void Awake() {
		if (!networkView.isMine){
			this.enabled = false;
			return;
		}
	}
	
	void Update () {
		
	}
	
	[RPC]
	public void PlayRPCSound(string clip){
		if (clip == SFX_KNIFE_MISS){
			knifeMissSound.Play();
		} else if (clip == SFX_KNIFE_HIT_PLAYER){
			knifeHitPlayerSound.Play();
		}
	}
	
	[RPC]
	public void PlayWalk(string clip){
		AudioSource sound = footstepConcreteSound;
		if (!sound.isPlaying){
			sound.Play();
		}
	}
}

using UnityEngine;
using System.Collections;

public class soundFX : MonoBehaviour {
	
	public AudioSource source;
	public AudioClip knifeMissSound;
	public AudioClip knifeHitPlayerSound;

	void Awake() {
		if (!networkView.isMine){
			this.enabled = false;
			return;
		}
	}
	
	void Update () {
		
	}
	
	[RPC]
	public void PlayRPCSound(int clipInt){
		if (clipInt == 0){
			source.clip = knifeMissSound;
		} else if (clipInt == 1){
			source.clip = knifeHitPlayerSound;
		}
		source.Play();
	}
}

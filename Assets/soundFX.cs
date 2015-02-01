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
	public AudioSource playerBurnt;
	public AudioSource treeRustle;
	
	public AudioSource kateYouHaveDied;
	
	public AudioSource ryanWatchYourBack;
	public AudioSource ryanTaze;
	public AudioSource ryanSwiggity;
	public AudioSource ryanSuckMyDick;
	public AudioSource ryanSpleen;
	public AudioSource ryanInternetToughGuy;
	public AudioSource ryanStabMe;
	public AudioSource ryanYoullNeverSeeMe;
	public AudioSource ryanFruityFruityBumBum;
	
	public static string SFX_KNIFE_MISS = "M";
	public static string SFX_KNIFE_HIT_PLAYER = "P";
	public static string SFX_WOOP = "W";
	public static string SFX_TREE = "T";
	public static string SFX_DIE = "D";
	public static string SFX_DIE_YOU = "Y";
	public static string SFX_BURNT = "B";
	
	bool leftFoot = false;
	AudioSource footstepStone1;
	AudioSource footstepStone2;
	
	void Awake(){
		int r = Random.Range(0, 3);
		if (r == 0){
			footstepStone1 = footstepStoneSound;
		} else if (r == 1){
			footstepStone1 = footstepStoneSound2;
		} else {
			footstepStone1 = footstepStoneSound3;
		}
		r = Random.Range(0, 3);
		if (r == 0){
			footstepStone2 = footstepStoneSound;
		} else if (r == 1){
			footstepStone2 = footstepStoneSound2;
		} else {
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
		} else if (clip == SFX_DIE_YOU){
			deathScreamSound.Play();
			Invoke("PlayYouHaveDied", 3);
		} else if (clip == SFX_BURNT){
			playerBurnt.Play();
		} else if (clip == SFX_TREE){
			treeRustle.Play();
		}
	}
	
	void PlayYouHaveDied(){
		kateYouHaveDied.Play();
	}
	
	int lastTaunt = 0;
	bool tauntAvailable = true;
	
	[RPC]
	public void Taunt(string taunt){
		if (tauntAvailable){
			tauntAvailable = false;
			int r = Random.Range(0, 9);
			while (r == lastTaunt){
				r = Random.Range(0, 9);
			}
			lastTaunt = r;
			if (r == 0){
				ryanWatchYourBack.Play();
			} else if (r == 1){
				ryanTaze.Play();
			} else if (r == 2){
				ryanSwiggity.Play();
			} else if (r == 3){
				ryanSuckMyDick.Play();
			} else if (r == 4){
				ryanSpleen.Play();
			} else if (r == 5){
				ryanInternetToughGuy.Play();
			} else if (r == 6){
				ryanStabMe.Play();
			} else if (r == 7){
				ryanYoullNeverSeeMe.Play();
			} else if (r == 8){
				ryanFruityFruityBumBum.Play();
			}
			Invoke("TauntAvailable", 3);
		}
	}
	
	void TauntAvailable(){
		tauntAvailable = true;
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
				sound = footstepStone1;
			} else {
				sound = footstepStone2;
			}
			sound.Play();
			leftFoot = !leftFoot;
		}
	}
}

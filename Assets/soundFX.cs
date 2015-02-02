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

	public AudioSource heatherInstructionsUpDown;
	public AudioSource heatherInstructionsMiddleClick;
	public AudioSource heatherInstructionsLeftClick;
	public AudioSource heatherIntroduction;
	public AudioSource heatherIntroductionWhatDo;
	public AudioSource heatherLevelFirePit;
	public AudioSource heatherLevelBunker;
	public AudioSource heatherOptionsJoin;
	public AudioSource heatherOptionsHost;
	public AudioSource heatherOptionsChooseLevel;
	public AudioSource heatherOptionsEnterIP;
	private AudioSource[] sounds;


	public static int SFX_MENU_INTRO = 1;
	public static int SFX_MENU_WHAT_DO = 2;
	public static int SFX_MENU_INSTRUCTIONS_NAV1 = 3;
	public static int SFX_MENU_INSTRUCTIONS_NAV2 = 4;
	public static int SFX_MENU_INSTRUCTIONS_NAV3 = 5;
	public static int SFX_MENU_OPTIONS_JOIN = 6;
	public static int SFX_MENU_OPTIONS_HOST = 7;
	public static int SFX_MENU_OPTIONS_ENTER_IP = 8;
	public static int SFX_MENU_OPTIONS_CHOOSE_LEVEL = 9;
	public static int SFX_MENU_LEVEL_FIREPIT = 10;
	public static int SFX_MENU_LEVEL_BUNKER = 11;
	public static int SFX_MENU_SKIP = 12;
		
	public static string SFX_KNIFE_MISS = "M";
	public static string SFX_KNIFE_HIT_PLAYER = "P";
	public static string SFX_WOOP = "W";
	public static string SFX_TREE = "T";
	public static string SFX_DIE = "D";
	public static string SFX_DIE_YOU = "Y";
	public static string SFX_BURNT = "B";
	
	bool leftFoot = true;
	AudioSource footstepStone1;
	AudioSource footstepStone2;
	
	private AudioSource[] tauntList;
	
	void Awake(){
		// Initialise footsteps
		AudioSource[] sourceList = new AudioSource[]{footstepStoneSound, footstepStoneSound2, 
				footstepStoneSound3};
		footstepStone1 = sourceList[Random.Range(0, sourceList.Length)];
		footstepStone2 = sourceList[Random.Range(0, sourceList.Length)];
		// Initialise taunt list
		tauntList = new AudioSource[]{ryanWatchYourBack, ryanTaze, ryanSwiggity, ryanSuckMyDick, 
				ryanSpleen, ryanInternetToughGuy, ryanStabMe, ryanYoullNeverSeeMe, ryanFruityFruityBumBum};
		sounds = new AudioSource[]{heatherIntroduction, heatherIntroductionWhatDo, 
				heatherInstructionsUpDown, heatherInstructionsLeftClick, heatherInstructionsMiddleClick,
				heatherOptionsJoin, heatherOptionsHost, heatherOptionsEnterIP, heatherOptionsChooseLevel,
				heatherLevelFirePit, heatherLevelBunker};
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

	public void PlayMenuSound(int sound, float delay){
		AudioSource soundToPlay = null;
		if(sound < 12){
			soundToPlay = sounds[sound - 1];
		}else if(sound == SFX_MENU_SKIP){
  			foreach(AudioSource a in sounds){
  				a.Stop();
  			}
		}
		if(sound != SFX_MENU_SKIP){
			soundToPlay.PlayDelayed(delay);
		}
	}
	
	int lastTaunt = 0;
	private AudioSource lastTauntSound;
	
	[RPC]
	public void Taunt(string taunt){
		if (lastTauntSound == null || !lastTauntSound.isPlaying){
			int r = Random.Range(0, tauntList.Length);
			while (r == lastTaunt){
				r = Random.Range(0, tauntList.Length);
			}
			lastTauntSound = tauntList[r];
			lastTauntSound.Play();
		}
	}
	
	[RPC]
	public void PlayWalk(){
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
	
	bool walkTimeout = false;
	
	[RPC]
	public void PlayWalkSlow(){
		if (!walkTimeout){
			PlayWalk();
			walkTimeout = true;
			Invoke("PlayWalkReset", 1);
		}
	}
	
	[RPC]
	public void PlayWalkReset(){
		walkTimeout = false;
	}
}

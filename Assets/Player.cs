using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	// Stab
	float health;
	RaycastHit hit;
	float reach = 20.0f;
	float damage = 20.0f;
	// Position
	float x, y;
	
	public AudioSource footstepConcreteSound;
	public AudioSource knifeMissSound;
	public AudioSource knifeHitPlayerSound;
	
	void Awake(){
		if (networkView.isMine){
			GetComponent<AudioListener>().enabled = true;
		}
	}
	
	void Start(){
		health = 100.0f;
	}
	
	void Update(){
		if (Input.GetMouseButtonDown(1)){
			Screen.lockCursor = !Screen.lockCursor;
		}

		if (Screen.lockCursor){
			x += Input.GetAxis("Mouse X") * 5;
			y -= Input.GetAxis("Mouse Y") * 4;

			var rotation = Quaternion.Euler(y, x, 0);
			transform.rotation = rotation;
		}
		
		if (Input.GetMouseButtonDown(0) && networkView.isMine){
			Debug.Log("Calling stab");
			Stab();
		}
	}
	
	void Stab(){
		Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward)*10, Color.red, reach, true);
		string soundRPC;
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, reach) && 
				hit.transform.tag == "PlayerForStab"){
			Debug.Log("Hit someone");
			soundRPC = soundFX.SFX_KNIFE_HIT_PLAYER;
			//networkView.RPC("takeDamage", RPCMode.Others, 0);
			knifeHitPlayerSound.Play();
		} else {
			soundRPC = soundFX.SFX_KNIFE_MISS;
			knifeMissSound.Play();
		}
		// Call damage on remote player
		networkView.RPC("PlayRPCSound", RPCMode.Others, soundRPC);
	}
	
	[RPC]
	void takeDamage(float amount){
		Debug.Log("Taking damage");
		health -= amount;
		if (health <= 0.0){
			Debug.Log("Died");
			Destroy(this);
		}
	}
}

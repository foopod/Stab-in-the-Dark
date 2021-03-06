using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	// Stab
	float health;
	RaycastHit hit;
	float reach = 20.0f;
	float damage = 10.0f;
	// Position
	float x, y;
	Vector3 spawnLocation;
	
	void Awake(){
		if (networkView.isMine){
			GetComponent<AudioListener>().enabled = true;
			Screen.lockCursor = true;
		}
		spawnLocation = gameObject.transform.position;
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
			Stab();
		}
		
		if (Input.GetMouseButtonDown(2) && networkView.isMine){
			networkView.RPC("Taunt", RPCMode.All, "");
		}
	}
	
	void Stab(){
		//takeDamage(Network.player.ipAddress, 100.0f);
		ArrayList potentialHits = new ArrayList();
		for (int i = -30; i <= 30; i += 10){
			RaycastHit hit;
			Vector3 slashVector = Vector3.forward;
			//Increment Slash angle
			slashVector.x += i * Mathf.PI/180;
			//Draw Debug rays
			Debug.DrawRay(transform.position, transform.TransformDirection(slashVector)*10, Color.red, 10, true);
			if (Physics.Raycast(transform.position, transform.TransformDirection(slashVector), out hit, reach)){
				potentialHits.Add(hit);
			}
		}
		
		string soundRPC = soundFX.SFX_KNIFE_MISS;
		foreach (RaycastHit hit in potentialHits){
			if (hit.transform.tag == "PlayerForStab"){
				soundRPC = soundFX.SFX_KNIFE_HIT_PLAYER;
				string id = hit.transform.parent.networkView.owner.ipAddress;
				networkView.RPC("takeDamage", RPCMode.Others, id, 100.0f);
			}
		}
		
		networkView.RPC("PlaySound", RPCMode.All, soundRPC);
	}
	
	[RPC]
	void takeDamage(string id, float amount){
		if (networkView.isMine){
			Debug.Log("Taking damage");
			health -= amount;
			Debug.Log("Remaining health : "+health);
			if (health <= 0.0){
				Debug.Log("Died");
				gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 3.0f, gameObject.transform.position.z);
				networkView.RPC("PlaySound", RPCMode.Others, soundFX.SFX_DIE);
				GetComponent<soundFX>().PlaySound(soundFX.SFX_DIE_YOU);
				health = 100.0f;
				Invoke("respawn", 5);
			}
		}
	}

	void respawn(){
		gameObject.transform.position = spawnLocation;
		networkView.RPC("PlaySound", RPCMode.All, soundFX.SFX_WOOP);
	}
		
	public void OnTriggerEnter(Collider obj){
		if (obj.tag == "Tree"){
			if (networkView.isMine){
				networkView.RPC("PlaySound", RPCMode.All, soundFX.SFX_TREE);
			}
		} else if (obj.tag == "Fire"){
			if (networkView.isMine){
				networkView.RPC("PlaySound", RPCMode.All, soundFX.SFX_BURNT);
				takeDamage(Network.player.ipAddress, 20.0f);
			}
		}
	}
}

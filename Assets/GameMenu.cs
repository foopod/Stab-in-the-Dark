using UnityEngine;
using gui = UnityEngine.GUILayout;

public class GameMenu : MonoBehaviour
{
    public GameObject PlayerPrefab;
    string ip = "127.0.0.1";
	
	void Awake(){
	}
	
    public void CreatePlayer()
    {
        connected = true;
		GetComponent<AudioListener>().enabled = false;
        var g = (GameObject)Network.Instantiate(PlayerPrefab, transform.position, transform.rotation, 1);
		// Enable player camera/listeners
        g.camera.enabled = true;
        camera.enabled = false;
    }
    void OnDisconnectedFromServer()
    {
        connected = false;
    }
    void OnPlayerDisconnected(NetworkPlayer pl)
    {
        Network.DestroyPlayerObjects(pl);
    }
    void OnConnectedToServer()
    {
        CreatePlayer();
    }
    void OnServerInitialized()
    {
        CreatePlayer();
    }
    bool connected;
	static int levelNum = 0;
	const int levelCount = 2;
    void OnGUI()
    {
        if (!connected)
        {
            ip = gui.TextField(ip);
            if (gui.Button("connect"))
            {
                Network.Connect(ip, 5300);
            }
            if (gui.Button("host"))
            {
                Network.InitializeServer(10, 5300, false);
            }
			if (gui.Button("change")){
				levelNum += 1;
				if (levelNum >= levelCount){
					levelNum = 0;
				}
				Application.LoadLevel(levelNum);
			}
        }
    }
}
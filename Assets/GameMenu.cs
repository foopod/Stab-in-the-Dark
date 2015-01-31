using UnityEngine;
using gui = UnityEngine.GUILayout;

public class GameMenu : MonoBehaviour
{
    public GameObject PlayerPrefab;
    string ip = "127.0.0.1";

    public void CreatePlayer()
    {
        connected = true;
        var g = (GameObject)Network.Instantiate(PlayerPrefab, transform.position, transform.rotation, 1);
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
        }
    }
}
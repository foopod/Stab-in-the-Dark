using UnityEngine;
using gui = UnityEngine.GUILayout;

public class GameMenu : MonoBehaviour
{
    public GameObject PlayerPrefab;
    string ip = "127.0.0.1";
    bool connected;

    //Menu option identifiers
    public static int MENU_HOST = 1;
    public static int MENU_JOIN = 2;
    public static int MENU_ENTER_IP = 3;
    public static int MENU_LEVEL_OPTIONS = 4;
    public static int MENU_FIREPIT = 5;
    public static int MENU_BUNKER = 6;

    //Used to iterate the menu
    int currentMenuItem = MENU_JOIN;
    bool menuEnabled = true;
    bool enteringIP = false;
    bool skipped = false;

	void Start(){
        //Play introduction on start
        GetComponent<soundFX>().PlayMenuSound(soundFX.SFX_MENU_INTRO, 0f);
        GetComponent<soundFX>().PlayMenuSound(soundFX.SFX_MENU_INSTRUCTIONS_NAV1, 5.5f);
        GetComponent<soundFX>().PlayMenuSound(soundFX.SFX_MENU_INSTRUCTIONS_NAV2, 8.5f);
        GetComponent<soundFX>().PlayMenuSound(soundFX.SFX_MENU_INSTRUCTIONS_NAV3, 11.5f);
        GetComponent<soundFX>().PlayMenuSound(soundFX.SFX_MENU_WHAT_DO, 14.5f);
        GetComponent<soundFX>().PlayMenuSound(soundFX.SFX_MENU_OPTIONS_JOIN, 16f);
        Invoke("skipIntro", 16);
	}

    void skipIntro(){
        if(skipped != true){
            if(Input.anyKey){
               playLocalMenuSound(soundFX.SFX_MENU_OPTIONS_JOIN); 
            }
            skipped = true;
        }
    }

    //cancels playing and plays new
    void playLocalMenuSound(int id){
        GetComponent<soundFX>().PlayMenuSound(soundFX.SFX_MENU_SKIP, 0f);
        GetComponent<soundFX>().PlayMenuSound(id, 0f);
    }

    void Update(){
        //navigate menu
        if(menuEnabled){
            if (Input.GetKeyDown(KeyCode.W) && skipped){
                //Moving up in the Main menu
                if(currentMenuItem == MENU_JOIN){
                    playLocalMenuSound(soundFX.SFX_MENU_OPTIONS_CHOOSE_LEVEL);
                    currentMenuItem = MENU_LEVEL_OPTIONS;
                } else if(currentMenuItem == MENU_HOST){
                    playLocalMenuSound(soundFX.SFX_MENU_OPTIONS_JOIN);
                    currentMenuItem = MENU_JOIN;
                } else if(currentMenuItem == MENU_LEVEL_OPTIONS){
                    playLocalMenuSound(soundFX.SFX_MENU_OPTIONS_HOST);
                    currentMenuItem = MENU_HOST;
                //Moving up in the level menu
                } else if(currentMenuItem == MENU_FIREPIT){
                    playLocalMenuSound(soundFX.SFX_MENU_LEVEL_BUNKER);
                    currentMenuItem = MENU_BUNKER;
                } else if(currentMenuItem == MENU_BUNKER){
                    playLocalMenuSound(soundFX.SFX_MENU_LEVEL_FIREPIT);
                    currentMenuItem = MENU_FIREPIT;
                }
            }
            if (Input.GetKeyDown(KeyCode.S) && skipped){
                //Moving down in the Main menu
                if(currentMenuItem == MENU_JOIN){
                    playLocalMenuSound(soundFX.SFX_MENU_OPTIONS_HOST);
                    currentMenuItem = MENU_HOST;
                } else  if(currentMenuItem == MENU_HOST){
                    playLocalMenuSound(soundFX.SFX_MENU_OPTIONS_CHOOSE_LEVEL);
                    currentMenuItem = MENU_LEVEL_OPTIONS;
                } else if(currentMenuItem == MENU_LEVEL_OPTIONS){
                    playLocalMenuSound(soundFX.SFX_MENU_OPTIONS_JOIN);
                    currentMenuItem = MENU_JOIN;
                //Moving down in the level menu
                } else if(currentMenuItem == MENU_FIREPIT){
                    playLocalMenuSound(soundFX.SFX_MENU_LEVEL_BUNKER);
                    currentMenuItem = MENU_BUNKER;
                } else if(currentMenuItem == MENU_BUNKER){
                    playLocalMenuSound(soundFX.SFX_MENU_LEVEL_FIREPIT);
                    currentMenuItem = MENU_FIREPIT;
                }
            }
            if (Input.GetMouseButtonDown(0) && skipped){
                //toggle onGUI to enter IP
                if(currentMenuItem == MENU_JOIN){
                    playLocalMenuSound(soundFX.SFX_MENU_OPTIONS_ENTER_IP);
                    currentMenuItem = MENU_ENTER_IP;
                    enteringIP = true;
                //Start hosting a game
                } else  if(currentMenuItem == MENU_HOST){
                    Network.InitializeServer(10, 5300, false);
                    menuEnabled = false;
                //Move to level Select
                } else  if(currentMenuItem == MENU_LEVEL_OPTIONS){
                    playLocalMenuSound(soundFX.SFX_MENU_LEVEL_FIREPIT);
                    currentMenuItem = MENU_FIREPIT;
                //Loading different levels
                } else  if(currentMenuItem == MENU_FIREPIT){
                    Application.LoadLevel(1);
                } else  if(currentMenuItem == MENU_BUNKER){
                    Application.LoadLevel(0);
                } else  if(currentMenuItem == MENU_ENTER_IP){
                    Debug.Log("joining a game at " + ip);
                    Network.Connect(ip, 5300);
                    menuEnabled = false;
                }
            }
            if (Input.GetMouseButtonDown(2) && skipped){
                //Moving back to previous menus
                if(currentMenuItem == MENU_ENTER_IP){
                    playLocalMenuSound(soundFX.SFX_MENU_OPTIONS_JOIN);
                    currentMenuItem = MENU_JOIN;
                    enteringIP = false;
                } else if(currentMenuItem == MENU_FIREPIT || currentMenuItem == MENU_BUNKER ){
                    playLocalMenuSound(soundFX.SFX_MENU_OPTIONS_HOST);
                    currentMenuItem = MENU_HOST;
                } else  if(currentMenuItem == MENU_FIREPIT){
                    playLocalMenuSound(soundFX.SFX_MENU_OPTIONS_JOIN);
                    currentMenuItem = MENU_LEVEL_OPTIONS;
                } else  if(currentMenuItem == MENU_BUNKER){
                    playLocalMenuSound(soundFX.SFX_MENU_OPTIONS_JOIN);
                    currentMenuItem = MENU_LEVEL_OPTIONS;
                }
            }
            if (Input.anyKey && !skipped){
                skipIntro();
            }
        }
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
    void OnGUI()
    {
        if (!connected)
        { 
            if(enteringIP){
                //Turning on GUI and setting it to focus
                GUI.SetNextControlName("MyTextField");
                ip = GUILayout.TextField(ip);
                GUI.FocusControl("MyTextField");
            }
        }
    }
}
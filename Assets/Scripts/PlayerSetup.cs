using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] componentsToDisable;

  
    [SerializeField]
    string remoteLayerName = "Remote Player";
    [SerializeField]
    string DontDrawLayerName = "DontDraw";
    public GameObject playerGraphics;

    [SerializeField]
    GameObject playerGUIPrefab;
    [HideInInspector]
    public GameObject playerGUIInstance;




    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();//the player on other machines or ur opponents


        }
        else
        {
            GameManager.instance.SetSceneCamera(false);

           SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(DontDrawLayerName));

            //setting up crosshair
            playerGUIInstance = Instantiate(playerGUIPrefab);
            playerGUIInstance.name = playerGUIPrefab.name;

            PlayerUI ui = playerGUIInstance.GetComponent<PlayerUI>();

            if (ui == null)
             Debug.LogError("No PlayerUI script found on player UI prefab");

            ui.SetPlayer(GetComponent<Player>());

            GetComponent<Player>().SetupPlayer();//calling setup function
        }

        string _username = "Loading..";
        if (UserAccountManager.IsLoggedIn)
               _username = UserAccountManager.PlayerUsername;
           
        else _username = transform.name;
        CmdSetUSername(transform.name, _username);


    }

    [Command]
    void CmdSetUSername(string _playerID,string _username)
    {
        Player player = GameManager.GetPlayer(_playerID);
        if (player != null)
        {
            Debug.Log(_username + " has joined!");
            player.username = _username;
        }

    }

  
    public  override void OnStartClient()//override values for parent class
    {
        base.OnStartClient();//parent class,here it is same as child class
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        GameManager.RegisterPlayer(_netID, _player);
    }

    

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
       
    }

    void SetLayerRecursively(GameObject _obj,int newLayer){

        _obj.layer = newLayer;

        foreach (Transform _child in _obj.transform)
        {
            SetLayerRecursively(_child.gameObject, newLayer);
        }
    }

    void DisableComponents()
    {

        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }



    void OnDisable()//when this gameObject is disabled
    {
        Destroy(playerGUIInstance);

        if(isLocalPlayer)
        GameManager.instance.SetSceneCamera(true);

        GameManager.UnRegisterPlayer(transform.name);
        
    }


}

using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {


    [SyncVar]
    private bool _isDead = false;
    public bool IsDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]//synchronizes variable over net
    private int currentHealth;
    
    [SerializeField]
    private Behaviour[] disableOnDead;
    private bool[] wasEnabled;
    private bool firstSetup = true;

    public GameObject DeathEffect;
    public GameObject SpawnEffect;
    [SerializeField]
    private GameObject[] disableGameObjectOnDead;

    public int Kills;
    public int Deaths;

    [SyncVar]
    public string username = "Loading..";

    private Transform respawnPosition;

    private void Update()
    {
        if (!isLocalPlayer)
            return;

    }

    public float GetCurrentHealthPct()
    {
        return (float)currentHealth / maxHealth;
    }

    public void SetupPlayer()
    {        
        //switch cameras
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCamera(false);
            GetComponent<PlayerSetup>().playerGUIInstance.SetActive(true);
        }

        CmdBroadcastNewPlayerSetup();
        
    }

    [Command]
    private void CmdBroadcastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()//informing all clients
    {
        if(firstSetup)
        {
            respawnPosition= transform;
            wasEnabled = new bool[disableOnDead.Length];
            for (int i = 0; i < disableOnDead.Length; i++)
            {
                wasEnabled[i] = disableOnDead[i].enabled;//save status of every component whether it's enabled or disabled
            }
            firstSetup = false;
        }
        SetDefaults();

    }

    [ClientRpc]//used when we want to send details from server to client
    public void RpcTakeDamage(int _amount,string _sourceID)
    {
        if (IsDead)
            return;

        currentHealth -= _amount;
        Debug.Log(transform.name + " has " + currentHealth + " health left.");
        if (currentHealth <=0)
        {
            Die(_sourceID);
        }
    }

    void Die(string _sourceID)
    {
        IsDead = true;

        Player sourcePlayer = GameManager.GetPlayer(_sourceID);
        if (sourcePlayer != null)
        {
            sourcePlayer.Kills++;
            GameManager.instance.onPlayerKilledcallback.Invoke(sourcePlayer.username, username);
        }


        Deaths++;

        //disabling components
        for (int i = 0; i < disableOnDead.Length; i++)
        {
            disableOnDead[i].enabled = false; //disable components
        }

        
        //disable gameobjects
        for (int i = 0; i < disableGameObjectOnDead.Length; i++)
        {
            disableGameObjectOnDead[i].SetActive(false); //disable components
        }
        
        //disable collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }

        //launch death effect
        GameObject _GFxIns = Instantiate(DeathEffect, transform.position, Quaternion.identity);
        Destroy(_GFxIns, 3f);

        //switch cameras
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCamera(true);
            GetComponent<PlayerSetup>().playerGUIInstance.SetActive(false);

        }

        Debug.Log(transform.name + " is DEAA..DD !!");

        StartCoroutine(ReSpawn());

    }



    private IEnumerator ReSpawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        
        //Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = respawnPosition.position;
        transform.rotation = respawnPosition.rotation;

        //yield return new WaitForSeconds(0.1f);
        SetupPlayer();
        

        Debug.Log(transform.name + "respawned.");
    }



    public void SetDefaults()
    {
        IsDead = false;
        currentHealth =  maxHealth;

        //enable components
        for (int i = 0; i < disableOnDead.Length; i++)
        {
            disableOnDead[i].enabled = wasEnabled[i]; //return their status
        }

        //enable collider
        Collider _col = GetComponent<Collider>();//since it is not a component, thus tackling separately
        if (_col != null)
        {
            _col.enabled = true;
        }

        //enable gameobject
        for (int i = 0; i < disableGameObjectOnDead.Length; i++)
        {
            disableGameObjectOnDead[i].SetActive(true); 
        }

       
       //create spawn effect
        GameObject _spawnEffect = Instantiate(SpawnEffect, transform.position, Quaternion.identity);
        Destroy(_spawnEffect, 3f);

    }
}

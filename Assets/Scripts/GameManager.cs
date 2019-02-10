using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public MatchSettings matchSettings;
    public Camera sceneCamera;

    public delegate void OnPlayerKilledCallback(string source, string player);
    public OnPlayerKilledCallback onPlayerKilledcallback;

    private void Awake()
    {
        if(instance!= null)
        {
            Debug.LogError("More than one GameManager in the scene !! ");
            this.enabled = false;

        }
        else
        {
            instance = this;
        }
    }

    public void SetSceneCamera(bool _setCamera)
    {

        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(_setCamera);
        }
           }

    #region Player Tracking
    private const string PLAYER_ID_PRFEIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _ID,Player _player)
    {
        string _playerID = PLAYER_ID_PRFEIX + _ID; //creating a name for our player
            players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }


    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

public  static Player[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }

    #endregion



}

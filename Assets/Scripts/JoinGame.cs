using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.Networking.Match;
using UnityEngine.UI;
using System.Collections;

public class JoinGame : MonoBehaviour {

    List<GameObject> roomList = new List<GameObject>();

    [SerializeField]
    private Text statusText;

    [SerializeField]
    private GameObject roomListItemPrefab;
    [SerializeField]
    private Transform roomListParent;


    private NetworkManager networkManager;



    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
        RefreshRoomList();

    }

    public void RefreshRoomList()
    {
        ClearRoomList();
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);//finding possible matches
        statusText.text = "Loading....";


    }

    
    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        statusText.text = "";
        if (!success||matchList == null)
        {
            statusText.text = "Couldn't get room list";
            return;
        }

     
        foreach(MatchInfoSnapshot match in matchList)
        {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
             _roomListItemGO.transform.SetParent(roomListParent);
           
            RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
            if (_roomListItem != null)
            {
                _roomListItem.Setup(match, JoinRoom);
            }
          
            roomList.Add(_roomListItemGO);
        }

        if (roomList.Count == 0)
        {
            statusText.text = "No rooms found at the moment ";

        }
    }

     void ClearRoomList()
    {
        for (int i= 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }
        roomList.Clear();//clearing all the references
    }

    public void JoinRoom(MatchInfoSnapshot _match)
    {
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        StartCoroutine(WaitForJoin());
    }

    IEnumerator WaitForJoin()
    {
        ClearRoomList();
        int countdown = 10;
        while (countdown > 0)
        {
            statusText.text = "JOINING IN "  + countdown ;
            yield return new WaitForSeconds(1);
            countdown--;
        }

        //failed to connect
        statusText.text = "Failed to connect";
        yield return new WaitForSeconds(1);

        MatchInfo matchInfo = networkManager.matchInfo;
        if (matchInfo != null)
        {
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();//if host quits,everyone is thrown out
           
        }
        RefreshRoomList();
        
    }
        
        
}

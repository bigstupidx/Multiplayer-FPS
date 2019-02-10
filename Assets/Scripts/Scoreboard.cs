using UnityEngine;

public class Scoreboard : MonoBehaviour {
    [SerializeField]
    GameObject playerScoreBoardItem;
    [SerializeField]
    Transform playerScoreBoardList;

    void OnEnable()
    {
        Player[] players = GameManager.GetAllPlayers();
        foreach(Player player in players)
        {

           GameObject ItemGO = Instantiate(playerScoreBoardItem, playerScoreBoardList);
            PlayerScoreboardItem item = ItemGO.GetComponent<PlayerScoreboardItem>();
            if (item != null)
            {
                item.Setup(player.username, player.Kills, player.Deaths);
            }

        }
        
    }
    private void OnDisable()
    {
        foreach (Transform child in playerScoreBoardList)
            Destroy(child.gameObject);
    }
}

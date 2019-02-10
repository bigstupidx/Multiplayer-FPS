using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreboardItem : MonoBehaviour {

    [SerializeField]
    Text usernameText;
    [SerializeField]
    Text killsText;
    [SerializeField]
     Text DeathsText;

    public void Setup(string username,int kills,int deaths)
    {
        usernameText.text = username;
        killsText.text = "KILLS " + kills;
        DeathsText.text = "DEATHS " + deaths;

    }
}

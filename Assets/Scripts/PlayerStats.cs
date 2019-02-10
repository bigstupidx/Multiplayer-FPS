using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public Text killCount;
    public Text deathCount;

    private void Start()
    {
        if(UserAccountManager.IsLoggedIn)
        UserAccountManager.instance.LoadorGetData(OnDataReceived);
    }

    void OnDataReceived(string data)
    {
        //if (killCount == null || deathCount == null)
        //    return;

        killCount.text = DataTranslator.DataToKills(data).ToString() + " KILLS";
        deathCount.text = DataTranslator.DataToDeaths(data).ToString()+ " DEATHS";
       
    }
}

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerScore: MonoBehaviour {


    int lastKills = 0;
    int lastDeaths = 0;
    Player player;


    private void Start()
    {
        player = GetComponent<Player>();
        StartCoroutine(SyncScoreLoop());

    }

    void OnDestroy()
    {
        if (player != null)
            SyncNow();
        
    }

    IEnumerator SyncScoreLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            SyncNow();
        }
    }

    void SyncNow()
    {
        if (UserAccountManager.IsLoggedIn)
            UserAccountManager.instance.LoadorGetData(OnDataReceived);
    }

      void OnDataReceived(string data)
    {
        
        if (player.Kills <= lastKills && player.Deaths <= lastDeaths)
            return;

        int killsSinceLast = player.Kills - lastKills;
        int deathsSinceLast = player.Deaths - lastDeaths;

        int kills = DataTranslator.DataToKills(data);
        int deaths = DataTranslator.DataToDeaths(data);

        int newKills = killsSinceLast + kills;
        int newDeaths = deathsSinceLast + deaths;
        lastKills = player.Kills;
        lastDeaths = player.Deaths;

        string newData = DataTranslator.ValueToData(newKills, newDeaths);
        Debug.Log("Syncing " + newData);
        UserAccountManager.instance.SaveorSetData(newData);
    }
}

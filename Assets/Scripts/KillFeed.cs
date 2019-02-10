using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFeed : MonoBehaviour {
    [SerializeField]
    GameObject killItemPrefab;

	public void Start()
    {
        GameManager.instance.onPlayerKilledcallback += OnKill;

    }

    public void OnKill(string source,string player)
    {
        if (player != null)
        {
            GameObject go = Instantiate(killItemPrefab, this.transform);
            go.GetComponent<KillFeedItem>().Setup(source, player);
            Destroy(go, 3f);
        }



    }
}

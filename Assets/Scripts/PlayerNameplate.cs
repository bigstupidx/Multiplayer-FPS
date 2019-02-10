using UnityEngine;
using UnityEngine.UI;

public class PlayerNameplate : MonoBehaviour {

    [SerializeField]
    private Text Nameplate;
    [SerializeField]
    private RectTransform healthBarRefill;

    [SerializeField]
    private Player player;
	
	// Update is called once per frame
	void Update () {
        Nameplate.text = player.username;
        healthBarRefill.localScale = new Vector3 (player.GetCurrentHealthPct(),1f,1f);
	}
}

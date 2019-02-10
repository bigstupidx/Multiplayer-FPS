using UnityEngine;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviour {
    [SerializeField]
    Text sourceText;
    [SerializeField]
    Text playerText;

	public void Setup(string source,string player)
    {
        sourceText.text = source;
        playerText.text = player;
    }
}

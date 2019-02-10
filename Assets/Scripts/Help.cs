using UnityEngine;

public class Help : MonoBehaviour {


    [SerializeField]
    private GameObject helpPanel;

   public void HelpButton()
    {
        helpPanel.SetActive(true);
    }

    public  void ExitHelp()
    {
        helpPanel.SetActive(false);
    }
}

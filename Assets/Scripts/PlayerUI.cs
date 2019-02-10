using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour {

    [SerializeField]
     RectTransform thrusterFillAmount;
    [SerializeField]
    RectTransform healthBarFillAmount;
    [SerializeField]
    Text ammunoText;
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject scoreborad;

    private Player player;
    private PlayerController controller;
    private WeaponManager weaponManager;

    private void Start()
    {
        PauseMenu.isOn = false;
    }


    public void  SetPlayer(Player _player)
    {
        player = _player;
        controller = player.GetComponent<PlayerController>();
        weaponManager = player.GetComponent<WeaponManager>();
    }

    void Update()
    {
        SetThrusterFuelAmount(controller.GetThrusterFillAmount());
        SetHealthBarAmount(player.GetCurrentHealthPct());
        SetAmmunoAmount(weaponManager.GetCurrentWeapon().CurrentBullets);
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreborad.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreborad.SetActive(false);
        }
        
    }

     public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;

    }

    void SetThrusterFuelAmount(float _amount)
    {
        thrusterFillAmount.localScale = new Vector3(1f, _amount, 1f);
    }

     void SetHealthBarAmount(float _amount)
    {
        healthBarFillAmount.localScale = new Vector3(1f, _amount, 1f);
    }

    void SetAmmunoAmount(int _number)
    {
        ammunoText.text = _number.ToString();
    }
}

using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

public class WeaponManager : NetworkBehaviour
{


    [SerializeField]
    private string WeaponLayerName = "Weapon";
    [SerializeField]
    private PlayerWeapons primaryWeapon;
    [SerializeField]
    private Transform weaponHolder;


    private PlayerWeapons currentWeapon;
    private WeaponGraphics currentGraphics;

    public bool isReloading = false;



    private void Start()
    {
        EquipWeapon(primaryWeapon);
        Reload();
    }


    public PlayerWeapons GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }


    void EquipWeapon(PlayerWeapons _weapon)
    {
        currentWeapon = _weapon;

        GameObject _weaponIns = (GameObject)Instantiate(currentWeapon.weaponGFX, weaponHolder.position, weaponHolder.rotation);

        currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
        _weaponIns.transform.SetParent(weaponHolder);

        if (currentGraphics == null)
            Debug.LogError("No currentGraphics found for the weapon" + _weapon.name);



        if (isLocalPlayer)
        {
            Utility.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(WeaponLayerName));//so that muzzleFlare also comes in line
            //thus giving same layer to all children too

        }
    }

    public void Reload()
    {
        if (isReloading)
            return;

        StartCoroutine(Reload_Coroutine());

    }


    private IEnumerator Reload_Coroutine()
    {
        Debug.Log("Is relaoding...");
        isReloading = true;
        CmdOnReload();
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentWeapon.CurrentBullets = currentWeapon.maxBullets;

        isReloading = false;


    }

    [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload()
    {
        Animator anim = currentGraphics.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Reload");
        }
    }
}


    

    


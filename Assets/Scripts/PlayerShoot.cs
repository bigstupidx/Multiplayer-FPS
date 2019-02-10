using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {


    private const string PLAYER_TAG = "Player";
        
 
    private PlayerWeapons currentWeapon;
    private WeaponManager weaponManager;

    
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private void Start()
    {
       
        if (cam == null)
        {
            Debug.LogError("Player Shoot: No camera/shoot point found");
            this.enabled = false;
        }
        weaponManager = GetComponent<WeaponManager>();
        
    }

   void Update()
    {
        if (PauseMenu.isOn)
            return;
        
        currentWeapon = weaponManager.GetCurrentWeapon();

        //if (currentWeapon.CurrentBullets < currentWeapon.maxBullets)
        //{
        //    if (Input.GetButtonDown("Reload"))
        //    {
        //        weaponManager.Reload();
        //        return;
        //    }

        //}
                 


        if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))

                Shoot();

        }
        else {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        } 
        

    }

    [Client]//run only on clients,no interference of net
    void Shoot()
    {
        if (!isLocalPlayer||weaponManager.isReloading)
            return;

        if (currentWeapon.CurrentBullets <= 0)
        {
            weaponManager.Reload();
            return;
        }

        currentWeapon.CurrentBullets--;

        Debug.Log("Remaining bullets: " + currentWeapon.CurrentBullets);

        CmdOnShoot();

        RaycastHit _Hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward,out _Hit, currentWeapon.range, mask))
        {
            if(_Hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_Hit.collider.name,currentWeapon.Damage,transform.name);
            }

            CmdOnHit(_Hit.point, _Hit.normal);
        }
        if (currentWeapon.CurrentBullets <= 0)
        {
            weaponManager.Reload();
        }
    }

    //sending shoot information to the server
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    //receiving shoot info from the server so that we can instantiate muzzleFlare and it is visible by all clients
    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlare.Play();
    }

    [Command]
    void CmdOnHit(Vector3 _position,Vector3 _normal)
    {
        RpcDoHitEffect(_position, _normal);
    }

    [ClientRpc]
    void RpcDoHitEffect(Vector3 _position,Vector3 _normal)
    {
        GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _position, Quaternion.LookRotation(_normal));
        Destroy(_hitEffect, 1f);
    }

    [Command]
    void CmdPlayerShot(string _playerID,int _damage,string _sourceID)
    {
         Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage,_sourceID);

    }
}

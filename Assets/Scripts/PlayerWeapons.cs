using UnityEngine;


[System.Serializable]
public class PlayerWeapons  {

    public string name = "GLOCK";
    public int Damage = 10;
    public float range = 200f;
    public float fireRate = 0f;
    public float reloadTime = 0.5f;
    public int maxBullets = 20;
    [HideInInspector]
    public int CurrentBullets;
    private WeaponGraphics weaponGraphics;

    public GameObject weaponGFX;

    public void Awake()
    {
        
       CurrentBullets = maxBullets;
    }
    
}

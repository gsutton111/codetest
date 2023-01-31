using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static AmmoSelector;

public class AmmoSupply : MonoBehaviour
{
    public AmmoSelector.AmmoType type;
    
    // Start is called before the first frame update
    void Awake()
    {
        enabled = false;
    }

    // Update is called once per frame
    public void Setup(AmmoSelector.AmmoType ammoType, Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        type = ammoType;

        enabled = true;
    }

    // When a player picks up the resupply
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        Debug.Log("Player touched a " + type.m_Name + " supply");
        TankShooting tank = other.GetComponent<TankShooting>();
        Debug.Log(tank);

        int i = AmmoSelector.instance.m_AmmoTypes.IndexOf(type);
        Debug.Log(type.m_Name + " ammo was: " + tank.m_Weapons[AmmoSelector.instance.m_AmmoTypes[i].m_Name].m_AmmoSupply);
        tank.m_Weapons[AmmoSelector.instance.m_AmmoTypes[i].m_Name].m_AmmoSupply = type.m_MaxAmmo;
        Debug.Log(type.m_Name + " ammo is now: " + tank.m_Weapons[AmmoSelector.instance.m_AmmoTypes[i].m_Name].m_AmmoSupply);

        AmmoSpawnManager.instance.AmmoSpawnPoints.Add(transform);
        Destroy(gameObject);
    }
}

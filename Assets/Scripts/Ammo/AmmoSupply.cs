using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static AmmoSelector;

public class AmmoSupply : MonoBehaviour
{
    public AmmoSelector.AmmoType type;
    private MeshRenderer meshRenderer;
    
    // Start is called before the first frame update
    void Awake()
    {
        enabled = false;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    public void Setup(AmmoSelector.AmmoType ammoType, UnityEngine.Vector3 spawnPoint, Material colour)
    {
        transform.position = spawnPoint;

        meshRenderer.material = colour;

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
        tank.m_Weapons[AmmoSelector.instance.m_AmmoTypes[i].m_Name].m_AmmoSupply = type.m_MaxAmmo;

        AmmoSpawnManager.instance.AmmoSpawnPoints.Add(gameObject.transform.position);
        Destroy(gameObject);
    }
}

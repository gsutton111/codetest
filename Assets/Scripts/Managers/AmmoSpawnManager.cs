using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AmmoSpawnManager : MonoBehaviour
{
    public static AmmoSpawnManager instance;

    public List<Transform> AmmoSpawnPoints;
    public AmmoSupply m_SupplyPrefab;
    public float m_AmmoStartDelay = 5f;
    public float m_AmmoSpawnDelay = 3f;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        InvokeRepeating("SpawnAmmo", m_AmmoStartDelay, m_AmmoSpawnDelay);
    }

    private void SpawnAmmo()
    {
        if (AmmoSpawnPoints.Count < 1 || AmmoSpawnPoints == null) return;

        int i = Random.Range(0, AmmoSelector.instance.m_AmmoTypes.Count);
        int j = Random.Range(0, AmmoSpawnPoints.Count);

        var supply = Instantiate(m_SupplyPrefab);
        supply.Setup(AmmoSelector.instance.m_AmmoTypes[i], AmmoSpawnPoints[j]);

        AmmoSpawnPoints.RemoveAt(j);
    }
}

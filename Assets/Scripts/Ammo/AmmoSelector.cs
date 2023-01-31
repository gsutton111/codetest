using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSelector : MonoBehaviour
{
    public static AmmoSelector instance;

    public class AmmoType
    {
        public string m_Name;

        // Variables for ShellExplosion.cs
        public float m_MaxDamage;                   // The amount of damage done if the explosion is centred on a tank.
        public float m_ExplosionForce;              // The amount of force added to a tank at the centre of the explosion.
        public float m_MaxLifeTime;                 // The time in seconds before the shell is removed.
        public float m_ExplosionRadius;             // The maximum distance away from the explosion tanks can be and are still affected.

        // Variables for TankShooting.cs
        public float m_MinLaunchForce;              // The force given to the shell if the fire button is not held.
        public float m_MaxLaunchForce;              // The force given to the shell if the fire button is held for the max charge time.
        public float m_MaxChargeTime;               // How long the shell can charge for before it is fired at max force.
        public int m_MaxAmmo;                       // The max number of shots a player can fire in a round
        public int m_FireRate;                      // The amount of rounds fired per second (set to 0 for non-automatic fire)
        public float m_CoolDown;                    // The delay in seconds before the ammo can be fired again

        public int m_Ammo;                          // The amount of ammo the player starts with for this ammo type
    }

    public List<AmmoType> m_AmmoTypes = new List<AmmoType>();   // A list of all available ammo types

    public void Awake()
    {
        instance = this;

        AmmoType missile = new AmmoType();
        missile.m_Name = "Missile";
        missile.m_MaxDamage = 100f;
        missile.m_ExplosionForce = 1000f;
        missile.m_MaxLifeTime = 2f;
        missile.m_ExplosionRadius = 5f;

        missile.m_MinLaunchForce = 15f;
        missile.m_MaxLaunchForce = 30f;
        missile.m_MaxChargeTime = 0.75f;
        missile.m_MaxAmmo = 30;
        missile.m_FireRate = 0;

        missile.m_CoolDown = 1f;
        missile.m_Ammo = 30;

        m_AmmoTypes.Add(missile);


        AmmoType sniper = new AmmoType();
        sniper.m_Name = "Sniper";
        sniper.m_MaxDamage = 200f;
        sniper.m_ExplosionForce = 200f;
        sniper.m_MaxLifeTime = 2f;
        sniper.m_ExplosionRadius = 1f;

        sniper.m_MinLaunchForce = 30f;
        sniper.m_MaxLaunchForce = 60f;
        sniper.m_MaxChargeTime = 1.5f;
        sniper.m_MaxAmmo = 10;
        sniper.m_FireRate = 0;

        sniper.m_CoolDown = 1.5f;
        sniper.m_Ammo = 0;

        m_AmmoTypes.Add(sniper);


        AmmoType grenade = new AmmoType();
        grenade.m_Name = "Grenade";
        grenade.m_MaxDamage = 250f;
        grenade.m_ExplosionForce = 2000f;
        grenade.m_MaxLifeTime = 5f;
        grenade.m_ExplosionRadius = 12f;

        grenade.m_MinLaunchForce = 5f;
        grenade.m_MaxLaunchForce = 10f;
        grenade.m_MaxChargeTime = 0.75f;
        grenade.m_MaxAmmo = 10;
        grenade.m_FireRate = 0;

        grenade.m_CoolDown = 2f;
        grenade.m_Ammo = 0;

        m_AmmoTypes.Add(grenade);


        AmmoType automatic = new AmmoType();
        automatic.m_Name = "Automatic";
        automatic.m_MaxDamage = 25f;
        automatic.m_ExplosionForce = 10f;
        automatic.m_MaxLifeTime = 2f;
        automatic.m_ExplosionRadius = 1f;

        automatic.m_MinLaunchForce = 20f;
        automatic.m_MaxLaunchForce = 25f;
        automatic.m_MaxChargeTime = 0f;
        automatic.m_MaxAmmo = 30;
        automatic.m_FireRate = 10;

        automatic.m_CoolDown = 0f;
        automatic.m_Ammo = 0;

        m_AmmoTypes.Add(automatic);
    }
}

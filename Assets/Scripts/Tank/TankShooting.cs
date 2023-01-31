//using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static AmmoSelector;

public class TankShooting : MonoBehaviour
{
    public static TankShooting instance;

    public class m_AmmoInfo
    {
        public int m_AmmoSupply;                            // The amount of ammo remaining for the weapon
        public float m_AmmoCooldown;                        // The current cooldown remaining for the weapon

        // Constructor to initialise the weapon data
        public m_AmmoInfo(int ammo, float cooldown)
        {
            m_AmmoSupply = ammo;                            // Set the ammo remaining for the weapon
            m_AmmoCooldown = cooldown;                      // Set the cooldown remaining for the weapon
        }
    }

    Dictionary<string, m_AmmoInfo> m_Weapons = new Dictionary<string, m_AmmoInfo>();

    public AmmoType m_AmmoType;                             // The currently selected ammo type
    public Rigidbody m_Shell;                               // Prefab of the shell.
    public Transform m_FireTransform;                       // A child of the tank where the shells are spawned.
    public Slider m_AimSlider;                              // A child of the tank that displays the current launch force.
    public AudioSource m_ShootingAudio;                     // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_ChargingClip;                        // Audio that plays when each shot is charging up.
    public AudioClip m_FireClip;                            // Audio that plays when each shot is fired.

    private float m_CurrentLaunchForce;                     // The force that will be given to the shell when the fire button is released.
    private float m_ChargeSpeed;                            // How fast the launch force increases, based on the max charge time.
    private int m_AmmoTypeIndex;
    private bool m_Charging;

    public bool IsCharging
    {
        get { return m_Charging; }
    }

    private void OnEnable()
    {
        for (int i = 0; i < AmmoSelector.instance.m_AmmoTypes.Count; i++)
        {
            m_Weapons[AmmoSelector.instance.m_AmmoTypes[i].m_Name] = new m_AmmoInfo(AmmoSelector.instance.m_AmmoTypes[i].m_Ammo, 0f);
        }

        // Set the starting ammo type to the default
        m_AmmoType = AmmoSelector.instance.m_AmmoTypes[0];

        // The rate that the launch force charges up is the range of possible forces by the max charge time.
        m_ChargeSpeed = (m_AmmoType.m_MaxLaunchForce - m_AmmoType.m_MinLaunchForce) / m_AmmoType.m_MaxChargeTime;

        // When the tank is turned on, set the slider's paramaters
        m_AimSlider.value = m_AmmoType.m_MinLaunchForce;
        m_AimSlider.minValue = m_AmmoType.m_MinLaunchForce;
        m_AimSlider.maxValue = m_AmmoType.m_MaxLaunchForce;

        // Reset the launch force, the UI and ammo
        m_CurrentLaunchForce = m_AmmoType.m_MinLaunchForce;
    }


    private void Start()
    {
        instance = this;
    }

    IEnumerator StartCooldown(string weaponName, float cooldownTime)
    {
        m_Weapons[weaponName].m_AmmoCooldown = cooldownTime;

        while (m_Weapons[weaponName].m_AmmoCooldown > 0)
        {
            yield return new WaitForEndOfFrame();
            m_Weapons[weaponName].m_AmmoCooldown -= Time.deltaTime;
        }
    }

    public void Switch()
    {
        m_AmmoTypeIndex = AmmoSelector.instance.m_AmmoTypes.IndexOf(m_AmmoType);    // Find which position the current ammo type is in list of collected ammo types
        int length = AmmoSelector.instance.m_AmmoTypes.Count;                       // Determine how many ammo types the player owns
        m_AmmoTypeIndex++;                                                          // Move the index to the right of collected ammo types
        if (m_AmmoTypeIndex >= length) m_AmmoTypeIndex = 0;                         // Loop back around if the index goes out of range
        m_AmmoType = AmmoSelector.instance.m_AmmoTypes[m_AmmoTypeIndex];            // Set the ammo type to the new selected ammo

        // Set the aim slider's paramaters every time a new ammo type is selected
        m_AimSlider.minValue = m_AmmoType.m_MinLaunchForce;
        m_AimSlider.maxValue = m_AmmoType.m_MaxLaunchForce;
    }

    public void BeginChargingShot()
    {
        // If current ammo type is out of ammo or is on cooldown, or the player is already charging a shot, end function
        bool noFire()
        {
            return
                m_Weapons[AmmoSelector.instance.m_AmmoTypes[m_AmmoTypeIndex].m_Name].m_AmmoSupply <= 0 ||
                m_Weapons[AmmoSelector.instance.m_AmmoTypes[m_AmmoTypeIndex].m_Name].m_AmmoCooldown > 0 ||
                m_Charging;
        }

        if (noFire()) return;

        m_CurrentLaunchForce = m_AmmoType.m_MinLaunchForce;

        // Change the clip to the charging clip and start it playing.
        m_ShootingAudio.clip = m_ChargingClip;
        m_ShootingAudio.Play();

        m_Charging = true;
    }

    public void FireChargedShot()
    {
        if (!m_Charging) return;

        Fire();
        m_Charging = false;
    }


    private void Update()
    {
        if (m_Charging)
        {
            m_CurrentLaunchForce = Mathf.Min(m_AmmoType.m_MaxLaunchForce, m_CurrentLaunchForce + m_ChargeSpeed * Time.deltaTime);
            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else
        {
            m_AimSlider.value = m_AmmoType.m_MinLaunchForce;
        }
    }


    private void Fire()
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        // Reset the launch force.  This is a precaution in case of missing button events.
        m_CurrentLaunchForce = m_AmmoType.m_MinLaunchForce;

        m_Weapons[AmmoSelector.instance.m_AmmoTypes[m_AmmoTypeIndex].m_Name].m_AmmoSupply--;
        StartCoroutine(StartCooldown(AmmoSelector.instance.m_AmmoTypes[m_AmmoTypeIndex].m_Name, AmmoSelector.instance.m_AmmoTypes[m_AmmoTypeIndex].m_CoolDown));

    }
}
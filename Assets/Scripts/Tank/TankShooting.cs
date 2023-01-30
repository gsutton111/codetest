using Boo.Lang;
using UnityEngine;
using UnityEngine.UI;
using static AmmoSelector;

public class TankShooting : MonoBehaviour
{
    public static TankShooting instance;

    public Rigidbody m_Shell;                   // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
    public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
    public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
    public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
    /*
    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
    public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.
    public int m_MaxAmmo = 30;                  // The max number of shots a player can fire in a round

    private int m_Ammo = 30;                    // The amount of ammo the player currently has
    */
    private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
    private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
    private bool m_Charging;

    private List<int> m_Ammo = new List<int>();
    public AmmoType m_AmmoType;

    public bool IsCharging
	{
		get { return m_Charging; }
	}
	
    private void OnEnable()
    {
        m_AmmoType = AmmoSelector.instance.m_AmmoTypes[0]; // Set the starting ammo type to the default

        // When the tank is turned on, reset the launch force, the UI and ammo
        m_CurrentLaunchForce = m_AmmoType.m_MinLaunchForce;
        m_AimSlider.value = m_AmmoType.m_MinLaunchForce;
        for (int i = 0; i < AmmoSelector.instance.m_AmmoTypes.Count; i++)
        {
            m_Ammo.Add(AmmoSelector.instance.m_AmmoTypes[i].m_Ammo);
        }
    }


    private void Start ()
    {
        instance = this;

        // The rate that the launch force charges up is the range of possible forces by the max charge time.
        //m_ChargeSpeed = (m_AmmoType.m_MaxLaunchForce - AmmoSelector.instance.m_AmmoType.m_MinLaunchForce) / AmmoSelector.instance.m_AmmoType.m_MaxChargeTime;
        m_ChargeSpeed = 20;
    }

    public void Switch()
    {
        int i = AmmoSelector.instance.m_AmmoTypes.IndexOf(m_AmmoType);  // Find which position the current ammo type is in list of collected ammo types
        int length = AmmoSelector.instance.m_AmmoTypes.Count;           // Determine how many ammo types the player owns
        i++;                                                            // Move the index to the right of collected ammo types
        if (i >= length) i = 0;                                         // Loop back around if the index goes out of range
        m_AmmoType = AmmoSelector.instance.m_AmmoTypes[i];              // Set the ammo type to the new selected ammo

        Debug.Log("Ammo type set to: " + AmmoSelector.instance.m_AmmoTypes[i]);
    }

    public void BeginChargingShot()
	{
        int i = AmmoSelector.instance.m_AmmoTypes.IndexOf(m_AmmoType);
        if (m_Ammo[i] <= 0 || m_Charging) return;

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
			m_CurrentLaunchForce = Mathf.Min(m_AmmoType.m_MaxLaunchForce, m_CurrentLaunchForce + m_ChargeSpeed*Time.deltaTime);
			m_AimSlider.value = m_CurrentLaunchForce;
		}
		else
		{
			m_AimSlider.value = m_AmmoType.m_MinLaunchForce;
		}
	}


    private void Fire ()
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; 

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play ();

        // Reset the launch force.  This is a precaution in case of missing button events.
        m_CurrentLaunchForce = m_AmmoType.m_MinLaunchForce;

        int i = AmmoSelector.instance.m_AmmoTypes.IndexOf(m_AmmoType);
        m_Ammo[i]--;
    }
}
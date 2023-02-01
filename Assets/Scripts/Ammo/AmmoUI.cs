using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameState;

public class AmmoUI : MonoBehaviour
{
    //public Vector3 offset = new Vector3(3f, 3f, 0f);
    public Vector3 offset = new Vector3(0f, 3f, 0f);

    public Sprite[] ammoTypeSprites;

    public Text ammoDisplay;
    public Image typeDisplay;
    public Image typeBackground;

    private TankThinker m_Tank;
    private Color m_Colour;
    private TankShooting m_Shooting;

    public void Setup(TankThinker tank)
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        m_Tank = tank;
        m_Colour = tank.player.PlayerInfo.Color;
        m_Shooting = tank.GetComponent<TankShooting>();

        transform.position = m_Tank.transform.position + offset;

        float cd = m_Shooting.m_Weapons[AmmoSelector.instance.m_AmmoTypes[m_Shooting.m_AmmoTypeIndex].m_Name].m_AmmoCooldown;
        int ammo = m_Shooting.m_Weapons[AmmoSelector.instance.m_AmmoTypes[m_Shooting.m_AmmoTypeIndex].m_Name].m_AmmoSupply;

        ammoDisplay.text = ammo.ToString();
        ammoDisplay.color = m_Colour;

        typeDisplay.sprite = ammoTypeSprites[m_Shooting.m_AmmoTypeIndex];
        typeDisplay.color = m_Colour;
        typeBackground.sprite = ammoTypeSprites[m_Shooting.m_AmmoTypeIndex];
        typeDisplay.fillAmount = 1 - cd / AmmoSelector.instance.m_AmmoTypes[m_Shooting.m_AmmoTypeIndex].m_CoolDown;
    }
    void Update()
    {
        transform.position = m_Tank.transform.position + offset;

        float cd = m_Shooting.m_Weapons[AmmoSelector.instance.m_AmmoTypes[m_Shooting.m_AmmoTypeIndex].m_Name].m_AmmoCooldown;
        int ammo = m_Shooting.m_Weapons[AmmoSelector.instance.m_AmmoTypes[m_Shooting.m_AmmoTypeIndex].m_Name].m_AmmoSupply;

        ammoDisplay.text = ammo.ToString();
        ammoDisplay.color = m_Colour;

        typeDisplay.sprite = ammoTypeSprites[m_Shooting.m_AmmoTypeIndex];
        typeDisplay.color = m_Colour;
        typeBackground.sprite = ammoTypeSprites[m_Shooting.m_AmmoTypeIndex];
        typeDisplay.fillAmount = 1 - cd / AmmoSelector.instance.m_AmmoTypes[m_Shooting.m_AmmoTypeIndex].m_CoolDown;
    }
}

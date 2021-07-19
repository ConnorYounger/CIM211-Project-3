using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour
{
    public GameObject[] leftArmWeapons;
    public GameObject[] rightArmWeapons;

    public int leftArmWeaponCode;
    public int rightArmWeaponCode;

    private void Start()
    {
        UpdateWeapons();
    }

    public void SetWeapons(int l, int r)
    {
        leftArmWeaponCode = l;
        rightArmWeaponCode = r;

        UpdateWeapons();
    }

    public void UpdateWeapons()
    {
        // Left Arms
        for(int i = 0; i < leftArmWeapons.Length; i++)
        {
            if(i == leftArmWeaponCode)
            {
                leftArmWeapons[i].SetActive(true);
            }
            else
            {
                leftArmWeapons[i].SetActive(false);
            }
        }

        //Right Arms
        for (int i = 0; i < rightArmWeapons.Length; i++)
        {
            if (i == rightArmWeaponCode)
            {
                rightArmWeapons[i].SetActive(true);
            }
            else
            {
                rightArmWeapons[i].SetActive(false);
            }
        }
    }
}

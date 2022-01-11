using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharStats : MonoBehaviour
{
    public string charName;
    public int charLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseEXP = 1000;

    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 30;
    public int[] mpLvlBonus;
    public int strength;
    public int defence;
    public int weaponPower;
    public int armorPower;
    public string equippedWeapon;
    public string equippedArmor;
    public Sprite charImage;

    // Start is called before the first frame update
    void Start()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;
        
        for(int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i-1] * 1.05f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            AddExp(1000);
        }
    }

    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;

        while(charLevel < maxLevel && currentEXP > expToNextLevel[charLevel])
        {
            currentEXP -= expToNextLevel[charLevel];
            charLevel++;
            
            //determine whether to add to str or def based on odd or even
            if(charLevel%2 == 0)
            {
                strength++;
            }
            else
            {
                defence++;
            }

            maxHP = Mathf.FloorToInt(maxHP * 1.05f);
            currentHP = maxHP;

            maxMP += mpLvlBonus[charLevel];
            currentMP = maxMP;

        }

        if(charLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }
}

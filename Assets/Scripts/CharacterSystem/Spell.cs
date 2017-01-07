using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell {

    private string sName;
    private int iAttack;
    private int iDefense;

    public Spell(string name, int attack, int defense)
    {
        sName = name;
        iAttack = attack;
        iDefense = defense;
    }

    public string GetName()
    {
        return sName;
    }

    public int GetAttack()
    {
        return iAttack;
    }

    public int GetDefense()
    {
        return iDefense;
    }
}

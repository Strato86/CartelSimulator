using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldModel
{
    public int maria;
    public int seeds;
    public float money;
    public string weapon;
    public bool hasAgreement;
    public bool isNarco;
    
    public static bool IsEqual(WorldModel a, WorldModel b)
    {
        return a.maria == b.maria &&
            a.seeds == b.seeds &&
            a.money == b.money &&
            a.weapon == b.weapon &&
            a.hasAgreement == b.hasAgreement &&
            a.isNarco == b.isNarco;
    }

    public static WorldModel Clone(WorldModel baseModel)
    {
        var clone = new WorldModel();
        clone.maria = baseModel.maria;
        clone.seeds = baseModel.seeds;
        clone.money = baseModel.money;
        clone.weapon = baseModel.weapon;
        clone.hasAgreement = baseModel.hasAgreement;
        clone.isNarco = baseModel.isNarco;

        return clone;
    }
}

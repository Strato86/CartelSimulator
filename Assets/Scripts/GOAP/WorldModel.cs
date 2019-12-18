using System;
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
    public int stealCount;
    
    public static bool IsEqual(WorldModel a, WorldModel b)
    {
        return a.maria == b.maria &&
            a.seeds == b.seeds &&
            a.money == b.money &&
            a.weapon == b.weapon &&
            a.hasAgreement == b.hasAgreement &&
            a.isNarco == b.isNarco &&
            a.stealCount == b.stealCount;
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
        clone.stealCount = baseModel.stealCount;

        return clone;
    }

    public static WorldModel UpdateValues(WorldModel wm, List<Func<WorldModel,WorldModel>> effects)
    {
        var newValue = Clone(wm);
        foreach (var e in effects)
        {
            newValue = e(newValue);
        }

        return newValue;
    }
}

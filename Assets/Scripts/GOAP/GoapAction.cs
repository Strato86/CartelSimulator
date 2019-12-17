using System.Collections.Generic;
using System;

public class GoapAction<State>
{
    public Func<State,State> Effect { get { return _effect; } }
    public List<Func<State,bool>> Preconditions { get { return _preConditions; } }
    public string Name { get { return _actionKey.ToString(); } }
    public float Cost { get { return _cost; } }

    PlayerActionKey _actionKey;
    List<Func<State, bool>> _preConditions;
    Func<State, State> _effect;
    float _cost;

    public GoapAction(PlayerActionKey key)
    {
        _actionKey = key;
        _preConditions = new List<Func<State, bool>>();
    }

    public GoapAction<State> AddPrecondition(Func<State, bool> pre)
    {
        _preConditions.Add(pre);
        return this;
    }

    public GoapAction<State> AddEffect(Func<State, State> eff)
    {
        _effect = eff;
        return this;
    }

    public GoapAction<State> AddCost(float cost)
    {
        _cost = cost;
        return this;
    }
}

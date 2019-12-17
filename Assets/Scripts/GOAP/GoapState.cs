using System;
using System.Collections.Generic;
using UnityEngine;

public class GoapState
{
    List<GoapAction<WorldModel>> _actions;
    WorldModel _currentWorldModel;
    Func<WorldModel, bool> _goal;
    Func<WorldModel, WorldModel, float,float> _heuristic;
    GoapAction<WorldModel> _generatedAction;

    public List<GoapAction<WorldModel>> Actions { get { return _actions; } }
    public Func<WorldModel, bool> Goal { get { return _goal; } }
    public WorldModel CurrentWorldModel { get { return _currentWorldModel; } }
    public GoapAction<WorldModel> GeneratedAction { get { return _generatedAction; } }
    public Func<WorldModel, WorldModel, float,float> Heuristic { get { return _heuristic; } }

    public GoapState(List<GoapAction<WorldModel>> actions, GoapAction<WorldModel> generatedAction, WorldModel initialValues, Func<WorldModel, bool> goal, Func<WorldModel, WorldModel, float,float> h)
    {
        _actions = actions;
        _currentWorldModel = initialValues;
        _goal = goal;
        _heuristic = h;
        _generatedAction = generatedAction;
    }

    public bool IsEqual(GoapState s)
    {
        var result = false;

        if (Equals(s.CurrentWorldModel,CurrentWorldModel))
            result = true;

        if (s.GeneratedAction != GeneratedAction)
            result = false;

        return result;
    }
}

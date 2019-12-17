using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Initial Values")]
    public int initialMaria;
    public int initialSeeds;
    public float initialMoney;
    public string initialWeapon;
    public bool initialHasAgreement;

    [Header("Money Values")]
    public float mariaSellPrice;
    public float seedPrice;
    public float gunPrice;
    public float acordPrice;
    public float salary;
    public float kidnapBounty;
    public float stealBounty;

    [Header("Costs")]
    public int plantMariaCost;
    public int getSeedCost;
    public int beNarcoCost;
    public int gobAgreementCost;
    public int kidnapCost;
    public int buyGunCost;
    public int sellMariaCost;
    public int workCost;
    public int stealCost;

    [Header("Spots")]
    public Transform sellMariaSpot;
    public Transform labSpot;
    public Transform farmSpot;
    public Transform getSeedSpot;
    public Transform getGunSpot;
    public Transform capitolioSpot;
    public Transform workSpot;
    public Transform kidnapHouse;
    
    const string WEAPON_GUN = "gun";
    const string WEAPON_KNIFE = "knife";

    private Queue<Tuple<string,WorldModel>> _actionQueue;
    private Walker _walker;
    void Start()
    {
        _walker = GetComponent<Walker>();
        _walker.runSpeed = 10;
        Plan();
    }

    void Update()
    {

    }

    void Plan()
    {
        var initialModel = new WorldModel();
        initialModel.maria = initialMaria;
        initialModel.seeds = initialSeeds;
        initialModel.money = initialMoney;
        initialModel.weapon = initialWeapon;
        initialModel.hasAgreement = initialHasAgreement;

        Func<WorldModel, bool> goal = (g) => g.isNarco;

        var actions = GetActions();

        var initialState = new GoapState(actions, null, initialModel, goal, Heuristic);

        var plan = new GOAP(initialState).Execute();

        _actionQueue = new Queue<Tuple<string, WorldModel>>();

        foreach (var step in plan)
        {
            if (!step.Item1) continue;
            var path = step.Item3.Reverse();
            foreach (var state in path)
            {
                if(state.GeneratedAction != null)
                {
                    _actionQueue.Enqueue(Tuple.Create(state.GeneratedAction.Name, state.CurrentWorldModel));
                }
            }
        }

        if(_actionQueue.Count == 0)
        {
            Debug.Log("Cant be a Narco");
        }
        else
        {
            ExecuteAction();
        }
    }

    void ExecuteAction()
    {
        if (_actionQueue.Count > 0)
        {
            var dequeued = _actionQueue.Dequeue();
            Debug.Log("Action: " + dequeued.Item1);
            StartCoroutine(dequeued.Item1, dequeued.Item2);
        }
        else
            Plan();
    }

    List<GoapAction<WorldModel>> GetActions()
    {
        var listOfActions = new List<GoapAction<WorldModel>>()
        {
            new GoapAction<WorldModel>(PlayerActionKey.GetLab)
                .AddPrecondition(x => x.hasAgreement == true)
                .AddPrecondition(x => x.money == 400)
                .AddEffect(x => 
                {
                    var wm = new WorldModel();
                    wm.isNarco = true;
                    return wm;
                })
                .AddCost(beNarcoCost),

            new GoapAction<WorldModel>(PlayerActionKey.PlantMaria)
                .AddPrecondition(x => x.seeds > 0)
                .AddEffect(x =>
                {
                    var wm = WorldModel.Clone(x);
                    wm.seeds --;
                    wm.maria ++;
                    return wm;
                })
                .AddCost(plantMariaCost),

            new GoapAction<WorldModel>(PlayerActionKey.Steal)
                .AddPrecondition(x => x.weapon == WEAPON_KNIFE)
                .AddEffect(x =>
                {
                    var wm = WorldModel.Clone(x);
                    wm.money += stealBounty;
                    return wm;
                })
                .AddCost(stealCost),

            new GoapAction<WorldModel>(PlayerActionKey.SellMaria)
                .AddPrecondition(x => x.maria >= 1)
                .AddEffect(x =>
                {
                    var wm = WorldModel.Clone(x);
                    wm.money += mariaSellPrice;
                    wm.maria --;
                    return wm;
                })
                .AddCost(sellMariaCost),

            new GoapAction<WorldModel>(PlayerActionKey.GetSeed)
                .AddPrecondition(x => x.money >= seedPrice)
                .AddEffect(x =>
                {
                    var wm = WorldModel.Clone(x);
                    wm.seeds ++;
                    wm.money -= seedPrice;
                    return wm;
                })
                .AddCost(getSeedCost),

            new GoapAction<WorldModel>(PlayerActionKey.BuyGun)
                .AddPrecondition(x => x.money >= gunPrice)
                .AddPrecondition(x => x.weapon != WEAPON_GUN)
                .AddEffect(x =>
                {
                    var wm = WorldModel.Clone(x);
                    wm.money -= gunPrice;
                    wm.weapon = WEAPON_GUN;
                    return wm;
                })
                .AddCost(buyGunCost),

            new GoapAction<WorldModel>(PlayerActionKey.DealGob)
                .AddPrecondition(x => x.money >= acordPrice)
                .AddPrecondition(x => x.hasAgreement == false)
                .AddEffect(x =>
                {
                    var wm = WorldModel.Clone(x);
                    wm.money -= acordPrice;
                    wm.hasAgreement = true;
                    return wm;
                })
                .AddCost(gobAgreementCost),

            new GoapAction<WorldModel>(PlayerActionKey.Work)
                .AddPrecondition(x => x.money < seedPrice)
                .AddEffect(x =>
                {
                    var wm = WorldModel.Clone(x);
                    wm.money += salary;
                    return wm;
                })
                .AddCost(gobAgreementCost),

            new GoapAction<WorldModel>(PlayerActionKey.Kidnap)
                .AddPrecondition(x => x.weapon == WEAPON_GUN)
                .AddPrecondition(x => x.hasAgreement == false)
                .AddEffect(x =>
                {
                    var wm = WorldModel.Clone(x);
                    wm.money += kidnapBounty;
                    wm.hasAgreement = true;
                    return wm;
                })
                .AddCost(kidnapCost)

        };

        return listOfActions;
    }

    float Heuristic(WorldModel next, WorldModel current,float cost)
    {
        var result = 10f;

        //El objetivo basicamente es generar suficiente plata como para conseguir el laboratorio
        //El acuerdo se consigue con plata, si bien sea comprando un arma para secuestrar a la hija del presidente y conseguir el acuerdo, o comprando el acuerdo directamente, por eso se traduce a plata
        result += current.hasAgreement ? 0 : gobAgreementCost/100;

        result -= result - current.money/100 > 0 ? (result - current.money)/100: result;
        

        if(next.money == current.money + stealBounty && stealBounty != salary)
        {
            result += 1;
        }

        if (next.isNarco)
            result = 0f;

        return result;
    }

    #region Corutines

    //Paso final!
    IEnumerator GetLab(WorldModel wm)
    {
        _walker.SetDestination(labSpot.position);

        while (!_walker.ReachDestionation)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        //TODO: BUY LAB
        
    }

    IEnumerator Steal(WorldModel wm)
    {
        _walker.SetDestination(labSpot.position);

        while (!_walker.ReachDestionation)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        ExecuteAction();
        //TODO: BUY LAB

    }

    IEnumerator PlantMaria(WorldModel wm)
    {
       _walker.SetDestination(farmSpot.position);

        while (!_walker.ReachDestionation)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        //TODO: Maria plant animation
        ExecuteAction();
    }

    IEnumerator SellMaria(WorldModel wm)
    {
        _walker.SetDestination(sellMariaSpot.position);

        while (!_walker.ReachDestionation)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        //TODO: Sell maria
        ExecuteAction();
    }

    IEnumerator GetSeed(WorldModel wm)
    {
        _walker.SetDestination(getSeedSpot.position);

        while (!_walker.ReachDestionation)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        //TODO: Buy Seeds

        ExecuteAction();
    }

    IEnumerator BuyGun(WorldModel wm)
    {
        _walker.SetDestination(getGunSpot.position);

        while (!_walker.ReachDestionation)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        //TODO: buy weapon

        ExecuteAction();
    }

    IEnumerator DealGob(WorldModel wm)
    {
        _walker.SetDestination(capitolioSpot.position);

        while (!_walker.ReachDestionation)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        //TODO: DEAL WITH GOB
        ExecuteAction();
    }

    IEnumerator Work(WorldModel wm)
    {
        _walker.SetDestination(workSpot.position);

        while (!_walker.ReachDestionation)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        //TODO: Work
        ExecuteAction();
    }

    IEnumerator Kidnap(WorldModel wm)
    {
        _walker.SetDestination(capitolioSpot.position);

        while (!_walker.ReachDestionation)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        //TODO: KIDNAP

        ExecuteAction();
    }
    #endregion

}

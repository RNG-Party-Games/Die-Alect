using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AI : AIPath
{
    public enum AIGoal { Inn, Shop, Smithy, Apartment1, Apartment2, Aimless, Waiting, Death};
    AIGoal goal;
    public float decisionTimer, smithyChance = 0.2f;
    float lastDecisionMade;
    Person p;
    bool atGoal = false;
    float deathTimer = 500;
    protected override void Start() {
        base.Start();
        p = GetComponent<Person>();
        Decide();
    }

    protected override void Update() {
        base.Update();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if (atGoal && Time.time - lastDecisionMade > decisionTimer) {
            Decide();
        }
    }
    public override void OnTargetReached() {
        if(!atGoal) {
            if (goal != AIGoal.Aimless && goal != AIGoal.Waiting && goal != AIGoal.Death) {
                p.Interact(goal);
                goal = AIGoal.Waiting;
            }
            else if(goal == AIGoal.Death) {
                GameInformation.instance.KillPerson(p);
            }
            p.ReachGoal();
            atGoal = true;
        }
    }

    public void FinishInteraction() {
        Decide();
    }

    public void SetDestination(Vector3 newdestination) {
        destination = newdestination;
    }

    public void Decide() {
        if (p.GetClass() != Person.PersonClass.Newborn) {
            int innamt = GameInformation.instance.GetAmountOfPeopleIn(AI.AIGoal.Inn);
            int shopamt = GameInformation.instance.GetAmountOfPeopleIn(AI.AIGoal.Shop);
            int smithyamt = GameInformation.instance.GetAmountOfPeopleIn(AI.AIGoal.Smithy);
            int apt1amt = GameInformation.instance.GetAmountOfPeopleIn(AI.AIGoal.Apartment1);
            int apt2amt = GameInformation.instance.GetAmountOfPeopleIn(AI.AIGoal.Apartment2);
            if(Time.time - p.GetBorn() > deathTimer) {
                goal = AIGoal.Death;
                destination = GameInformation.instance.GetDock();
            }
            else if (innamt > 0 && p.GetEnergy() < 0.33) {
                goal = AIGoal.Inn;
                destination = GameInformation.instance.GetEntrances(0);
            }
            else if (shopamt > 0 && p.GetFood() < 0.33) {
                goal = AIGoal.Shop;
                destination = GameInformation.instance.GetEntrances(2);
            }
            else if (smithyamt > 0 && p.GetProductivity() < 0.33) {
                goal = AIGoal.Smithy;
                destination = GameInformation.instance.GetEntrances(1);
            }
            else if (apt1amt + apt2amt > 0 && p.GetSocial() < 0.33) {
                goal = AIGoal.Apartment1;
                float decisionIndex = Random.Range(0.0f, 1.0f);
                if ((apt1amt > 0 && apt2amt > 0 && decisionIndex < 0.5f) || (apt1amt > 0)) {
                    destination = GameInformation.instance.GetEntrances(3);
                }
                else {
                    destination = GameInformation.instance.GetEntrances(4);
                }
            }
            else {
                float decisionIndex = Random.Range(0.0f, 1.0f);
                if (decisionIndex <= 0.5f) {
                    goal = AIGoal.Aimless;
                    destination = GameInformation.instance.GetWalkablePoint();
                }
                else {
                    goal = AIGoal.Waiting;
                    destination = transform.position;
                }
            }
            lastDecisionMade = Time.time;
            decisionTimer = Random.Range(5.0f, 15.0f);
            atGoal = false;
            SearchPath();
        }
    }
}

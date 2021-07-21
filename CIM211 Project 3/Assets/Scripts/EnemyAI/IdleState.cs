using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class IdleState : State
    {
        public IdleState(Enemy enemy) : base(enemy) { }

        private float timer;

        public override void Tick()
        {
            if(timer < enemy.stateWaitTime)
            {
                timer += Time.deltaTime;
                //Debug.Log(timer);
            }
            else
            {
                enemy.SetState(new RomingState(enemy));
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entering Idle State");

            enemy.navAgent.SetDestination(enemy.transform.position);
            timer = 0;
        }

        public override void OnStateExit()
        {
            Debug.Log("Exiting Idle State");
        }
    }
}

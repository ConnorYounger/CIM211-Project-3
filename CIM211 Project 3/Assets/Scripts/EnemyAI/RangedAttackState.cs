using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class RangedAttackState : State
    {
        public RangedAttackState(Enemy enemy) : base(enemy) { }

        public override void Tick()
        {
            
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entering Roming State");
            
        }

        public override void OnStateExit()
        {
            Debug.Log("Exiting Roming State");
        }
    }
}

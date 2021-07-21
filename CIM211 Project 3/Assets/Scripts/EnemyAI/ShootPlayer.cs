using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class ShootPlayer : State
    {
        public ShootPlayer(Enemy enemy) : base(enemy) { }

        public override void Tick()
        {

        }

        public override void OnStateEnter()
        {
            Debug.Log("Entering Attack State");

            enemy.navAgent.SetDestination(enemy.player.transform.position);
        }

        public override void OnStateExit()
        {
            Debug.Log("Entering Attack State");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class EngagePlayerState : State
    {
        public EngagePlayerState(Enemy enemy) : base(enemy) { }

        private bool hasReachedPlayer;

        private float destinationPointDistance = 1f;
        private float coolDownTimer;

        public override void Tick()
        {
            if(!hasReachedPlayer && Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < destinationPointDistance)
            {
                ReachedPlayer();
            }

            EngageCoolDown();
        }

        void ReachedPlayer()
        {
            coolDownTimer = enemy.idleCoolDownTime;

            hasReachedPlayer = true;

            enemy.navAgent.SetDestination(enemy.transform.position);

            Debug.DrawLine(enemy.transform.position, enemy.player.transform.position, Color.cyan);
        }

        void EngageCoolDown()
        {
            if (hasReachedPlayer)
            {
                if (coolDownTimer > 0)
                    coolDownTimer -= 1 * Time.deltaTime;
                else
                    enemy.SetState(new RomingState(enemy));
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entering Attack State");

            enemy.navAgent.SetDestination(enemy.player.transform.position);
        }

        public override void OnStateExit()
        {
            Debug.Log("Entering Attack State");
            hasReachedPlayer = false;
        }
    }
}

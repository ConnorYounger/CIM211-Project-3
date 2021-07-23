using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class MeleeAttackState : State
    {
        public MeleeAttackState(Enemy enemy) : base(enemy) { }

        public float attackDistance = 1;

        public float attackCoolDown = 1;
        private float coolDownTimer;
        private bool canAttack = true;

        private float collisionTimer;
        private int attackStage;

        public override void Tick()
        {
            ChasePlayer();
            AttackPlayer();
            CoolDownTimer();
            AttackStages();
        }

        void AttackStages()
        {
            if (attackStage > 0) {
                if (collisionTimer > 0)
                {
                    collisionTimer -= Time.deltaTime;
                }
                else
                {
                    if(attackStage == 1)
                    {
                        enemy.meleeHitCollider.SetActive(true);
                        collisionTimer = 0.4f;
                        attackStage = 2;
                    }
                    else if(attackStage == 2)
                    {
                        enemy.meleeHitCollider.SetActive(false);
                        attackStage = 0;
                    }
                }
            }
        }

        void CoolDownTimer()
        {
            if (!canAttack && coolDownTimer > 0)
                coolDownTimer -= Time.deltaTime;

            if (!canAttack && coolDownTimer <= 0)
                canAttack = true;
        }

        void ChasePlayer()
        {
            if(Vector3.Distance(enemy.transform.position, enemy.player.transform.position) > attackDistance)
            {
                if(!enemy.navAgent.enabled)
                    enemy.navAgent.enabled = true;

                enemy.navAgent.SetDestination(enemy.player.transform.position);
            }
            else
            {
                if (enemy.navAgent.enabled)
                    enemy.navAgent.enabled = false;
            }
        }

        void AttackPlayer()
        {
            if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < attackDistance)
            {
                if (canAttack)
                {
                    canAttack = false;
                    coolDownTimer = attackCoolDown;

                    attackStage = 1;
                    collisionTimer = 0.2f;

                    //play attack animation
                }
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entering MeleeAttack State");
            canAttack = true;
            attackStage = 0;
            collisionTimer = 0;
            coolDownTimer = 0;
        }

        public override void OnStateExit()
        {
            Debug.Log("Exiting MeleeAttack State");
        }
    }
}

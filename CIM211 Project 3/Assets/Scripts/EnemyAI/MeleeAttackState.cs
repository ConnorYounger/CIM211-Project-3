using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StatePattern
{
    public class MeleeAttackState : State
    {
        public MeleeAttackState(Enemy enemy) : base(enemy) { }

        public float attackDistance = 1.5f;

        public float attackCoolDown = 1;
        private float coolDownTimer;
        private bool canAttack = true;

        private float collisionTimer;
        private int attackStage;

        private float targetPointTime = 1;
        private float targetPointTimer;
        private float extraRotationSpeed = 10;

        public override void Tick()
        {
            ChasePlayer();
            AttackPlayer();
            CoolDownTimer();
            AttackStages();

            PointTimer();

            if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < attackDistance)
                extraRotation();
        }

        void extraRotation()
        {
            Vector3 lookrotation = enemy.navAgent.steeringTarget - enemy.transform.position;
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);
        }

        void PointTimer()
        {
            if(targetPointTimer > 0)
            {
                targetPointTimer -= Time.deltaTime;
            }
            else
            {
                targetPointTimer = targetPointTime;
                SetTargetPoint();
            }
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

                //PointTimer();

                //enemy.navAgent.SetDestination(enemy.player.transform.position);
            }
            else
            {
                if (enemy.navAgent.enabled)
                {
                    //enemy.navAgent.enabled = false;
                }
            }
        }

        void SetTargetPoint()
        {
            if(Vector3.Distance(enemy.transform.position, enemy.player.transform.position) > attackDistance)
                enemy.navAgent.SetDestination(RandomNavmeshLocation(3));
            else
                enemy.navAgent.SetDestination(RandomNavmeshLocation(0));
        }

        public Vector3 RandomNavmeshLocation(float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += enemy.player.transform.position;

            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;

            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
            }

            return finalPosition;
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

            enemy.PlayAlertSound();
        }

        public override void OnStateExit()
        {
            Debug.Log("Exiting MeleeAttack State");
        }
    }
}

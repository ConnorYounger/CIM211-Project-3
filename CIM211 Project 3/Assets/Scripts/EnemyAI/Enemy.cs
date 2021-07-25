using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StatePattern
{
    public class Enemy : MonoBehaviour
    {
        public GameObject player;

        public GameObject wanderPointCollection;
        public GameObject idlePointCollection;

        [Header("Animation")]
        public Animator animator;
        public GameObject meleeHitCollider;

        [Header("Enemy Stats")]
        public InvDeadBody inventory;
        public float meleeDamage = 10;
        public float rangedDamage;
        public EnemyHealth enemyHealth;
        
        [Header("Weapon Refrences")]
        public Transform leftArmShootPoint;
        public Transform rightArmShootPoint;
        public Transform aimPoint;

        public GameObject projectile;
        public GameObject grenade;

        [Header("State Stats")]
        public float enemyVisionDistance = 10;
        public float stateWaitTime = 10;
        public float idleCoolDownTime = 10;

        public float travelTime = 7;
        public int travelChance = 10;

        public float numbOfAlerts = 2;

        public bool hasFoundPlayer;

        private State currentState;

        public NavMeshAgent navAgent;

        private void Start()
        {
            Debug.Log("Startt");
            player = GameObject.Find("Player");

            currentState = new IdleState(this);

            navAgent = gameObject.GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            currentState.Tick();
        }

        public void SetStats()
        {
            enemyVisionDistance *= 1 + inventory.eyes.vision / 2;

            enemyHealth.maxHealth += inventory.body.maxHealthMultiplier + inventory.head.maxHealthMultiplier;

            travelTime *= 1.0f + (inventory.lungs.maxStaminaMultiplier / 100f);

            travelChance = (int)Mathf.Round(inventory.brain.brain);

            navAgent.speed *= 1 + (inventory.leftLeg.movementSpeedMultiplier / 4) + (inventory.rightLeg.movementSpeedMultiplier / 4);

            numbOfAlerts *= Mathf.Round(inventory.brain.brain);
        }

        public void SetState(State state)
        {
            currentState.OnStateExit();
            currentState = state;
            currentState.OnStateEnter();
        }

        public void DealMeleeDamage()
        {
            player.GetComponent<PlayerHealth>().TakeDamage(meleeDamage);
        }
    }
}

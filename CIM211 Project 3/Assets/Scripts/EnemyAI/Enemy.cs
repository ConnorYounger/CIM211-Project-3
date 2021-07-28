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

        public GameObject destroyFx;

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
            Debug.Log("Set Enemy Stats");

            if(inventory.eyes != null)
                enemyVisionDistance *= 1 + inventory.eyes.vision / 2;

            if (inventory.body != null)
                enemyHealth.maxHealth += inventory.body.maxHealthMultiplier;
            if (inventory.head != null)
                enemyHealth.maxHealth += inventory.head.maxHealthMultiplier;

            if (inventory.heart != null)
                enemyHealth.healthRegenMultiplier += inventory.heart.autoHealMultilpier;

            if (inventory.lungs != null)
                travelTime *= 1.0f + (inventory.lungs.maxStaminaMultiplier / 100f);

            if (inventory.brain != null)
            {
                travelChance -= (int)Mathf.Round(inventory.brain.brain);
                numbOfAlerts *= Mathf.Round(inventory.brain.brain);
            }

            if (inventory.leftLeg != null)
                navAgent.speed *= 1 + (inventory.leftLeg.movementSpeedMultiplier / 4);
            if (inventory.rightLeg != null)
                navAgent.speed *= 1 + (inventory.rightLeg.movementSpeedMultiplier / 4);

            if ((inventory.leftArm && inventory.leftArm.leftArmWeaponCode > 2) || (inventory.rightArm && inventory.rightArm.rightArmWeaponCode > 2))
            {
                rangedDamage = 10;
            }
        }

        public void OnDisable()
        {
            if (destroyFx)
            {
                GameObject fx = Instantiate(destroyFx, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
                Destroy(fx, 5);
            }

            if (meleeHitCollider)
                meleeHitCollider.SetActive(false);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StatePattern
{
    public class Enemy : MonoBehaviour
    {
        public GameObject player;

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
        public GameObject spine;

        public GameObject projectile;
        public GameObject grenade;

        public AudioSource laudioSource;
        public AudioSource raudioSource;

        public AudioClip projectileSound;
        public AudioClip grenadeSound;

        [Header("State Stats")]
        public int currentWave;

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

        [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip[] alertSound;
        public AudioClip[] footStepSounds;
        public float footStepIntival;

        private void Start()
        {
            //Debug.Log("Startt");
            player = GameObject.Find("Player");

            currentState = new IdleState(this);

            navAgent = gameObject.GetComponent<NavMeshAgent>();

            if (!audioSource)
                gameObject.GetComponent<AudioSource>();

            audioSource.volume = PlayerPrefs.GetFloat("audioVolume");

            StartCoroutine("FootStepSounds");
        }

        IEnumerator FootStepSounds()
        {
            yield return new WaitForSeconds(footStepIntival);

            if (animator.GetBool("isWalking") && !enemyHealth.isDead)
            {
                int rand = Random.Range(0, footStepSounds.Length);
                PlaySound(footStepSounds[rand]);
            }

            StartCoroutine("FootStepSounds");
        }

        public void PlaySound(AudioClip sound)
        {
            //GameObject soundOb = Instantiate(new GameObject(), transform.position, transform.rotation);
            //AudioSource aSource = soundOb.AddComponent<AudioSource>();

            //aSource.volume = PlayerPrefs.GetFloat("audioVolume") / 2;
            //aSource.spatialBlend = 1;
            //aSource.maxDistance = 100;
            //aSource.clip = sound;
            //aSource.Play();

            //Destroy(soundOb, sound.length);

            if(enemyHealth.spawManager)
                enemyHealth.spawManager.PlayFootStepSound(sound, transform);
        }

        private void Update()
        {
            currentState.Tick();
        }

        public void SetStats()
        {
            //Debug.Log("Set Enemy Stats");

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

            if (inventory.leftLeg != null && navAgent != null)
                navAgent.speed *= 1 + (inventory.leftLeg.movementSpeedMultiplier / 4);
            if (inventory.rightLeg != null && navAgent != null)
                navAgent.speed *= 1 + (inventory.rightLeg.movementSpeedMultiplier / 4);

            if (inventory.leftArm && inventory.leftArm.leftArmWeaponCode > 2)
            {
                animator.SetBool("leftArm", true);
                rangedDamage = 10;
            }

            if(inventory.rightArm && inventory.rightArm.rightArmWeaponCode > 2)
            {
                animator.SetBool("rightArm", true);
                rangedDamage = 10;
            }

            // Set melee damage
            if (currentWave >= 4)
            {
                meleeDamage = 10;
            }
            else if(inventory.leftArm)
            {
                if(inventory.leftArm.leftArmWeaponCode == 1)
                {
                    meleeDamage = 10;
                }
                else if(inventory.leftArm.leftArmWeaponCode == 2)
                {
                    meleeDamage = 15;
                }
            }
            else if(inventory.rightArm)
            {
                if(inventory.rightArm.rightArmWeaponCode == 1)
                {
                    meleeDamage = 10;
                }
                else if(inventory.rightArm.rightArmWeaponCode == 2)
                {
                    meleeDamage = 15;
                }
            }
            else
            {
                meleeDamage = 5;
            }

            enemyHealth.currentHealth = enemyHealth.maxHealth;
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

        public void PlayAlertSound()
        {
            int rand = Random.Range(0, alertSound.Length);

            audioSource.clip = alertSound[rand];
            audioSource.Play();
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

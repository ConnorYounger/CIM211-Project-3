using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StatePattern
{
    public class RangedAttackState : State
    {
        public RangedAttackState(Enemy enemy) : base(enemy) { }

        private bool hasLeftWeapon;
        private bool hasRightWeapon;

        public float leftArmFireRate;
        public float rightArmFireRate;

        public float lFireRateTimer;
        public float RFireRateTimer;

        public float bulletSpread = 0.05f;

        public float followDistance = 10;
        public float pointRadius = 10;

        public GameObject lProjectile;
        public GameObject rProjectile;

        private bool moveTowardsPlayer;
        private float moveCoolDownTime = 5;
        private float moveTimer;

        // Melee
        private float meleeAttackRange = 2;
        private int attackStage;
        private float collisionTimer;
        private float coolDownTimer;
        private float attackCoolDown = 1;
        private bool canMeleeAttack = true;

        private Vector3 targetPoint;

        public override void Tick()
        {
            ShootAtPlayer();
            FireRateCoolDowns();
            SteerTowardsPlayer();
            FollowPlayer();
            MoveCoolDownTimer();
            CheckForMeleeAttack();
            MeleeAttackStages();
            CoolDownTimer();
        }

        void CheckForMeleeAttack()
        {
            if (MeleeDistanceCheck() && canMeleeAttack && (!hasLeftWeapon || !hasRightWeapon))
            {
                canMeleeAttack = false;
                coolDownTimer = attackCoolDown;

                attackStage = 1;
                collisionTimer = 0.2f;

                enemy.animator.SetBool("meleeAttack", true);
                //play attack animation
            }
        }

        void CoolDownTimer()
        {
            if (!canMeleeAttack && coolDownTimer > 0)
                coolDownTimer -= Time.deltaTime;

            if (!canMeleeAttack && coolDownTimer <= 0)
                canMeleeAttack = true;
        }

        void MeleeAttackStages()
        {
            if (attackStage > 0)
            {
                if (collisionTimer > 0)
                {
                    collisionTimer -= Time.deltaTime;
                }
                else
                {
                    if (attackStage == 1)
                    {
                        enemy.meleeHitCollider.SetActive(true);
                        collisionTimer = 0.4f;
                        attackStage = 2;
                    }
                    else if (attackStage == 2)
                    {
                        enemy.meleeHitCollider.SetActive(false);
                        enemy.animator.SetBool("meleeAttack", false);
                        attackStage = 0;
                    }
                }
            }
        }

        bool MeleeDistanceCheck()
        {
            if (Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < meleeAttackRange)
                return true;
            else
                return false;
        }

        void MoveCoolDownTimer()
        {
            if (!moveTowardsPlayer && moveTimer > 0)
                moveTimer -= Time.deltaTime;
            else
                moveTowardsPlayer = true;

            if (Vector3.Distance(enemy.transform.position, targetPoint) < 1 && attackStage == 0)
            {
                if (enemy.animator)
                {
                    enemy.animator.SetBool("isWalking", false);
                }
            }
            else
            {
                if (enemy.animator)
                {
                    enemy.animator.SetBool("isWalking", true);
                }
            }
        }

        void FollowPlayer()
        {
            if(moveTowardsPlayer)
            {
                moveTimer = moveCoolDownTime;
                moveTowardsPlayer = false;

                targetPoint = RandomNavmeshLocation(pointRadius);
                enemy.navAgent.SetDestination(targetPoint);
            }
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

        void SteerTowardsPlayer()
        {
            float damping = 2;
            Vector3 lookPos = enemy.player.transform.position - enemy.transform.position;
            lookPos.y = 0;

            var rotation = (Quaternion.LookRotation(lookPos));
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, Time.deltaTime * damping);

            AimAtPlayer();
        }

        void AimAtPlayer()
        {
            float damping = 2;
            Vector3 lookPos = enemy.player.transform.position - enemy.aimPoint.transform.position;

            var rotation = (Quaternion.LookRotation(lookPos));
            enemy.aimPoint.transform.rotation = Quaternion.Slerp(enemy.aimPoint.transform.rotation, rotation, Time.deltaTime * damping);

            if (enemy.spine)
            {
                //float d = 2;
                //Vector3 l = enemy.player.transform.position - enemy.spine.transform.position;

                //Quaternion p = enemy.spine.transform.localRotation;
                //Quaternion q = new Quaternion(p.x, 0, p.z, p.w);

                //var r = (Quaternion.LookRotation(l));
                //enemy.spine.transform.localRotation = Quaternion.Slerp(q, r, Time.deltaTime * d);

                //Transform look = enemy.spine.transform;
                //look.LookAt(enemy.player.transform.position);
                //Vector3 v = new Vector3(look.rotation.x, 0, look.rotation.z);

                //Quaternion p = enemy.spine.transform.rotation;
                //Quaternion q = new Quaternion(p.x + 90, p.y, p.z, p.w);

                //var r2 = (Quaternion.LookRotation(v));
                //enemy.spine.transform.localRotation = Quaternion.Slerp(q, r2, Time.deltaTime * 2); ;
            }
        }

        public float CalculateRotationValue(float value)
        {
            return (value / 360) * 30000;
        }

        void FireRateCoolDowns()
        {
            if(lFireRateTimer > 0)
                lFireRateTimer -= Time.deltaTime;

            if (RFireRateTimer > 0)
                RFireRateTimer -= Time.deltaTime;
        }

        void ShootAtPlayer()
        {
            if (CanSeePlayer())
            {
                if (hasLeftWeapon)
                {
                    if (lFireRateTimer <= 0)
                    {
                        LeftArmShoot();
                    }
                }

                if (hasRightWeapon)
                {
                    if (RFireRateTimer <= 0)
                    {
                        RightArmShoot();
                    }
                }
            }
        }

        void LeftArmShoot()
        {
            float rand = Random.Range(-(leftArmFireRate/3), (leftArmFireRate / 3));
            lFireRateTimer = rand + leftArmFireRate;
            enemy.laudioSource.Play();

            FireProjectile(lProjectile, enemy.leftArmShootPoint);
        }

        void RightArmShoot()
        {
            float rand = Random.Range(-(rightArmFireRate / 3), (rightArmFireRate / 3));
            RFireRateTimer = rand + rightArmFireRate;
            enemy.raudioSource.Play();

            FireProjectile(rProjectile, enemy.rightArmShootPoint);
        }

        void FireProjectile(GameObject projectile, Transform shootPoint)
        {
            RaycastHit hit;
            Vector3 shootDir = enemy.aimPoint.forward;
            shootDir.x += Random.Range(-bulletSpread, bulletSpread);
            shootDir.y += Random.Range(-bulletSpread, bulletSpread);

            Physics.Raycast(enemy.aimPoint.position, shootDir, out hit, 1000, ~LayerMask.GetMask("Vision"));

            GameObject bullet = GameObject.Instantiate(projectile, shootPoint.position, shootPoint.rotation);

            if (hit.collider != null)
                bullet.transform.LookAt(hit.point);

            bullet.layer = 11;
            bullet.GetComponent<Projectile>().speed = 30;
            bullet.GetComponent<Projectile>().damage = 10;

            GameObject.Destroy(bullet, 10);
        }

        bool CanSeePlayer()
        {
            RaycastHit hit = CalculateHit(enemy.transform.position, enemy.player.transform.position, ~LayerMask.GetMask("Vision"));

            if (hit.collider.name == "Player")
                return true;
            else
                return false;
        }

        RaycastHit CalculateHit(Vector3 start, Vector3 end, int layerMask)
        {
            RaycastHit hit;
            Vector3 rayAngle = (end - start).normalized;

            Physics.Raycast(start, rayAngle, out hit, 100, layerMask);
            return hit;
        }

        void SetWeaponStats()
        {
            // left Arm
            if (enemy.inventory.leftArm != null && enemy.inventory.leftArm.leftArmWeaponCode > 2)
            {
                if (enemy.inventory.leftArm.leftArmWeaponCode == 3)
                {
                    leftArmFireRate = 2;

                    lProjectile = enemy.projectile;
                    enemy.laudioSource.clip = enemy.projectileSound;
                }
                else if (enemy.inventory.leftArm.leftArmWeaponCode == 4)
                {
                    leftArmFireRate = 0.4f;

                    lProjectile = enemy.projectile;
                    enemy.laudioSource.clip = enemy.projectileSound;
                }
                else if (enemy.inventory.leftArm.leftArmWeaponCode == 5)
                {
                    leftArmFireRate = 0.7f;

                    lProjectile = enemy.grenade;
                    enemy.laudioSource.clip = enemy.grenadeSound;
                }

                hasLeftWeapon = true;
                lFireRateTimer = leftArmFireRate;
            }

            // Right Arm
            if (enemy.inventory.rightArm != null && enemy.inventory.rightArm.rightArmWeaponCode > 2)
            {
                if (enemy.inventory.rightArm.rightArmWeaponCode == 3)
                {
                    rightArmFireRate = 2;

                    rProjectile = enemy.projectile;
                    enemy.raudioSource.clip = enemy.projectileSound;
                }
                else if (enemy.inventory.rightArm.rightArmWeaponCode == 4)
                {
                    rightArmFireRate = 0.4f;

                    rProjectile = enemy.projectile;
                    enemy.raudioSource.clip = enemy.projectileSound;
                }
                else if (enemy.inventory.rightArm.rightArmWeaponCode == 5)
                {
                    rightArmFireRate = 0.7f;

                    rProjectile = enemy.grenade;
                    enemy.raudioSource.clip = enemy.grenadeSound;
                }

                hasRightWeapon = true;
                RFireRateTimer = rightArmFireRate;
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entering RangedAttack State");
            SetWeaponStats();

            enemy.PlayAlertSound();

            if (enemy.inventory.leftArm && enemy.inventory.leftArm.leftArmWeaponCode > 2)
            {
                enemy.animator.SetBool("leftArm", true);
            }

            if (enemy.inventory.rightArm && enemy.inventory.rightArm.rightArmWeaponCode > 2)
            {
                enemy.animator.SetBool("rightArm", true);
            }
        }

        public override void OnStateExit()
        {
            Debug.Log("Exiting RangedAttack State");
        }
    }
}

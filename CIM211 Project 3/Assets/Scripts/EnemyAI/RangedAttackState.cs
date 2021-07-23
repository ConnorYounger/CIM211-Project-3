using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public GameObject lProjectile;
        public GameObject rProjectile;

        public override void Tick()
        {
            ShootAtPlayer();
            FireRateCoolDowns();
            SteerTowardsPlayer();
        }

        void SteerTowardsPlayer()
        {
            float damping = 2;
            Vector3 lookPos = enemy.player.transform.position - enemy.transform.position;
            lookPos.y = 0;

            var rotation = (Quaternion.LookRotation(lookPos));
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, Time.deltaTime * damping);
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
            lFireRateTimer = leftArmFireRate;

            FireProjectile(lProjectile, enemy.leftArmShootPoint);
        }

        void RightArmShoot()
        {
            RFireRateTimer = rightArmFireRate;

            FireProjectile(rProjectile, enemy.rightArmShootPoint);
        }

        void FireProjectile(GameObject projectile, Transform shootPoint)
        {
            //GameObject bullet = GameObject.Instantiate(projectile, shootPoint.position, shootPoint.rotation);

            RaycastHit hit;
            Vector3 shootDir = shootPoint.forward;
            shootDir.x += Random.Range(-bulletSpread, bulletSpread);
            shootDir.y += Random.Range(-bulletSpread, bulletSpread);

            Physics.Raycast(shootPoint.position, shootDir, out hit, 1000, ~LayerMask.GetMask("Player"));

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
                }
                else if (enemy.inventory.leftArm.leftArmWeaponCode == 4)
                {
                    leftArmFireRate = 0.7f;

                    lProjectile = enemy.projectile;
                }
                else if (enemy.inventory.leftArm.leftArmWeaponCode == 5)
                {
                    leftArmFireRate = 0.7f;

                    lProjectile = enemy.grenade;
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
                }
                else if (enemy.inventory.rightArm.rightArmWeaponCode == 4)
                {
                    rightArmFireRate = 0.7f;

                    rProjectile = enemy.projectile;
                }
                else if (enemy.inventory.rightArm.rightArmWeaponCode == 5)
                {
                    rightArmFireRate = 0.7f;

                    rProjectile = enemy.grenade;
                }

                hasRightWeapon = true;
                RFireRateTimer = rightArmFireRate;
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entering RangedAttack State");
            SetWeaponStats();
        }

        public override void OnStateExit()
        {
            Debug.Log("Exiting RangedAttack State");
        }
    }
}

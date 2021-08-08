using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StatePattern
{
    public class RomingState : State
    {
        public RomingState(Enemy enemy) : base(enemy) { }

        private List<Vector3> debugLines;

        public float pointSearchRange = 10;

        private GameObject centerPoint;
        private List<Transform> closeByPoints;
        private Vector3 targetPoint;

        private float pointDistance = 2;
        private float idleTime = 5;
        private float idleTimer;

        private float traveltime = 5;
        private float traveltimer;

        private bool searchingForIdlePoint;
        private bool roming;

        public override void Tick()
        {
            if (roming)
            {
                DrawDebugLines();
                SearchForNewIdlePoint();
                MoveToIdlePoint();
                IdleTimer();
                TravelTimer();
                SearchForPlayer();
            }
        }

        void SearchForPlayer()
        {
            if (enemy.hasFoundPlayer)
            {
                //AlertNearByEnemies();

                if (enemy.rangedDamage > 0)
                {
                    enemy.SetState(new RangedAttackState(enemy));
                }
                else
                {
                    enemy.SetState(new MeleeAttackState(enemy));
                }
            }
        }

        void AlertNearByEnemies()
        {
            Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, enemy.enemyVisionDistance);

            int alertedEnemies = 0;

            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<Enemy>() && alertedEnemies < enemy.numbOfAlerts)
                {
                    collider.GetComponent<Enemy>().hasFoundPlayer = true;
                    alertedEnemies++;
                }
            }
        }

        void SearchForNewIdlePoint()
        {
            if (idleTimer <= 0 && searchingForIdlePoint)
            {
                int trv = Random.Range(0, enemy.travelChance);

                if (trv == 0)
                    enemy.SetState(new TravelState(enemy));
                else
                {
                    //if (closeByPoints.Count > 0)
                    //{
                    //    debugLines.Clear();
                    //    int rand = Random.Range(0, closeByPoints.Count);

                    //    targetPoint = closeByPoints[rand];
                    //    debugLines.Add(targetPoint.position);
                    //}
                    //else
                    //{
                    //    Debug.LogError("No close by points");
                    //}

                    //if (targetPoint != null)
                    //{
                    //    traveltimer = traveltime;
                    //    searchingForIdlePoint = false;
                    //    enemy.navAgent.enabled = true;
                    //    enemy.navAgent.SetDestination(targetPoint.position);
                    //}

                    traveltimer = traveltime;
                    searchingForIdlePoint = false;
                    enemy.navAgent.enabled = true;
                    enemy.navAgent.SetDestination(RandomNavmeshLocation(pointSearchRange));
                }
            }
        }

        public Vector3 RandomNavmeshLocation(float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += centerPoint.transform.position;

            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;

            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
            }

            targetPoint = finalPosition;

            return finalPosition;
        }

        void TravelTimer()
        {
            if (traveltimer > 0)
                traveltimer -= Time.deltaTime;

            if(traveltimer <= 0 && !searchingForIdlePoint && idleTimer <= 0)
            {
                GoIdle();
            }
        }

        void MoveToIdlePoint()
        {
            if(idleTimer <= 0 && !searchingForIdlePoint && targetPoint != null)
            {
                if(Vector3.Distance(enemy.transform.position, targetPoint) < pointDistance)
                {
                    GoIdle();
                }
            }

            if (targetPoint != null)
            {
                //Debug.Log(Vector3.Distance(enemy.transform.position, targetPoint) < pointDistance / 2);

                if (Vector3.Distance(enemy.transform.position, targetPoint) < pointDistance / 2)
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
            else
            {
                Debug.Log("No targetPoint");
            }

            // ANIMATOR: PLAY WALKING
        }

        void GoIdle()
        {
            //enemy.navAgent.enabled = false;
            idleTimer = idleTime;
            searchingForIdlePoint = true;

            Debug.Log("Idle");

            if (enemy.animator)
            {
                enemy.animator.SetBool("isWalking", false);
            }
            // ANIMATOR: PLAY IDLE
        }

        void IdleTimer()
        {
            if (idleTimer > 0)
                idleTimer -= Time.deltaTime;
        }

        void SetCloseByPoints()
        {
            closeByPoints.Clear();

            Collider[] colliders = Physics.OverlapSphere(centerPoint.transform.position, pointSearchRange);

            foreach(Collider collider in colliders)
            {

                if(collider.tag == "IdlePoint")
                {
                    closeByPoints.Add(collider.transform);
                }
            }

            //if (closeByPoints.Count > 0)
            //{
            //    foreach (Transform point in closeByPoints)
            //    {
            //        Debug.Log(point);
            //    }
            //}
        }

        void DrawDebugLines()
        {
            if(debugLines.Count > 0)
            {
                foreach(Vector3 transform in debugLines)
                {
                    Debug.DrawLine(enemy.transform.position, transform, Color.red);
                }
            }
        }

        RaycastHit CalculateHit(Vector3 start, Vector3 end)
        {
            RaycastHit hit;
            Vector3 rayAngle = (end - start).normalized;

            Physics.Raycast(start, rayAngle, out hit);
            return hit;
        }

        public override void OnStateEnter()
        {
            roming = true;

            Debug.Log("Entering Roming State");
            closeByPoints = new List<Transform>();
            debugLines = new List<Vector3>();

            idleTimer = 0;

            if(centerPoint == null)
            {
                centerPoint = new GameObject();
            }

            centerPoint.transform.position = enemy.gameObject.transform.position;

            //SetCloseByPoints();
            searchingForIdlePoint = true;
        }

        public override void OnStateExit()
        {
            roming = false;

            Debug.Log("Exiting Roming State");

            targetPoint = new Vector3();
            searchingForIdlePoint = false;
            enemy.navAgent.enabled = true;
            GameObject.Destroy(centerPoint);
        }
    }
}

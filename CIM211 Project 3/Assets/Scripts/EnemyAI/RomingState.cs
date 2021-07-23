using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class RomingState : State
    {
        public RomingState(Enemy enemy) : base(enemy) { }

        private List<Vector3> debugLines;

        public float pointSearchRange = 10;

        private GameObject centerPoint;
        private List<Transform> closeByPoints;
        private Transform targetPoint;

        private float pointDistance = 2;
        private float idleTime = 5;
        private float idleTimer;

        private float traveltime = 5;
        private float traveltimer;

        private bool searchingForIdlePoint;

        public override void Tick()
        {
            DrawDebugLines();
            SearchForNewIdlePoint();
            MoveToIdlePoint();
            IdleTimer();
            TravelTimer();
            SearchForPlayer();
        }

        void SearchForPlayer()
        {
            if (enemy.hasFoundPlayer)
            {
                if(enemy.rangedDamage > 0)
                {
                    enemy.SetState(new RangedAttackState(enemy));
                }
                else
                {
                    enemy.SetState(new MeleeAttackState(enemy));
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
                    if (closeByPoints.Count > 0)
                    {
                        debugLines.Clear();
                        int rand = Random.Range(0, closeByPoints.Count);

                        targetPoint = closeByPoints[rand];
                        debugLines.Add(targetPoint.position);
                    }
                    else
                    {
                        Debug.LogError("No close by points");
                    }

                    if (targetPoint != null)
                    {
                        traveltimer = traveltime;
                        searchingForIdlePoint = false;
                        enemy.navAgent.enabled = true;
                        enemy.navAgent.SetDestination(targetPoint.position);
                    }
                }
            }
        }

        void TravelTimer()
        {
            if (traveltimer > 0)
                traveltimer -= Time.deltaTime;

            if(traveltimer <= 0 && !searchingForIdlePoint && idleTimer <= 0 && targetPoint)
            {
                GoIdle();
            }
        }

        void MoveToIdlePoint()
        {
            if(idleTimer <= 0 && !searchingForIdlePoint && targetPoint != null)
            {
                if(Vector3.Distance(enemy.transform.position, targetPoint.transform.position) < pointDistance)
                {
                    GoIdle();
                }
            }
        }

        void GoIdle()
        {
            enemy.navAgent.enabled = false;
            idleTimer = idleTime;
            searchingForIdlePoint = true;
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
            Debug.Log("Entering Roming State");
            closeByPoints = new List<Transform>();
            debugLines = new List<Vector3>();

            idleTimer = 0;

            if(centerPoint == null)
            {
                centerPoint = new GameObject();
            }

            centerPoint.transform.position = enemy.gameObject.transform.position;

            SetCloseByPoints();
            searchingForIdlePoint = true;
        }

        public override void OnStateExit()
        {
            Debug.Log("Exiting Roming State");

            targetPoint = null;
            searchingForIdlePoint = false;
            enemy.navAgent.enabled = true;
            GameObject.Destroy(centerPoint);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StatePattern
{
    public class TravelState : State
    {
        public TravelState(Enemy enemy) : base(enemy) { }

        private List<Vector3> debugLines;

        private GameObject centerPoint;
        private List<Transform> closeByPoints;
        private Transform targetPoint;

        public float pointSearchRange = 20;
        public float travelTimer;

        private bool isTraveling;

        public override void Tick()
        {
            Travel();
            DrawDebugLines();
            SearchForPlayer();
        }

        void SearchForPlayer()
        {
            if (enemy.hasFoundPlayer)
            {
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

        void Travel()
        {
            if(travelTimer > 0)
            {
                travelTimer -= Time.deltaTime;
            }

            if(travelTimer <= 0 && isTraveling)
            {
                enemy.SetState(new RomingState(enemy));
            }
        }

        void SetCloseByPoints()
        {
            //closeByPoints.Clear();

            //Collider[] colliders = Physics.OverlapSphere(centerPoint.transform.position, pointSearchRange);

            //foreach (Collider collider in colliders)
            //{
            //    if (collider.tag == "IdlePoint")
            //    {
            //        closeByPoints.Add(collider.transform);
            //    }
            //}

            //if(closeByPoints.Count > 0)
            //{
            //    int rand = Random.Range(0, closeByPoints.Count);
            //    targetPoint = closeByPoints[rand];
            //    enemy.navAgent.enabled = true;
            //    enemy.navAgent.SetDestination(targetPoint.position);

            //    debugLines.Clear();
            //    debugLines.Add(targetPoint.position);
            //}
            //else
            //{
            //    enemy.SetState(new RomingState(enemy));
            //}

            Debug.Log("Search for a point");

            enemy.navAgent.enabled = true;
            enemy.navAgent.SetDestination(RandomNavmeshLocation(pointSearchRange));
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

            Debug.Log("random point");

            return finalPosition;
        }

        void DrawDebugLines()
        {
            if (debugLines.Count > 0)
            {
                foreach (Vector3 transform in debugLines)
                {
                    Debug.DrawLine(enemy.transform.position, transform, Color.green);
                }
            }
        }

        public override void OnStateEnter()
        {
            Debug.Log("Entering Travel State");
            closeByPoints = new List<Transform>();
            debugLines = new List<Vector3>();

            travelTimer = enemy.travelTime;
            isTraveling = true;

            if (centerPoint == null)
            {
                centerPoint = new GameObject();
            }

            centerPoint.transform.position = enemy.player.transform.position;

            SetCloseByPoints();
        }

        public override void OnStateExit()
        {
            Debug.Log("Exiting Travel State");

            isTraveling = false;
            GameObject.Destroy(centerPoint);
        }
    }
}

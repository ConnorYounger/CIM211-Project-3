using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class RomingState : State
    {
        public RomingState(Enemy enemy) : base(enemy) { }

        private float pointFindDistance = 8;
        private float destinationPointDistance = 1f;
        private float idleCoolDownTimer;

        private bool hasReachedDestination;
        private bool foundIdlePoint;

        private string state;

        private Transform targetPoint;
        private Transform lastTargetPoint;

        private List<Transform> possiblePoints;
        private List<Vector3> debugLines;

        public override void Tick()
        {
            SearchForPlayer();
            ReachedDestinationPoint();

            if (targetPoint)
            {
                if(foundIdlePoint)
                    Debug.DrawLine(enemy.transform.position, targetPoint.position, Color.green);
                else
                    Debug.DrawLine(enemy.transform.position, targetPoint.position, Color.red);
            }

            if(idleCoolDownTimer > 0)
                idleCoolDownTimer -= 1 * Time.deltaTime;

            DrawDebugLines();
        }

        void DrawDebugLines()
        {
            if(debugLines.Count > 0)
            {
                foreach(Vector3 transform in debugLines)
                {
                    Debug.DrawLine(enemy.transform.position, transform, Color.grey);
                }
            }
        }

        void SearchForPlayer()
        {
            if(idleCoolDownTimer <= 0 && Vector3.Distance(enemy.transform.position, enemy.player.transform.position) < pointFindDistance)
            {
                RaycastHit hit = CalculateHit(enemy.transform.position, enemy.player.transform.position);
                
                Debug.DrawLine(enemy.transform.position, hit.point, Color.blue);

                if (hit.collider != null && hit.collider.gameObject == enemy.player.gameObject)
                {
                    SetAttackState();
                    Debug.DrawLine(enemy.transform.position, hit.collider.transform.position, Color.cyan);
                }
            }
        }

        void SetAttackState()
        {
            enemy.SetState(new EngagePlayerState(enemy));
        }

        void ReachedDestinationPoint()
        {
            if (targetPoint && Vector3.Distance(enemy.transform.position, targetPoint.transform.position) < destinationPointDistance && !hasReachedDestination)
            {
                hasReachedDestination = true;

                switch (state)
                {
                    case "CheckForClosePoints":
                        CheckForClosePoints();
                        break;
                    case "FoundIdlePoint":
                        enemy.SetState(new IdleState(enemy));
                        break;
                }
            }
        }

        void CheckForClosePoints()
        {
            possiblePoints.Clear();

            // Search for each transform within range
            foreach (Transform point in enemy.wanderPointCollection.transform)
            {
                if (Vector3.Distance(enemy.transform.position, point.position) < pointFindDistance)
                {
                    if (point == targetPoint)
                    {
                        lastTargetPoint = targetPoint;
                        //Debug.Log("Last Target point: " + lastTargetPoint);
                    }

                    if (point != lastTargetPoint || point != targetPoint)
                        possiblePoints.Add(point);
                }
            }

            if (possiblePoints.Count > 0)
            {
                // Pick a random transform 
                int rand = Random.Range(0, possiblePoints.Count);

                targetPoint = possiblePoints[rand];
            }
            else
            {
                // Find the closest transform if there are none in range
                float distance = 100;
                int index = 0;

                for (int i = 0; i < enemy.wanderPointCollection.transform.childCount; i++)
                {
                    if (Vector3.Distance(enemy.transform.position, enemy.wanderPointCollection.transform.GetChild(i).position) < distance)
                    {
                        distance = Vector3.Distance(enemy.transform.position, enemy.wanderPointCollection.transform.GetChild(i).position);
                        index = i;
                    }
                }

                targetPoint = enemy.wanderPointCollection.transform.GetChild(index);
            }

            Debug.Log("Target Point: " + targetPoint);
            enemy.navAgent.SetDestination(targetPoint.position);

            hasReachedDestination = false;

            if(idleCoolDownTimer <= 0)
                SearchForIdlePoint();
        }

        void SearchForIdlePoint()
        {
            possiblePoints.Clear();
            debugLines.Clear();

            if (enemy.idlePointCollection)
            {
                // Search for each transform within range
                foreach (Transform point in enemy.idlePointCollection.transform)
                {
                    // If the idle point is within range
                    if (Vector3.Distance(enemy.transform.position, point.position) < pointFindDistance)
                    {
                        // Check to see if the cat can see the idle spot
                        RaycastHit hit = CalculateHit(enemy.transform.position, point.position);

                        Debug.DrawLine(enemy.transform.position, hit.point, Color.green);

                        debugLines.Add(hit.point);

                        // If the cat can see the idle point, add it to the list of possible points
                        if (hit.collider != null && hit.collider.gameObject == point.gameObject)
                        {
                            possiblePoints.Add(point);
                        }
                    }
                }

                SetCloestPossiblePoint(true);
            }
        }

        void SetCloestPossiblePoint(bool idlePoint)
        {
            // If there are possible points, find the closest idle spot out of all the possible points
            if (possiblePoints.Count > 0)
            {
                float distance = pointFindDistance;
                int index = 0;

                for (int i = 0; i < possiblePoints.Count; i++)
                {
                    // Compare each possible point's distance
                    if (Vector3.Distance(enemy.transform.position, possiblePoints[i].position) < distance)
                    {
                        distance = Vector3.Distance(enemy.transform.position, possiblePoints[i].position);
                        index = i;
                    }
                }

                if (idlePoint)
                {
                    foundIdlePoint = true;
                    state = "FoundIdlePoint";
                }
                else
                {
                    state = "FoundNPCBaby";
                }

                // Set the target point to the closest idle point the cat can see
                targetPoint = possiblePoints[index];
                enemy.navAgent.SetDestination(targetPoint.position);

                Debug.Log("FOUND IDLE SPOT: " + targetPoint);
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

            foundIdlePoint = false;
            state = "CheckForClosePoints";
            possiblePoints = new List<Transform>();
            debugLines = new List<Vector3>();
            lastTargetPoint = enemy.transform;
            idleCoolDownTimer = enemy.idleCoolDownTime;

            CheckForClosePoints();
        }

        public override void OnStateExit()
        {
            Debug.Log("Exiting Roming State");
        }
    }
}

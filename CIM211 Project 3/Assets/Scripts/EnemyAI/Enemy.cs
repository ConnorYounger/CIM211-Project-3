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
        public Transform[] otherBabies;

        public float stateWaitTime = 10;
        public float idleCoolDownTime = 10;

        public float travelTime = 7;
        public int travelChance = 10;

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

        public void SetState(State state)
        {
            currentState.OnStateExit();
            currentState = state;
            currentState.OnStateEnter();
        }
    }
}

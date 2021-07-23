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

        [Header("Animation")]
        public Animator animator;
        public GameObject meleeHitCollider;

        [Header("Enemy Stats")]
        public float meleeDamage;
        public float rangedDamage;

        [Header("State Stats")]
        public float playerVisionDistance = 10;
        public float playerVisionAngle = 20;
        public float stateWaitTime = 10;
        public float idleCoolDownTime = 10;

        public float travelTime = 7;
        public int travelChance = 10;

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

        public void SetState(State state)
        {
            currentState.OnStateExit();
            currentState = state;
            currentState.OnStateEnter();
        }
    }
}

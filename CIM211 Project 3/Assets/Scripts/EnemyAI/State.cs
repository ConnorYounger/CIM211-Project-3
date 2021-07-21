using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public abstract class State
    {
        protected Enemy enemy;

        public State(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public abstract void Tick();

        public virtual void OnStateEnter() { }

        public virtual void OnStateExit() { }
    }
}

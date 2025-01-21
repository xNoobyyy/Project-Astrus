using System;
using Animals;
using UnityEngine;

namespace Pathfinding {
    public class Animal : MonoBehaviour {
        public Vector2 Target { get; private set; }
        public Vector2 Position { get; private set; }
        public float Health { get; private set; }

        public float Health { get; private se
        public AStar aStar;
        public Collider2D collider;

        public BaseAnimal baseAnimal;

        private void Awake() {
            aStar = new AStar(this);
        }
    }
}
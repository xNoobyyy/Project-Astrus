using System.Collections;
using Creatures;
using Player;
using UnityEditor.Animations;
using UnityEngine;

namespace Items {
    public abstract class BowItem : Item {
        private const float MaxDistance = 100f;
        private const float ArrowSpeed = 50f;

        public int Damage { get; private set; }

        protected BowItem(string name, string description, Sprite icon, AnimatorController animator, int damage) : base(
            name, description, icon, animator) {
            Damage = damage;
        }

        public override void OnUse(Transform player, Vector3 position) {
            var v = (Vector2)(position - player.position);
            PlayerItem.Instance.StartThirdPartyCoroutine(Shoot(player, v.normalized));

            OnAttack();
        }

        private IEnumerator Shoot(Transform player, Vector2 direction) {
            var distanceFlown = 0f;


            var arrow = Object.Instantiate(PlayerItem.Instance.arrowPrefab, player.position,
                Quaternion.FromToRotation(Vector2.right, direction));

            while (distanceFlown < MaxDistance) {
                var step = ArrowSpeed * Time.deltaTime;
                Vector2 currentPos = arrow.transform.position;
                var nextPos = currentPos + direction * step;
                var hit = Physics2D.Raycast(currentPos, direction, step);

                if (hit.collider != null) {
                    arrow.transform.position = hit.point;
                    arrow.transform.SetParent(hit.collider.transform);

                    if (!hit.collider.CompareTag("Obstacle")) {
                        var creature = hit.collider.GetComponent<CreatureBase>();
                        if (creature != null)
                            creature.OnAttack(player, Damage);
                    }

                    yield return new WaitForSeconds(5f);
                    Object.Destroy(arrow);
                    yield break;
                }

                arrow.transform.position = nextPos;
                distanceFlown += step;
                yield return null;
            }

            Object.Destroy(arrow);
        }


        protected virtual void OnAttack() { }
    }
}
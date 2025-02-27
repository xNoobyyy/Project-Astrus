using System;
using System.Collections;
using System.Linq;
using Creatures;
using Player;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Items {
    public abstract class BowItem : Item {
        private const float MaxDistance = 100f;
        private const float ArrowSpeed = 50f;

        private bool inactive;

        public int Damage { get; private set; }

        protected BowItem(string name, string description, Sprite icon, AnimatorController animator, int damage) : base(
            name, description, icon, animator) {
            Damage = damage;
        }

        public override void OnUse(Transform player, Vector3 position, ClickType clickType) {
            if (PlayerItem.Instance.IsBusy || inactive) return;

            PlayerItem.Instance.StartThirdPartyCoroutine(Shoot(player, position, clickType));
        }

        private IEnumerator Shoot(Transform player, Vector3 position, ClickType clickType) {
            inactive = true;
            if (clickType == ClickType.Left) {
                PlayerItem.Instance.bowContainer.SpriteRenderer.color = Color.white.WithAlpha(0.5f);
            } else {
                PlayerItem.Instance.bowContainerShift.SpriteRenderer.color = Color.white.WithAlpha(0.5f);
            }

            var distanceFlown = 0f;

            var start = clickType == ClickType.Left
                ? PlayerItem.Instance.bowContainer.SpriteRenderer.transform.position
                : PlayerItem.Instance.bowContainerShift.SpriteRenderer.transform.position;
            var direction = (Vector2)(position - start).normalized;
            var arrow = Object.Instantiate(PlayerItem.Instance.arrowPrefab, start,
                Quaternion.FromToRotation(Vector2.left, direction));

            while (distanceFlown < MaxDistance) {
                var step = ArrowSpeed * Time.deltaTime;
                Vector2 currentPos = arrow.transform.position;
                var nextPos = currentPos + direction * step;
                var hits = Physics2D.RaycastAll(currentPos, direction, step);
                Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
                var hit = hits.FirstOrDefault(hit =>
                    hit.collider.CompareTag("Obstacle") || hit.collider.GetComponent<CreatureBase>() != null);

                if (hit.collider != null) {
                    arrow.transform.position = hit.point;
                    arrow.transform.SetParent(hit.collider.transform);

                    var creature = hit.collider.GetComponent<CreatureBase>();
                    if (creature != null) {
                        creature.OnAttack(player, Damage);
                        OnAttack(creature, player);
                    }


                    yield return new WaitForSeconds(1f);

                    inactive = false;
                    if (clickType == ClickType.Left) {
                        PlayerItem.Instance.bowContainer.SpriteRenderer.color = Color.white;
                    } else {
                        PlayerItem.Instance.bowContainerShift.SpriteRenderer.color = Color.white;
                    }

                    yield return new WaitForSeconds(4f);

                    Object.Destroy(arrow);
                    yield break;
                }

                arrow.transform.position = nextPos;
                distanceFlown += step;
                yield return null;
            }

            inactive = false;
            if (clickType == ClickType.Left) {
                PlayerItem.Instance.bowContainer.SpriteRenderer.color = Color.white;
            } else {
                PlayerItem.Instance.bowContainerShift.SpriteRenderer.color = Color.white;
            }

            Object.Destroy(arrow);
        }


        protected virtual void OnAttack(CreatureBase creature, Transform player) { }
    }
}
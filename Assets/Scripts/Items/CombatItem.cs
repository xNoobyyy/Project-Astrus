using Logic;
using Player;
using TextDisplay;
using UnityEngine;

namespace Items {
    public abstract class CombatItem : Item {
        private static readonly int AttackDirection = Animator.StringToHash("attack_direction");

        public int Damage { get; private set; }

        protected CombatItem(string name, string description, Sprite icon, RuntimeAnimatorController animator, int damage) :
            base(name, description, icon, animator) {
            Damage = damage;
        }

        public override void OnUse(Transform player, Vector3 position, ClickType clickType) {
            if (LogicScript.Instance.accessableInventoryManager.CurrentSlot.Item is not CombatItem) return;
            if (PlayerItem.Instance.IsAttacking) return;
            if (TextDisplayManager.Instance.textDisplay.isDialogueActive) return;

            var zCoord = PlayerItem.Instance.mainCamera.WorldToScreenPoint(PlayerItem.Instance.transform.position).z;
            var mousePosition = Input.mousePosition;
            mousePosition.z = zCoord;

            var pos = PlayerItem.Instance.mainCamera.ScreenToWorldPoint(mousePosition);

            var v = (pos - PlayerItem.Instance.transform.position).normalized;
            PlayerItem.Instance.attackDirection = v;
            var angle = -Vector2.SignedAngle(Vector2.up, v);
            if (angle < 0) angle += 360f;

            // Set the attack direction for the animator
            var direction = angle switch { >= 315f or < 45f => 1, < 135f => 2, < 225f => 3, _ => 4 };
            PlayerItem.Instance.animator.SetInteger(AttackDirection, direction);
            PlayerItem.Instance.IsAttacking = true;

            // Rotate the attack collider
            var colliderRotation = -((angle + 180f) % 360f);
            PlayerItem.Instance.attackCollider.transform.localRotation = Quaternion.Euler(0f, 0f, colliderRotation);

            OnAttack();
        }

        protected virtual void OnAttack() { }
    }
}
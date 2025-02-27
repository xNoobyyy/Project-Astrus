using Items;
using Items.Items;
using UnityEngine;

namespace Player.Bow {
    public class Bow : MonoBehaviour {
        private static readonly int AnimatorBowType = Animator.StringToHash("type");

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;

        // ReSharper disable once InconsistentNaming
        private Camera _mainCamera; // Backing field

        private Camera MainCamera {
            get {
                if (_mainCamera == null) {
                    _mainCamera = Camera.main;
                }

                return _mainCamera;
            }
            set => _mainCamera = value;
        }

        public SpriteRenderer SpriteRenderer => spriteRenderer;

        private void Awake() {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Update() {
            var zCoord = MainCamera.WorldToScreenPoint(transform.position).z;
            var mousePosition = Input.mousePosition;
            mousePosition.z = zCoord;

            var worldPosition = (Vector2)MainCamera.ScreenToWorldPoint(mousePosition);

            var playerPosition = (Vector2)PlayerMovement.Instance.transform.position;

            var v = worldPosition - playerPosition;

            var angle = Vector2.SignedAngle(Vector2.up, v);
            var rotation = transform.localEulerAngles;
            rotation.z = angle;

            transform.localEulerAngles = rotation;
        }

        public void SetItem(BowItem item) {
            switch (item) {
                case GlomtomBow:
                    animator.enabled = true;
                    animator.SetInteger(AnimatorBowType, 0);
                    break;
                case FireGlomtomBow:
                    animator.enabled = true;
                    animator.SetInteger(AnimatorBowType, 1);
                    break;
                default:
                    animator.enabled = false;
                    spriteRenderer.sprite = item.Icon;
                    break;
            }
        }
    }
}
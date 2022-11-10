using System;
using System.Collections;
using UnityEngine;

namespace Fps.Controller
{
    public class Player : Character
    {
        [field: Header("Components")]
        [field: SerializeField]
        private Camera Camera { get; set; }

        [field: Header("Aim parameters")]
        [field: SerializeField, Range(0.01f, 2f)]
        private float HorizontalSensitivity { get; set; } = 1f;
        [field: SerializeField, Range(0.01f, 2f)]
        private float VerticalSensitivity { get; set; } = 1f;
        [field: SerializeField, Range(60f, 120f)]
        private float CameraPitchLimit { get; set; } = 85f;

        [field: Header("Movement parameters")]
        [field: SerializeField, Range(0.01f, 0.2f)]
        private float ForwardMovementOnPress { get; set; } = 0.15f;
        [field: SerializeField, Range(0.01f, 0.2f)]
        private float RightMovementOnPress { get; set; } = 0.15f;
        [field: SerializeField, Range(0.01f, 2f)]
        private float RunMultiplier { get; set; } = 1.5f;

        [field: Header("Jump parameters")]
        [field: SerializeField, Range(0.01f, 5f)]
        private float JumpHeight { get; set; } = 0.8f;
        [field: SerializeField, Range(0.01f, 1f)]
        private float JumpTime { get; set; } = 0.35f;
        [field: SerializeField, Range(0.1f, 2f)]
        private float JumpGravityMultiplier { get; set; } = 1f;

        [field: Header("General parameters")]
        [field: SerializeField, Range(0.0005f, 0.008f), Tooltip("Also works as the mass")]
        private float SlopeDecelerationMultiplier { get; set; } = 0.004f;
        [field: SerializeField, Range(1f, 3f)]
        private float PlayerHeight { get; set; } = 1f;
        [field: SerializeField]
        private LayerMask FloorLayerMask { get; set; }

        // TODO: remove this parameter, should be specific to the weapon,
        // not the player
        [field: SerializeField]
        private int WeaponMagCapacity { get; set; } = 10;

        // TODO: remove this parameter, should be specific to the weapon,
        // not the player
        [field: SerializeField]
        private float RateOfFire { get; set; } = 0.2f;

        // Pontuation related
        private int InitialPontuation { get; set; } = 0;
        public int Pontuation { get; set; }

        // Shooting related
        public int AmmoOnMag { get; set; }
        public int StoredAmmo { get; set; }
        private bool Reloading { get; set; }
        private float TimeSinceLastShot { get; set; }

        // Aim/movement related
        private CameraRotation CameraRotationEulerAngles { get; set; }
        private Vector3 PlaneNormal { get; set; } = Vector3.up;
        private Vector3 ProjectedContactPoint { get; set; }

        // Jump related
        private float Gravity { get; set; }
        private float JumpVerticalVelocity { get; set; }
        private Coroutine JumpingCoroutine { get; set; }
        private bool IsJumping => JumpingCoroutine != null;

        private void Awake()
        {
            GunController.RateOfFire = RateOfFire;
        }

        private void Start()
        {
            CharacterInitialization();
            SetupInitialPlayerState();
        }

        public void SetupInitialPlayerState()
        {
            _health = MaxHealth;
            Dead = false;
            AmmoOnMag = WeaponMagCapacity;
            StoredAmmo = 0;
            Pontuation = InitialPontuation;
            GameController.ShowCursor(false);
            TimeSinceLastShot = RateOfFire;
            CameraRotationEulerAngles = new CameraRotation(Camera.transform.localEulerAngles);
            SetupGravity();
        }

        private void SetupGravity()
        {
            // th is the time will take to take the player to the top of the parabola
            // This is the reciprocal of that value
            var oneOverTime = 1f / JumpTime;

            // v0 = 2 * JumpHeight / th
            JumpVerticalVelocity = 2f * JumpHeight * oneOverTime;
            // g = -2 * JumpHeight / (th ^ 2)
            Gravity = -(JumpVerticalVelocity) * oneOverTime;
        }

        protected void Update()
        {
            CheckCharacterHealth();

            var delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Aim(delta.x, -delta.y);

            if (Input.GetMouseButton(0)
                && !Dead
                && !Reloading
                && TimeSinceLastShot >= RateOfFire)
            {
                if (CanShoot())
                {
                    Shoot();
                    return;
                }

                if (CanReload())
                {
                    Reload();
                    return;
                }

                // TODO: let the player know that they don't have enough bullets
                // Example: sound effect or a message on screen
            }

            TimeSinceLastShot += Time.deltaTime;
        }

        private bool CanShoot()
        {
            return AmmoOnMag > 0;
        }

        private bool CanReload()
        {
            return StoredAmmo > 0
                && AmmoOnMag < WeaponMagCapacity;
        }

        public async void Reload()
        {
            if (Reloading || !CanReload())
                return;

            Reloading = true;

            await GunController.Reload();

            StoredAmmo = Math.Max(StoredAmmo - (WeaponMagCapacity - AmmoOnMag), 0);
            AmmoOnMag = WeaponMagCapacity;
            Reloading = false;
        }

        protected new void Shoot()
        {
            base.Shoot();
            AmmoOnMag--;
            TimeSinceLastShot = 0f;
        }

        private void FixedUpdate()
        {
            float rightMove = 0f;
            float forwardMove = 0f;

            // TODO: generalize input logic elsewhere to allow the user to change keybindings
            if (Input.GetKey(KeyCode.W))
                forwardMove++;
            if (Input.GetKey(KeyCode.S))
                forwardMove--;
            if (Input.GetKey(KeyCode.A))
                rightMove--;
            if (Input.GetKey(KeyCode.D))
                rightMove++;

            // TODO: generalize input logic elsewhere to allow the user to change keybindings
            if (Input.GetKey(KeyCode.LeftShift))
            {
                forwardMove *= RunMultiplier;
                rightMove *= RunMultiplier;
            }

            transform.position += Move(forwardMove, rightMove);

            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit raycastHit, float.MaxValue, FloorLayerMask))
            {
                PlaneNormal = raycastHit.normal;
                ProjectedContactPoint = raycastHit.point + (PlayerHeight * Vector3.up);

                // Fix player height
                if (!IsJumping)
                    transform.position = new Vector3(transform.position.x, ProjectedContactPoint.y, transform.position.z);
            }
            else
            {
                // Shouldn't happen but we treat it anyway
                PlaneNormal = Vector3.up;
                ProjectedContactPoint = transform.position - (PlayerHeight * Vector3.up);
            }

            // TODO: generalize input logic elsewhere to allow the user to change keybindings
            if (Input.GetKey(KeyCode.Space) && !IsJumping)
                JumpingCoroutine = StartCoroutine(ParabolicMovement(JumpGravityMultiplier, null, () => JumpingCoroutine = null));

            // TODO: generalize input logic elsewhere to allow the user to change keybindings
            if (Input.GetKey(KeyCode.R))
                Reload();
        }

        private Vector3 Move(float forwardMovementDirection, float rightMovementDirection)
        {
            var slopeAtenuation = Vector3.zero;
            var slopeDeclination = Vector3.Dot(PlaneNormal, Vector3.up);

            if (!Mathf.Approximately(slopeDeclination, 1f) && !IsJumping)
            {
                var planeRight = Vector3.Cross(PlaneNormal, Vector3.up);
                var planeForward = Vector3.Cross(planeRight, PlaneNormal);

                slopeAtenuation = (float)Math.Acos(slopeDeclination) * SlopeDecelerationMultiplier * Gravity * planeForward;
            }

            var cameraForward = Camera.transform.forward;
            // Should not consider plane inclination if jumping
            var relativeUp = !IsJumping ? PlaneNormal : Vector3.up;
            // > 0 if points up (same direction), 0 if points forward, < 0 if points down (oposite directions)
            var forwardProjectionOnNormal = Vector3.Dot(cameraForward, relativeUp);
            var forwardRelativeToCamera = cameraForward - (forwardProjectionOnNormal * relativeUp);

            var cameraRight = Camera.transform.right;
            // > 0 if points up (same direction), 0 if points forward, < 0 if points down (oposite directions)
            var rightProjectionOnNormal = Vector3.Dot(cameraRight, PlaneNormal);
            var rightRelativeToCamera = cameraRight - (rightProjectionOnNormal * PlaneNormal);

            return (forwardMovementDirection * ForwardMovementOnPress * forwardRelativeToCamera.normalized)
                + (rightMovementDirection * RightMovementOnPress * rightRelativeToCamera.normalized)
                + slopeAtenuation;
        }

        // (10/09/2022) Based on "Math for Game Programmers: Building a Better Jump", available at: https://www.youtube.com/watch?v=hG9SzQxaCm8
        private IEnumerator ParabolicMovement(
            float descendGravityMultiplier,
            Action onMovementPeak,
            Action onComplete)
        {
            SetupGravity();

            var verticalVelocity = JumpVerticalVelocity;
            var gravity = Gravity;

            var waitForFixedUpdate = new WaitForFixedUpdate();
            var updatedFallGravity = false;

            do
            {
                var deltaTime = Time.fixedDeltaTime;
                var heightIncrement = (verticalVelocity * deltaTime) + (gravity * deltaTime * deltaTime * 0.5f);

                // Prevents miscalculation
                if (transform.position.y + heightIncrement <= ProjectedContactPoint.y)
                {
                    transform.position = new Vector3(transform.position.x, ProjectedContactPoint.y, transform.position.z);
                    break;
                }

                transform.position += heightIncrement * Vector3.up;
                verticalVelocity += gravity * deltaTime;

                if (!updatedFallGravity
                    && verticalVelocity <= 0f)
                {
                    onMovementPeak?.Invoke();
                    updatedFallGravity = true;
                    gravity *= descendGravityMultiplier;
                }

                yield return waitForFixedUpdate;
            }
            while (transform.position.y > ProjectedContactPoint.y);

            transform.position = new Vector3(transform.position.x, ProjectedContactPoint.y, transform.position.z);

            onComplete?.Invoke();
        }

        // Based on Freya's solution, available at: https://twitter.com/FreyaHolmer/status/1445398551891259416
        // Accessed in 15/08/2022
        private void Aim(float horizontalOffset, float verticalOffset)
        {
            CameraRotationEulerAngles.X = Mathf.Clamp(CameraRotationEulerAngles.X + (verticalOffset * VerticalSensitivity), -CameraPitchLimit, CameraPitchLimit) % 360f;
            CameraRotationEulerAngles.Y = (CameraRotationEulerAngles.Y + (horizontalOffset * HorizontalSensitivity)) % 360f;

            Camera.transform.localRotation = Quaternion.Euler(CameraRotationEulerAngles.X, CameraRotationEulerAngles.Y, CameraRotationEulerAngles.Z);
        }

        private class CameraRotation
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }

            public CameraRotation(Vector3 eulerAngles)
            {
                X = eulerAngles.x;
                Y = eulerAngles.y;
                Z = eulerAngles.z;
            }
        }
    }
}
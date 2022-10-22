using System;
using System.Collections;
using UnityEngine;

namespace Fps.Controller
{
    public class PlayerTest : Player
    {
        [field: SerializeField]
        private Camera Camera { get; set; }

        [field: SerializeField]
        private LayerMask FloorLayerMask { get; set; }

        [field: SerializeField, Range(0.01f, 2f)]
        private float HorizontalSensitivity { get; set; } = 0.5f;
        [field: SerializeField, Range(0.01f, 2f)]
        private float VerticalSensitivity { get; set; } = 0.5f;

        [field: SerializeField, Range(0.01f, 0.2f)]
        private float ForwardMovementOnPress { get; set; } = 0.05f;
        [field: SerializeField, Range(0.01f, 0.2f)]
        private float RightMovementOnPress { get; set; } = 0.05f;
        [field: SerializeField, Range(0.01f, 2f)]
        private float RunMultiplier { get; set; } = 1.5f;

        [field: SerializeField, Range(0.01f, 5f)]
        private float JumpHeight { get; set; } = 1f;

        [field: SerializeField, Range(0.01f, 1f)]
        private float JumpTime { get; set; } = 0.5f;

        [field: SerializeField, Range(0.1f, 2f)]
        private float JumpGravityMultiplier { get; set; } = 1f;

        [field: SerializeField, Range(0.0005f, 0.008f), Tooltip("Also works as the mass")]
        private float SlopeDecelerationMultiplier { get; set; } = 0.008f;

        [field: SerializeField, Range(60f, 120f)]
        private float CameraPitchLimit { get; set; } = 90f;

        [field: SerializeField, Range(1f, 3f)]
        private float PlayerHeight { get; set; } = 1f;

        private CameraRotation CameraRotationEulerAngles { get; set; }
        private Vector3 PlaneNormal { get; set; } = Vector3.up;
        private float Gravity { get; set; }
        private float VerticalVelocity { get; set; }
        private Vector3 ProjectedContactPoint { get; set; }
        private Coroutine Jumping { get; set; }
        private bool IsJumping => Jumping != null;

        private void Start()
        {
            CameraRotationEulerAngles = new CameraRotation(Camera.transform.localEulerAngles);
            SetupGravity();
        }

        private void SetupGravity()
        {
            // th is the time will take to take the player to the top of the parabola
            // This is the reciprocal of that value
            var oneOverTime = 1f / JumpTime;

            // v0 = 2 * JumpHeight / th
            VerticalVelocity = 2f * JumpHeight * oneOverTime;
            // g = -2 * JumpHeight / (th ^ 2)
            Gravity = -(VerticalVelocity) * oneOverTime;
        }

        private void FixedUpdate()
        {
            float rightMove = 0f;
            float forwardMove = 0f;

            if (Input.GetKey(KeyCode.W))
                forwardMove++;
            if (Input.GetKey(KeyCode.S))
                forwardMove--;
            if (Input.GetKey(KeyCode.A))
                rightMove--;
            if (Input.GetKey(KeyCode.D))
                rightMove++;

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

            if (Input.GetKey(KeyCode.Space) && !IsJumping)
                Jumping = StartCoroutine(ParabolicMovement(JumpGravityMultiplier, null, () => Jumping = null));
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

            var verticalVelocity = VerticalVelocity;
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

        new private void Update()
        {
            base.Update();

            var delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Aim(delta.x, -delta.y);
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
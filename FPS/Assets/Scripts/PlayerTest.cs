using System;
using System.Collections;
using UnityEngine;

namespace Fps.Controller
{
    public class PlayerTest : Player
    {
        [field: SerializeField]
        private Camera Camera { get; set; }

        [field: SerializeField, Range(0.01f, 2f)]
        private float HorizontalSensitivity { get; set; } = 0.5f;
        [field: SerializeField, Range(0.01f, 2f)]
        private float VerticalSensitivity { get; set; } = 0.5f;

        [field: SerializeField, Range(0.01f, 0.2f)]
        private float ForwardMovementOnPress { get; set; } = 0.05f;
        [field: SerializeField, Range(0.01f, 0.2f)]
        private float RightMovementOnPress { get; set; } = 0.05f;

        [field: SerializeField, Range(0.01f, 5f)]
        private float JumpHeight { get; set; } = 1f;

        [field: SerializeField, Range(0.01f, 1f)]
        private float JumpTime { get; set; } = 0.5f;

        [field: SerializeField, Range(0.1f, 2f)]
        private float JumpGravityMultiplier { get; set; } = 1f;

        [field: SerializeField, Range(60f, 120f)]
        private float CameraPitchLimit { get; set; } = 90f;

        private CameraRotation CameraRotationEulerAngles { get; set; }

        private Coroutine Jumping { get; set; }

        private void Start()
        {
            CameraRotationEulerAngles = new CameraRotation(Camera.transform.localEulerAngles);
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

            transform.position += Move(forwardMove, rightMove);

            if (Input.GetKey(KeyCode.Space) && Jumping == null)
                Jumping = StartCoroutine(ParabolicMovement(JumpTime, JumpGravityMultiplier, transform.position.y, null, () => Jumping = null));
        }

        // TODO: treat values when the player aims completely up/down
        private Vector3 Move(float forwardMovementDirection, float rightMovementDirection)
        {
            // TODO: get the actual plane normal (for slopes, for example)
            var planeNormal = Vector3.up;

            var cameraForward = Camera.transform.forward;
            // > 0 if points up (same direction), 0 if points forward, < 0 if points down (oposite directions)
            var forwardProjectionOnNormal = Vector3.Dot(cameraForward, planeNormal);
            var forwardRelativeToCamera = cameraForward - (forwardProjectionOnNormal * planeNormal);

            var cameraRight = Camera.transform.right;
            // > 0 if points up (same direction), 0 if points forward, < 0 if points down (oposite directions)
            var rightProjectionOnNormal = Vector3.Dot(cameraRight, planeNormal);
            var rightRelativeToCamera = cameraRight - (rightProjectionOnNormal * planeNormal);

            return (forwardMovementDirection * ForwardMovementOnPress * forwardRelativeToCamera) + (rightMovementDirection * RightMovementOnPress * rightRelativeToCamera);
        }

        // (10/09/2022) Based on "Math for Game Programmers: Building a Better Jump", available at: https://www.youtube.com/watch?v=hG9SzQxaCm8
        private IEnumerator ParabolicMovement(
            float time,
            float descendGravityMultiplier,
            float finalVerticalPosition,
            Action onMovementPeak,
            Action onComplete)
        {
            // th is the time will take to take the player to the top of the parabola
            // This is the reciprocal of that value
            var oneOverTime = 1f / time;

            // v0 = 2 * JumpHeight / th
            var verticalVelocity = 2f * JumpHeight * oneOverTime;
            // g = -2 * JumpHeight / (th ^ 2)
            var gravity = -(verticalVelocity) * oneOverTime;

            var waitForFixedUpdate = new WaitForFixedUpdate();
            var updatedFallGravity = false;

            do
            {
                var deltaTime = Time.fixedDeltaTime;
                var heightIncrement = (verticalVelocity * deltaTime) + (gravity * deltaTime * deltaTime * 0.5f);

                // Prevents miscalculation
                if (transform.position.y + heightIncrement <= finalVerticalPosition)
                {
                    transform.position = new Vector3(transform.position.x, finalVerticalPosition, transform.position.z);
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
            while (transform.position.y > finalVerticalPosition);

            transform.position = new Vector3(transform.position.x, finalVerticalPosition, transform.position.z);

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
            CameraRotationEulerAngles.X = Mathf.Clamp(CameraRotationEulerAngles.X + verticalOffset * VerticalSensitivity, -CameraPitchLimit, CameraPitchLimit) % 360f;
            CameraRotationEulerAngles.Y = (CameraRotationEulerAngles.Y + horizontalOffset * HorizontalSensitivity) % 360f;

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
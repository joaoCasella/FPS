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

        new private void Update()
        {
            base.Update();

            var delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Camera.transform.rotation = Aim(delta.x, -delta.y);
        }

        // Based on Freya's solution, available at: https://twitter.com/FreyaHolmer/status/1445398551891259416?s=20&t=LJk2oZ_EK4gVGXm7y3SP6g
        // Accessed in 15/08/2022
        // TODO: clamp the resulting rotation when looking up/down (pitch)
        private Quaternion Aim(float horizontalOffset, float verticalOffset)
        {
            var rotation = Camera.transform.rotation;

            var horizontalRotation = Quaternion.AngleAxis(horizontalOffset * HorizontalSensitivity, Vector3.up);
            var verticalRotation = Quaternion.AngleAxis(verticalOffset * VerticalSensitivity, Vector3.right);

            return horizontalRotation * rotation * verticalRotation;
        }
    }
}
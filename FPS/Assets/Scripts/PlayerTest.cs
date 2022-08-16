using UnityEngine;

namespace Fps.Controller
{
    public class PlayerTest : Player
    {
        [field: SerializeField, Range(0.01f, 2f)]
        private float HorizontalSensitivity { get; set; } = 0.5f;
        [field: SerializeField, Range(0.01f, 2f)]
        private float VerticalSensitivity { get; set; } = 0.5f;

        private const float TargetFps = 60;
        private const float ForwardMovementOnPress = 0.1f;

        private const float RightMovementOnPress = 0.1f;

        private void Start()
        {
            Application.targetFrameRate = (int) TargetFps;
        }

        new private void Update()
        {
            base.Update();

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

            var delta = TargetFps * Time.deltaTime * new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            transform.SetPositionAndRotation(Move(forwardMove, rightMove), Aim(delta.x, -delta.y));
        }

        private Vector3 Move(float forwardMovementOnPress, float rightMovementOnPress)
        {
            return transform.position + forwardMovementOnPress * ForwardMovementOnPress * transform.forward + rightMovementOnPress * RightMovementOnPress * transform.right;
        }

        // Based on Freya's solution, available at: https://twitter.com/FreyaHolmer/status/1445398551891259416?s=20&t=LJk2oZ_EK4gVGXm7y3SP6g
        // Accessed in 15/08/2022
        private Quaternion Aim(float horizontalOffset, float verticalOffset)
        {
            var rotation = transform.rotation;

            var horizontalRotation = Quaternion.AngleAxis(horizontalOffset * HorizontalSensitivity, Vector3.up);
            var verticalRotation = Quaternion.AngleAxis(verticalOffset * VerticalSensitivity, Vector3.right);

            return horizontalRotation * rotation * verticalRotation;
        }
    }
}
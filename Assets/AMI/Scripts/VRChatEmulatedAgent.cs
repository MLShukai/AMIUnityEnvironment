using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;

namespace AMI {
    [RequireComponent(typeof(CharacterController))]
    public class VRChatEmulatedAgent : Agent
    {
        public float moveSpeed = 5f;
        public float rotationSpeed = 100f;
        public float jumpSpeed = 5f;
        public float dashMultiplier = 2f;
        public float gravity = -9.81f;

        public Transform spawnPoint;

        private CharacterController controller;
        private Vector3 playerVelocity;
        private bool isGrounded;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            controller = GetComponent<CharacterController>();
            
            if (spawnPoint == null){
                spawnPoint = transform;
            }
        }

        public override void OnEpisodeBegin()
        {
            Respawn();
        }

        private float moveVertical;
        private float moveHorizontal;
        private float rotation;
        private bool jump;
        private bool run;


        public override void OnActionReceived(ActionBuffers actions)
        {
            // Discrete actions, size = 5.
            // 0: Move vertical   -> 0: stop, 1: forward, 2: backward.
            // 1: Move horizontal -> 0: stop, 1: left, 2: right.
            // 2: Look horizontal -> 0: stop, 1: left, 2: right.
            // 3: Jump            -> 0: clear, 1: do.
            // 4: run            -> 0: clear, 1: do.

            switch (actions.DiscreteActions[0])
            {
                case 0:
                    moveVertical = 0f;
                    break;
                case 1:
                    moveVertical = 1f;
                    break;
                case 2:
                    moveVertical = -1f;
                    break;
            }

            switch (actions.DiscreteActions[1])
            {
                case 0:
                    moveHorizontal = 0f;
                    break;
                case 1:
                    moveHorizontal = -1f;
                    break;
                case 2:
                    moveHorizontal = 1f;
                    break;
            }

            switch (actions.DiscreteActions[2])
            {
                case 0:
                    rotation = 0f;
                    break;
                case 1:
                    rotation = -1f;
                    break;
                case 2:
                    rotation = 1f;
                    break;
            }

            jump = actions.DiscreteActions[3] == 1;
            run = actions.DiscreteActions[4] == 1;
        }


        private void Respawn(){
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var actions = actionsOut.DiscreteActions;

            // Vertical movement (W/S)
            if (Input.GetKey(KeyCode.W))
            {
                actions[0] = 1;  // Forward
            }
            else if (Input.GetKey(KeyCode.S))
            {
                actions[0] = 2;  // Backward
            }
            else
            {
                actions[0] = 0;  // Stop
            }

            // Horizontal movement (A/D)
            if (Input.GetKey(KeyCode.A))
            {
                actions[1] = 1;  // Left
            }
            else if (Input.GetKey(KeyCode.D))
            {
                actions[1] = 2;  // Right
            }
            else
            {
                actions[1] = 0;  // Stop
            }

            // Look horizontal (Left/Right arrow keys)
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                actions[2] = 1;  // Look left
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                actions[2] = 2;  // Look right
            }
            else
            {
                actions[2] = 0;  // Stop looking
            }

            // Jump (Space)
            actions[3] = Input.GetKey(KeyCode.Space) ? 1 : 0;

            // Run (Shift)
            actions[4] = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? 1 : 0;
        }

        private bool jumpPushed = false;
        void Update()
        {
            isGrounded = controller.isGrounded;

            // 移動
            Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;

            // ダッシュ
            if (jump)
            {
                movement *= dashMultiplier;
            }

            controller.Move(movement * moveSpeed * Time.deltaTime);

            // ジャンプ

            if (jump && !jumpPushed && isGrounded)
            {
                playerVelocity.y = jumpSpeed;
                jumpPushed = true;
            }

            if (jumpPushed && !jump){
                jumpPushed = false;
            }

            // 重力の適用
            if (!isGrounded)
            {
                playerVelocity.y += gravity * Time.deltaTime;
            }
            controller.Move(playerVelocity * Time.deltaTime);

            // 回転
            transform.Rotate(Vector3.up * rotation * rotationSpeed * Time.deltaTime);
        }
    }
}

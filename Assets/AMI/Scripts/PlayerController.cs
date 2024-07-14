using UnityEngine;

namespace AMI{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float rotationSpeed = 100f;
        public float jumpSpeed = 5f;
        public float dashMultiplier = 2f;
        public float gravity = -9.81f;

        private CharacterController controller;
        private Vector3 playerVelocity;
        private bool isGrounded;

        void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            isGrounded = controller.isGrounded;

            // 移動
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;

            // ダッシュ
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movement *= dashMultiplier;
            }

            controller.Move(movement * moveSpeed * Time.deltaTime);

            // ジャンプ
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                playerVelocity.y = jumpSpeed;
            }

            // 重力の適用
            if (!isGrounded)
            {
                playerVelocity.y += gravity * Time.deltaTime;
            }
            controller.Move(playerVelocity * Time.deltaTime);

            // 回転
            float rotation = 0f;
            if (Input.GetKey(KeyCode.O))
            {
                rotation = -1f;
            }
            else if (Input.GetKey(KeyCode.P))
            {
                rotation = 1f;
            }
            transform.Rotate(Vector3.up * rotation * rotationSpeed * Time.deltaTime);
        
        }
    }
}
using UnityEngine;

namespace Knownt
{
    public class Player : MonoBehaviour
    {
        [Header("Player config")]
        public float moveSpeed;
        public float aimSpeed;
        public GameObject projectile;

        private bool m_Charging;
        private Vector2 m_Rotation;
        private PlayerControls m_Controls;

        public void Awake()
        {
            m_Controls = new PlayerControls();

            m_Controls.Player.Fire.performed +=
                ctx =>
                {
                    Fire();
                };
        }

        private void OnEnable()
        {
            m_Controls.Enable();
        }

        private void OnDisable()
        {
            m_Controls.Disable();
        }

        public void Update()
        {
            var move = m_Controls.Player.Move.ReadValue<Vector2>();
            var look = m_Controls.Player.Look.ReadValue<Vector2>();
            
            Look(look);
            Move(move);
        }

        private void Move(Vector2 direction)
        {
            if (direction.sqrMagnitude < 0.01f)
                return;

            var scaledMoveSpeed = moveSpeed * Time.deltaTime;

            var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * (Vector3)direction;
            Debug.Log(move);
            transform.position += move * scaledMoveSpeed;
        }

        private void Look(Vector2 rotate)
        {
            if (rotate.sqrMagnitude < 0.01)
                return;
            var scaledRotateSpeed = aimSpeed * Time.deltaTime;
            m_Rotation.y += rotate.x * scaledRotateSpeed;
            m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
            transform.localEulerAngles = m_Rotation;
        }

        private void Fire()
        {/*
            var transform = this.transform;
            var newProjectile = Instantiate(projectile);
            newProjectile.transform.position = transform.position + transform.forward * 0.6f;
            newProjectile.transform.rotation = transform.rotation;
            const int size = 1;
            newProjectile.transform.localScale *= size;
            newProjectile.GetComponent<Rigidbody>().mass = Mathf.Pow(size, 3);
            newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * 20f, ForceMode.Impulse);
            newProjectile.GetComponent<MeshRenderer>().material.color =
                new Color(Random.value, Random.value, Random.value, 1.0f);*/
        }

        /*
        private IEnumerator BurstFire(int burstAmount)
        {
            for (var i = 0; i < burstAmount; ++i)
            {
                Fire();
                yield return new WaitForSeconds(0.1f);
            }
        }*/
    }
}

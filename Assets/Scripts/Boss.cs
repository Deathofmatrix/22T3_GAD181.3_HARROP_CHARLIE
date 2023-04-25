using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonCrawler_Chaniel
{
    public class Boss : MonoBehaviour
    {
        public bool isActive;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float turnSpeed;
        [SerializeField] private float bulletSpeed;

        public float timeBetweenShots = 1f;
        private float timeSinceLastShot = 0f;
        [SerializeField] private int numberOfDirections = 1;

        [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth = 300;

        [SerializeField] private Canvas canvas;
        [SerializeField] private Slider bossHealthSlider;
        [SerializeField] private SoundManager soundManager;

        private void Start()
        {
            soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
            canvas = GameObject.Find("Canvas - Overlay").GetComponent<Canvas>();
            bossHealthSlider = canvas.gameObject.transform.Find("Boss Health").GetComponent<Slider>();
            bossHealthSlider.gameObject.SetActive(false);
            isActive = false;
            bossHealthSlider.maxValue = maxHealth;
            currentHealth = maxHealth;
        }

        private void Update()
        {
            bossHealthSlider.value = currentHealth;

            if (isActive)
            {
                bossHealthSlider.gameObject.SetActive(true);
                TurnBoss();
                ChangePhase();

                timeSinceLastShot += Time.deltaTime;

                if (timeSinceLastShot >= timeBetweenShots)
                {
                    Quaternion bulletRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - 90f);
                    ShootBullet(bulletRotation);
                    timeSinceLastShot = 0f;
                }

                if (currentHealth == 0)
                {
                    GameObject.Find("Scene Manager").GetComponent<SceneLoader>().LoadThisScene("VictoryScene");
                }

                if (numberOfDirections == 1)
                {
                    soundManager.mainMusic.pitch = 1.1f;
                }

                if (numberOfDirections == 2)
                {
                    soundManager.mainMusic.pitch = 1.2f;
                }

                if (numberOfDirections == 4)
                {
                    soundManager.mainMusic.pitch = 1.3f;
                }
            }
        }

        private void ShootBullet(Quaternion rotation)
        {
            float angleStep = 360f / numberOfDirections;

            for (int i = 0; i < numberOfDirections; i++)
            {
                float angle = i * angleStep;
                Quaternion bulletRotation = rotation * Quaternion.Euler(0, 0, angle);

                GameObject bullet = Instantiate(bulletPrefab, transform.position, bulletRotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.velocity = bullet.transform.up * bulletSpeed;
            }
        }

        private void TurnBoss()
        {
            transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("PlayerBullet"))
            {
                TakeDamage(10);
            }
        }

        private void TakeDamage(int damage)
        {
            currentHealth -= damage;    
        }
        
        private void ChangePhase()
        {
            if (currentHealth <= (maxHealth / 4))
            {
                numberOfDirections = 4;
            }

            else if (currentHealth <= (maxHealth / 2))
            {
                numberOfDirections = 2;
            }
        }
    }

}


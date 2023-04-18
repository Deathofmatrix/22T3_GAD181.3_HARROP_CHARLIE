using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class ShootingEnemy : MonoBehaviour
    {
        public float speed;
        //public float turnSpeed;
        public Transform target;
        public float minimumDistance;
        public float lookDistance;
        [SerializeField] private Rigidbody2D rigidBody2D;
        [SerializeField] float time;
        [SerializeField] Vector3 direction;

        public GameObject projectile;
        public float timeBetweenShots;
        [SerializeField] private float nextShotTime;

        private void Start()
        {
        }

        private void FindTarget()
        {
            if (PlayerCharacterManager.golem.GetComponent<GolemController>().golemActivated == true)
            {
                target = PlayerCharacterManager.golem.transform;
            }
            else if (PlayerCharacterManager.golem.GetComponent<GolemController>().golemActivated == false && PlayerCharacterManager.allPlayers != null)
            {
                float closestDistance = Mathf.Infinity;

                foreach (GameObject gameObject in PlayerCharacterManager.allPlayers)
                {
                    float distance = Vector3.Distance(gameObject.transform.position, transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        target = gameObject.transform;
                    }
                }
            }
        }

        private void Update()
        {
            FindTarget();
        }

        private void FixedUpdate()
        {
            if (target != null)
            {
                time = Time.time;
                direction = (target.transform.position - rigidBody2D.transform.position).normalized;


                if (Vector2.Distance(transform.position, target.position) < lookDistance)
                {
                    if (Time.time > nextShotTime)
                    {
                        nextShotTime = Time.time + timeBetweenShots;
                        GameObject newBullet = Instantiate(projectile, transform.position, Quaternion.identity);
                        newBullet.GetComponent<Rigidbody2D>().velocity = direction * speed;
                    }
                }

                if (Vector2.Distance(transform.position, target.position) < minimumDistance)
                {

                    rigidBody2D.MovePosition(rigidBody2D.transform.position + direction * -speed * Time.fixedDeltaTime);
                    //rigidBody2D.velocity = transform.forward  * -speed ;

                    //var enemyRotation = Quaternion.LookRotation(target.position - transform.position);

                    //rigidBody2D.MoveRotation(Quaternion.RotateTowards(transform.rotation, enemyRotation, turnSpeed));
                }
            } 
        }

        private void OnDestroy()
        {
            if (PlayerCharacterManager.golem != null)
            {
                GolemController golemScript = PlayerCharacterManager.golem.GetComponent<GolemController>();
                golemScript.OnEnemyKill();
            }
        }
    }
}


using DungeonCrawler_Chaniel;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DungeonCrawler_Chaniel
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private SceneLoader sceneLoader;
        public Room room;

        public float speed;
        public int damage;
        public Transform target;
        public float minimumDistance;
        [SerializeField] private Rigidbody2D rigidBody2D;
        [SerializeField] Vector3 direction;
        //[SerializeField] bool inWall = false;
        //[SerializeField] bool moved = false;

        private void Start()
        {
            Debug.Log(name + transform.localPosition);
            //room = GameObject.Find("Room1").GetComponent<Room>();
            //room.enemiesSpawned++;
            sceneLoader = GameObject.Find("Scene Manager").GetComponent<SceneLoader>();
            transform.parent.parent.gameObject.GetComponent<Room>().enemiesInRoom++;
            //StartCoroutine(MoveOutOfWall());
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

        void FixedUpdate()
        {
            if (target != null)
            {
                direction = (target.transform.position - rigidBody2D.transform.position).normalized;
                rigidBody2D.MovePosition(rigidBody2D.transform.position + direction * speed * Time.fixedDeltaTime);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //if (collision.gameObject.CompareTag("PlayerBullet"))
            //{
            //    Destroy(collision.gameObject);
            //    Destroy(gameObject);
            //}
            //if (collision.gameObject.CompareTag("Wall"))
            //{
            //    //inWall = true;
            //}

            if (collision.gameObject.CompareTag("Golem"))
            {
                //ScoreManager.finalScore = ScoreManager.score;
                //sceneLoader.LoadThisScene("GameOver");
                GolemController golemScript = collision.gameObject.GetComponent<GolemController>();
                golemScript.ReduceFuel(damage);
                FindObjectOfType<SoundManager>().Play("Hurt");
            }
        }

        private void OnDestroy()
        {
            transform.parent.parent.gameObject.GetComponent<Room>().enemiesInRoom--;
            if (PlayerCharacterManager.golem != null)
            {
                ScoreManager.score++;
                GolemController golemScript = PlayerCharacterManager.golem.GetComponent<GolemController>();
                golemScript.OnEnemyKill();
            }  
        }
        
        //public IEnumerator MoveOutOfWall()
        //{
        //    if (inWall && moved == false)
        //    {
        //        Vector2.MoveTowards(transform.position, PlayerCharacterManager.golem.transform.position, 5f);
        //    }
        //    yield return new WaitForSeconds(1f);

        //    inWall = false;
        //    moved = true;
        //}
    }
}


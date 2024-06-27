using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Knownt
{
    public class Enemy : MonoBehaviour
    {
        private static bool pointsSetuped;

        [SerializeField]
        private GameObject pathGameObject;
        private List<Pathpoint> path;
        [SerializeField]
        private LayerMask layerMask;
        [SerializeField]
        private int startingPoint;

        private Pathpoint nextPathpoint;
        private bool alerted = false;
        private bool playerInSight = false;
        private bool searching = false;
        private Vector3 alertPosition;
        private Queue<Pathpoint> pathToFollow;
        private GameObject player;

        [SerializeField]
        private float speed;
        [SerializeField]
        private float fieldOfVisionWidth;
        [SerializeField]
        private float fieldOfVisionLength;

        // Start is called before the first frame update
        void Start()
        {
            path = pathGameObject.GetComponentsInChildren<Pathpoint>().ToList();
            FillPathpointsData();
            nextPathpoint = path[startingPoint];
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            SearchPlayer();

            if (playerInSight)
            {
                MoveToNextPoint(player.transform.position);
            }

            else if (alerted)
            {
                if (pathToFollow.Count > 0)
                {
                    MoveToNextPoint(pathToFollow.Peek().transform.position);
                }
                else
                {
                    MoveToNextPoint(alertPosition);
                }
            }
            
            else
            {
                MoveToNextPoint(nextPathpoint.transform.position);
            }
        }

        private void FillPathpointsData()
        {
            if (!pointsSetuped)
            {
                foreach (Pathpoint point in path)
                {
                    foreach (Pathpoint otherPoint in path)
                    {
                        if (otherPoint != point)
                        {
                            RaycastHit2D hit;
                            Physics2D.queriesStartInColliders = false;
                            hit = Physics2D.Raycast(point.transform.position, otherPoint.transform.position - point.transform.position, Mathf.Infinity, layerMask);
                            if (hit.collider != null && hit.transform.gameObject == otherPoint.gameObject)
                            {
                                Pathpoint adjacentPoint = hit.transform.gameObject.GetComponent<Pathpoint>();
                                point.adjacentPoints.Add(adjacentPoint);
                                point.distanceFromPathpoint.Add(adjacentPoint, Vector3.Distance(adjacentPoint.transform.position, point.transform.position));
                            }
                        }
                    }
                }
                pointsSetuped = true;
            }
        }

        private void MoveToNextPoint(Vector3 point)
        {
            float distanceFormPoint = Vector2.Distance(point, transform.position);
            if (distanceFormPoint > speed * Time.deltaTime)
            {
                Vector3 direction = (point - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
                float alfa = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
                gameObject.transform.rotation = Quaternion.Euler(0, 0, alfa - 90); //Sorry
            }
            else
            {
                transform.position = point;
                if (playerInSight)
                {
                    playerInSight = false;
                    FindNewPoint();
                }
                else if (alerted)
                {
                    if (pathToFollow.Count < 1)
                    {
                        alerted = false;
                        FindNewPoint();
                    }
                    else
                    {
                        pathToFollow.Dequeue();
                    }
                }
                else if (searching)
                {
                    RaycastHit2D hit;
                    hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, fieldOfVisionLength);
                    if (hit.transform.gameObject == player)
                    {
                        playerInSight = true;
                    }
                    searching = false;
                }
                else
                {
                    nextPathpoint = nextPathpoint.adjacentPoints[UnityEngine.Random.Range(0, nextPathpoint.adjacentPoints.Count)];
                }
            }
        }

        private void FindNewPoint()
        {
            nextPathpoint = null;
            int i = 0;
            while (nextPathpoint == null)
            {
                RaycastHit2D hit;
                hit = Physics2D.Raycast(transform.position, path[i].transform.position - transform.position, Mathf.Infinity, layerMask);
                if (hit.transform.gameObject == path[i].gameObject)
                {
                    nextPathpoint = path[i];
                }
                i++;
            }
        }

        private void FindShortestPathToPoint(GameObject endPoint)
        {
            List<Pathpoint> unvisitedPathpoints = path.ToList();
            Dictionary<Pathpoint, float> pathpointsMinDistance = new Dictionary<Pathpoint, float>();
            Dictionary<Pathpoint, List<Pathpoint>> shortestPathToPoints = new Dictionary<Pathpoint, List<Pathpoint>>();

            float minDistanceToEndPoint = Mathf.Infinity;
            
            Pathpoint currentPoint;
            List<Pathpoint> shortestPathToEndPoint = new List<Pathpoint>();

            float lowestDistancePoint;

            Physics2D.SyncTransforms();

            RaycastHit2D hitEnd;
            hitEnd = Physics2D.Raycast(transform.position, endPoint.transform.position - transform.position, Mathf.Infinity, layerMask);

            if (hitEnd.transform.gameObject == endPoint)
            {
                unvisitedPathpoints.Clear();
            }

            for (int i = 0; i < unvisitedPathpoints.Count; i++)
            {
                RaycastHit2D hit;
                hit = Physics2D.Raycast(transform.position, unvisitedPathpoints[i].transform.position - transform.position, Mathf.Infinity, layerMask);
                if (hit.transform.gameObject == unvisitedPathpoints[i].gameObject)
                {
                    pathpointsMinDistance.Add(unvisitedPathpoints[i], hit.distance);
                    shortestPathToPoints.Add(unvisitedPathpoints[i], new List<Pathpoint>() { unvisitedPathpoints[i] });
                }
                else
                {
                    pathpointsMinDistance.Add(unvisitedPathpoints[i], Mathf.Infinity);
                }
            }

            while (unvisitedPathpoints.Count > 0)
            {
                lowestDistancePoint = Mathf.Infinity;
                currentPoint = null;
                for (int i = 0; i < unvisitedPathpoints.Count; ++i)
                {
                    if (pathpointsMinDistance[unvisitedPathpoints[i]] < lowestDistancePoint)
                    {
                        currentPoint = unvisitedPathpoints[i];
                    }
                }

                unvisitedPathpoints.Remove(currentPoint);

                for (int i = 0; i < currentPoint.adjacentPoints.Count; i++)
                {
                    Pathpoint point = currentPoint.adjacentPoints[i];
                    if (unvisitedPathpoints.Contains(point))
                    {
                        float distanceFromPoint = pathpointsMinDistance[currentPoint] + currentPoint.distanceFromPathpoint[point];
                        if (pathpointsMinDistance[point] > distanceFromPoint)
                        {
                            pathpointsMinDistance[point] = distanceFromPoint;
                            shortestPathToPoints[point] = shortestPathToPoints[currentPoint].ToList();
                            shortestPathToPoints[point].Add(point);
                        }
                    }
                }
                RaycastHit2D hit;
                hit = Physics2D.Raycast(currentPoint.transform.position, endPoint.transform.position - currentPoint.transform.position, Mathf.Infinity, layerMask);

                if (hit.collider != null && hit.transform.gameObject == endPoint)
                {
                    if (minDistanceToEndPoint > hit.distance + pathpointsMinDistance[currentPoint])
                    {
                        minDistanceToEndPoint = hit.distance + pathpointsMinDistance[currentPoint];
                        shortestPathToEndPoint = shortestPathToPoints[currentPoint].ToList();
                    }
                }
            }

            pathToFollow = new Queue<Pathpoint>(shortestPathToEndPoint);
        }

        private void SearchPlayer()
        {
            float angleDiferenceFromPlayer;
            angleDiferenceFromPlayer = Vector3.Angle(transform.up, player.transform.position - transform.position);
            if (Mathf.Abs(angleDiferenceFromPlayer) < fieldOfVisionWidth)
            {
                RaycastHit2D hit;
                hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, fieldOfVisionLength);

                if (hit.transform != null && hit.transform.gameObject == player)
                {
                    playerInSight = true;
                }
                else if (playerInSight)
                {
                    searching = true;
                    playerInSight = false;
                    float closestDistance = Mathf.Infinity;
                    foreach (Pathpoint point in path)
                    {
                        float currentDistance = Vector3.Distance(point.transform.position, player.transform.position);
                        if (currentDistance < closestDistance)
                        {
                            closestDistance = currentDistance;
                            nextPathpoint = point;
                        }
                    }
                    
                }
            }
        }

        public void OnAlerted(GameObject alertEmiter)
        {
            alertPosition = alertEmiter.transform.position;
            alerted = true;
            FindShortestPathToPoint(alertEmiter);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                CanvasController.Instance.ShowGameOverUI();
                PauseController.Pause();
            }
        }
    } 
}

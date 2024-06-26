using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Knownt
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private List<Pathpoint> path;
        [SerializeField]
        private int playerLayer;

        private Pathpoint nextPathpoint;
        private bool alerted = false;
        private bool playerInSight = false;
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
            FillPathpointsData();
            nextPathpoint = path[0];
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            SearchPlayer();    
            if (alerted)
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
            else if (!playerInSight)
            {
                MoveToNextPoint(nextPathpoint.transform.position);
            }
            else
            {
                MoveToNextPoint(player.transform.position);
            }
        }

        private void FillPathpointsData()
        {
            foreach (Pathpoint point in path)
            {
                foreach (Pathpoint otherPoint in path)
                {
                    if (otherPoint != point)
                    {
                        RaycastHit2D hit;
                        Physics2D.queriesStartInColliders = false;
                        hit = Physics2D.Raycast(point.transform.position,otherPoint.transform.position - point.transform.position);
                        if (hit.transform.gameObject == otherPoint.gameObject)
                        {
                            Pathpoint adjacentPoint = hit.transform.gameObject.GetComponent<Pathpoint>();
                            point.adjacentPoints.Add(adjacentPoint);
                            point.distanceFromPathpoint.Add(adjacentPoint, Vector3.Distance(adjacentPoint.transform.position, point.transform.position));
                        }
                    }
                }
            }
        }

        private void MoveToNextPoint(Vector3 point)
        {
            float distanceFormPoint = Vector2.Distance(point, transform.position);
            if (distanceFormPoint > speed * Time.deltaTime)
            {
                Vector3 direction = (point - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                transform.position = point;
                if (playerInSight)
                {
                    playerInSight = false;
                    FindNewPoint();
                }
                else
                {
                    if (alerted)
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
                    else
                    {
                        nextPathpoint = nextPathpoint.adjacentPoints[UnityEngine.Random.Range(0, nextPathpoint.adjacentPoints.Count)];
                    }
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
                hit = Physics2D.Raycast(transform.position, path[i].transform.position - transform.position);
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
            hitEnd = Physics2D.Raycast(transform.position, endPoint.transform.position - transform.position);

            if (hitEnd.transform.gameObject == endPoint)
            {
                unvisitedPathpoints.Clear();
            }

            for (int i = 0; i < unvisitedPathpoints.Count; i++)
            {
                RaycastHit2D hit;
                hit = Physics2D.Raycast(transform.position, unvisitedPathpoints[i].transform.position - transform.position);
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
                hit = Physics2D.Raycast(currentPoint.transform.position, endPoint.transform.position - currentPoint.transform.position);

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
            Debug.Log(transform.up);
            float angleDiferenceFromPlayer;
            angleDiferenceFromPlayer = Vector3.Angle(transform.up, player.transform.position - transform.position);
            Debug.DrawRay(transform.position, transform.up * 5, UnityEngine.Color.red, 100);
            if (Mathf.Abs(angleDiferenceFromPlayer) < fieldOfVisionWidth)
            {
                RaycastHit2D hit;
                hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, fieldOfVisionLength);
                if (hit.transform.gameObject == player)
                {
                    playerInSight = true;
                }
            }
        }

        public void OnAlerted(GameObject alertEmiter)
        {
            alertPosition = alertEmiter.transform.position;
            alerted = true;
            FindShortestPathToPoint(alertEmiter);
        }
    } 
}

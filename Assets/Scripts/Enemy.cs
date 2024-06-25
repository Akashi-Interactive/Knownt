using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Knownt
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private List<Pathpoint> path;

        private Pathpoint nextPathpoint;
        private bool alerted;
        private Vector3 alertPosition;
        private Queue<Pathpoint> pathToFollow;

        [SerializeField]
        private float speed;

        // Start is called before the first frame update
        void Start()
        {
            FillPathpointsData();
            nextPathpoint = path[0];
        }

        // Update is called once per frame
        void Update()
        {
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
            else
            {
                MoveToNextPoint(nextPathpoint.transform.position);
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
                if (alerted)
                {
                    if (pathToFollow.Count < 0)
                    {
                        alerted = false;
                    }
                    else
                    {
                        pathToFollow.Dequeue();
                    }
                }
                else
                {
                    nextPathpoint = nextPathpoint.adjacentPoints[Random.Range(0, nextPathpoint.adjacentPoints.Count)];
                }
            }
        }

        private void FindShortestPathToPoint(GameObject endPoint, Pathpoint startingPoint)
        {
            List<Pathpoint> unvisitedPathpoints = path;
            Dictionary<Pathpoint, float> pathpointsMinDistance = new Dictionary<Pathpoint, float>();
            Dictionary<Pathpoint, List<Pathpoint>> shortestPathToPoints = new Dictionary<Pathpoint, List<Pathpoint>>();

            float minDistanceToEndPoint = Mathf.Infinity;
            
            Pathpoint currentPoint;
            List<Pathpoint> shortestPathToEndPoint = new List<Pathpoint>();

            float lowestDistancePoint;

            for (int i = 0; i < unvisitedPathpoints.Count; i++)
            {
                pathpointsMinDistance[unvisitedPathpoints[i]] = Mathf.Infinity;
            }

            pathpointsMinDistance[startingPoint] = 0;
            shortestPathToPoints[startingPoint].Add(startingPoint);

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

                for (int i = 0; i < currentPoint.adjacentPoints.Count; i++)
                {
                    Pathpoint point = currentPoint.adjacentPoints[i];
                    if (unvisitedPathpoints.Contains(point))
                    {
                        float distanceFromPoint = pathpointsMinDistance[currentPoint] + currentPoint.distanceFromPathpoint[point];
                        if (pathpointsMinDistance[point] > distanceFromPoint)
                        {
                            pathpointsMinDistance[point] = distanceFromPoint;
                            shortestPathToPoints[point] = shortestPathToPoints[currentPoint];
                            shortestPathToPoints[point].Add(point);
                        }
                    }
                }
                RaycastHit2D hit;
                hit = Physics2D.Raycast(currentPoint.transform.position, endPoint.transform.position - currentPoint.transform.position);

                if (hit.transform.gameObject == endPoint)
                {
                    if (minDistanceToEndPoint > hit.distance + pathpointsMinDistance[currentPoint])
                    {
                        minDistanceToEndPoint = hit.distance + pathpointsMinDistance[currentPoint];
                        shortestPathToEndPoint = shortestPathToPoints[currentPoint];
                    }
                }

                unvisitedPathpoints.Remove(currentPoint);

            }

            pathToFollow = new Queue<Pathpoint>(shortestPathToEndPoint);
        }

        public void OnAlerted(GameObject alertEmiter)
        {
            alertPosition = alertEmiter.transform.position;
            alerted = true;
            FindShortestPathToPoint(alertEmiter, nextPathpoint);
        }
    } 
}

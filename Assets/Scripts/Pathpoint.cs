using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knownt
{
    public class Pathpoint : MonoBehaviour
    {
        public List<Pathpoint> adjacentPoints = new List<Pathpoint>();
        public Dictionary<Pathpoint, float> distanceFromPathpoint = new Dictionary<Pathpoint, float>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.3f;
        private int j;

        public Vector3 GetWaypoint(int index)
        {
            return transform.GetChild(index).position;
        }
        public int GetNextIndex(int index)
        {
            if (index + 1 == transform.childCount)
            {
                return 0;
            }
            else
            {
                return index + 1;
            }
        }

        private void OnDrawGizmos()
        {
            int waypointsCount = transform.childCount;
            for (int i = 0; i < waypointsCount; i++)
            {
                j = this.GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
        }
    }
}

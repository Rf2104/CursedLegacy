using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RF
{
    using UnityEngine;
    using UnityEngine.AI;

    namespace RF
    {
        [RequireComponent(typeof(NavMeshAgent))]
        public class DrawPath : MonoBehaviour
        {
            private NavMeshAgent agent;
            private LineRenderer lineRenderer;

            void Start()
            {
                agent = GetComponent<NavMeshAgent>();
                lineRenderer = GetComponent<LineRenderer>();

                lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startColor = Color.yellow;
                lineRenderer.endColor = Color.yellow;
                lineRenderer.startWidth = 0.5f;
                lineRenderer.endWidth = 0.5f;
            }

            void OnDrawGizmosSelected()
            {
                if (agent == null || agent.path == null)
                    return;

                var path = agent.path;
                lineRenderer.positionCount = path.corners.Length;

                for (int i = 0; i < path.corners.Length; i++)
                {
                    lineRenderer.SetPosition(i, path.corners[i]);
                }
            }
        }
    }

}


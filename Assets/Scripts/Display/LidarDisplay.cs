namespace Assets.Scripts
{
    using Assets.Scripts.ROS.Data.Message;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Displays Lidar points using GPU instancing for high-performance rendering.
    /// </summary>
    internal class LidarDisplay : MonoBehaviour
    {
        [SerializeField]
        private Mesh lidarPointMesh;

        [SerializeField]
        private Material lidarPointMaterial;

        [SerializeField]
        private float density = 0.5f;

        [SerializeField]
        private float pointScale = 0.5f;

        [SerializeField]
        [Range(1, 1023)]
        private int maxPoints = 1023;

        private List<Matrix4x4> matrices = new List<Matrix4x4>();
        private Vector3[] points = new Vector3[0];
        private bool initialized = false;
        private new bool enabled = false;

        public void Enable()
        {
            if (!initialized)
            {
                Initialize();
            }
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }

        public void Initialize()
        {
            if (lidarPointMesh == null || lidarPointMaterial == null)
            {
                Debug.LogError("[LidarDisplay] Mesh or Material is not assigned.");
                return;
            }

            initialized = true;
        }

        private void Update()
        {
            if (!enabled || !initialized || points == null) return;

            matrices.Clear();
            for (int i = 0; i < points.Length; i++)
            {
                var matrix = Matrix4x4.TRS(points[i], Quaternion.identity, Vector3.one * pointScale);
                matrices.Add(matrix);
            }

            for (int i = 0; i < matrices.Count; i += maxPoints)
            {
                int batchCount = Mathf.Min(maxPoints, matrices.Count - i);
                Graphics.DrawMeshInstanced(
                    lidarPointMesh,
                    0,
                    lidarPointMaterial,
                    matrices.GetRange(i, batchCount)
                );
            }
        }

        public void UpdatePoints(RosMessage<ScanMsg> message)
        {
            if (!initialized) return;

            List<Vector3> validPoints = new List<Vector3>();

            for (int i = 0; i < message.msg.ranges.Length; i++)
            {
                if (!message.msg.ranges[i].HasValue) continue;

                float range = message.msg.ranges[i].Value;
                float angle = message.msg.angle_min + message.msg.angle_increment * i;
                Vector3 point = PositionManager.ConvertLidarToUnityVector(range, angle);

                point = new Vector3(
                    Mathf.Round(point.x / pointScale) * pointScale,
                    Mathf.Round(point.y / pointScale) * pointScale,
                    Mathf.Round(point.z / pointScale) * pointScale
                );

                float pointSquaredDistanceToOrigin = point.sqrMagnitude;
                bool tooClose = pointSquaredDistanceToOrigin < density * density ||
                                validPoints.Any(existingPoint =>
                                    (existingPoint - point).sqrMagnitude < density * density);

                if (tooClose) continue;

                validPoints.Add(point);
            }

            this.points = validPoints.ToArray();
        }
    }
}

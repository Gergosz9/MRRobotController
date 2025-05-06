namespace Assets.Scripts
{
    using Assets.Scripts.ROS.Data.Message;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    internal class LidarDisplay : MonoBehaviour
    {
        [SerializeField]
        private GameObject lidarPointPrefab;

        // Minimum distance between lidar points to be displayed in meters
        [SerializeField] 
        private float density = 5f;

        private List<GameObject> lidarPoints;
        private Vector3[] points = new Vector3[0];

        private void Start()
        {
            // Initialize the Lidar display
            if (lidarPointPrefab == null)
            {
                Debug.LogError("[LidarDisplay] Lidar point prefab is not assigned.");
                return;
            }

            lidarPoints = new List<GameObject>();

            for(int i= 0; i < 1000; i++)
            {
                GameObject point = Instantiate(lidarPointPrefab, transform);
                point.SetActive(false);
                lidarPoints.Add(point);
            }
        }

        private void FixedUpdate()
        {
            UpdatePoints(this.points);
        }

        public void UpdatePoints(Vector3[] points)
        {
            Vector3 lastPlacedPoint = Vector3.zero;

            int lidarIndex = 0;

            for (int i = 0; i < points.Length; i++)
            {
                lidarPoints[lidarIndex].transform.position = points[i];
                lidarPoints[lidarIndex].SetActive(true);
                lastPlacedPoint = points[i];
                lidarIndex++;
            }

            for (int i = lidarIndex; i < lidarPoints.Count; i++)
            {
                lidarPoints[i].SetActive(false);
            }
        }

        public void UpdatePointsTest(Vector3[] points)
        {
            foreach(Vector3 p in points)
            {
                //Debug.DrawRay(p, Vector3.up);
            }
        }

        public void displayScan(RosMessage<ScanMsg> message)
        {
            Vector3[] points = new Vector3[message.msg.ranges.Length];

            for (int i = 0; i < message.msg.ranges.Length; i++)
            {
                if (!message.msg.ranges[i].HasValue)
                {
                    continue;
                }
                float range = message.msg.ranges[i].Value;
                float angle = message.msg.angle_min + message.msg.angle_increment * i;
                Vector3 rospoint = new Vector3(
                    range * Mathf.Cos(angle),
                    0,
                    range * Mathf.Sin(angle)
                );

                Vector3 unitypoint = rospoint;

                bool istooclose = false;

                for(int j=0;j<points.Length;j++)
                {
                    if (Vector3.Distance(points[j], unitypoint) < density)
                    {
                        istooclose = true;
                        break;
                    }
                }

                if (istooclose)
                {
                    continue;
                }
                points[i] = unitypoint;
            }

            this.points = points;
        }
    }
}

namespace Assets.Scripts
{
    using Assets.Scripts.ROS.Data.Message;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;


    /// <summary>
    /// LidarDisplay is a Unity MonoBehaviour that displays Lidar points in the scene recieved from the ROS.
    /// </summary>
    internal class LidarDisplay : MonoBehaviour
    {
        [SerializeField]
        private GameObject lidarPointPrefab;

        // Minimum distance between lidar points to be displayed in meters
        [SerializeField] 
        private float density = 5f;

        private List<GameObject> lidarPoints = new List<GameObject>();
        private Vector3[] points = new Vector3[0];
        private bool initialized = false;
        private bool enabled = false;
        public void Enable()
        {
            if(!initialized)
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
            if (lidarPointPrefab == null)
            {
                Debug.LogError("[LidarDisplay] Lidar point prefab is not assigned.");
                return;
            }

            for(int i= 0; i < 1000; i++)
            {
                GameObject point = Instantiate(lidarPointPrefab, transform);
                point.SetActive(false);
                lidarPoints.Add(point);
            }

            initialized = true;
        }

        private void FixedUpdate()
        {
            if (enabled)
                DisplayPoints(points);
            else
                lidarPoints.ForEach(point => point.SetActive(false));

        }

        private void DisplayPoints(Vector3[] points)
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

        public void UpdatePoints(RosMessage<ScanMsg> message)
        {
            if (initialized)
            {
                List<Vector3> validPoints = new List<Vector3>();

                for (int i = 0; i < message.msg.ranges.Length; i++)
                {
                    if (!message.msg.ranges[i].HasValue)
                    {
                        continue;
                    }

                    float range = message.msg.ranges[i].Value;
                    float angle = message.msg.angle_min + message.msg.angle_increment * i;
                    Vector3 point = PositionManager.TranslateLidarToUnityVector(range, angle);

                    bool istooclose = validPoints.Any(existingPoint =>
                        Vector3.Distance(existingPoint, point) < density ||
                        Vector3.Distance(point, Vector3.zero) < density);

                    if (istooclose)
                    {
                        continue;
                    }

                    validPoints.Add(point);
                }

                this.points = validPoints.ToArray();
            }
        }
    }
}

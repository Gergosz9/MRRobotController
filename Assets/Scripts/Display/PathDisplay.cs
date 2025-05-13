using Assets.Scripts.ROS.Data.Message;
using Assets.Scripts.ROS.Data.Message.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;

internal class PathDisplay : MonoBehaviour
{
    private new bool enabled = false;

    [SerializeField]
    private LineRenderer lineRenderer;

    private List<Vector3> pathPoints = new List<Vector3>();

    public void Enable()
    {
        enabled = true;
        lineRenderer.enabled = true;
    }

    public void Disable()
    {
        enabled = false;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (enabled)
        {
            lineRenderer.positionCount = pathPoints.Count;
            for (int i = 0; i < pathPoints.Count; i++)
            {
                lineRenderer.SetPosition(i, pathPoints[i]);
            }
        }

    }

    public void UpdatePath(RosMessage<PathMsg> plan)
    {
        List<Vector3> pathPoints = new List<Vector3>();
        for (int i = 0; i < plan.msg.poses.Length; i++)
        {
            Vector3 current = PositionManager.ConvertToUnityVector(plan.msg.poses[i].pose.position);

            pathPoints.Add(current);
        }
        this.pathPoints = pathPoints;
    }
}

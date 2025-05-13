using Assets.Scripts.ROS.Data.Message;
using UnityEngine;

internal class PathDisplay : MonoBehaviour
{
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
        initialized = true;
    }

    private void Update()
    {
        if (!enabled || !initialized) return;
        // Update the path display here

    }

    public void UpdatePath(RosMessage<PlanMsg> plan)
    {
        for (int i = 0; i < plan.msg.poses.Length; i++)
        {
            Vector3 current = PositionManager.TranslateToUnityPoint(plan.msg.poses[i].position, PositionManager.Odom);

            Debug.DrawLine(current, current + Vector3.up * 0.1f, Color.red, 5000f);
        }
    }
}

using Assets.Scripts.ROS.Data.Message;
using Assets.Scripts.ROS.Data.Message.Primitives;
using Assets.Scripts.ROS.Network;
using Newtonsoft.Json;
using System;
using UnityEngine;
using Time = Assets.Scripts.ROS.Data.Message.Primitives.Time;


/// <summary>
/// PositionManager is responsible for managing the position of the robot in Unity and translating between Unity and ROS coordinate systems.
/// Contains methods to convert between Unity and ROS poses and vectors.
/// </summary>
public class PositionManager : MonoBehaviour
{
    [SerializeField]
    static Pose relativeCenter = new Pose(Vector3.zero,Quaternion.identity);

    [SerializeField]
    private static WebSocketClient webSocketClient;

    public Pose RelativeCenter
    {
        get { return relativeCenter; }
        set { relativeCenter = value; }
    }

    Pose currentPose;
    public Pose CurrentPose
    {
        get { return currentPose; }
        set
        {
            currentPose = value;
        }
    }

    void Start()
    {
        currentPose = new Pose();
    }

    public static void SendRobotTo(Pose goal)
    {
        Pose convertedGoal = ConvertToROSPose(goal);

        GoalPoseMsg msg = msg = new GoalPoseMsg
        {
            header = new Header
            {
                stamp = new Time
                {
                    sec = (uint)DateTime.Now.Second,
                    nanosec = (uint)DateTime.Now.Millisecond * 1000
                },
                frame_id = "base_link"
            },
            pose = convertedGoal
        };
        string message = JsonConvert.SerializeObject(new RosMessage<GoalPoseMsg>("publish", "/goal_pose", msg));


        webSocketClient.SendMessage(message);
        Debug.Log("Sending goal: " + convertedGoal.ToString());

    }

    public static Pose ConvertToUnityPose(Pose pose)
    {
        Pose unityPose = new Pose(ConvertToUnityVector(pose.position), ConvertToUnityQuarternion(pose.rotation));
        return unityPose;
    }

    public static Pose ConvertToROSPose(Pose pose)
    {
        Pose rosPose = new Pose(ConvertToROSVector(pose.position), ConvertToROSQuaternion(pose.rotation));
        return rosPose;
    }

    public static Vector3 ConvertToUnityVector(Vector3 vector)
    {
        Vector3 unityVector = new Vector3(vector.y, vector.z, -vector.x);
        return relativeCenter.position + unityVector;
    }

    public static Vector3 ConvertToROSVector(Vector3 vector)
    {
        vector -= relativeCenter.position;
        Vector3 rosVector = new Vector3(-vector.z, vector.x, vector.y);
        return rosVector;
    }

    public static Quaternion ConvertToUnityQuarternion(Quaternion quaternion)
    {
        Quaternion unityQuaternion = new Quaternion(quaternion.y, quaternion.z, -quaternion.x, quaternion.w);
        return relativeCenter.rotation * unityQuaternion;
    }

    public static Quaternion ConvertToROSQuaternion(Quaternion quaternion)
    {
        quaternion = Quaternion.Inverse(relativeCenter.rotation) * quaternion;
        Quaternion rosQuaternion = new Quaternion(-quaternion.z, quaternion.x, quaternion.y, quaternion.w);
        return rosQuaternion;
    }

    public static Vector3 ConvertLidarToUnityVector(float range, float angle)
    {
        Vector3 rospoint = new Vector3(
            range * Mathf.Cos(angle),
            range * Mathf.Sin(angle),
            0f
        );

        return ConvertToUnityVector(rospoint);
    }
}

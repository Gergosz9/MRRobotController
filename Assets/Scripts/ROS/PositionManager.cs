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
    // <summary>
    // Pose is a 3D pose in Unity coordinates, the position is offset by the relative center and the rotation is rotated by the relative center
    // </summary>
    public static Pose ConvertToUnityPose(Pose pose)
    {
        Pose unityPose = new Pose(ConvertToUnityVector(pose.position), ConvertToUnityQuarternion(pose.rotation));
        return unityPose;
    }
    // <summary>
    // Pose is a 3D pose in Unity coordinates, the position is offset by the relative center and the rotation is rotated by the relative center
    // </summary>
    public static Pose ConvertToROSPose(Pose pose)
    {
        Pose rosPose = new Pose(ConvertToROSVector(pose.position), ConvertToROSQuaternion(pose.rotation));
        return rosPose;
    }
    // <summary>
    // Vector3 is a 3D vector in Unity coordinates, it's only offset by the relative center
    // </summary>
    public static Vector3 ConvertToUnityVector(Vector3 vector)
    {
        Vector3 unityVector = new Vector3(vector.y, vector.z, -vector.x);
        return relativeCenter.position + unityVector;
    }
    // <summary>
    // Vector3 is a 3D vector in ROS coordinates, it's only offset by the relative center
    // </summary>
    public static Vector3 ConvertToROSVector(Vector3 vector)
    {
        vector -= relativeCenter.position;
        Vector3 rosVector = new Vector3(-vector.z, vector.x, vector.y);
        return rosVector;
    }
    //<summary>
    // Quaternion is a rotation in Unity coordinates, it's rotated by the relative center
    // </summary>
    public static Quaternion ConvertToUnityQuarternion(Quaternion quaternion)
    {
        Quaternion unityQuaternion = new Quaternion(quaternion.y, quaternion.z, -quaternion.x, quaternion.w);
        return relativeCenter.rotation * unityQuaternion;
    }
    // <summary>
    // Quaternion is a rotation in ROS coordinates, it's rotated by the relative center
    // </summary>
    public static Quaternion ConvertToROSQuaternion(Quaternion quaternion)
    {
        quaternion = Quaternion.Inverse(relativeCenter.rotation) * quaternion;
        Quaternion rosQuaternion = new Quaternion(-quaternion.z, quaternion.x, quaternion.y, quaternion.w);
        return rosQuaternion;
    }
    // <summary>
    // Point is a 3D point in ROS coordinates, it's rotated by the relative center then offset
    // </summary>
    public static Vector3 ConvertToUnityPoint(Vector3 point)
    {
        Vector3 unityDirection = new Vector3(point.y, point.z, -point.x);
        Vector3 rotatedDirection = relativeCenter.rotation * unityDirection;
        return relativeCenter.position + rotatedDirection;
    }
    // <summary>
    // Point is a 3D point in Unity coordinates, it's rotated by the relative center then offset
    // </summary>
    public static Vector3 ConvertToROSPoint(Vector3 point)
    {
        point -= relativeCenter.position;
        Vector3 rosDirection = new Vector3(-point.z, point.x, point.y);
        return Quaternion.Inverse(relativeCenter.rotation) * rosDirection;
    }

    public static Vector3 ConvertLidarToUnityVector(float range, float angle)
    {
        Vector3 rospoint = new Vector3(
            range * Mathf.Cos(angle),
            range * Mathf.Sin(angle),
            0f
        );

        return ConvertToUnityPoint(rospoint);
    }
}

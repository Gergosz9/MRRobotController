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
internal class PositionManager : MonoBehaviour
{
    [SerializeField]
    private WebSocketClient webSocketClient;
    [SerializeField]
    private GameObject positionPrefab;

    static Pose base_link;
    static Pose odom;
    static Pose world;

    private static GameObject baseLinkPrefab;
    private static GameObject odomPrefab;
    private static GameObject worldPrefab;

    public static void EnableVisualization(bool enable)
    {
        baseLinkPrefab.SetActive(enable);
        odomPrefab.SetActive(enable);
        worldPrefab.SetActive(enable);
    }

    static Pose baseLinkToOdomOffset;
    static Pose odomToWorldOffset;

    public void Awake()
    {
        baseLinkPrefab = Instantiate(positionPrefab, Vector3.zero, Quaternion.identity);
        odomPrefab = Instantiate(positionPrefab, Vector3.zero, Quaternion.identity);
        worldPrefab = Instantiate(positionPrefab, Vector3.zero, Quaternion.identity);
    }

    public static Pose Base_link
    {
        get { return base_link; }
        set
        {
            base_link = value;
            RecalculateOdomFromBaseLink();
            RecalculateWorldFromOdom();
        }
    }

    public static Pose Odom
    {
        get { return odom; }
    }

    public static Pose World
    {
        get { return world; }
    }

    public static Pose BaseLinkToOdomOffset
    {
        get { return baseLinkToOdomOffset; }
        set
        {
            baseLinkToOdomOffset = value;
            RecalculateBaseLinkFromOdom();
        }
    }

    public static Pose OdomToWorldOffset
    {
        get { return odomToWorldOffset; }
        set
        {
            odomToWorldOffset = value;
            RecalculateOdomFromWorld();
            RecalculateBaseLinkFromOdom();
        }
    }

    public static void RecalculateOdomFromBaseLink()
    {
        odom = ConvertToUnityPose(BaseLinkToOdomOffset, base_link);
        odomPrefab.transform.position = odom.position;
        odomPrefab.transform.rotation = odom.rotation;
    }

    public static void RecalculateWorldFromOdom()
    {
        world = ConvertToUnityPose(odomToWorldOffset, odom);
        worldPrefab.transform.position = world.position;
        worldPrefab.transform.rotation = world.rotation;
    }

    public static void RecalculateBaseLinkFromOdom()
    {
        var odomToBaseLinkOffset = new Pose(-BaseLinkToOdomOffset.position, Quaternion.Inverse(BaseLinkToOdomOffset.rotation));
        base_link = ConvertToUnityPose(odomToBaseLinkOffset, odom);
        baseLinkPrefab.transform.position = base_link.position;
        baseLinkPrefab.transform.rotation = base_link.rotation;
    }

    public static void RecalculateOdomFromWorld()
    {
        var worldToOdomOffset = new Pose(-OdomToWorldOffset.position, Quaternion.Inverse(OdomToWorldOffset.rotation));
        odom = ConvertToUnityPose(worldToOdomOffset, world);
        odomPrefab.transform.position = base_link.position;
        odomPrefab.transform.rotation = base_link.rotation;
    }

    public static Pose ConvertToROSPose(Pose pose)
    {
        Vector3 offset = pose.position - base_link.position;
        offset = Quaternion.Inverse(base_link.rotation) * offset;
        Vector3 shiftedVec = base_link.position + offset;
        shiftedVec = shiftedVec - base_link.position;
        Vector3 rosPosition = new Vector3(-shiftedVec.z, shiftedVec.x, shiftedVec.y);
        Quaternion rosRotation = Quaternion.Inverse(base_link.rotation) * pose.rotation;
        rosRotation = new Quaternion(-rosRotation.z, rosRotation.x, rosRotation.y, rosRotation.w);
        return new Pose(rosPosition, rosRotation);
    }

    public static Vector3 TranslateToUnityPoint(Vector3 point, Pose origin)
    {
        Vector3 shiftedPoint = point + origin.position;
        Vector3 offset = shiftedPoint - origin.position;
        offset = origin.rotation * offset;
        return origin.position + offset;
    }

    public static Pose ConvertToUnityPose(Pose pose, Pose origin)
    {
        Vector3 unityPosition = new Vector3(pose.position.y, pose.position.z, -pose.position.x);
        unityPosition = TranslateToUnityPoint(unityPosition, origin);
        Quaternion unityRotation = new Quaternion(pose.rotation.y, pose.rotation.z, -pose.rotation.x, pose.rotation.w);
        unityRotation = origin.rotation * unityRotation;
        return new Pose(unityPosition, unityRotation);
    }

    public static Vector3 ConvertLidarToUnityVector(float range, float angle)
    {
        Vector3 rospoint = new Vector3(
            range * Mathf.Cos(angle),
            range * Mathf.Sin(angle),
            0f
        );

        return TranslateToUnityPoint(rospoint, base_link);
    }

    public void SendRobotTo(Pose goal)
    {
        Pose convertedGoal = ConvertToROSPose(goal);
        convertedGoal.position.z = 0;

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

    public static void UpdateOffsets(TransformStamped[] transforms)
    {
        foreach (TransformStamped tf in transforms)
        {
            if (tf.child_frame_id == "base_link")
            {
                BaseLinkToOdomOffset = new Pose(tf.transform.translation, tf.transform.rotation);
            }
            else if (tf.child_frame_id == "odom")
            {
                OdomToWorldOffset = new Pose(tf.transform.translation, tf.transform.rotation);
            }
        }
    }
    //// <summary>
    //// Pose is a 3D pose in Unity coordinates, the position is offset by the relative center and the rotation is rotated by the relative center
    //// </summary>
    //public static Pose ConvertToUnityPose(Pose pose)
    //{
    //    Pose unityPose = new Pose(ConvertToUnityVector(pose.position), ConvertToUnityQuarternion(pose.rotation));
    //    return unityPose;
    //}
    //// <summary>
    //// Pose is a 3D pose in Unity coordinates, the position is offset by the relative center and the rotation is rotated by the relative center
    //// </summary>
    //public static Pose ConvertToROSPose(Pose pose)
    //{
    //    Pose rosPose = new Pose(ConvertToROSVector(pose.position), ConvertToROSQuaternion(pose.rotation));
    //    return rosPose;
    //}
    //// <summary>
    //// Vector3 is a 3D vector in Unity coordinates, it's only offset by the relative center
    //// </summary>
    //public static Vector3 ConvertToUnityVector(Vector3 vector)
    //{
    //    Vector3 unityDirection = new Vector3(vector.y, vector.z, -vector.x);
    //    Vector3 rotatedDirection = relativeCenter.rotation * unityDirection;
    //    return relativeCenter.position + rotatedDirection;
    //}
    //// <summary>
    //// Vector3 is a 3D vector in ROS coordinates, it's only offset by the relative center
    //// </summary>
    //public static Vector3 ConvertToROSVector(Vector3 vector)
    //{
    //    vector -= relativeCenter.position;
    //    vector = Quaternion.Inverse(relativeCenter.rotation) * vector;
    //    Vector3 rosDirection = new Vector3(-vector.z, vector.x, vector.y);
    //    return rosDirection;
    //}
    ////<summary>
    //// Quaternion is a rotation in Unity coordinates, it's rotated by the relative center
    //// </summary>
    //public static Quaternion ConvertToUnityQuarternion(Quaternion quaternion)
    //{
    //    Quaternion unityQuaternion = new Quaternion(quaternion.y, quaternion.z, -quaternion.x, quaternion.w);
    //    return relativeCenter.rotation * unityQuaternion;
    //}
    //// <summary>
    //// Quaternion is a rotation in ROS coordinates, it's rotated by the relative center
    //// </summary>
    //public static Quaternion ConvertToROSQuaternion(Quaternion quaternion)
    //{
    //    quaternion = Quaternion.Inverse(relativeCenter.rotation) * quaternion;
    //    Quaternion rosQuaternion = new Quaternion(-quaternion.z, quaternion.x, quaternion.y, quaternion.w);
    //    return rosQuaternion;
    //}
}

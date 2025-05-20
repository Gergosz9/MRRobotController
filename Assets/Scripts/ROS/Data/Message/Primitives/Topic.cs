namespace Assets.Scripts.ROS.Data.Message.Primitives
{
    /// <summary>
    /// Defines the topic names used in ROS messages.
    /// </summary>
    internal static class Topic
    {
        public const string scan = "/scan";
        public const string scansim = "/scan_sim";
        public const string costmap = "/global_costmap/costmap";
        public const string goalpose = "/goal_pose";
        public const string plan = "/plan";
        public const string plansmoothed = "/plan_smoothed";
        public const string globalpath = "/global_path"; //plan
        public const string tf = "/tf";
    }
}

using Assets.Scripts.ROS.Data.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class CostMapDisplay : MonoBehaviour
{

    [SerializeField]
    private float pointScale = 0.5f;
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

    public void UpdateCostMap(RosMessage<CostMapMsg> costmap)
    {
        uint width = costmap.msg.info.width;
        uint height = costmap.msg.info.height;
        float resolution = costmap.msg.info.resolution;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Calculate the index in the 1D data array
                int index = y * (int)width + x;
                int cost = costmap.msg.data[index];

                Vector3 origin = PositionManager.ConvertToUnityVector(costmap.msg.info.origin.position);

                float wx = origin.x + y * resolution;
                float wy = origin.y;
                float wz = origin.z - x * resolution;

                if (enabled)
                {
                    Color color = Color.white;
                    switch (cost)
                    {
                        case 0:
                            color = Color.white;
                            break;
                        case -1:
                            color = Color.red;
                            break;
                        case 99:
                        case 100:
                            color = Color.blue;
                            break;
                        default:
                            color = Color.black;
                            break;
                    }
                    // Draw a line for each cell (for example, vertical line representing cost)
                    Debug.DrawLine(
                            new Vector3(wx, wy, wz),
                            new Vector3(wx, wy + 0.1f, wz),
                            color,
                            5000f
                        );
                }
            }
        }
    }
}

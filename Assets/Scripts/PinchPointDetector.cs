using MixedReality.Toolkit;
using MixedReality.Toolkit.Input;
using MixedReality.Toolkit.Subsystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


/// <summary>
/// PinchPointDetector is responsible for detecting pinch gestures using the MRTK HandsAggregatorSubsystem.
/// It uses the MRTKRayInteractor to determine the pinch point and updates a preview pointer.
/// </summary>
public class PinchPointDetector : MonoBehaviour
{
    private HandsAggregatorSubsystem aggregator;
    [SerializeField]
    private MRTKRayInteractor rayInteractorRight;
    [SerializeField]
    private MRTKRayInteractor rayInteractorLeft;
    [SerializeField]
    private GameObject previewPointer;

    private void Start()
    {
        aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
        if (aggregator == null)
        {
            Debug.LogError("[PinchPointDetector] HandsAggregatorSubsystem not found or not running.");
        }
    }

    private void FixedUpdate()
    {
        if (aggregator != null)
        {
            DetectPinch();
        }
    }

    bool isActivelyPinching = false;
    public void DetectPinch()
    {
        bool handIsValidLeft = aggregator.TryGetPinchProgress(XRNode.LeftHand, out bool isReadyToPinchLeft, out bool isPinchingLeft, out float pinchAmountLeft);
        bool handIsValidRight = aggregator.TryGetPinchProgress(XRNode.RightHand, out bool isReadyToPinchRight, out bool isPinchingRight, out float pinchAmountRight);

        if ((handIsValidLeft && isPinchingLeft))
        {
            DetectPoint(rayInteractorLeft);
            isActivelyPinching = true;
        }

        if ((handIsValidRight && isPinchingRight))
        {
            DetectPoint(rayInteractorRight);
            isActivelyPinching = true;
        }

        if(!isPinchingLeft && !isPinchingRight)
        {
            previewPointer.SetActive(false);
            if (isActivelyPinching)
            {
                //PositionManager.SendRobotTo(new Pose(rayInteractorLeft.rayEndPoint, Quaternion.identity));
                Debug.Log("[PinchPointDetector] Pinch detected.");
            }
            isActivelyPinching = false;
        }
    }

    public void DetectPoint(MRTKRayInteractor rayInteractor)
    {
        Vector3 v = rayInteractor.rayEndPoint;
        previewPointer.transform.position = v;
        previewPointer.SetActive(true);

        // TODO: Add logic to handle the detected point
    }
}

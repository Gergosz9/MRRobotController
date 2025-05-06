using MixedReality.Toolkit;
using MixedReality.Toolkit.Input;
using MixedReality.Toolkit.Subsystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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

    public void DetectPinch()
    {
        bool handIsValidLeft = aggregator.TryGetPinchProgress(XRNode.LeftHand, out bool isReadyToPinchLeft, out bool isPinchingLeft, out float pinchAmountLeft);
        bool handIsValidRight = aggregator.TryGetPinchProgress(XRNode.RightHand, out bool isReadyToPinchRight, out bool isPinchingRight, out float pinchAmountRight);

        if ((handIsValidLeft && isPinchingLeft))
        {
            Debug.Log("Pinch detected!");
            DetectPoint(rayInteractorLeft);
        }

        if ((handIsValidRight && isPinchingRight))
        {
            Debug.Log("Pinch detected!");
            DetectPoint(rayInteractorRight);
        }

        if(!isPinchingLeft && !isPinchingRight)
        {
            previewPointer.SetActive(false);
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

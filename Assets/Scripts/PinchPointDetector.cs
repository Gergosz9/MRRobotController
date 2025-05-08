using MixedReality.Toolkit;
using MixedReality.Toolkit.Input;
using MixedReality.Toolkit.Subsystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;


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
            if (DetectPinch())
            {
                CalculatePreviewPointerTransform(DetectPinchLocation());
            }
            else
            {
                previewPointer.SetActive(false);
            }
        }
    }

    public MRTKRayInteractor DetectPinchLocation()
    {
        bool handIsValidLeft = aggregator.TryGetPinchProgress(XRNode.LeftHand, out bool isReadyToPinchLeft, out bool isPinchingLeft, out float pinchAmountLeft);
        bool handIsValidRight = aggregator.TryGetPinchProgress(XRNode.RightHand, out bool isReadyToPinchRight, out bool isPinchingRight, out float pinchAmountRight);

        if ((handIsValidLeft && isPinchingLeft))
            return rayInteractorLeft;

        if ((handIsValidRight && isPinchingRight))
            return rayInteractorRight;

        return null;
    }

    public bool DetectPinch()
    {
        bool handIsValidLeft = aggregator.TryGetPinchProgress(XRNode.LeftHand, out bool isReadyToPinchLeft, out bool isPinchingLeft, out float pinchAmountLeft);
        bool handIsValidRight = aggregator.TryGetPinchProgress(XRNode.RightHand, out bool isReadyToPinchRight, out bool isPinchingRight, out float pinchAmountRight);
        if ((handIsValidLeft && isPinchingLeft) || (handIsValidRight && isPinchingRight))
        {
            return true;
        }
        return false;
    }

    bool isPinchHeld = false;
    public bool DetectPinchRelease()
    {
        if(DetectPinch())
        {
            isPinchHeld = true;
        }
        else if (isPinchHeld)
        {
            isPinchHeld = false;
            return true;
        }

        return false;
    }

    public void CalculatePreviewPointerTransform(MRTKRayInteractor rayInteractor)
    {
        Vector3 v = rayInteractor.rayEndPoint;
        previewPointer.transform.position = v;
        Vector3 q = rayInteractor.rayEndPoint - rayInteractor.rayOriginTransform.position;
        q.y = 0;
        previewPointer.transform.rotation = Quaternion.LookRotation(q);
        previewPointer.SetActive(true);
    }
}

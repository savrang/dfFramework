using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform cameraTransform;
    private Transform _target;

// The distance in the x-z plane to the target

    public float distance = 7.0f;
    public float centerOffsetDistance = -3.0f;

    // the height we want the camera to be above the target
    public float height = 3.0f;

    public float angularSmoothLag = 0.3f;
    public float angularMaxSpeed = 15.0f;

    //var heightSmoothLag = 0.3;

    //var clampHeadPositionScreenSpace = 7.75;

    private Vector3 headOffset = Vector3.zero;
    private Vector3 centerOffset = Vector3.zero;

    private float heightVelocity = 0.0f;
    private float angleVelocity = 0.0f;
    private float targetHeight = 100000.0f;

    public float cameraDistance = 0.0f;
    private float miniDistance = -5.0f;
    private float maxDistance = 5.0f;

    void Awake()
    {


        if (!cameraTransform && Camera.main)
            cameraTransform = Camera.main.transform;
        if (!cameraTransform)
        {
            Debug.Log("Please assign a camera to the ThirdPersonCamera script.");
            enabled = false;
        }


        _target = transform;

        centerOffset = transform.forward * centerOffsetDistance;

        //Cut(_target, centerOffset);
        Apply(transform, Vector3.zero);
    }

    void DebugDrawStuff()
    {
        Debug.DrawLine(_target.position, _target.position + headOffset);

    }

    float AngleDistance(float a, float b)
    {
        a = Mathf.Repeat(a, 360);
        b = Mathf.Repeat(b, 360);

        return Mathf.Abs(b - a);
    }

    void Apply(Transform dummyTarget, Vector3 dummyCenter)
    {
        Vector3 targetCenter = _target.position + centerOffset;
        Vector3 targetHead = _target.position + headOffset;

        //	DebugDrawStuff();

        // Calculate the current & target rotation angles
        float originalTargetAngle = _target.eulerAngles.y;
        float currentAngle = cameraTransform.eulerAngles.y;

        // Adjust real target angle when camera is locked
        float targetAngle = originalTargetAngle;

        currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref angleVelocity, angularSmoothLag, angularMaxSpeed);

        targetHeight = targetCenter.y + height;

        // Damp the height
        float currentHeight = targetHeight;//cameraTransform.position.y;
                                           //currentHeight = Mathf.SmoothDamp (currentHeight, targetHeight, heightVelocity, heightSmoothLag);

        // Convert the angle into a rotation, by which we then reposition the camera
        Quaternion currentRotation = Quaternion.Euler(0, currentAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        cameraTransform.position = targetCenter;
        cameraTransform.position += currentRotation * Vector3.back * distance;

        // Set the height of the camera
        cameraTransform.position = new Vector3(cameraTransform.position.x, currentHeight, cameraTransform.position.z);

        // Always look at the target	
        //SetUpRotation(targetCenter, targetHead);

        Vector3 cameraZoomDir = transform.position - cameraTransform.position;
        cameraZoomDir.Normalize();


        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            cameraDistance += 0.5f;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cameraDistance -= 0.5f;
        }

        cameraDistance = Mathf.Clamp(cameraDistance, miniDistance, maxDistance);

        cameraTransform.position += (cameraZoomDir * cameraDistance);
    }

    void LateUpdate()
    {
        Apply(transform, Vector3.zero);
    }

    void Cut(Transform dummyTarget, Vector3 dummyCenter)
    {
        //var oldHeightSmooth = heightSmoothLag;

        //heightSmoothLag = 0.001;

        Apply(transform, Vector3.zero);

        //heightSmoothLag = oldHeightSmooth;
    }

    void SetUpRotation(Vector3 centerPos, Vector3 headPos)
    {
        // Now it's getting hairy. The devil is in the details here, the big issue is jumping of course.
        // * When jumping up and down we don't want to center the guy in screen space.
        //  This is important to give a feel for how high you jump and avoiding large camera movements.
        //   
        // * At the same time we dont want him to ever go out of screen and we want all rotations to be totally smooth.
        //
        // So here is what we will do:
        //
        // 1. We first find the rotation around the y axis. Thus he is always centered on the y-axis
        // 2. When grounded we make him be centered
        // 3. When jumping we keep the camera rotation but rotate the camera to get him back into view if his head is above some threshold
        // 4. When landing we smoothly interpolate towards centering him on screen
        var cameraPos = cameraTransform.position;
        var offsetToCenter = centerPos - cameraPos;

        // Generate base rotation only around y-axis
        Quaternion yRotation = Quaternion.LookRotation(new Vector3(offsetToCenter.x, 0, offsetToCenter.z));

        Vector3 relativeOffset = Vector3.forward * distance + Vector3.down * height;
        cameraTransform.rotation = yRotation * Quaternion.LookRotation(relativeOffset);


        /*
            // Calculate the projected center position and top position in world space
            var centerRay = cameraTransform.camera.ViewportPointToRay(Vector3(.5, 0.5, 1));
            var topRay = cameraTransform.camera.ViewportPointToRay(Vector3(.5, clampHeadPositionScreenSpace, 1));

            var centerRayPos = centerRay.GetPoint(distance);
            var topRayPos = topRay.GetPoint(distance);

            var centerToTopAngle = Vector3.Angle(centerRay.direction, topRay.direction);

            var heightToAngle = centerToTopAngle / (centerRayPos.y - topRayPos.y);

            var extraLookAngle = heightToAngle * (centerRayPos.y - centerPos.y);
            if (extraLookAngle < centerToTopAngle)
            {
                extraLookAngle = 0;
            }
            else
            {
                extraLookAngle = extraLookAngle - centerToTopAngle;
                cameraTransform.rotation *= Quaternion.Euler(-extraLookAngle, 0, 0);
            }
        */
    }

    Vector3 GetCenterOffset()
    {
        return centerOffset;
    }
}

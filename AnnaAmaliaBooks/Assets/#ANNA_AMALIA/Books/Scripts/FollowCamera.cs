using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class FollowCamera : MonoBehaviour
{
    [BoxGroup("Settings")]

    [ToggleGroup("Settings/enableMovement", "Movement")]
    [SerializeField]
    private bool enableMovement = true;

    [ToggleGroup("Settings/enableMovement")]
    [SerializeField]
    private float followMoveSpeed = 2.0f;

    [ToggleGroup("Settings/enableRotation", "Rotation")]
    [SerializeField]
    public bool enableRotation = true;

    [ToggleGroup("Settings/enableRotation")]
    [SerializeField]
    private float followRotationSpeed = 45f;

    [BoxGroup("Settings")]
    [SerializeField]
    private Vector2 distanceToCameraMinMax = new Vector2(1.0f, 3.0f);

    [BoxGroup("Settings")]
    [SerializeField]
    [Range(0, 1)]
    private float isOutOfViewThreshhold = 0.1f;

    //private Camera cam;

    [BoxGroup("Debug")]
    [ShowInInspector]
    [ReadOnly]
    public bool isMoving { get => Vector3.Distance(targetPosition, transform.position) > 0.01f; }

    [BoxGroup("Debug")]
    [ShowInInspector]
    [ReadOnly]
    public bool isRotating { get => dotProduct < 0.99999f; }

    [BoxGroup("Debug")]
    [ShowInInspector]
    [ReadOnly]
    public bool isOutOfView { get => IsTargetOutOfView(); }

    private Vector3 targetPosition = Vector3.zero;
    private Vector3 cameraLookDirection = Vector3.zero;
    private Vector3 lookDirection = Vector3.zero;
    Quaternion targetRotation = Quaternion.identity;
    private float distanceToCamera { get => (Camera.main == null) ? 0.0f : Vector3.Distance(Camera.main.transform.position, transform.position); }
    private float dotProduct = 0.0f;

    private Coroutine currentMoveCoroutine;
    private Coroutine currentRotateCoroutine;

    private void Update()
    {
        if (Camera.main == null) return;

        cameraLookDirection = (transform.position - Camera.main.transform.position).normalized;
        dotProduct = Vector3.Dot(cameraLookDirection, Camera.main.transform.forward);


        if (isOutOfView)
        {
            CalculateRotationValues();
            CalculatePositionValues();

            if (currentMoveCoroutine == null)
                currentMoveCoroutine = StartCoroutine(MoveIntoCameraSightCoroutine(0.2f));

            if (currentRotateCoroutine == null)
                currentRotateCoroutine = StartCoroutine(RotateTowardsCameraCoroutine(0.2f));
        }
    }
    public void StopMoving()
    {
        if (currentMoveCoroutine == null) return;

        StopCoroutine(currentMoveCoroutine);
        currentMoveCoroutine = null;
    }


    public void StopRotating()
    {
        if (currentRotateCoroutine == null) return;

        StopCoroutine(currentRotateCoroutine);
        currentRotateCoroutine = null;
    }

    private void CalculatePositionValues()
    {
        float targetDistance = Mathf.Clamp(distanceToCamera, distanceToCameraMinMax.x, distanceToCameraMinMax.y);
        targetPosition = Camera.main.transform.position + Camera.main.transform.forward * targetDistance;

        //Debug.Log($"Target Position: {targetPosition} / Target Rotation: {targetRotation.eulerAngles}");
    }

    private void CalculateRotationValues()
    {
        lookDirection = (Camera.main.transform.position - targetPosition).normalized;
        targetRotation = Quaternion.LookRotation(lookDirection);
    }

    private void RotateTowardsCamera()
    {
        if (!enableRotation) return;

        //transform.DORotate(lookRotation.eulerAngles, 1.0f).OnStart(() => isMoving = true).OnComplete(() => isMoving = false);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * followRotationSpeed);
    }

    private void MoveIntoCameraSight()
    {
        if (!enableMovement) return;

        transform.position = Vector3.Slerp(transform.position, targetPosition, Time.deltaTime * followMoveSpeed);
    }

    private bool IsTargetOutOfView()
    {
        if (Camera.main == null) return false;

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        bool value = (pos.x < 0 - isOutOfViewThreshhold || pos.x > 1 + isOutOfViewThreshhold || pos.y < 0 - isOutOfViewThreshhold || pos.y > 1 + isOutOfViewThreshhold);
        return value;
    }

    IEnumerator MoveIntoCameraSightCoroutine(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);

        while (enableMovement && isMoving)
        {
            MoveIntoCameraSight();
            yield return null;
        }

        currentMoveCoroutine = null;
    }

    IEnumerator RotateTowardsCameraCoroutine(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);

        while (enableRotation && isRotating)
        {
            RotateTowardsCamera();
            yield return null;
        }

        currentRotateCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, lookDirection);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetPosition, 0.25f);
    }
}

using System;
using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{
    public Transform target;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;
    private float originalY;
    [SerializeField]
    private Transform cameraTransform;
    bool targetSet = false;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        targetSet = true;
        if (newTarget == null)
        {
            targetSet = false;
        }
        m_LastTargetPosition = target.position;
        m_OffsetZ = (cameraTransform.position - target.position).z;
        originalY = cameraTransform.position.y;
    }

    // Update is called once per frame
    private void Update()
    {
        if (targetSet)
        {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }
            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.up * m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(cameraTransform.position, aheadTargetPos, ref m_CurrentVelocity, damping);
            newPos.y = originalY;

            cameraTransform.position = newPos;

            m_LastTargetPosition = target.position;
        }
    }
}

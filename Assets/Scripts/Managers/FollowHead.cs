using UnityEngine;

/// <summary>
/// 让一个UI根物体跟随头部（相机）
/// 用于暂停面板/菜单等：始终保持在视野前方固定距离，并面向头部
/// </summary>
public class FollowHeadUI : MonoBehaviour
{
    [Header("Target (Head/Camera)")]
    [Tooltip("一般拖 XR Origin 里的 Main Camera")]
    public Transform head;

    [Header("Position")]
    [Tooltip("UI距离头部的前方距离（米）")]
    public float distance = 1.6f;

    [Tooltip("UI相对头部的高度偏移（米）。负值表示稍微低一点更舒服")]
    public float heightOffset = -0.15f;

    [Tooltip("允许UI跟随头部的最大水平角度（度）。超过则快速拉回到前方")]
    public float maxYawAngle = 60f;

    [Header("Smoothing")]
    [Tooltip("位置跟随速度（越大越跟手）")]
    public float positionLerp = 12f;

    [Tooltip("旋转跟随速度（越大越跟手）")]
    public float rotationLerp = 16f;

    [Header("Behavior")]
    [Tooltip("只在该物体处于激活状态时跟随（建议开启，避免平时也动）")]
    public bool onlyWhenChildrenActive = true;

    private void Reset()
    {
        // 尝试自动找主相机
        if (Camera.main != null) head = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (head == null) return;

        if (onlyWhenChildrenActive && !HasAnyActiveChild())
            return;

        // 计算头部水平前方向（去掉上下俯仰，避免UI跟着点头抖）
        Vector3 forward = head.forward;
        forward.y = 0f;
        if (forward.sqrMagnitude < 0.0001f) forward = Vector3.forward;
        forward.Normalize();

        Vector3 toUI = (transform.position - head.position);
        toUI.y = 0f;

        // 如果UI跑到侧面太多，则强制回到正前方
        if (toUI.sqrMagnitude > 0.0001f)
        {
            float yaw = Vector3.SignedAngle(forward, toUI.normalized, Vector3.up);
            if (Mathf.Abs(yaw) > maxYawAngle)
            {
                // 直接把目标方向拉回到正前方
                toUI = forward * distance;
            }
        }

        Vector3 targetPos = head.position + forward * distance + Vector3.up * heightOffset;

        // 位置平滑
        transform.position = Vector3.Lerp(transform.position, targetPos, 1f - Mathf.Exp(-positionLerp * Time.unscaledDeltaTime));

        // 让UI面向头部（只绕Y轴）
        Vector3 lookDir = head.position - transform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude < 0.0001f) lookDir = -forward;

        Quaternion targetRot = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 1f - Mathf.Exp(-rotationLerp * Time.unscaledDeltaTime));
    }

    private bool HasAnyActiveChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
                return true;
        }
        return false;
    }
}

using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    public Transform target;
    public float speed = 1.5f;

    private void Update()
    {
        if (target == null) return;

        Vector3 to = target.position;
        Vector3 pos = transform.position;

        // 只在水平面移动（避免上下浮动）
        to.y = pos.y;

        transform.position = Vector3.MoveTowards(pos, to, speed * Time.deltaTime);

        // 面向目标（可选）
        Vector3 dir = (to - pos);
        if (dir.sqrMagnitude > 0.001f)
            transform.forward = dir.normalized;
    }
}

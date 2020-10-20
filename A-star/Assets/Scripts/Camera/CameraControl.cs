using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [HideInInspector] public new Camera camera;

    //缩放系数
    private float zoom = 1f;
    //摄像机大小限制
    public float minCameraSize = 4f, maxCameraSize = 10f;

    //移动速度
    public float moveSpeed = 30f;

    private void Start()
    {
        camera = GetComponent<Camera>();
        ResetCamera();
    }

    private void Update()
    {
        //缩放增量不为0就调整缩放
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0f)
        {
            AdjustZoom(zoomDelta);
        }

        //x或者y轴增量不为0就调整位置
        float xDelat = Input.GetAxis("Horizontal"), yDelta = Input.GetAxis("Vertical");
        if (xDelat != 0f || yDelta != 0f)
        {
            AdjustPosition(xDelat, yDelta);
        }
    }

    //缩放调整
    private void AdjustZoom(float delta)
    {
        //限制缩放系数在0-1之间
        zoom = Mathf.Clamp01(zoom - delta);
        //根据缩放在范围内插值
        camera.orthographicSize = Mathf.Lerp(minCameraSize, maxCameraSize, zoom);
    }

    //位置调整
    private void AdjustPosition(float xDelta,float yDelta)
    {
        //确定移动方向
        Vector3 direction = new Vector3(xDelta, yDelta, 0).normalized;
        //输入的最大值作为阻尼系数保持平滑感
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(yDelta));
        //计算移动距离
        float distance = moveSpeed * damping * Time.deltaTime;

        Vector3 position = camera.transform.position;
        position -= direction * distance;
        camera.transform.position = position;
    }

    //重置摄像机
    public void ResetCamera()
    {
        Vector3 position = transform.localPosition;
        position.x = 0;
        position.y = 0;
        transform.localPosition = position;

        camera.orthographicSize = maxCameraSize;
    }
}

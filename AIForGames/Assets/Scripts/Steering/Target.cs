using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static Vector2 target2D;
    public static Vector3 target3D;
    Camera mainCamera;

    [SerializeField]
    float radius;

    public float Radius { get { return radius; } }

    [SerializeField]
    bool positionToMouse;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        target2D = new(transform.position.x,transform.position.z);
        target3D = new(transform.position.x, 1.0f, transform.position.z);
    }

    void Update()
    {
        if (positionToMouse && Input.GetMouseButtonDown(1))
        {
            SetPositionTowardsMouseCursor();
        }
        else
        {
            transform.position = this.transform.position;
            target2D.Set(transform.position.x, transform.position.z);
            target3D.Set(transform.position.x, 1.0f, transform.position.z);
        }
    }

    private void SetPositionTowardsMouseCursor()
    {
        //Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = new Vector3(mousePosition.x, 0, mousePosition.z);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            transform.position = new Vector3(hit.point.x, 1.0f, hit.point.z);
            target2D.Set(hit.point.x, hit.point.z);
            target3D.Set(hit.point.x, 1.0f, hit.point.z);
            PlayerManager.instance.isTargetChanged = true;
        }
    }
}

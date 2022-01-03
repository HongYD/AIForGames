using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
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
    }

    void Update()
    {
        if (positionToMouse)
        {
            SetPositionTowardsMouseCursor();
        }
        else
        {
            transform.position = new Vector3(-10.0f, 0, 1.0f);
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
            transform.position = new Vector3(hit.point.x, 0, hit.point.z);
        }
    }
}

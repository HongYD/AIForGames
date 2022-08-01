using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private enum CannonState
    {
        None,
        Static,
        Fix,
        Shoot
    }

    public GameObject target;
    public GameObject bullet;
    public float muzzleV;
    public Vector3 gravity;
    private CannonState _state;
    private GameObject _cannon;
    private GameObject _muzzle;
    private GameObject _tube;
    private Vector3 _curFireDir;
    private float _angleDiffLimit = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        _state = CannonState.None;
        _cannon = this.gameObject;
        _muzzle = _cannon.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        _tube = _cannon.transform.GetChild(0).GetChild(1).gameObject;
        _curFireDir = _curFireDir = (_muzzle.transform.position - _tube.transform.position).normalized;
        _tube.transform.rotation = Quaternion.LookRotation(_curFireDir);
    }

    // Update is called once per frame
    void Update()
    {
        FixCannon();
    }

    Vector3 CalculateFiringSolution(Vector3 start, Vector3 end)
    {
        float ttt;
        Vector3 delta = start - end;
        float a = Vector3.Dot(gravity, gravity);
        float b = -4 * (Vector3.Dot(gravity, delta) + muzzleV * muzzleV);
        float c = 4 * Vector3.Dot(delta, delta);

        if (4 * a * c > b * b)
        {
            return Vector3.zero;
        }
        else
        {
            float time0 = Mathf.Sqrt((-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a));
            float time1 = Mathf.Sqrt((-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a));
            if (time0 < 0)
            {
                if (time1 < 0)
                {
                    return Vector3.zero;
                }
                else
                {
                    ttt = time1;
                }
            }
            else
            {
                if (time1 < 0)
                {
                    ttt = time0;
                }
                else
                {
                    ttt = Mathf.Min(time0, time1);
                }
            }
            return (2 * delta - gravity * ttt * ttt) / (2 * muzzleV * ttt);
        }
    }

    void ShootBullet()
    {
        GameObject.Instantiate(bullet, _muzzle.transform.position, Quaternion.identity);
        _state = CannonState.Shoot;
    }

    void FixCannon()
    {
        Vector3 newDir = CalculateFiringSolution(_muzzle.transform.position, target.transform.position);
        _tube.transform.rotation = Quaternion.LookRotation(-newDir);
        //Quaternion rotationAngle = Quaternion.FromToRotation(_curFireDir, newDir);
        //_tube.transform.transform.Rotate(rotationAngle.eulerAngles.x * Time.deltaTime, rotationAngle.eulerAngles.y * Time.deltaTime, 0);
        ////float rotateX = Mathf.Clamp(rotationAngle.eulerAngles.x * Time.deltaTime, -90.0f,90.0f);
        ////float rotateY = Mathf.Clamp(rotationAngle.eulerAngles.y * Time.deltaTime, -50.0f,50.0f);
        ////Debug.Log("Angle Diff: " + rotateX + " ," + rotateY);
        ////if (rotateX > 0.01f)
        ////{
        ////    _tube.transform.Rotate(Vector3.right, rotateX);
        ////}
        ////if(rotateY > 0.01f)
        ////{
        ////    _tube.transform.Rotate(Vector3.up, rotateY);
        ////}
        //_curFireDir = (_muzzle.transform.position - _tube.transform.position).normalized;

        //Debug.DrawRay(_muzzle.transform.position, newDir, Color.red);
        //Vector3 targetDirInXZ = new Vector3(newDir.x, 0, newDir.z);
        //Vector3 curDirInXZ = new Vector3(_curFireDir.x, 0, _curFireDir.z);

        //Vector3 targetDirInY = new Vector3(newDir.x, newDir.y, 0);
        //Vector3 curDirInY = new Vector3(newDir.x, _curFireDir.y, 0);

        //float angleDiffInXZ = Vector3.SignedAngle(curDirInXZ, targetDirInXZ, Vector3.up) * Time.deltaTime;
        //float angleDiffinY = Vector3.SignedAngle(curDirInY, targetDirInY, Vector3.right);

        //if (Mathf.Abs(angleDiffInXZ) > 0.01f)
        //{
        //    //_tube.transform.Rotate(Vector3.up, angleDiffInXZ, Space.World);
        //    _tube.transform.rotation = Quaternion.Euler(new Vector3(0, /*angleDiffInXZ + */_tube.transform.rotation.eulerAngles.y - angleDiffInXZ, 0));
        //    //_tube.transform.Rotate(Vector3.right, angleDiffinY, Space.World);
        //}

    }
}

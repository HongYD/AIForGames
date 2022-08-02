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
        Vector3 delta = end - start;
        float a = Vector3.Dot(gravity, gravity);
        float b = -4 * (muzzleV * muzzleV - Vector3.Dot(gravity, delta));
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
        Debug.DrawRay(_muzzle.transform.position, newDir, Color.red);
        _tube.transform.rotation = Quaternion.LookRotation(newDir);
    }
}

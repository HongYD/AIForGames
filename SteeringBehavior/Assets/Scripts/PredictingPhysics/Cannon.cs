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
    public float muzzleV;
    public Vector3 gravity;
    public int bulletTrajectoryLen;
    private CannonState _state;
    private GameObject _cannon;
    private GameObject _muzzle;
    private GameObject _tube;
    private GameObject _tail;
    public GameObject bulletPrefab;
    private Vector3 _curFireDir;
    private Vector3 _culcDir;
    private Vector3 _preCulcDir;
    private List<Vector3> _bulleyTrajectory;
    private List<Bullet> bullets;
    private float frameRate = 1.0f / 30.0f;
    [SerializeField]
    private float _time;
    [Range(0f, 1.0f)]
    public float speedOffSet = 0.1f;

    
    // Start is called before the first frame update
    void Start()
    {
        _state = CannonState.None;
        _cannon = this.gameObject;
        _muzzle = _cannon.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        _tail= _cannon.transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        _tube = _cannon.transform.GetChild(0).GetChild(1).gameObject;
        _curFireDir = (_muzzle.transform.position - _tail.transform.position).normalized;
        _tube.transform.rotation = Quaternion.LookRotation(_curFireDir);
        _culcDir = Vector3.zero;
        _preCulcDir = Vector3.zero;
        bullets = new List<Bullet>();
        _bulleyTrajectory = new List<Vector3>();
        _time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        FixCannon();
        CalculateBulletTrajectory();
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ShootBullet();
        }
        UpdateBullet();
    }

    Vector3 CalculateFiringSolution(Vector3 start, Vector3 end, out float ttt)
    {
        ttt = 0;
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
        Bullet b = new Bullet(_bulleyTrajectory, _muzzle.transform.position,bulletPrefab);
        bullets.Add(b);
    }

    void UpdateBullet()
    {
        if (bullets.Count > 0)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].bullet != null)
                {
                    bullets[i].OnUpdate();
                }
            }
        }
    }

    void FixCannon()
    {
        _culcDir = CalculateFiringSolution(_muzzle.transform.position, target.transform.position, out _time);
        _curFireDir = (_muzzle.transform.position - _tail.transform.position).normalized;
        Quaternion newRot = Quaternion.identity;
        if (_culcDir != Vector3.zero)
        {
            newRot = Quaternion.LookRotation(_culcDir);
        }
        else
        {
            newRot = Quaternion.LookRotation(_preCulcDir);
        }
        float rotInX = newRot.eulerAngles.x;
        float rotInY = newRot.eulerAngles.y;
        float rotInZ = newRot.eulerAngles.z;
        Vector2 targetXZ = new Vector2(target.transform.position.x, target.transform.position.z);
        Vector2 cannonXZ = new Vector2(this.transform.position.x, this.transform.position.z);
        if (Vector2.Distance(targetXZ, cannonXZ) > 4.0f && _culcDir != Vector3.zero)
        {
            _tube.transform.rotation = Quaternion.Euler(rotInX, rotInY, rotInZ);
            _preCulcDir = _culcDir;
        }
        Debug.DrawRay(_muzzle.transform.position, _culcDir, Color.red);
        Debug.DrawRay(_muzzle.transform.position, _curFireDir, Color.blue);
    }

    void CalculateBulletTrajectory()
    {
        if (_culcDir != Vector3.zero)
        {
            _bulleyTrajectory.Clear();
        }
        Vector3 p0 = _muzzle.transform.position;
        float t = 0;
        for (int i = 0; i < bulletTrajectoryLen && t <_time; i++)
        {
            t += _time * frameRate * speedOffSet;
            Vector3 pt = p0 + _culcDir * muzzleV * t + (gravity * t * t) / 2;
            _bulleyTrajectory.Add(pt);
        }        
    }

    private void OnDrawGizmos()
    {
        if (_bulleyTrajectory != null && _bulleyTrajectory.Count > 0)
        {
            for (int i = 0; i < _bulleyTrajectory.Count; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_bulleyTrajectory[i], 0.1f);
            }
        }
    }
}

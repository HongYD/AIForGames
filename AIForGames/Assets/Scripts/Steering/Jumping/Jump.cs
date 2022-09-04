using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum JumpStateMachine
{
    None,
    Move,
    Jump,
    Land
}
public class Jump : SteeringBase, ISubject
{
    public JumpPoint _jumpPoint;
    public Transform finalTarget;
    private bool _canAchieve = false;
    public float _maxYSpeed = 5.0f;
    private Kinematic _target;
    private List<Vector3> _jumpTrajectory;
    public Vector3 gravity = new Vector3(0, -9.8f, 0);
    private float time;
    [SerializeField]
    private JumpStateMachine _stateMachine;
    private bool _isReachedFirstTarget;
    private int _jumpIndex;
    public float jumpSpeed = 2.0f;
    private List<GameObject> _observers = new List<GameObject>();
    private GameObject Creator;

    private void OnEnable()
    {
        Creator = GameObject.Find("Creator");
        Attach(Creator);

    }

    private void Start()
    {
        _jumpTrajectory = new List<Vector3>();
        _stateMachine = JumpStateMachine.Move;
        this.GetComponent<Arrive>().target = _jumpPoint.jumpLocation;
    }
    private void Update()
    {
        if(_stateMachine == JumpStateMachine.Move)
        {
            if(!_isReachedFirstTarget && GetXZDistance(_jumpPoint.jumpLocation.position) < 0.1f)
            {
                _isReachedFirstTarget = true;
                _stateMachine = JumpStateMachine.Jump;
                _jumpIndex = 0;
            }
            if(_isReachedFirstTarget && GetXZDistance(finalTarget.position) < 0.1f)
            {
                Notify();
                Destroy(this.gameObject);
            }
        }
        else if(_stateMachine == JumpStateMachine.Jump)
        {
            GetSteeringOutput();
            if(_isReachedFirstTarget && ((this.transform.position - _jumpPoint.landingLocation.position).magnitude < 0.1f
                || _jumpIndex >= _jumpTrajectory.Count))
            {
                _stateMachine = JumpStateMachine.Land;
            }
        }
        else if (_stateMachine == JumpStateMachine.Land)
        {
            _jumpIndex = 0;
            _jumpTrajectory.Clear();
            this.GetComponent<Arrive>().target = finalTarget;
            _stateMachine = JumpStateMachine.Move;
        }
        else
        {

        }
    }

    protected override void GetSteeringOutput()
    {
        _target = CalculateTarget();
        if (_canAchieve && _jumpTrajectory.Count <= 0)
        {
            ScheduleJumpAction(_jumpTrajectory);
        }
        
    }

    private void LateUpdate()
    {
        if (_jumpTrajectory.Count > 0 && _jumpIndex < _jumpTrajectory.Count)
        {
            this.transform.position = _jumpTrajectory[_jumpIndex];
            _jumpIndex++;
        }
    }

    private Kinematic CalculateTarget()
    {
        _target = new Kinematic();
        _target.position = _jumpPoint.jumpLocation.position;
        float sqrtTerm = Mathf.Sqrt(2 * Mathf.Abs(gravity.y) * _jumpPoint.deltaPosition.y + _maxYSpeed * _maxYSpeed);
        time = ((_maxYSpeed - sqrtTerm) / Mathf.Abs(gravity.y)) * Time.deltaTime;
        if (!CheckJumpTime(time))
        {
            time = (_maxYSpeed + sqrtTerm)/ Mathf.Abs(gravity.y);
            CheckJumpTime(time);
        }
        return _target;
    }
    private bool CheckJumpTime(float time)
    {
        float vx = _jumpPoint.deltaPosition.x / time;
        float vz = _jumpPoint.deltaPosition.z / time;
        float speedSq = vx * vx + vz * vz;

        if(speedSq < maxSpeed * maxSpeed)
        {
            _target.velocity.x = vx;
            _target.velocity.z = vz;
            _target.velocity.y = _maxYSpeed;
            _canAchieve = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ScheduleJumpAction(List<Vector3> jumpTrajectory)
    {
        jumpTrajectory.Clear();
        Vector3 p0 = _jumpPoint.jumpLocation.position;
        float i = 0;
        while (i < time)
        {
            Vector3 pt = p0 + _target.velocity * i + (gravity * i * i) / 2;
            jumpTrajectory.Add(new Vector3(pt.x, pt.y + 1.0f, pt.z));
            i += (Time.deltaTime * jumpSpeed);
        }
        jumpTrajectory.Add(new Vector3(_jumpPoint.landingLocation.position.x,_jumpPoint.landingLocation.position.y + 1.0f, _jumpPoint.landingLocation.position.z));
    }

    private float GetXZDistance(Vector3 target)
    {
        Vector2 pos2D = new Vector2(this.transform.position.x, this.transform.position.z);
        Vector2 target2D = new Vector2(target.x, target.z);
        return (pos2D - target2D).magnitude;
    }

    private void OnDrawGizmos()
    {
        if (_jumpTrajectory != null &&_jumpTrajectory.Count > 0)
        {
            for(int i = 0; i < _jumpTrajectory.Count; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(_jumpTrajectory[i], 0.1f);
            }
        }
    }

    public void Attach(GameObject observer)
    {
        _observers.Add(observer);
    }

    public void Detach(GameObject observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var o in _observers)
        {
            o.GetComponent<PlayerCreator>().Receive();
        }
    }
}

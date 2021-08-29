using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour
{
    public delegate void ReachedTarget();

    public ReachedTarget OnReachedTarget;

    public float StoppingDistance = 0.25F;

    public Transform LookAt;

    private Animator _animator;
    private static readonly int Walking = Animator.StringToHash("Walking");
    private SpriteRenderer _spriteRenderer;
    private NavMeshAgent _navMeshAgent;
    private Transform _transform;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _transform = transform;

        _navMeshAgent.updateRotation = false;
    }

    private void Update()
    {
        var position = _transform.position;
        if (OnReachedTarget != null && Vector3.Distance(position, _navMeshAgent.destination) < StoppingDistance)
        {
            OnReachedTarget.Invoke();
            OnReachedTarget = null;
        }

        LookAtLookAtTarget();

        var velocity = _navMeshAgent.velocity;
        if (velocity.magnitude > GlobalGameState.Tolerance * 10F)
        {
            _animator.SetBool(Walking, true);

            if (LookAt == null)
            {
                _spriteRenderer.flipX = velocity.x < 0;
            }
        }
        else
        {
            _animator.SetBool(Walking, false);
        }
    }

    public void Click(Vector3 clickedWorldPosition)
    {
        OnReachedTarget = null;

        WalkToPosition(clickedWorldPosition);
    }

    public void WalkToPosition(Vector3 clickedWorldPosition)
    {
        if (NavMesh.SamplePosition(clickedWorldPosition, out NavMeshHit navMeshHit, 1F, NavMesh.AllAreas))
        {
            LookAt = null;
            _navMeshAgent.destination = navMeshHit.position;
        }
    }

    private void LookAtLookAtTarget()
    {
        if (LookAt != null)
        {
            _spriteRenderer.flipX = LookAt.position.x < _transform.position.x;
        }
    }
}

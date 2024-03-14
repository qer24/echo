using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
// using Sirenix.OdinInspector;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

public class Wendigo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform Player;
    [SerializeField] private SoundObject PlayerSoundObject;
    [SerializeField] private PlayerMicTracker MicTracker;
    [SerializeField] private Animator Animator;
    [SerializeField] private GameObject Gfx;

    [Space]
    [SerializeField] private bool StartInSpawnState;

    [Space]
    [SerializeField] private GameObject SoundSource;
    [SerializeField] private EventReference SpotPlayerSound;
    [SerializeField] private StudioEventEmitter IdleSound;
    [SerializeField] private EventReference AttackSound;

    [Header("Spawn stress")]
    [SerializeField] private float StressToSpawn = 100;
    [SerializeField, Range(0f, 1f)] private float StressChance = 0.1f;
    [SerializeField] private float StressInterval = 1f;
    [SerializeField] private MinMaxFloat StressAdd;

    [Header("Spawn")]
    [SerializeField] private float[] SpawnAngles;
    [SerializeField] private MinMaxFloat RandomAngleAdd;
    [SerializeField] private MinMaxFloat SpawnDistance;
    [SerializeField] private List<GameObject> PossibleFloorsToSpawn;
    [SerializeField] private LayerMask SpawnLayerMask;
    [SerializeField] private LayerMask ObstacleLayerMask;
    [SerializeField] private float ObstacleCheckDistance;

    [Header("Spotting")]
    [SerializeField] private float TimeToSpot;
    [SerializeField] private float DistanceToRun;
    [SerializeField] private float RunCheckRange;
    [SerializeField] private LayerMask SpotLayerMask;

    [Space]
    [SerializeField] private float TimeToBore;

    [Header("Running back")]
    [SerializeField] private float RunDistance;
    [SerializeField] private float RunTime;
    [SerializeField] private float DisappearDistance = 50f;
    [SerializeField] private float DisappearTime = 0.25f;
    [SerializeField] private float FacePlayerSpeed;
    [SerializeField] private float CrossFadeTime;

    [Header("Eyes")]
    [SerializeField] private Renderer[] EyesRenderers;
    [SerializeField] private Material WhiteEyesMaterial, RedEyesMaterial;
    [SerializeField] private float[] RedEyesPercentages;

    [Header("Chase")]
    [SerializeField] private NavMeshAgent NavMeshAgent;
    [SerializeField] private float MaxChaseTime;
    [SerializeField] private float KillRange;
    [SerializeField] private float TimeToKill = 0.5f;
    [SerializeField] private WendigoKillPlayer KillPlayer;

    private enum State { Idle, Running, TryingToSpawn, Attacking }
    private State _state;

    private float _visibleTimer;
    private float _boredTimer;

    private float _stressTimer;
    private float _spawnStress;

    private int _spawnCounter;
    private bool _redEyes = true;

    private float _chaseTimer;
    private float _killTimer;

    private StarterAssetsInputs _playerInput;

    private readonly Collider[] _obstacleHits = new Collider[1];
    private readonly RaycastHit[] _playerHits = new RaycastHit[1];

    private void Awake()
    {
        _playerInput = Player.GetComponent<StarterAssetsInputs>();
    }

    private void Start()
    {
        if (StartInSpawnState)
        {
            _state = State.TryingToSpawn;
            Gfx.SetActive(false);
        }

        StartCoroutine(FirstPhaseCoroutine());

        MicTracker.OnSoundWave += TryStartle;
    }

    private IEnumerator FirstPhaseCoroutine()
    {
        while (true)
        {
            if (_state == State.Idle) yield return IdleState();
            else if (_state == State.TryingToSpawn) yield return TryToSpawnState();
            else if (_state == State.Attacking) yield return AttackState();
            else yield return null;
        }
    }

    private IEnumerator IdleState()
    {
        RotateToPlayer(out var direction);
        TrySpotPlayer(direction);

        _boredTimer += Time.deltaTime;
        if (_boredTimer > TimeToBore)
        {
            _boredTimer = 0f;
            if (_redEyes) _spawnCounter = RedEyesPercentages.Length - 1;
            IdleSound.Stop();
            _state = State.TryingToSpawn;
            Gfx.SetActive(false);
        }

        yield return null;
    }

    private void RotateToPlayer(out Vector3 direction)
    {
        direction = Player.position - transform.position;
        direction.y = 0;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, FacePlayerSpeed * Time.deltaTime);
    }

    private IEnumerator RunState(Vector3 runPos)
    {
        Animator.CrossFade("Run Back", CrossFadeTime);

        _state = State.Running;

        var startPos = transform.position;
        var timer = 0f;
        while (timer < RunTime)
        {
            var progress = timer / RunTime;
            transform.position = Vector3.Lerp(startPos, runPos, progress);
            timer += Time.deltaTime;
            yield return null;
        }

        var dissapearPosition = runPos - transform.forward * DisappearDistance;

        timer = 0f;
        while (timer < DisappearTime)
        {
            var progress = timer / DisappearTime;
            transform.position = Vector3.Lerp(runPos, dissapearPosition, progress);
            timer += Time.deltaTime;
            yield return null;
        }

        IdleSound.Stop();
        _state = State.TryingToSpawn;
        Gfx.SetActive(false);
    }

    private IEnumerator TryToSpawnState()
    {
        _stressTimer += Time.deltaTime;

        if (_stressTimer > StressInterval)
        {
            _stressTimer = 0f;

            if (new Squirrel3().Value() < StressChance)
            {
                _spawnStress += StressAdd.RandomValue;
            }

            if (_spawnStress > StressToSpawn)
            {
                TrySpawn();
            }
        }

        yield return null;
    }

    private IEnumerator AttackState()
    {
        NavMeshAgent.SetDestination(Player.position);

        if (!NavMesh.SamplePosition(Player.position, out _, 1f, NavMesh.AllAreas))
        {
            _chaseTimer += Time.deltaTime * 2f;
        }
        else
        {
            _chaseTimer += Time.deltaTime;
        }

        var playerDir = Player.position - transform.position;
        if (_chaseTimer > MaxChaseTime)
        {
            var runDirection = -playerDir.normalized;
            var runPosition = transform.position + runDirection * RunDistance;
            StopCoroutine(nameof(RunState));
            StartCoroutine(RunState(runPosition));
        }

        var distanceToPlayerSqr = (Player.position - transform.position).sqrMagnitude;
        if (distanceToPlayerSqr < KillRange * KillRange)
        {
            if (Physics.RaycastNonAlloc(transform.position + Vector3.up * 0.5f, playerDir.normalized, _playerHits, KillRange, SpotLayerMask) <= 0) yield break;
            if (_playerHits[0].transform != Player) yield break;

            _killTimer += Time.deltaTime;
            if (_killTimer > TimeToKill)
            {
                Gfx.GetComponent<Collider>().enabled = false;
                KillPlayer.Kill();
                StopAllCoroutines();
                NavMeshAgent.enabled = false;
                enabled = false;
            }
        }
        else
        {
            _killTimer = 0f;
        }

        yield return null;
    }

    private void TryStartle()
    {
        if (_state == State.Idle)
        {
            _visibleTimer = TimeToSpot;
        }
    }

    private void TrySpotPlayer(Vector3 playerDir)
    {
        var distanceToPlayerSqr = playerDir.sqrMagnitude;
        var distanceToRun = PlayerSoundObject.Radius - DistanceToRun;
        if (distanceToPlayerSqr < distanceToRun * distanceToRun)
        {
            var runCheckDistance = PlayerSoundObject.Radius - RunCheckRange;
            if (_playerInput.sprint && distanceToPlayerSqr > runCheckDistance * runCheckDistance)
            {
                var playerForward = Player.forward;
                var dot = Vector3.Dot(-playerDir.normalized, playerForward);
                if (dot < 0.5f) return;
            }
            else
            {
                _visibleTimer += Time.deltaTime; //double the speed of spotting when player is sprinting close
            }

            if (Physics.RaycastNonAlloc(transform.position + Vector3.up * 0.5f, playerDir.normalized, _playerHits, distanceToRun, SpotLayerMask) <= 0) return;
            if (_playerHits[0].transform != Player) return;

            _visibleTimer += Time.deltaTime;
            var timeToSpot = _redEyes ? TimeToSpot * 0.5f : TimeToSpot;
            if (_visibleTimer > timeToSpot)
            {
                if (!_redEyes)
                {
                    SpotPlayerSound.PlayAttached(SoundSource);
                    var runDirection = -playerDir.normalized;
                    var runPosition = transform.position + runDirection * RunDistance;
                    StopCoroutine(nameof(RunState));
                    StartCoroutine(RunState(runPosition));
                }
                else
                {
                    _killTimer = 0f;
                    _chaseTimer = 0f;
                    AttackSound.PlayAttached(SoundSource);
                    Animator.CrossFade("Run1", CrossFadeTime);
                    NavMeshAgent.enabled = true;
                    _state = State.Attacking;
                }
            }
        }
        else
        {
            _visibleTimer = 0f;
        }

        if (distanceToPlayerSqr * 0.5f < distanceToRun * distanceToRun)
        {
            _boredTimer = 0f;
        }
    }

    private void TrySpawn()
    {
        var iterations = 0;
        while (iterations < 200)
        {
            iterations++;

            var playerForward = Player.forward;

            var randomAngle = SpawnAngles.GetRandomElement() + RandomAngleAdd.RandomValue;
            var direction = Quaternion.AngleAxis(randomAngle, Vector3.up) * playerForward;
            var spawnPosition = Player.position + direction * SpawnDistance.RandomValue;

            if (Physics.Raycast(spawnPosition + Vector3.up * 0.5f, Vector3.down, out var hit, 1f, SpawnLayerMask))
            {
                var floor = hit.collider.gameObject;

                if (PossibleFloorsToSpawn.Contains(floor))
                {
                    var hits = Physics.OverlapSphereNonAlloc(hit.point, ObstacleCheckDistance, _obstacleHits, ObstacleLayerMask);
                    if (hits == 1)
                    {
                        continue;
                    }

                    transform.position = hit.point;
                    break;
                }
            }
        }

        if (iterations == 500)
        {
            Debug.LogWarning("Failed to find a valid spawn position");
            _spawnStress *= 0.5f;
            return;
        }

        _spawnStress = 0f;
        _visibleTimer = 0f;
        _state = State.Idle;
        Gfx.SetActive(true);
        Animator.CrossFade("Idle", CrossFadeTime);
        IdleSound.Play();

        SetEyes(_spawnCounter);
        _spawnCounter++;
    }

    private void SetEyes(int currentSpawn)
    {
        _redEyes = new Squirrel3().Value() < RedEyesPercentages[currentSpawn];
        foreach (var rend in EyesRenderers)
        {
            rend.material = _redEyes ? RedEyesMaterial : WhiteEyesMaterial;
        }

        if (_redEyes) _spawnCounter = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, KillRange);
    }
}

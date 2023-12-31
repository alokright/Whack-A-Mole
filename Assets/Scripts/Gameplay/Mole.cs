using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField] bool IsMoving = false;
    [SerializeField] bool IsAlive = true;
    [SerializeField] bool IsDead;
    [SerializeField] bool IsHidden;
    [SerializeField] float Duration = 1f;
    [SerializeField] float AliveDuration = 5f;
    [SerializeField] ParticleSystem HitFX;
    [SerializeField] ParticleSystem SpawnFX;
    private Coroutine MoveRoutine = null;
    private Camera mainCamera;
    private LayerMask moleLayerMask;
    private bool IsGameOver = false;
    private GameConfig gameConfig;
    void Start()
    {
        moleLayerMask = LayerMask.GetMask("Mole");
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        EventManager.GameStateEvents.OnGameOver += OnGameOver;
        EventManager.GameStateEvents.OnGamePaused += OnGameOver;
    }
    private void OnDisable()
    {
        EventManager.GameStateEvents.OnGameOver -= OnGameOver;
        EventManager.GameStateEvents.OnGamePaused -= OnGameOver;
    }

    private void OnGameOver()
    {
        if (IsAlive)
        {
           IsGameOver = true;
        }
    }

    IEnumerator StartMoving()
    {
        transform.localPosition = gameConfig.MoleHidePosition;
        Vector3 to = gameConfig.MoleShowPosition;
        float elapsedTime = 0f;
        float t = 0;
        SpawnFX.Stop();
        SpawnFX.Play();
        while (IsMoving && elapsedTime <= Duration)
        {
            yield return null;
            t = elapsedTime / Duration;
            elapsedTime += Time.deltaTime;
            
            transform.localPosition = Vector3.Lerp(transform.localPosition,to,t);
            if ((to - transform.localPosition).magnitude < 0.01f)
                IsMoving = false;
            
        }
        transform.localPosition = to;

        yield return new WaitForSeconds(AliveDuration);
        IsMoving = true;
        to = gameConfig.MoleHidePosition;
        elapsedTime = 0f;
        t = 0;
        while (IsMoving && elapsedTime <= Duration)
        {
            yield return null;
            t = elapsedTime / Duration;
            elapsedTime += Time.deltaTime;

            transform.localPosition = Vector3.Lerp(transform.localPosition, to, t);
            if ((to - transform.localPosition).magnitude < 0.01f)
                IsMoving = false;

        }
        transform.localPosition = to;
        MoleMissed();
    }

    private static int debugCount = 0;
    public void ShowMole(GameConfig config, float moveDuration, float aliveTime)
    {
        gameConfig = config;
        if (IsGameOver)
            return;
        IsAlive = true;
        IsMoving = true;
        IsGameOver = false;
        Duration = moveDuration;
        AliveDuration = aliveTime;
        MoveRoutine = StartCoroutine(StartMoving());
    }

    void Update()
    {
        if (!IsAlive)
            return;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            CheckTap(Input.mousePosition);
        }

#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            CheckTap(Input.touches[0].position);
        }
#endif
    }

    void CheckTap(Vector2 position)
    {
        Ray ray = mainCamera.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moleLayerMask))
        {
            if (hit.collider.gameObject == gameObject)
            {
                MoleKilled();
                HitFX.Stop();
                HitFX.Play();
                //Instantiate(HitFX, transform);
            }
        }
    }
    private void MoleMissed()
    {
        ObjectPoolManager.Instance.ReturnObject(gameObject.transform.parent.gameObject);

        if(!IsGameOver)
            EventManager.GameActionEvents.MoleMissed();
        IsAlive = false;
    }


    private void MoleKilled()
    {
        IsAlive = false;
        IsMoving = false;
        StopCoroutine(MoveRoutine);
        StartCoroutine(HideMole());
        EventManager.GameActionEvents.MoleKilled();
    }
    IEnumerator HideMole()
    {
        yield return new WaitForSeconds(0.3f);
        transform.localPosition = new Vector3(0f, -1f, 0f);
        ObjectPoolManager.Instance.ReturnObject(gameObject.transform.parent.gameObject);
    }
    #region UNIT TESTS
#if UNITY_EDITOR
    [SerializeField]bool testMovement = false;
    [SerializeField] bool testDirectionForward = true;
    private void LateUpdate()
    {
        if (testMovement)
        {
            testMovement = false;
            StopCoroutine("StartMoving");
            StartCoroutine(StartMoving());
        }
    }
#endif
    #endregion
}

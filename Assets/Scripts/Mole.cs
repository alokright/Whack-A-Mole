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
    [SerializeField] float fromY = -0.5f;
    [SerializeField] float ToY = 1.25f;
    [SerializeField] float Duration = 1f;
    [SerializeField] float Speed = 1f;
    [SerializeField] float AliveDuration = 5f;
    private Coroutine MoveRoutine = null;
    private Camera mainCamera;
    private LayerMask moleLayerMask;
    void Start()
    {
        moleLayerMask = LayerMask.GetMask("Mole");
        mainCamera = Camera.main; // Cache the main camera
    }

    IEnumerator StartMoving()
    {
        
        transform.localPosition = new Vector3(0f, fromY, 0f);
        Vector3 to = new Vector3(0f,ToY,0f);
        float elapsedTime = 0f;
        float t = 0;
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
        Debug.Log("Mole Moving Down"+debugCount);
        IsMoving = true;
        to = new Vector3(0f, fromY, 0f);
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
    public void ShowMole()
    {
        IsAlive = true;
        IsMoving = true;
        MoveRoutine = StartCoroutine(StartMoving());
        Debug.LogError("MOLE SHOWING"+debugCount++);
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
                Debug.Log("Mole Tapped"+ debugCount);
                MoleKilled();
            }
        }
    }
    private void MoleMissed()
    {
        ObjectPoolManager.Instance.ReturnObject(gameObject.transform.parent.gameObject);
        Debug.Log("Mole Missed"+ debugCount);
        GameEventManager.MoleMissed();
        IsAlive = false;
    }


    private void MoleKilled()
    {
        IsAlive = false;
        IsMoving = false;
        StopCoroutine(MoveRoutine);
        transform.localPosition = new Vector3(0f, -1f, 0f);
        ObjectPoolManager.Instance.ReturnObject(gameObject.transform.parent.gameObject);
        GameEventManager.MoleKilled();
    }

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
}

using System.Collections;
using UnityEngine;

public class Nozzle : MonoBehaviour
{
    [SerializeField] private GameObject foamPrefab;

    private bool moveDownwards, moveUpwards;
    private float nextFoamTime, timePassed;
    private Vector3 upPosition, downPosition;
    private ParticleSystem ps;
    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();

        moveDownwards = true;
        moveUpwards = true;

        nextFoamTime = 0.002f;
        timePassed = nextFoamTime;

        upPosition = transform.localPosition;
        downPosition = upPosition - new Vector3(0, 1f, 0);
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //if (timePassed > nextFoamTime)// create next foam
            //{
                timePassed = 0F;
                CreateFoam();
                ps.Emit(1);
            //}
            if (moveDownwards)
                StartCoroutine(MoveDownwards());
            timePassed += Time.deltaTime;
        }
        else
            if (moveUpwards)
            StartCoroutine(MoveUpwards());
    }

    private void CreateFoam()
    {
        var foam = Instantiate(foamPrefab);
        foam.transform.localPosition = GetFoamPos();
        foam.name = "foam";
    }

    private Vector3 GetFoamPos()
    {
        var x = Random.Range(-0.2f, 0.2f);
        var z = Random.Range(-0.4f, 0.4f);
        return new Vector3(x, 1f, z);
    }

    private IEnumerator MoveDownwards()
    {
        moveDownwards = false;
        float totalMovementTime = 0.5f;
        float currentMovementTime = 0f;
        var target = downPosition;
        while (Vector3.Distance(transform.localPosition, target) > 0)
        {
            currentMovementTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, currentMovementTime / totalMovementTime);
            yield return null;
        }
        moveUpwards = true;
    }
    private IEnumerator MoveUpwards()
    {
        moveUpwards = false;
        float totalMovementTime = 0.5f;
        float currentMovementTime = 0f;
        var target = upPosition;

        while (Vector3.Distance(transform.localPosition, target) > 0)
        {
            currentMovementTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, currentMovementTime / totalMovementTime);
            yield return null;
        }
        moveDownwards = true;
    }

}

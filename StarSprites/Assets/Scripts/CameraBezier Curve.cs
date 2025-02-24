using System.Collections;
using UnityEngine;

public class CameraBezierCurve : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;

    private int routeToGo;
    private float tParam;

    public Camera MainCamera;
    private Vector3 cameraPos;

    private float speedModifier;

    private bool coroutineAllowed;

    private void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.5f;
        coroutineAllowed = true;

    }

    private void Update()
    {
        if (coroutineAllowed)
            StartCoroutine(GoByTheRoute(routeToGo));

    }

    private IEnumerator GoByTheRoute(int routenumber)
    {
        coroutineAllowed = false;

        Vector2 p0 = routes[routenumber].GetChild(0).position;
        Vector2 p1 = routes[routenumber].GetChild(1).position;
        Vector2 p2 = routes[routenumber].GetChild(2).position;
        Vector2 p3 = routes[routenumber].GetChild(3).position;

        while(tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;

            cameraPos = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;


            MainCamera.transform.position = cameraPos;
            yield return new WaitForEndOfFrame();

        }

        tParam = 0f;

        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
            routeToGo = 0;

        coroutineAllowed = true;
    }
}

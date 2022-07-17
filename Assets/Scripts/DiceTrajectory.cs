using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTrajectory : MonoBehaviour
{

    DiceThrower diceThrower;

    [SerializeField]
    [Min(0)]
    int amountOfPoints = 20;
    [SerializeField]
    [Min(0f)]
    float lineLength = 10f;
    [SerializeField]
    [Min(0)]
    float pointMovePeriod = 1f;

    [SerializeField]
    GameObject dot;
    [SerializeField]
    [Min(0f)]
    float dotSize = 10f;

    bool show = false;

    List<Vector3> linePoints = new List<Vector3>();
    List<Transform> dots = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        diceThrower = GetComponent<DiceThrower>();
        if (diceThrower == null)
        {
            Destroy(this);
            return;
        }

        GameManager.Instance.playerEvents.OnDiceThrowCharge += UpdateTrajectory;
    }

    // Update is called once per frame
    void Update()
    {
        if (!show) return;

        if (dots.Count > amountOfPoints)
        {
            for (int i = amountOfPoints; i < dots.Count; i++)
            {
                Destroy(dots[i].gameObject);
            }
            dots.RemoveRange(amountOfPoints, dots.Count - amountOfPoints);
        } else if (dots.Count < amountOfPoints)
        {
            int diff = amountOfPoints - dots.Count;
            for (int i = 0; i < diff; i++)
            {
                SpawnLineDot();
            }
        }

        List<Vector3> points = GetFinalTrajectoryPoints();

        UpdateDotsPosition(points);
    }

    public void ShowTrajectory(bool value)
    {
        show = value;
        if (!value)
        {
            foreach (Transform dot in dots)
            {
                Destroy(dot.gameObject);
            }
            dots.Clear();
            linePoints.Clear();
        }
    }

    void UpdateTrajectory(object sender, OnDiceThrowChargeArgs args)
    {
        if (!show) return;

        linePoints.Clear();

        Vector3 velocity = diceThrower.throwDirection * args.power;
        float stepTime = 0.1f;

        for (int i = 0; i < 100; i++)
        {
            float time = stepTime * i;

            Vector3 offset = new Vector3(
                velocity.x * time,
                velocity.y * time + .5f * Physics.gravity.y * time * time,
                velocity.z * time
            );

            Vector3 finalPos = transform.position + offset;

            linePoints.Add(finalPos);

            if (finalPos.x > 20 || finalPos.x < -20 || finalPos.y < -5 || finalPos.z > 50 || finalPos.z < -5) break;
        }
    }

    Vector3 GetTrajectoryPointPosition(float distance)
    {
        if (linePoints.Count == 0 || distance <= 0) return transform.position;

        for (int i = 0; i < linePoints.Count - 1; i++)
        {
            Vector3 a = linePoints[i];
            Vector3 b = linePoints[i + 1];

            float dist = Vector3.Distance(a, b);

            if (distance >= dist)
            {
                distance -= dist;
            } else
            {
                return a + ((b - a) / dist * distance);
            }
        }
        return linePoints[linePoints.Count - 1];
    }

    List<Vector3> GetFinalTrajectoryPoints()
    {
        List<Vector3> finalPoints = new List<Vector3>();

        float step = lineLength / (amountOfPoints + 1);

        for (int i = 0; i < amountOfPoints; i++)
        {
            Vector3 point = GetTrajectoryPointPosition(step * i + (Time.time * pointMovePeriod) % 1 * step);

            if (i > 0)
            {
                Vector3 lastPoint = finalPoints[i - 1];

                Ray ray = new Ray(lastPoint, (point - lastPoint).normalized);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, step))
                {
                    finalPoints.Add(lastPoint + ray.direction * hit.distance);
                    break;
                }
            }

            finalPoints.Add(point);
        }

        return finalPoints;
    }

    void UpdateDotsPosition(List<Vector3> finalPoints)
    {
        for (int i = 0; i < amountOfPoints; i++)
        {
            if (i < finalPoints.Count)
            {
                dots[i].position = finalPoints[i];
                dots[i].GetComponent<MeshRenderer>().enabled = true;
                dots[i].localScale = Vector3.one * dotSize;
                if (i == finalPoints.Count - 1)
                {
                    dots[i].localScale = Vector3.one * 3 * dotSize;
                }
            } else
            {
                dots[i].GetComponent<MeshRenderer>().enabled = false; 
            }
        }
    }

    void SpawnLineDot()
    {
        if (dot == null) return;
        GameObject dotObj = Instantiate(dot, Vector3.zero, Quaternion.identity, transform);
        dotObj.GetComponent<MeshRenderer>().enabled = false;
        dots.Add(dotObj.transform);
    }

}

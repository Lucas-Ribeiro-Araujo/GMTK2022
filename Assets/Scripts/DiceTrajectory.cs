using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceTrajectory : MonoBehaviour
{

    DiceThrower diceThrower;

    [SerializeField]
    int amountOfPoints = 20;
    [SerializeField]
    float lineLength = 10f;
    [SerializeField]
    float pointMovePeriod = 1f;

    List<Vector3> linePoints = new List<Vector3>();

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
        
    }

    private void OnDrawGizmos()
    {
        List<Vector3> points = GetFinalTrajectoryPoints();
        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.color = Color.white;

            float radius = 0.1f;

            if (i == points.Count - 1)
            {
                radius = 0.3f;
            }

            Gizmos.DrawSphere(points[i], radius);
        }
    }

    public void UpdateTrajectory(object sender, OnDiceThrowChargeArgs args)
    {
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

    public Vector3 GetTrajectoryPointPosition(float distance)
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

    public List<Vector3> GetFinalTrajectoryPoints()
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

}

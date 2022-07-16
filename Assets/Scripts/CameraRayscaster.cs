using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRayscaster : MonoBehaviour
{
    IClickable colInterface;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
        else if (Input.GetMouseButtonUp(0) && colInterface != null)
        {
            colInterface.OnRelease();
            colInterface = null;
        }
    }

    private void OnClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            Component hitComponents = hit.collider.gameObject.GetComponent(typeof(IClickable));

            if (hit.collider.gameObject.GetComponent(typeof(IClickable)) is IClickable)
            {
                colInterface = hitComponents as IClickable;
                colInterface.OnClick();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragImage : MonoBehaviour
{
    private void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z);
        //FoldManager.dragAction(this.gameObject, mousePosition);
    }

    private void Update()
    {
        if(FoldManager.chkdrag)
        {
            if (!Input.GetMouseButton(0))
            {
                FoldManager.chkdrag = false;
            }
        }
    }
}

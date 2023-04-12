using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportScaler : MonoBehaviour
{
    [SerializeField] private float ScaleUpPerItem;
    [SerializeField] ActivatorManager act;

    private void Update()
    {
        if (act == null) act = PostProcessingManager.instance.gameObject.GetComponent<ActivatorManager>();
        if (act.ActiveObject == this.transform.parent.parent.parent.parent.gameObject)
        {
            int ChildCount = this.transform.childCount;
            this.GetComponent<RectTransform>().sizeDelta = new Vector2(0, ChildCount * ScaleUpPerItem);
        }
    }
}

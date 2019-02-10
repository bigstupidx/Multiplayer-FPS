using UnityEngine;

public class Utility  {

	public static void SetLayerRecursively(GameObject _obj,int newLayer)
    {
        if(_obj == null)
        {
            Debug.LogError("No object passed to set layer");
            return;
        }
        _obj.layer = newLayer;

        foreach (Transform _child in _obj.transform)
        {
            SetLayerRecursively(_child.gameObject, newLayer);
        }
    }
}

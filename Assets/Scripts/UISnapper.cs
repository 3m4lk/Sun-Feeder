using UnityEngine;

[ExecuteInEditMode]
public class UISnapper : MonoBehaviour
{
    private void Update()
    {
        RectTransform[] transforms = GetComponentsInChildren<RectTransform>();

        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].localPosition = new Vector3Int((int)transforms[i].localPosition.x, (int)transforms[i].localPosition.y, (int)transforms[i].localPosition.z);
            transforms[i].localScale = new Vector3Int((int)transforms[i].localScale.x, (int)transforms[i].localScale.y, (int)transforms[i].localScale.z);
            transforms[i].sizeDelta = new Vector2Int((int)transforms[i].sizeDelta.x, (int)transforms[i].sizeDelta.y);
        }
    }
}

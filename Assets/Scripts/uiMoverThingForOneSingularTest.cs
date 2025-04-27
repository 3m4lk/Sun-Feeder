using UnityEngine;

//[ExecuteInEditMode]
public class uiMoverThingForOneSingularTest : MonoBehaviour
{
    public Vector2 offset;

    /*public RectTransform canvas;

    //public Camera camToTest;
    //public RectTransform thingToMove;
    //public Transform followedObject;

    //public bool stop;
    //[ExecuteInEditMode]
    /*void Update()
    {
        if (!stop)
        {
            Vector2 uiElementPos = RectTransformUtility.WorldToScreenPoint(camToTest, followedObject.position);
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, uiElementPos, camToTest, out Vector2 outpoint);

            thingToMove.localPosition = uiElementPos + offset;
        }
    }//*/
    public Vector2 getScreenPoint(Camera camInput, Vector3 input)
    {
        return RectTransformUtility.WorldToScreenPoint(camInput, input) + offset;
    } // this is an awful "solution" that doesn't even work correctly, but it'll work for now (even in its broken state, here at Malk Interactive er're selling illusions :) )
}
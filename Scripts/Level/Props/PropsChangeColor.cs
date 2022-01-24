using UnityEngine;

namespace HyperCasualFramework
{
    public class PropsChangeColor : PropsBase
    {
        [SerializeField, Header("目標階層")]
        protected LayerMask targetLayer;

        [SerializeField, Header("目標顏色")]
        protected Color targetColor = Color.white;

        protected override void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & targetLayer) != 0)
            {
                ChangeSelfColor changeSelfColor = other.GetComponent<ChangeSelfColor>();
                if(changeSelfColor != null)
                {
                    changeSelfColor.SetColor(targetColor);
                }

                ChangeTargetColor changeTargetColor = other.GetComponent<ChangeTargetColor>();
                if(changeTargetColor != null)
                {
                    changeTargetColor.SetColor(targetColor);
                }
            }
        }
    }
}

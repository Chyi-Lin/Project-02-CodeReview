using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualFramework
{
    public class ChangeParticleColor : MonoBehaviour
    {
        [SerializeField, Header("Default")]
        protected Color defaultColor = Color.white;

        [SerializeField, Header("Setting")]
        protected ParticleSystemRenderer targetRenderer;

        protected MaterialPropertyBlock _propertyBlock;

        protected void Awake()
        {
            Initialization();
        }

        protected void Initialization()
        {
            _propertyBlock = new MaterialPropertyBlock();
            _propertyBlock.SetColor("_Color", defaultColor);
            targetRenderer.SetPropertyBlock(_propertyBlock);
        }

        public void SetColor(Color color)
        {
            _propertyBlock.SetColor("_Color", color);
            targetRenderer.SetPropertyBlock(_propertyBlock);
        }
    }

}

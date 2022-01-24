using MoreMountains.Feedbacks;
using UnityEngine;

namespace HyperCasualFramework
{
    public class GravityTile : MaterialTile
    {
        [SerializeField, Header("Feedbacks")]
        protected MMFeedbacks collisionEnterFeedbacks;

        public override void SetColor(Color color)
        {
            
        }

        protected override void CollisionEnterEvent(GameObject go)
        {
            collisionEnterFeedbacks?.PlayFeedbacks(this.transform.position);
        }

        protected override void Initialization()
        {
            
        }
    }
}

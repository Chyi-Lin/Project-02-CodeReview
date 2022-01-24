using Cinemachine;
using UnityEngine;

namespace HyperCasualFramework
{
    public class CinemachineFollowTarget : FollowTarget
    {
        [Header("Virtual Camera")]
        public CinemachineVirtualCamera virtualCamera;

        /// <summary>
        /// 更新跟隨目標，由 Cinemachine Virtual Camera 來控制
        /// </summary>
        protected override void UpdateFollowTarget()
        {
            if (HasTarget() == false)
                return;

            virtualCamera.Follow = target.transform;
        }
    }
}


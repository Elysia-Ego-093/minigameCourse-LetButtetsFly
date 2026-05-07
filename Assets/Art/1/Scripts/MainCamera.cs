using UnityEngine;

namespace FantasyBattlegroundsPixelArtOriginal
{
    // 删除了 [ExecuteInEditMode]，这样摄像机在编辑状态下就不会乱动，随你摆放！
    public class MainCamera : MonoBehaviour
    {
        private Transform player;

        public bool smoothCamera = true;
        
        [Tooltip("如果勾选，摄像机将只在X轴左右跟随，不会上下移动")]
        public bool lockVerticalAxis = false;
        
        public bool lockCameraSize = false;
        public float cameraSize = 5f;

        // 用来存储游戏开始时，摄像机和玩家之间的初始高度差
        private float initialYOffset; 

        private void Start()
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                
                // 【核心魔法】游戏开始的第一帧，自动计算你当前摆放的摄像机与玩家的高度差
                initialYOffset = transform.position.y - player.position.y;
            }
        }

        private void Update()
        {
            if (player == null) return;

            Camera.main.orthographicSize = lockCameraSize ? 5f : cameraSize;
            float smoothSpeed = 5.0f;

            // 使用计算好的 initialYOffset，永远保持你设定的完美构图
            Vector3 desiredPosition = new Vector3(
                player.position.x, 
                lockVerticalAxis ? transform.position.y : player.position.y + initialYOffset, 
                -10f
            );
            
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            transform.position = smoothCamera ? smoothedPosition : desiredPosition;
        }
    }
}
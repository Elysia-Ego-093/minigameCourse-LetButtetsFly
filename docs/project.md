# LetButtetsFly - 迷你射击游戏项目简介

## 1. 项目概述

**LetButtetsFly** 是一个基于 Unity 引擎开发的2D横版射击游戏原型项目，旨在实现基础的第三人称射击游戏机制。项目名称"让子弹飞"体现了其核心玩法——通过控制角色进行移动、跳跃、冲刺和射击，体验流畅的战斗交互。

### 项目目标
- 实现完整的2D角色控制系统（移动、跳跃、冲刺）
- 构建可扩展的武器系统和弹药管理
- 实现子弹碰撞检测和伤害计算机制
- 提供实时的UI反馈（血量、体力、弹药显示）
- 采用组件化架构，便于后续功能扩展

### 技术栈
- **游戏引擎**: Unity 2021+ (2D)
- **编程语言**: C#
- **UI框架**: TextMesh Pro
- **架构模式**: 组件化设计 + 抽象基类继承

## 2. 项目目录结构

```
Assets/
├── Scripts/                    # 核心脚本目录
│   ├── BasePlayerController.cs # 玩家控制器抽象基类
│   ├── PlayerController.cs     # 具体玩家控制器实现
│   ├── Bullet.cs              # 子弹逻辑实现
│   ├── GunData.cs             # 武器数据配置（ScriptableObject）
│   ├── MainMenu.cs            # 主菜单逻辑
│   └── UIManager.cs           # UI管理器
├── TextMesh Pro/              # TextMesh Pro字体和资源
Packages/                      # Unity包管理配置
ProjectSettings/               # Unity项目设置
docs/                          # 项目文档
└── README.md                  # 项目说明文件
```

## 3. 核心功能模块实现

### 3.1 角色控制系统

#### 基础架构设计
项目采用**抽象基类继承模式**，将通用功能封装在 `BasePlayerController` 中，具体输入逻辑由子类 `PlayerController` 实现。

**BasePlayerController.cs 主要特性：**
- **移动系统**: 支持左右移动，速度可配置 (`moveSpeed = 8f`)
- **跳跃系统**: 支持二段跳 (`maxJumpCount = 2`)，包含地面检测
- **冲刺系统**: 消耗体力的加速移动 (`sprintSpeedMultiplier = 2f`)
- **体力管理**: 体力消耗与恢复机制 (`maxStamina = 100f`, `staminaRecoverDelay = 2f`)
- **受击系统**: 击退效果处理 (`knockbackDuration = 0.3f`)
- **生命系统**: 血量管理 (`maxBlood = 1000f`)

**PlayerController.cs 具体实现：**
- 键盘输入映射（A/D移动，Space跳跃，K冲刺，J射击，R切换武器，Q装填）
- 多武器支持（List<GunData> guns）
- 武器切换逻辑
- 射击冷却控制（基于fireRate）

### 3.2 武器与弹药系统

#### GunData.cs (ScriptableObject)
采用Unity的ScriptableObject实现数据驱动的武器配置：

```csharp
[CreateAssetMenu(fileName = "NewGun", menuName = "Gun Data")]
public class GunData : ScriptableObject
{
    public GameObject bulletPrefab;    // 子弹预制体
    public string gunName = "Pistol";  // 武器名称
    public float fireRate = 5f;        // 射速（发/秒）
    public float bulletSpeed = 10f;    // 子弹速度
    public float bulletATK = 200f;     // 子弹伤害
    public float force_x = 10f;        // 击退力X轴
    public float force_y = 5f;         // 击退力Y轴
    public int nowAmmo = -1;           // 当前弹药数量
    public bool needAmmo = true;       // 是否需要弹药
    public int maxAmmo = 100;          // 最大弹药容量
}
```

**系统特点：**
- 支持无限武器类型配置
- 弹药管理系统（装填、消耗、显示）
- 可视化编辑器配置界面

### 3.3 子弹与碰撞系统

#### Bullet.cs 实现细节
```csharp
public class Bullet : MonoBehaviour
{
    private float speed;
    private float ATK;
    private Vector2 force;
    private Rigidbody2D rb;

    public virtual void SetStatus(float s, float lastMoveDirection, float atk, float force_x, float force_y)
    {
        speed = s;
        rb.velocity = new Vector2(lastMoveDirection * speed, 0);
        ATK = atk;
        force = new Vector2(force_x * lastMoveDirection, force_y);
    }

    void Update()
    {
        // 边界检测自动销毁
        if (transform.position.x > 12 || transform.position.x < -12)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController Player = collision.GetComponent<PlayerController>();
            if (Player != null)
            {
                Player.Attacked(ATK, force);  // 调用受击方法
                Destroy(gameObject);          // 子弹销毁
            }
        }
    }
}
```

**核心机制：**
- 基于物理的子弹飞行（Rigidbody2D）
- 碰撞检测触发伤害计算
- 自动边界清理防止内存泄漏
- 击退力传递给受击目标

### 3.4 UI反馈系统

#### UIManager.cs 功能实现
```csharp
public class UIManager : MonoBehaviour
{
    public PlayerController player;
    public Slider bloodBar;      // 血量条
    public Slider staminaBar;    // 体力条  
    public TMP_Text ammoText;    // 弹药文本显示

    void Update()
    {
        UpdateBlood();     // 实时更新血量
        UpdateStamina();   // 实时更新体力
        UpdateAmmo();      // 实时更新弹药
    }
}
```

**UI特性：**
- 实时数据绑定（每帧更新）
- 使用Slider组件显示进度条
- TextMesh Pro高质量文本渲染
- 模块化设计，易于扩展

## 4. 系统架构优势

### 4.1 可扩展性
- **抽象基类设计**: 便于添加新的玩家类型（如Player2Controller）
- **数据驱动**: 武器属性通过ScriptableObject配置，无需修改代码
- **组件化**: 各功能模块独立，耦合度低

### 4.2 性能优化
- **对象池友好**: 子弹系统支持后续改造为对象池
- **边界清理**: 防止场景中积累过多无效对象
- **高效更新**: UI只在必要时更新相关数值

### 4.3 开发友好性
- **可视化配置**: Unity Inspector中直接调整参数
- **清晰的代码结构**: 功能分离明确
- **完善的注释**: 关键逻辑都有详细说明

## 5. 后续扩展方向

1. **多人游戏支持**: 添加网络同步功能
2. **敌人AI系统**: 实现智能敌人行为
3. **关卡系统**: 多场景切换和关卡设计
4. **音效系统**: 添加射击、受伤、背景音乐等音效
5. **动画系统**: 角色和武器动画集成
6. **存档系统**: 游戏进度保存和读取
7. **成就系统**: 完成特定目标获得奖励

## 6. 使用说明

### 开发环境要求
- Unity 2021.3 LTS 或更高版本
- Visual Studio 或 JetBrains Rider（推荐）

### 快速开始
1. 在Unity中打开项目根目录
2. 打开场景文件（通常在Scenes目录下）
3. 按Play按钮开始游戏测试
4. 使用键盘控制：
   - A/D: 左右移动
   - Space: 跳跃（支持二段跳）
   - K: 冲刺（消耗体力）
   - J: 射击
   - R: 切换武器
   - Q: 装填弹药

### 武器配置
1. 在Project窗口右键 → Create → Gun Data 创建新武器
2. 在Inspector中配置武器属性
3. 将武器拖拽到PlayerController的guns列表中

---

*项目版本: 1.0.0 | 最后更新: 2026年4月*
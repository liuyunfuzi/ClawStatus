# ClawStatus 设计规范文档

## 项目概述

ClawStatus 是一款 OpenClaw 桌面状态挂件（Windows WPF 应用），采用机械龙虾吉祥物作为核心视觉形象。

**设计版本：** 1.0  
**创建日期：** 2026-03-19  
**设计师：** ui-designer

---

## 1. 主窗口设计

### 规格
- **尺寸：** 64x64 像素
- **背景：** 透明（支持拖拽到桌面任意位置）
- **窗口类型：** 无边框、置顶显示

### 布局
```
┌────────────────────┐
│                    │
│    🦞 龙虾头部     │
│   (胸部发光区)     │
│                    │
└────────────────────┘
```

### 机械龙虾吉祥物设计要点
- **风格：** 科技感机械龙虾形象
- **主色调：** 蓝色（发光触须）
- **状态指示器：** 胸部发光区域
- **眼睛：** 红色（默认状态）

---

## 2. 5 种状态动画

### 状态定义

| 状态 | 名称 | 描述 | 视觉表现 |
|------|------|------|----------|
| 🟢 | Idle (空闲) | Agent 无任务，系统正常 | 蓝色呼吸灯，平静姿态 |
| 🟡 | Busy (忙碌) | Agent 正在执行任务 | 橙色常亮，触须微动 |
| 🔥 | Fire (火爆) | 多任务并发，高负载 | 红色闪烁，快速触须动画 |
| 🔴 | Error (错误) | API 连接失败或异常 | 红色强闪烁，警示姿态 |
| ⚫ | Offline (离线) | 服务未运行或断开 | 灰色熄灭，灯光关闭 |

### 动画序列帧规格

每状态 8-10 帧 PNG 序列，建议播放速度 30-60fps

```
animations/
├── idle/        # 🟢 空闲（8 帧）- 蓝色呼吸灯
│   ├── frame_001.png
│   ├── frame_002.png
│   └── ...
├── busy/        # 🟡 忙碌（10 帧）- 橙色常亮 + 触须摆动
│   ├── frame_001.png
│   └── ...
├── fire/        # 🔥 火爆（10 帧）- 红色闪烁 + 快速动画
│   ├── frame_001.png
│   └── ...
├── error/       # 🔴 错误（8 帧）- 红色强闪烁
│   ├── frame_001.png
│   └── ...
└── offline/     # ⚫ 离线（8 帧）- 灰色熄灭过渡
    ├── frame_001.png
    └── ...
```

### 状态颜色值

| 状态 | 主色 | 辅助色 | 发光色 |
|------|------|--------|--------|
| Idle | #1E3A5F | #2E5A8F | #4A90E2 (呼吸) |
| Busy | #B8860B | #DAA520 | #FFA500 (常亮) |
| Fire | #8B0000 | #DC143C | #FF0000 (闪烁) |
| Error | #8B0000 | #FF0000 | #FF0000 (强闪) |
| Offline | #4A4A4A | #696969 | 无发光 |

---

## 3. 4 套主题设计

### 主题映射

| 主题名 | 风格 | 参考图 | 配色方案 |
|--------|------|--------|----------|
| DefaultTheme | 科技金属蓝 | 1.png/2.png | 深蓝金属 + 蓝光 |
| DarkTheme | 专业银灰 | 6.png | 银灰金属 + 蓝光 |
| CuteTheme | 卡通圆润 | 5.png | 浅蓝/青色圆润风 |
| PinkTheme | 柔和粉彩 | 7.png | 粉彩风格 |

### XAML 主题资源字典

主题文件位于 `themes/` 目录，每套主题包含：
- 主色调定义
- 发光效果颜色
- 字体颜色
- 背景透明度

---

## 4. 主题切换接口

### 在 App.xaml 中引用主题

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="themes/DefaultTheme.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### 运行时切换主题

```csharp
// ThemeManager.cs
public class ThemeManager
{
    public static void SetCurrentTheme(string themeName)
    {
        var app = Application.Current;
        var dict = app.Resources.MergedDictionaries[0];
        dict.Source = new Uri($"themes/{themeName}Theme.xaml", UriKind.Relative);
    }
}

// 使用示例
ThemeManager.SetCurrentTheme("DarkTheme");    // 切换深色主题
ThemeManager.SetCurrentTheme("CuteTheme");    // 切换可爱主题
ThemeManager.SetCurrentTheme("PinkTheme");    // 切换粉色主题
```

### 主题属性访问

```csharp
// 从资源字典获取主题颜色
var primaryColor = (Brush)Application.Current.FindResource("PrimaryBrush");
var glowColor = (Brush)Application.Current.FindResource("GlowBrush");
```

---

## 5. 动画帧使用说明

### 加载动画序列

```csharp
public class AnimationPlayer
{
    private Image _imageControl;
    private List<BitmapImage> _frames;
    private int _currentFrame = 0;
    
    public void LoadState(string stateName)
    {
        _frames = new List<BitmapImage>();
        string path = $"animations/{stateName}/";
        
        for (int i = 1; i <= 10; i++)
        {
            var frame = new BitmapImage();
            frame.BeginInit();
            frame.UriSource = new Uri($"{path}frame_{i:D3}.png", UriKind.Relative);
            frame.EndInit();
            _frames.Add(frame);
        }
    }
    
    public void StartAnimation(int fps = 30)
    {
        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(1000 / fps)
        };
        timer.Tick += (s, e) =>
        {
            _imageControl.Source = _frames[_currentFrame];
            _currentFrame = (_currentFrame + 1) % _frames.Count;
        };
        timer.Start();
    }
}
```

### 状态切换

```csharp
// 根据 Agent 状态切换动画
switch (agentStatus)
{
    case AgentStatus.Idle:
        animationPlayer.LoadState("idle");
        break;
    case AgentStatus.Busy:
        animationPlayer.LoadState("busy");
        break;
    case AgentStatus.Fire:
        animationPlayer.LoadState("fire");
        break;
    case AgentStatus.Error:
        animationPlayer.LoadState("error");
        break;
    case AgentStatus.Offline:
        animationPlayer.LoadState("offline");
        break;
}
animationPlayer.StartAnimation(30);
```

---

## 6. 文件结构

```
design-assets/
├── animations/
│   ├── idle/           # 🟢 空闲状态（8 帧）
│   │   ├── frame_001.png
│   │   ├── frame_002.png
│   │   └── ...
│   ├── busy/           # 🟡 忙碌状态（10 帧）
│   │   └── ...
│   ├── fire/           # 🔥 火爆状态（10 帧）
│   │   └── ...
│   ├── error/          # 🔴 错误状态（8 帧）
│   │   └── ...
│   └── offline/        # ⚫ 离线状态（8 帧）
│       └── ...
├── themes/
│   ├── DefaultTheme.xaml   # 默认主题（科技金属蓝）
│   ├── DarkTheme.xaml      # 深色主题（专业银灰）
│   ├── CuteTheme.xaml      # 可爱主题（圆润风）
│   └── PinkTheme.xaml      # 粉色主题（粉彩风）
└── design-spec.md      # 本设计规范文档
```

---

## 7. 设计资源下载

- **GitHub 仓库：** https://github.com/liuyunfuzi/ClawStatus/tree/main/design-assets
- **MinIO 路径：** `hiclaw/hiclaw-storage/shared/projects/ClawStatus/design-assets/`

---

## 8. 联系与支持

如有设计相关问题，请在项目房间 @ui-designer 询问。

**设计工具：** Figma / Photoshop / Blend for Visual Studio  
**输出格式：** PNG (动画帧), XAML (主题资源)

---

*最后更新：2026-03-19*

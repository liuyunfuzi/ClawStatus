# ClawStatus - OpenClaw 桌面状态挂件

一款轻量级的 Windows WPF 应用程序，用于监控 OpenClaw Agent 状态。

## 功能特性

- 🖥️ **透明背景窗口** - 64x64 像素小巧挂件
- 🎨 **多主题支持** - 默认/深色/可爱主题
- 📊 **实时状态监控** - 5 种状态动画指示
- 🔄 **自动更新** - 每 5 秒刷新状态
- 🖱️ **拖拽移动** - 随意放置桌面任意位置

## 状态指示

| 状态 | 视觉效果 | 说明 |
|------|----------|------|
| 🟢 空闲 | 蓝色呼吸灯 | 无活跃 Agent |
| 🟡 忙碌 | 橙色常亮 | 1-4 个活跃 Agent |
| 🔥 火爆 | 红色快速闪烁 | 5+ 个活跃 Agent |
| 🔴 错误 | 红色强闪烁 | API 连接失败 |
| ⚫ 离线 | 灰色熄灭 | 服务未启动 |

## 项目结构

```
ClawStatus/
├── src/ClawStatus/
│   ├── Views/              # 窗口和控件
│   ├── Services/           # 服务层
│   │   ├── IStatusService.cs
│   │   └── StatusService.cs
│   ├── Models/             # 数据模型
│   │   └── StatusModels.cs
│   ├── Resources/          # 样式和主题
│   ├── App.xaml            # 应用程序入口
│   └── MainWindow.xaml     # 主窗口
└── task-infra-009/         # 任务文档
    └── plan.md
```

## 技术要求

- .NET 8.0
- Windows 10/11
- WPF

## 配置

通过环境变量配置 API 地址：

```bash
set CLAWSTATUS_API_URL=http://localhost:8080
```

## 构建

```bash
cd src/ClawStatus
dotnet build
```

## 发布

```bash
dotnet publish -c Release -r win-x64 --self-contained
```

## 许可证

MIT License

---

**HiClaw Team** © 2026

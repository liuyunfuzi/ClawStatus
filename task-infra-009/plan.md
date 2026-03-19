# Task Plan: task-infra-009 - 搭建 WPF 项目框架

**Task ID**: task-infra-009
**Assigned to**: wpf-dev
**Started**: 2026-03-19T06:48:00Z

## Steps

- [x] Step 1: 安装 .NET 8 SDK
- [x] Step 2: 创建项目目录结构 (src/ClawStatus/{Views,Services,Models,Resources})
- [x] Step 3: 创建项目文件 (ClawStatus.csproj)
- [x] Step 4: 创建 App.xaml 和 App.xaml.cs
- [x] Step 5: 创建 MainWindow.xaml - 透明背景窗口、拖拽、置顶
- [x] Step 6: 创建 MainWindow.xaml.cs - 状态显示逻辑、动画效果
- [x] Step 7: 创建服务接口 IStatusService
- [x] Step 8: 创建服务实现 StatusService
- [x] Step 9: 创建数据模型 StatusModels
- [ ] Step 10: 创建主题资源字典（预留主题切换接口）
- [ ] Step 11: 等待 GitHub 仓库创建后推送代码
- [ ] Step 12: 编写 README.md

## Notes

### 已完成功能

1. **项目结构**
   - .NET 8 + WPF 项目框架
   - 分层架构：Views/Services/Models/Resources

2. **主窗口功能**
   - 64x64 像素透明背景窗口
   - 支持拖拽移动
   - 置顶显示 (Topmost)
   - 5 种状态动画效果：
     - 🟢 空闲：蓝色呼吸灯
     - 🟡 忙碌：橙色常亮
     - 🔥 火爆：红色快速闪烁
     - 🔴 错误：红色强闪烁
     - ⚫ 离线：灰色熄灭

3. **服务层**
   - IStatusService 接口定义
   - StatusService 实现（调用 OpenClaw API）
   - 支持配置 API 地址（环境变量 CLAWSTATUS_API_URL）

4. **数据模型**
   - SystemStatus - 系统状态
   - TokenUsage - Token 使用量
   - SessionInfo - 会话信息

### 待完成

1. **主题系统**：需要创建多套主题资源字典（默认、深色、可爱）
   - 依赖 ui-designer 提供设计资源
   - 预留主题切换接口

2. **GitHub 推送**：等待 github-specialist 创建仓库

### 技术要点

- 使用 `AllowsTransparency="True"` + `Background="Transparent"` 实现透明窗口
- 使用 `MouseLeftButtonDown` + `DragMove()` 实现拖拽
- 使用 `Topmost="True"` 实现置顶显示
- 使用 `DoubleAnimation` 实现呼吸灯效果

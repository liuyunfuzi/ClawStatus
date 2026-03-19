using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ClawStatus.Services;

namespace ClawStatus
{
    /// <summary>
    /// 主窗口 - 透明背景、可拖拽、置顶显示
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IStatusService _statusService;
        private System.Windows.Threading.DispatcherTimer? _updateTimer;

        public MainWindow()
        {
            InitializeComponent();
            _statusService = new StatusService();
        }

        /// <summary>
        /// 窗口拖拽处理
        /// </summary>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// 窗口加载完成
        /// </summary>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 初始化状态服务
            await _statusService.InitializeAsync();
            
            // 启动定时更新
            _updateTimer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _updateTimer.Tick += async (s, args) => await UpdateStatusAsync();
            _updateTimer.Start();

            // 首次更新状态
            await UpdateStatusAsync();
        }

        /// <summary>
        /// 更新状态显示
        /// </summary>
        private async Task UpdateStatusAsync()
        {
            var status = await _statusService.GetStatusAsync();
            UpdateVisualState(status);
        }

        /// <summary>
        /// 根据状态更新视觉效果
        /// </summary>
        private void UpdateVisualState(AgentStatus status)
        {
            switch (status)
            {
                case AgentStatus.Idle:
                    SetIdleState();
                    break;
                case AgentStatus.Busy:
                    SetBusyState();
                    break;
                case AgentStatus.Heavy:
                    SetHeavyState();
                    break;
                case AgentStatus.Error:
                    SetErrorState();
                    break;
                case AgentStatus.Offline:
                    SetOfflineState();
                    break;
            }
        }

        #region 状态视觉效果

        /// <summary>
        /// 空闲状态 - 蓝色呼吸灯
        /// </summary>
        private void SetIdleState()
        {
            var animation = new DoubleAnimation
            {
                From = 0.3,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(1.5),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            ChestLight.BeginAnimation(System.Windows.Controls.Primitives.RangeBase.OpacityProperty, animation);
            ChestLight.Fill = System.Windows.Media.Brushes.DeepSkyBlue;
            LeftEye.Fill = System.Windows.Media.Brushes.DarkBlue;
            RightEye.Fill = System.Windows.Media.Brushes.DarkBlue;
        }

        /// <summary>
        /// 忙碌状态 - 橙色常亮
        /// </summary>
        private void SetBusyState()
        {
            ChestLight.Fill = System.Windows.Media.Brushes.Orange;
            ChestLight.Opacity = 1.0;
            LeftEye.Fill = System.Windows.Media.Brushes.DarkBlue;
            RightEye.Fill = System.Windows.Media.Brushes.DarkBlue;
        }

        /// <summary>
        /// 火爆状态 - 红色闪烁
        /// </summary>
        private void SetHeavyState()
        {
            var animation = new DoubleAnimation
            {
                From = 0.2,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(0.3),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            ChestLight.BeginAnimation(System.Windows.Controls.Primitives.RangeBase.OpacityProperty, animation);
            ChestLight.Fill = System.Windows.Media.Brushes.Red;
            LeftEye.Fill = System.Windows.Media.Brushes.Red;
            RightEye.Fill = System.Windows.Media.Brushes.Red;
        }

        /// <summary>
        /// 错误状态 - 红色强闪烁
        /// </summary>
        private void SetErrorState()
        {
            var animation = new DoubleAnimation
            {
                From = 0.0,
                To = 1.0,
                Duration = TimeSpan.FromSeconds(0.15),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            ChestLight.BeginAnimation(System.Windows.Controls.Primitives.RangeBase.OpacityProperty, animation);
            ChestLight.Fill = System.Windows.Media.Brushes.Red;
            LeftEye.Fill = System.Windows.Media.Brushes.Red;
            RightEye.Fill = System.Windows.Media.Brushes.Red;
        }

        /// <summary>
        /// 离线状态 - 灰色熄灭
        /// </summary>
        private void SetOfflineState()
        {
            ChestLight.Fill = System.Windows.Media.Brushes.Gray;
            ChestLight.Opacity = 0.2;
            LeftEye.Fill = System.Windows.Media.Brushes.Gray;
            RightEye.Fill = System.Windows.Media.Brushes.Gray;
        }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            _updateTimer?.Stop();
            _statusService.Dispose();
            base.OnClosed(e);
        }
    }

    /// <summary>
    /// Agent 状态枚举
    /// </summary>
    public enum AgentStatus
    {
        Idle,      // 空闲
        Busy,      // 忙碌
        Heavy,     // 火爆
        Error,     // 错误
        Offline    // 离线
    }
}

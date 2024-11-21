using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace ChickenKeyboard
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    
    public partial class MainWindow : System.Windows.Window
    {
        KeyboardHook k_hook = new KeyboardHook();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetToggleButtonStates();
            k_hook.KeyDownEvent += new System.Windows.Forms.KeyEventHandler(hook_KeyDown);//钩住键按下
            k_hook.Start();//安装键盘钩子
        }

        private void SetToggleButtonStates()
        {
            TBc.IsChecked = Properties.Settings.Default.c;
            TBt.IsChecked = Properties.Settings.Default.t;
            TBr.IsChecked = Properties.Settings.Default.r;
            TBl.IsChecked = Properties.Settings.Default.l;
            TBctrl.IsChecked = Properties.Settings.Default.ctrl;
            TBj.IsChecked = Properties.Settings.Default.j;
            TBn.IsChecked = Properties.Settings.Default.n;
        }

        private void SetProperties()
        {
            Properties.Settings.Default.c = TBc.IsChecked.Value;
            Properties.Settings.Default.t = TBt.IsChecked.Value;
            Properties.Settings.Default.r = TBr.IsChecked.Value;
            Properties.Settings.Default.l = TBl.IsChecked.Value;
            Properties.Settings.Default.ctrl = TBctrl.IsChecked.Value;
            Properties.Settings.Default.j = TBj.IsChecked.Value;
            Properties.Settings.Default.n = TBn.IsChecked.Value;
            Properties.Settings.Default.Save();
        }

        private void ToggleButtons_Click(object sender, RoutedEventArgs e)
        {
            SetProperties();
        }

        private void SetWindowVisible(object sender, RoutedEventArgs e)
        {
            this.Show();
            this.Activate();
        }

        private void ShutdownApp(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            HandyControl.Controls.NotifyIcon.ShowBalloonTip("鸡键盘", "已最小化至托盘图标运行，对图标右键可以打开菜单。", NotifyIconInfoType.Info, "ChickenKeyboard");
        }
        
        private void description_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            description.Text = "小黑子，请在下面开启或关闭某个按键的音效。";
        }

        private void description_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            description.Text = "ikun，请在下面开启或关闭某个按键的音效。";
        }
        
        private void PlaySoundForKey(KeyEventArgs e, Keys key, bool settingFlag, Stream soundStream)
        {
            if (e.KeyValue == (int)key && settingFlag)
            {
                SoundPlayer player = new SoundPlayer();
                player.Stream = soundStream;
                player.LoadAsync(); // 异步加载声音
                player.Play();  // 使用Play()异步播放
            }
        }

        private void HandleKeyPress(KeyEventArgs e)
        {
            PlaySoundForKey(e, Keys.C, Properties.Settings.Default.c, Properties.Resources.C);
            PlaySoundForKey(e, Keys.T, Properties.Settings.Default.t, Properties.Resources.T);
            PlaySoundForKey(e, Keys.R, Properties.Settings.Default.r, Properties.Resources.R);
            PlaySoundForKey(e, Keys.L, Properties.Settings.Default.l, Properties.Resources.L);
            PlaySoundForKey(e, Keys.LControlKey, Properties.Settings.Default.ctrl, Properties.Resources.Ctrl);
            PlaySoundForKey(e, Keys.RControlKey, Properties.Settings.Default.ctrl, Properties.Resources.Ctrl);
            PlaySoundForKey(e, Keys.J, Properties.Settings.Default.j, Properties.Resources.J);
            PlaySoundForKey(e, Keys.N, Properties.Settings.Default.n, Properties.Resources.N);
        }

        private void hook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            HandleKeyPress(e);
        }
    }
}

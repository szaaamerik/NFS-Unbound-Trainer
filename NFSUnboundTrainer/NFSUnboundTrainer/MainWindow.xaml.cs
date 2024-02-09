using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace NFSUnboundTrainer;

public sealed partial class MainWindow : INotifyPropertyChanged
{
    private bool _attached;

    public bool Attached
    {
        get => _attached;
        set
        {
            if (_attached == value) return;
            _attached = value;
            OnPropertyChanged(nameof(Attached));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public static string ProcessName => "NeedForSpeedUnbound.exe";
    public static string GameName => "Need For Speed Unbound";
    private readonly Cheats _cheats;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        _cheats = new Cheats(this);
        Task.Run(_cheats.OpenGame);
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void Minimize_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Close();
        _cheats.TrainerClose();
        Application.Current.Shutdown();
    }

    private void Github_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        const string targetUrl = "https://github.com/szaaamerik/NFS-Unbound-Trainer";
        var processStartInfo = new ProcessStartInfo
        {
            UseShellExecute = true,
            FileName = targetUrl
        };
        Process.Start(processStartInfo);
    }

    private void Donation_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        const string targetUrl = "https://www.buymeacoffee.com/merika";
        var processStartInfo = new ProcessStartInfo
        {
            UseShellExecute = true,
            FileName = targetUrl
        };
        Process.Start(processStartInfo);
    }

    private async void CreditsSwitch_OnToggled(object sender, RoutedEventArgs e)
    {
        if (!_attached || sender is not ToggleSwitch toggleSwitch)
        {
            return;
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = false;
        }
        
        if (_cheats.CreditsDetourAddress == 0)
        {
            await _cheats.CheatCredits();
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = true;
        }
        
        if (_cheats.CreditsDetourAddress <= 0) return;
        _cheats.Mem.WriteMemory(_cheats.CreditsDetourAddress + 0x31, toggleSwitch.IsOn ? (byte)1 : (byte)0);
        _cheats.Mem.WriteMemory(_cheats.CreditsDetourAddress + 0x32, Convert.ToInt32(CreditsNum.Value));
    }

    private void CreditsNum_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
    {
        if (!_attached || sender is not NumericUpDown numericUpDown)
        {
            return;
        }

        if (_cheats.CreditsDetourAddress == 0)
        {
            return;
        }
        
        _cheats.Mem.WriteMemory(_cheats.CreditsDetourAddress + 0x32, Convert.ToInt32(numericUpDown.Value));
    }

    private async void UnlimitedNos_OnToggled(object sender, RoutedEventArgs e)
    {
        if (!_attached || sender is not ToggleSwitch toggleSwitch)
        {
            return;
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = false;
        }
        
        if (_cheats.NosDetourAddress == 0)
        {
            await _cheats.CheatNos();
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = true;
        }
        
        if (_cheats.NosDetourAddress <= 0) return;
        _cheats.Mem.WriteMemory(_cheats.NosDetourAddress + 0x31, toggleSwitch.IsOn ? (byte)1 : (byte)0);
    }

    private async void UnlimitedVehicleHealth_OnToggled(object sender, RoutedEventArgs e)
    {
        if (!_attached || sender is not ToggleSwitch toggleSwitch)
        {
            return;
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = false;
        }
        
        if (_cheats.HealthDetourAddress == 0)
        {
            await _cheats.CheatHealth();
        }
        
        if (_attached)
        {
            toggleSwitch.IsEnabled = true;
        }
        
        if (_cheats.HealthDetourAddress <= 0) return;
        _cheats.Mem.WriteMemory(_cheats.HealthDetourAddress + 0x50, toggleSwitch.IsOn ? (byte)1 : (byte)0);
    }

    private async void PoliceReinforcementsTimeFreeze_OnToggled(object sender, RoutedEventArgs e)
    {
        if (!_attached || sender is not ToggleSwitch toggleSwitch)
        {
            return;
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = false;
        }
        
        if (_cheats.ReinforcementsDetourAddress == 0)
        {
            await _cheats.CheatReinforcements();
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = true;
        }
        
        if (_cheats.ReinforcementsDetourAddress <= 0) return;
        _cheats.Mem.WriteMemory(_cheats.ReinforcementsDetourAddress + 0x2A, toggleSwitch.IsOn ? (byte)1 : (byte)0);
    }

    private async void Arrest_OnToggled(object sender, RoutedEventArgs e)
    {
        if (!_attached || sender is not ToggleSwitch toggleSwitch)
        {
            return;
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = false;
        }
        
        if (_cheats.ArrestDetourAddress == 0)
        {
            await _cheats.CheatArrest();
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = true;
        }
        
        if (_cheats.ArrestDetourAddress <= 0) return;
        _cheats.Mem.WriteMemory(_cheats.ArrestDetourAddress + 0x2B, toggleSwitch.IsOn ? (byte)1 : (byte)0);
    }

    private async void FreeCustomToggle_OnToggled(object sender, RoutedEventArgs e)
    {
        if (!_attached || sender is not ToggleSwitch toggleSwitch)
        {
            return;
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = false;
        }
        
        if (_cheats.FreeCustomDetourAddress == 0)
        {
            await _cheats.CheatFreeCustom();
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = true;
        }
        
        if (_cheats.FreeCustomDetourAddress <= 0) return;
        _cheats.Mem.WriteMemory(_cheats.FreeCustomDetourAddress + 0x2F, toggleSwitch.IsOn ? (byte)1 : (byte)0);     
    }

    private async void FreeVehiclesToggle_OnToggled(object sender, RoutedEventArgs e)
    {
        if (!_attached || sender is not ToggleSwitch toggleSwitch)
        {
            return;
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = false;
        }
        
        if (_cheats.FreeCarsDetourAddress == 0)
        {
            await _cheats.CheatFreeCars();
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = true;
        }
        
        if (_cheats.FreeCarsDetourAddress <= 0) return;
        _cheats.Mem.WriteMemory(_cheats.FreeCarsDetourAddress + 0x2A, toggleSwitch.IsOn ? (byte)1 : (byte)0);     
    }

    private async void HeatToggle_OnToggled(object sender, RoutedEventArgs e)
    {
        if (!_attached || sender is not ToggleSwitch toggleSwitch)
        {
            return;
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = false;
        }
        
        if (_cheats.HeatLevelDetourAddress == 0)
        {
            await _cheats.CheatHeatLevel();
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = true;
        }
        
        if (_cheats.HeatLevelDetourAddress <= 0) return;
        _cheats.Mem.WriteMemory(_cheats.HeatLevelDetourAddress + 0x30, toggleSwitch.IsOn ? (byte)1 : (byte)0);     
        _cheats.Mem.WriteMemory(_cheats.HeatLevelDetourAddress + 0x31, Convert.ToInt32(HeatLevelSlider.Value));     
    }

    private void HeatLevelSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!_attached || sender is not Slider slider)
        {
            return;
        }

        if (_cheats.HeatLevelDetourAddress == 0)
        {
            return;
        }
        
        _cheats.Mem.WriteMemory(_cheats.HeatLevelDetourAddress + 0x31, Convert.ToInt32(slider.Value));
    }

    private async void VelocityToggle_OnToggled(object sender, RoutedEventArgs e)
    {
        if (!_attached || sender is not ToggleSwitch toggleSwitch)
        {
            return;
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = false;
        }
        
        if (_cheats.MovementHookDetourAddress == 0)
        {
            await _cheats.CheatMovementHook();
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = true;
        }
        
        if (_cheats.MovementHookDetourAddress <= 0) return;
        _cheats.Mem.WriteMemory(_cheats.MovementHookDetourAddress + 0x152, toggleSwitch.IsOn ? (byte)1 : (byte)0);     
        _cheats.Mem.WriteMemory(_cheats.MovementHookDetourAddress + 0x154, 0.3f);     
        _cheats.Mem.WriteMemory(_cheats.MovementHookDetourAddress + 0x158, 2.23694f);     
        _cheats.Mem.WriteMemory(_cheats.MovementHookDetourAddress + 0x15C, 1f + Convert.ToSingle(VelocitySlider.Value / 150));     
        _cheats.Mem.WriteMemory(_cheats.MovementHookDetourAddress + 0x160, 450f);     
        _cheats.Mem.WriteMemory(_cheats.MovementHookDetourAddress + 0x164, 10f);     
    }

    private async void FreezeAiToggle_OnToggled(object sender, RoutedEventArgs e)
    {
        if (!_attached || sender is not ToggleSwitch toggleSwitch)
        {
            return;
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = false;
        }

        if (_cheats.MovementHookDetourAddress == 0)
        {
            await _cheats.CheatMovementHook();
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = true;
        }
        
        if (_cheats.MovementHookDetourAddress <= 0) return;
        _cheats.Mem.WriteMemory(_cheats.MovementHookDetourAddress + 0x151, toggleSwitch.IsOn ? (byte)1 : (byte)0);     
    }

    private async void BrakeToggle_OnToggled(object sender, RoutedEventArgs e)
    {
        if (!_attached || sender is not ToggleSwitch toggleSwitch)
        {
            return;
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = false;
        }
        
        if (_cheats.MovementHookDetourAddress == 0)
        {
            await _cheats.CheatMovementHook();
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = true;
        }
        
        if (_cheats.MovementHookDetourAddress <= 0) return;
        _cheats.Mem.WriteMemory(_cheats.MovementHookDetourAddress + 0x153, toggleSwitch.IsOn ? (byte)1 : (byte)0);     
        _cheats.Mem.WriteMemory(_cheats.MovementHookDetourAddress + 0x168, 0.1f);
        _cheats.Mem.WriteMemory(_cheats.MovementHookDetourAddress + 0x16C, 1f - Convert.ToSingle(BrakeHackSlider.Value / 100));
        _cheats.Mem.WriteMemory(_cheats.MovementHookDetourAddress + 0x170, 15f);
    }

    private void VelocitySlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!_attached || sender is not Slider slider)
        {
            return;
        }

        if (_cheats.MovementHookDetourAddress == 0)
        {
            return;
        }
        
        _cheats.Mem.WriteMemory(_cheats.MovementHookDetourAddress + 0x15C, 1f + Convert.ToSingle(slider.Value / 150));     
    }

    private void BrakeHackSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!_attached || sender is not Slider slider)
        {
            return;
        }

        if (_cheats.MovementHookDetourAddress == 0)
        {
            return;
        }
        
        _cheats.Mem.WriteMemory(_cheats.MovementHookDetourAddress + 0x16C, 1f - Convert.ToSingle(slider.Value / 100));     
    }

    private async void LapTimeToggle_OnToggled(object sender, RoutedEventArgs e)
    {
        if (!_attached || sender is not ToggleSwitch toggleSwitch)
        {
            return;
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = false;
        }
        
        if (_cheats.LapTimeDetourAddress == 0)
        {
            await _cheats.CheatLapTime();
        }

        if (_attached)
        {
            toggleSwitch.IsEnabled = true;
        }
        
        if (_cheats.LapTimeDetourAddress <= 0) return;
        _cheats.Mem.WriteMemory(_cheats.LapTimeDetourAddress + 0x29, toggleSwitch.IsOn ? (byte)1 : (byte)0);     
    }
}
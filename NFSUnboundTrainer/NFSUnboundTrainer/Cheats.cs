using System.Windows;
using System.Windows.Media;
using Memory;
using Memory.Types;

namespace NFSUnboundTrainer;

public class Cheats
{
    public readonly Mem Mem = new();
    private readonly MainWindow _mainWindow;
    private bool _isCancellationRequested;

    private long _minRange;
    private long _maxRange;

    public nuint MovementHookAddress, MovementHookDetourAddress;
    public nuint PlayerIdAddress, PlayerIdDetourAddress;
    public nuint GasAddrAddress, GasAddrDetourAddress;
    public nuint CreditsAddress, CreditsDetourAddress;
    public nuint NosAddress, NosDetourAddress;
    public nuint HealthAddress, HealthDetourAddress;
    public nuint ReinforcementsAddress, ReinforcementsDetourAddress;
    public nuint ArrestAddress, ArrestDetourAddress;
    public nuint FreeCustomAddress, FreeCustomDetourAddress;
    public nuint FreeCarsAddress, FreeCarsDetourAddress;
    public nuint HeatLevelAddress, HeatLevelDetourAddress;
    public nuint LapTimeAddress, LapTimeDetourAddress;
    
    public Cheats(MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }

    public void OpenGame()
    {
        while (!_isCancellationRequested)
        {
            Task.Delay(_mainWindow.Attached ? 1000 : 500).Wait();
            
            if (Mem.OpenProcess(MainWindow.ProcessName) == Mem.OpenProcessResults.Success)
            {
                HandleOpenGame();
            }
            else
            {
                HandleCloseGame();
            }
        }
    }

    private void HandleOpenGame()
    {
        if (_mainWindow.Attached)
        {
            return;
        }

        _minRange = Mem.MProc.Process.MainModule!.BaseAddress;
        _maxRange = _minRange + Mem.MProc.Process.MainModule.ModuleMemorySize;
        
        _mainWindow.Dispatcher.Invoke(() =>
        {
            _mainWindow.GameStatusLabel.Foreground = Brushes.Green;
            _mainWindow.GameStatusLabel.Text = "On";
            _mainWindow.ProcessIdLabel.Text = Mem.MProc.ProcessId.ToString();
        });
        _mainWindow.Attached = true;
    }

    private void HandleCloseGame()
    {
        if (!_mainWindow.Attached)
        {
            return;
        }

        _mainWindow.Attached = false;
        _mainWindow.Dispatcher.Invoke(() =>
        {
            _mainWindow.GameStatusLabel.Foreground = Brushes.Red;
            _mainWindow.GameStatusLabel.Text = "Off";
            _mainWindow.ProcessIdLabel.Text = "0";
            _mainWindow.ArrestToggle.IsOn = false;
            _mainWindow.BrakeToggle.IsOn = false;
            _mainWindow.CreditsToggle.IsOn = false;
            _mainWindow.HeatToggle.IsOn = false;
            _mainWindow.VelocityToggle.IsOn = false;
            _mainWindow.FreeCustomToggle.IsOn = false;
            _mainWindow.FreeVehiclesToggle.IsOn = false;
            _mainWindow.FreezeAiToggle.IsOn = false;
            _mainWindow.UnlimitedNosToggle.IsOn = false;
            _mainWindow.UnlimitedVehicleHealthToggle.IsOn = false;
            _mainWindow.PoliceReinforcementsTimeFreezeToggle.IsOn = false;
            _mainWindow.LapTimeToggle.IsOn = false;
        });
        ResetTrainer();
    }

    private void ResetTrainer()
    {
        var fields = typeof(Cheats).GetFields().Where(f => f.FieldType == typeof(nuint));
        foreach (var field in fields)
        {
            field.SetValue(this, UIntPtr.Zero);
        }
    }

    public void TrainerClose()
    {
        _isCancellationRequested = true;
        if (CreditsAddress > 44)
        {
            Mem.WriteArrayMemory(CreditsAddress, new byte[] { 0x48, 0x8B, 0x00, 0x48, 0x8B, 0x40, 0x38, 0x48, 0x83, 0xC4, 0x20, 0x5B, 0xC3, 0xCC });
            Imps.VirtualFreeEx(Mem.MProc.Handle, CreditsDetourAddress, 0,Imps.MemRelease);
        }

        if (NosAddress > 0)
        {
            Mem.WriteArrayMemory(NosAddress, new byte[] { 0xF3, 0x0F, 0x10, 0x83, 0xBC, 0x05, 0x00, 0x00, 0xF3, 0x0F, 0x5E, 0xC1, 0xF3, 0x0F, 0x5F, 0xC6 });
            Imps.VirtualFreeEx(Mem.MProc.Handle, NosDetourAddress, 0, Imps.MemRelease);
        }

        if (HealthAddress > 0)
        {
            Mem.WriteArrayMemory(HealthAddress, new byte[] { 0x48, 0x8B, 0x03, 0x4C, 0x8D, 0x8C, 0x24, 0x70, 0x15, 0x00, 0x00, 0x44, 0x8B, 0xC7 });
            Imps.VirtualFreeEx(Mem.MProc.Handle, HealthDetourAddress, 0, Imps.MemRelease);
        }

        if (ReinforcementsAddress > 0)
        {
            Mem.WriteArrayMemory(ReinforcementsAddress, new byte[] { 0xF3, 0x0F, 0x5C, 0xCE, 0xF3, 0x0F, 0x11, 0x88, 0x88, 0x00, 0x00, 0x00, 0x40, 0x38, 0xBB, 0x84, 0x01, 0x00, 0x00 });
            Imps.VirtualFreeEx(Mem.MProc.Handle, ReinforcementsDetourAddress, 0, Imps.MemRelease);
        }

        if (ArrestAddress > 0)
        {
            Mem.WriteArrayMemory(ArrestAddress, new byte[] { 0xF3, 0x41, 0x0F, 0x5C, 0xC0, 0xF3, 0x0F, 0x11, 0x83, 0x58, 0x01, 0x00, 0x00, 0x40, 0x38, 0xAB, 0x8B, 0x01, 0x00, 0x00});
            Imps.VirtualFreeEx(Mem.MProc.Handle, ArrestDetourAddress, 0, Imps.MemRelease);
        }

        if (FreeCustomAddress > 0)
        {
            Mem.WriteArrayMemory(FreeCustomAddress, new byte[] { 0x49, 0x8B, 0x07, 0x66, 0x0F, 0x6E, 0xB0, 0x98, 0x00, 0x00, 0x00, 0x0F, 0x5B, 0xF6 });
            Imps.VirtualFreeEx(Mem.MProc.Handle, FreeCustomDetourAddress, 0, Imps.MemRelease);
        }

        if (FreeCarsAddress > 0)
        {
            Mem.WriteArrayMemory(FreeCarsAddress, new byte[] { 0x44, 0x89, 0x49, 0x70, 0x44, 0x89, 0x49, 0x78, 0x8B, 0x42, 0x58, 0x89, 0x81, 0x98, 0x00, 0x00, 0x00 });
            Imps.VirtualFreeEx(Mem.MProc.Handle, FreeCarsDetourAddress, 0, Imps.MemRelease);
        }

        if (HeatLevelAddress > 0)
        {
            Mem.WriteArrayMemory(HeatLevelAddress, new byte[] { 0xF3, 0x0F, 0x10, 0x81, 0xD0, 0x00, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC });
            Imps.VirtualFreeEx(Mem.MProc.Handle, HeatLevelDetourAddress, 0, Imps.MemRelease);
        }

        if (MovementHookAddress > 0)
        {
            Mem.WriteArrayMemory(MovementHookAddress, new byte[] { 0x0F, 0x10, 0x04, 0x88, 0x48, 0x8B, 0x86, 0x18, 0x0B, 0x00, 0x00, 0x42, 0x8D, 0x0C, 0x95, 0x00, 0x00, 0x00, 0x00 });
            Imps.VirtualFreeEx(Mem.MProc.Handle, MovementHookDetourAddress, 0, Imps.MemRelease);
        }

        if (PlayerIdAddress > 20)
        {
            Mem.WriteArrayMemory(PlayerIdAddress, new byte[] { 0x8B, 0xF8, 0xFF, 0x92, 0xD8, 0x00, 0x00, 0x00, 0x48, 0x8B, 0x13, 0x48, 0x8B, 0xCB });
            Imps.VirtualFreeEx(Mem.MProc.Handle, PlayerIdDetourAddress, 0, Imps.MemRelease);
        }

        if (GasAddrAddress > 0)
        {
            Mem.WriteArrayMemory(GasAddrAddress, new byte[] { 0x4C, 0x8B, 0x00, 0x41, 0xFF, 0x90, 0xA0, 0x00, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x4C, 0x24, 0x34 });
            Imps.VirtualFreeEx(Mem.MProc.Handle, GasAddrDetourAddress, 0, Imps.MemRelease);
        }
        
        
    }
    
    private async Task<nuint> SmartAobScan(string search)
    {
        Imps.GetSystemInfo(out var info);
        UIntPtr address = 0;
        
        Imps.Native_VirtualQueryEx(Mem.MProc.Handle, address, out Imps.MemoryBasicInformation64 memInfo, info.PageSize);
        address = memInfo.BaseAddress + (UIntPtr)memInfo.RegionSize;

        var scanStartAddr = _minRange;
        while (address < (ulong)_maxRange)
        {
            Imps.Native_VirtualQueryEx(Mem.MProc.Handle, address, out memInfo, info.PageSize);
            if (address == memInfo.BaseAddress + memInfo.RegionSize)
            {
                break;
            }

            var scanEndAddr = (long)memInfo.BaseAddress + (long)memInfo.RegionSize;

            nuint retAddress;
            if (scanEndAddr - scanStartAddr > 500000000)
            {
                retAddress = await ScanRange(search, scanStartAddr, scanEndAddr);
            }
            else
            {
                retAddress = (await Mem.AoBScan(scanStartAddr, scanEndAddr, search)).FirstOrDefault();
            }

            if (retAddress != 0)
            {
                return retAddress;
            }

            scanStartAddr = scanEndAddr;
            address = memInfo.BaseAddress + (UIntPtr)memInfo.RegionSize;
        }

        return 0;
    }

    private async Task<nuint> ScanRange(string search, long startAddr, long endAddr)
    {
        for (var i = 1; i < 2; i++)
        {
            var end = startAddr + (endAddr - startAddr) / 2 * i;
            var retAddress = (await Mem.AoBScan(startAddr, end, search)).FirstOrDefault();
            if (retAddress != 0)
            {
                return retAddress;
            }
        }
        return 0;
    }

    private async Task GetPlayerId()
    {
        PlayerIdAddress = 0;
        PlayerIdDetourAddress = 0;

        const string sig = "0F 29 ? ? ? 0F 57 ? FF 90";
        PlayerIdAddress = await SmartAobScan(sig) + 20;
        if (PlayerIdAddress > 20)
        {
            var asm = new byte[]
            {
                0x8B, 0xF8, 0xFF, 0x92, 0xD8, 0x00, 0x00, 0x00, 0x48, 0x89, 0x0D, 0x14, 0x00, 0x00, 0x00, 0x48, 0x8B,
                0x13, 0x48, 0x8B, 0xCB
            };
            PlayerIdDetourAddress = Mem.CreateFarDetour(PlayerIdAddress, asm, 14);
            return;
        }
        
        ShowError("Player Id", sig);
    }

    private async Task GetGasAddr()
    {
        GasAddrAddress = 0;
        GasAddrDetourAddress = 0;

        const string sig = "4C 8B ? 41 FF ? ? ? ? ? F3 0F ? ? ? ? 48 8D";
        GasAddrAddress = await SmartAobScan(sig);
        if (GasAddrAddress > 0)
        {
            var asm = new byte[]
            {
                0x4C, 0x8B, 0x00, 0x57, 0x48, 0x8B, 0xB8, 0x74, 0x1F, 0x00, 0x00, 0x48, 0x89, 0x3D, 0x2A, 0x00, 0x00,
                0x00, 0x48, 0x8B, 0xB8, 0x78, 0x1F, 0x00, 0x00, 0x48, 0x89, 0x3D, 0x20, 0x00, 0x00, 0x00, 0x5F, 0x41,
                0xFF, 0x90, 0xA0, 0x00, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x4C, 0x24, 0x34
            };
            GasAddrDetourAddress = Mem.CreateFarDetour(GasAddrAddress, asm, 16);
            return;
        }
        
        ShowError("Gas Addr", sig);
    }
    
    public async Task CheatMovementHook()
    {
        MovementHookAddress = 0;
        MovementHookDetourAddress = 0;
        
        const string sig = "0F 10 ? ? 48 8B ? ? ? ? ? 42 8D ? ? ? ? ? ? 0F 59";
        MovementHookAddress = await SmartAobScan(sig);
        if (MovementHookAddress > 0)
        {
            await GetPlayerId();
            if (PlayerIdAddress <= 20)
            {
                return;
            }

            await GetGasAddr();
            if (GasAddrAddress == 0)
            {
                return;
            }

            var gasAddr = BitConverter.GetBytes(GasAddrDetourAddress + 0x3C);
            var playerId = BitConverter.GetBytes(PlayerIdDetourAddress + 0x23);
            var asm = new byte[]
            {
                0x56, 0x41, 0x51, 0x48, 0x83, 0xEC, 0x30, 0xF3, 0x0F, 0x7F, 0x04, 0x24, 0xF3, 0x0F, 0x7F, 0x4C, 0x24,
                0x10, 0xF3, 0x0F, 0x7F, 0x54, 0x24, 0x20, 0x49, 0xB9, gasAddr[0], gasAddr[1], gasAddr[2], gasAddr[3],
                gasAddr[4], gasAddr[5], gasAddr[6], gasAddr[7], 0x48, 0xBE, playerId[0], playerId[1], playerId[2],
                playerId[3], playerId[4], playerId[5], playerId[6], playerId[7], 0x80, 0x3D, 0x1E, 0x01, 0x00, 0x00,
                0x01, 0x75, 0x1C, 0x48, 0x3B, 0x0E, 0x74, 0x17, 0xC7, 0x04, 0x88, 0x00, 0x00, 0x00, 0x00, 0xC7, 0x44,
                0x88, 0x04, 0x00, 0x00, 0x00, 0x00, 0xC7, 0x44, 0x88, 0x08, 0x00, 0x00, 0x00, 0x00, 0x48, 0x3B, 0x0E,
                0x75, 0x76, 0xF3, 0x0F, 0x10, 0x0C, 0x88, 0xF3, 0x0F, 0x10, 0x54, 0x88, 0x08, 0xF3, 0x0F, 0x59, 0xC9,
                0xF3, 0x0F, 0x59, 0xD2, 0xF3, 0x0F, 0x58, 0xCA, 0xF3, 0x0F, 0x51, 0xC1, 0xF3, 0x0F, 0x10, 0xC8, 0xF3,
                0x0F, 0x59, 0x0D, 0xDB, 0x00, 0x00, 0x00, 0x80, 0x3D, 0xCE, 0x00, 0x00, 0x00, 0x01, 0x75, 0x46, 0xF3,
                0x41, 0x0F, 0x10, 0x01, 0x0F, 0x2F, 0x05, 0xC2, 0x00, 0x00, 0x00, 0x76, 0x38, 0x0F, 0x2F, 0x0D, 0xC5,
                0x00, 0x00, 0x00, 0x77, 0x2F, 0x0F, 0x2F, 0x0D, 0xC0, 0x00, 0x00, 0x00, 0x72, 0x26, 0xF3, 0x0F, 0x10,
                0x04, 0x88, 0xF3, 0x0F, 0x10, 0x54, 0x88, 0x08, 0xF3, 0x0F, 0x59, 0x05, 0xA3, 0x00, 0x00, 0x00, 0xF3,
                0x0F, 0x59, 0x15, 0x9B, 0x00, 0x00, 0x00, 0xF3, 0x0F, 0x11, 0x04, 0x88, 0xF3, 0x0F, 0x11, 0x54, 0x88,
                0x08, 0x80, 0x3D, 0x80, 0x00, 0x00, 0x00, 0x01, 0x75, 0x43, 0x48, 0x3B, 0x0E, 0x75, 0x3E, 0x0F, 0x2F,
                0x0D, 0x8F, 0x00, 0x00, 0x00, 0x76, 0x35, 0xF3, 0x41, 0x0F, 0x10, 0x41, 0x04, 0x0F, 0x2F, 0x05, 0x78,
                0x00, 0x00, 0x00, 0x76, 0x26, 0xF3, 0x0F, 0x10, 0x04, 0x88, 0xF3, 0x0F, 0x10, 0x54, 0x88, 0x08, 0xF3,
                0x0F, 0x59, 0x05, 0x67, 0x00, 0x00, 0x00, 0xF3, 0x0F, 0x59, 0x15, 0x5F, 0x00, 0x00, 0x00, 0xF3, 0x0F,
                0x11, 0x04, 0x88, 0xF3, 0x0F, 0x11, 0x54, 0x88, 0x08, 0xF3, 0x0F, 0x6F, 0x04, 0x24, 0xF3, 0x0F, 0x6F,
                0x4C, 0x24, 0x10, 0xF3, 0x0F, 0x6F, 0x54, 0x24, 0x20, 0x48, 0x83, 0xC4, 0x30, 0x41, 0x59, 0x5E, 0x0F,
                0x10, 0x04, 0x88, 0x48, 0x8B, 0x86, 0x18, 0x0B, 0x00, 0x00, 0x42, 0x8D, 0x0C, 0x95, 0x00, 0x00, 0x00,
                0x00
            };
            MovementHookDetourAddress = Mem.CreateFarDetour(MovementHookAddress, asm, 19);
            return;
        }
        
        ShowError("Movement Hook", sig);
    }

    public async Task CheatCredits()
    {
        CreditsAddress = 0;
        CreditsDetourAddress = 0;

        const string sig = "48 89 ? ? ? 53 48 83 EC ? 48 83 C1";
        CreditsAddress = await SmartAobScan(sig) + 44;
        if (CreditsAddress > 44)
        {
            var asm = new byte[]
            {
                0x48, 0x8B, 0x00, 0x80, 0x3D, 0x27, 0x00, 0x00, 0x00, 0x01, 0x75, 0x0D, 0x51, 0x48, 0x8B, 0x0D, 0x1E,
                0x00, 0x00, 0x00, 0x48, 0x89, 0x48, 0x38, 0x59, 0x48, 0x8B, 0x40, 0x38, 0x48, 0x83, 0xC4, 0x20, 0x5B,
                0xC3
            };

            CreditsDetourAddress = Mem.CreateFarDetour(CreditsAddress, asm, 14);
            return;
        }
        
        ShowError("Credits", sig);
    }

    public async Task CheatNos()
    {
        NosAddress = 0;
        NosDetourAddress = 0;

        const string sig = "F3 0F ? ? ? ? ? ? F3 0F ? ? F3 0F ? ? F3 0F ? ? ? ? ? ? F3 0F ? ? ? 0F 2E";
        NosAddress = await SmartAobScan(sig);
        if (NosAddress > 0)
        {
            var asm = new byte[]
            {
                0x80, 0x3D, 0x2A, 0x00, 0x00, 0x00, 0x01, 0x75, 0x0A, 0xC7, 0x83, 0xBC, 0x05, 0x00, 0x00, 0x00, 0xC0, 
                0x79, 0x44, 0xF3, 0x0F, 0x10, 0x83, 0xBC, 0x05, 0x00, 0x00, 0xF3, 0x0F, 0x5E, 0xC1, 0xF3, 0x0F, 0x5F,
                0xC6
            };

            NosDetourAddress = Mem.CreateFarDetour(NosAddress, asm, 16);
            return;
        }
        
        ShowError("Nos", sig);
    }

    public async Task CheatHealth()
    {
        HealthAddress = 0;
        HealthDetourAddress = 0;

        const string sig = "48 8B ? 4C 8D ? ? ? ? ? ? 44 8B ? 49 8B ? 48 8B ? FF 90 ? ? ? ? 48 8D ? ? ? ? ? ? E8 ? ? ? ? EB";
        HealthAddress = await SmartAobScan(sig);
        if (HealthAddress > 0)
        {
            var asm = new byte[]
            {
                0x48, 0x8B, 0x03, 0x4C, 0x8D, 0x8C, 0x24, 0x70, 0x15, 0x00, 0x00, 0x44, 0x8B, 0xC7, 0x80, 0x3D, 0x3B,
                0x00, 0x00, 0x00, 0x01, 0x75, 0x2B, 0x57, 0x48, 0x8B, 0xBB, 0x00, 0x01, 0x00, 0x00, 0x48, 0x83, 0xFF,
                0x00, 0x74, 0x1C, 0x48, 0x83, 0xEC, 0x10, 0xF3, 0x0F, 0x7F, 0x04, 0x24, 0xF3, 0x0F, 0x10, 0x47, 0x44,
                0xF3, 0x0F, 0x11, 0x47, 0x40, 0xF3, 0x0F, 0x6F, 0x04, 0x24, 0x48, 0x83, 0xC4, 0x10, 0x5F
            };

            HealthDetourAddress = Mem.CreateFarDetour(HealthAddress, asm, 14);
            return;
        }
        
        ShowError("Health", sig);
    }

    public async Task CheatReinforcements()
    {
        ReinforcementsAddress = 0;
        ReinforcementsDetourAddress = 0;

        const string sig = "F3 0F ? ? F3 0F ? ? ? ? ? ? 40 38 ? ? ? ? ? 74 ? 0F 2F";
        ReinforcementsAddress = await SmartAobScan(sig);
        if (ReinforcementsAddress > 0)
        {
            var asm = new byte[]
            {
                0x80, 0x3D, 0x23, 0x00, 0x00, 0x00, 0x01, 0x74, 0x04, 0xF3, 0x0F, 0x5C, 0xCE, 0xF3, 0x0F, 0x11, 0x88,
                0x88, 0x00, 0x00, 0x00, 0x40, 0x38, 0xBB, 0x84, 0x01, 0x00, 0x00
            };

            ReinforcementsDetourAddress = Mem.CreateFarDetour(ReinforcementsAddress, asm, 19);
            return;
        }
        
        ShowError("Health", sig);
    }

    public async Task CheatArrest()
    {
        ArrestAddress = 0;
        ArrestDetourAddress = 0;

        const string sig = "F3 41 ? ? ? F3 0F ? ? ? ? ? ? 40 38";
        ArrestAddress = await SmartAobScan(sig);
        if (ArrestAddress > 0)
        {
            var asm = new byte[]
            {
                0x80, 0x3D, 0x24, 0x00, 0x00, 0x00, 0x01, 0x74, 0x05, 0xF3, 0x41, 0x0F, 0x5C, 0xC0, 0xF3, 0x0F, 0x11,
                0x83, 0x58, 0x01, 0x00, 0x00, 0x40, 0x38, 0xAB, 0x8B, 0x01, 0x00, 0x00
            };
            ArrestDetourAddress = Mem.CreateFarDetour(ArrestAddress, asm, 20);
            return;
        }
        
        ShowError("Impervious To Arrest", sig);
    }

    public async Task CheatFreeCustom()
    {
        FreeCustomAddress = 0;
        FreeCustomDetourAddress = 0;

        const string sig = "49 8B ? 66 0F ? ? ? ? ? ? 0F 5B";
        FreeCustomAddress = await SmartAobScan(sig);
        if (FreeCustomAddress > 0)
        {
            var asm = new byte[]
            {
                0x49, 0x8B, 0x07, 0x80, 0x3D, 0x25, 0x00, 0x00, 0x00, 0x01, 0x75, 0x0A, 0xC7, 0x80, 0x98, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x66, 0x0F, 0x6E, 0xB0, 0x98, 0x00, 0x00, 0x00, 0x0F, 0x5B, 0xF6
            };
            FreeCustomDetourAddress = Mem.CreateFarDetour(FreeCustomAddress, asm, 14);
            return;
        }
        
        ShowError("Free Custom Vehicles", sig);
    }

    public async Task CheatFreeCars()
    {
        FreeCarsAddress = 0;
        FreeCarsDetourAddress = 0;

        const string sig = "44 89 ? ? 44 89 ? ? 8B 42 ? 89 81";
        FreeCarsAddress = await SmartAobScan(sig);
        if (FreeCarsAddress > 0)
        {
            var asm = new byte[]
            {
                0x44, 0x89, 0x49, 0x70, 0x44, 0x89, 0x49, 0x78, 0x8B, 0x42, 0x58, 0x80, 0x3D, 0x18, 0x00, 0x00, 0x00,
                0x01, 0x75, 0x02, 0x31, 0xC0, 0x89, 0x81, 0x98, 0x00, 0x00, 0x00
            };
            FreeCarsDetourAddress = Mem.CreateFarDetour(FreeCarsAddress, asm, 17);
            return;
        }
        
        ShowError("Free Cars", sig);
    }


    public async Task CheatHeatLevel()
    {
        HeatLevelAddress = 0;
        HeatLevelDetourAddress = 0;

        const string sig = "E9 ? ? ? ? 0F 57 ? C3 F3 0F";
        HeatLevelAddress = await SmartAobScan(sig) + 9;
        if (HeatLevelAddress > 9)
        {
            var asm = new byte[]
            {
                0x80, 0x3D, 0x29, 0x00, 0x00, 0x00, 0x01, 0x75, 0x10, 0xF3, 0x0F, 0x2A, 0x05, 0x20, 0x00, 0x00, 0x00,
                0xF3, 0x0F, 0x11, 0x81, 0xD0, 0x00, 0x00, 0x00, 0xF3, 0x0F, 0x10, 0x81, 0xD0, 0x00, 0x00, 0x00, 0xC3
            };
            HeatLevelDetourAddress = Mem.CreateFarDetour(HeatLevelAddress, asm, 14);
            return;
        }
        
        ShowError("Heat Level", sig);
    }

    public async Task CheatLapTime()
    {
        LapTimeAddress = 0;
        LapTimeDetourAddress = 0;

        const string sig = "F3 0F ? ? ? F3 0F ? ? ? ? ? ? F3 0F ? ? ? F3 0F ? ? ? ? ? ? 48 8B ? ? ? ? ? E8";
        LapTimeAddress = await SmartAobScan(sig);
        if (LapTimeAddress > 0)
        {
            var asm = new byte[]
            {
                0x80, 0x3D, 0x22, 0x00, 0x00, 0x00, 0x01, 0x74, 0x05, 0xF3, 0x0F, 0x58, 0x46, 0x08, 0xF3, 0x0F, 0x11,
                0x87, 0x70, 0x02, 0x00, 0x00, 0xF3, 0x0F, 0x58, 0x4E, 0x08
            };
            LapTimeDetourAddress = Mem.CreateFarDetour(LapTimeAddress, asm, 18);
            return;
        }
        
        ShowError("Lap Time", sig);
    }
    
    private static void ShowError(string feature, string sig)
    {
        MessageBox.Show(
            $"Address for this feature wasn't found!\nPlease try to activate the cheat again or try to restart the game and the trainer.\n\nIf this error still occur, please (Press Ctrl+C) to copy, and make an issue on the github repository.\n\nFeature: {feature}\nSignature: {sig}\n\nTrainer Version: {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}",
            "NFS Unbound Trainer Error", 0, MessageBoxImage.Error);
    }
}
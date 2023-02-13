using System;
using Microsoft.Win32;

Console.Title = "SwitchToEdge for Serein";
Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", "Serein-Winform.exe", 6001, RegistryValueKind.DWord);
Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", "Serein-Winform.exe", 1, RegistryValueKind.DWord);
Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", "Serein-WPF.exe", 6001, RegistryValueKind.DWord);
Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", "Serein-WPF.exe", 1, RegistryValueKind.DWord);
Console.WriteLine("写入注册表成功。按任意键退出");
Console.ReadKey();
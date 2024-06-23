using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Serein.Lite.Utils.Native;

public static partial class ListViewHelper
{
    [LibraryImport("uxtheme.dll", StringMarshalling = StringMarshalling.Utf16)]
    public static partial int SetWindowTheme(
        IntPtr hwnd,
        string pszSubAppName,
        string? pszSubIdList
    );

    public static void SetExploreTheme(this Control control)
    {
        SetWindowTheme(control.Handle, "Explorer", null);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Jint;
using Jint.Native;
using Jint.Runtime;
using Serein.Core.Utils;

namespace Serein.Core.Services.Plugins.Js.BuiltInModules;

public static partial class FileSystem
{
    internal static void DisposeAll()
    {
        foreach (var stream in FileStreams.Values)
        {
            stream.Close();
            stream.Dispose();
        }
        FileStreams.Clear();
    }

    internal static readonly Dictionary<int, FileStream> FileStreams = [];

    private static FileStream GetFileStream(int fd)
    {
        return !FileStreams.TryGetValue(fd, out var stream)
            ? throw new IOException("Invalid file descriptor")
            : stream;
    }

    public static void AccessSync(string path, int mode = 0)
    {
        throw new NotSupportedException();
    }

    public static void AppendFileSync(string path, string data, JsValue? options = default)
    {
        if (options is null)
        {
            AppendFileSync(path, EncodingMap.UTF8.GetBytes(data));
        }
        else if (options.Type == Types.String)
        {
            AppendFileSync(path, Encoding.GetEncoding(options.AsString()).GetBytes(data));
        }
        else if (options.Type == Types.Object)
        {
            var encoding = options.AsObject().Get("encoding").AsString();
            AppendFileSync(path, Encoding.GetEncoding(encoding).GetBytes(data));
        }
    }

    public static void AppendFileSync(string path, byte[] data, JsValue? options = default)
    {
        using var file = File.Open(path, FileMode.Append);
        file.Write(data);
        file.Flush();
        file.Close();
    }

    public static void ChmodSync(string path, int mode)
    {
        throw new NotSupportedException();
    }

    public static void ChownSync(string path, int uid, int gid)
    {
        throw new NotSupportedException();
    }

    public static void CloseSync(int fd)
    {
        var fileStream = GetFileStream(fd);
        fileStream.Close();
        fileStream.Dispose();
        FileStreams.Remove(fd);
    }

    public static void CopyFileSync(string src, string dest, int flags = 0)
    {
        File.Copy(src, dest, flags != 1);
    }

    public static void CpSync(string src, string dest, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public static bool ExistsSync(string path)
    {
        return Path.Exists(path);
    }

    public static void FchmodSync(int fd, int mode)
    {
        throw new NotSupportedException();
    }

    public static void FchownSync(int fd, int uid, int gid)
    {
        throw new NotSupportedException();
    }

    public static void FdatasyncSync(int fd)
    {
        throw new NotSupportedException();
    }

    public static void FstatSync(int fd, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public static void FsyncSync(int fd)
    {
        GetFileStream(fd).Flush();
    }

    public static void FtruncateSync(int fd, int len = 0)
    {
        GetFileStream(fd).SetLength(len);
    }

    public static void FutimesSync(int fd, DateTime atime, DateTime mtime)
    {
        var fileStream = GetFileStream(fd);
        File.SetLastAccessTime(fileStream.Name, atime);
        File.SetLastWriteTime(fileStream.Name, mtime);
    }

    public static string[] GlobSync(string pattern, JsValue? options = default)
    {
        return GlobSync([pattern], options);
    }

    public static string[] GlobSync(string[] pattern, JsValue? options = default)
    {
        if (options?.Type == Types.Object)
        {
            var o = options.AsObject();

            var cwdValue = o.Get("cwd");
            var cwd = cwdValue.IsUndefined()
                ? Directory.GetCurrentDirectory()
                : cwdValue.AsString();

            var excludeValue = o.Get("exclude");
            var exclude = excludeValue.IsUndefined()
                ? null
                : excludeValue.TryCast<Func<string, bool>>();

            var list = new List<string>();
            foreach (var p in pattern)
            {
                list.AddRange(Directory.GetFiles(cwd, p));
            }
            return exclude is null ? [.. list] : [.. list.Where(exclude)];
        }
        else
        {
            var list = new List<string>();
            foreach (var p in pattern)
            {
                list.AddRange(Directory.GetFiles(Directory.GetCurrentDirectory(), p));
            }
            return [.. list];
        }
    }

    public static void LchmodSync(string path, int mode)
    {
        throw new NotSupportedException();
    }

    public static void LchownSync(string path, int uid, int gid)
    {
        throw new NotSupportedException();
    }

    public static void LutimesSync(string path, int atime, int mtime)
    {
        throw new NotSupportedException();
    }

    public static void LinkSync(string existingPath, string newPath)
    {
        throw new NotSupportedException();
    }

    public static void LstatSync(string path, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public static void MkdirSync(string path, JsValue? options = default)
    {
        if (options is null)
        {
            MkdirSync(path, 511);
        }
        else if (options.Type == Types.Number)
        {
            MkdirSync(path, (int)options.AsNumber());
        }
        else if (options.Type == Types.Object)
        {
            var o = options.AsObject();
            var mode = o.Get("mode");
            if (mode.IsUndefined())
            {
                MkdirSync(path, 511);
            }
            else
            {
                MkdirSync(path, (int)mode.AsNumber());
            }
        }
    }

    public static void MkdirSync(
        string path,
        int mode = 511 /* 0o777 */
    )
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            Directory.CreateDirectory(path);
        }
        else
        {
#pragma warning disable CA1416
            Directory.CreateDirectory(path, (UnixFileMode)mode);
#pragma warning restore CA1416
        }
    }

    public static string MkdtempSync(string prefix, JsValue? options = default)
    {
        var path = Path.Combine(Path.GetTempPath(), prefix, Guid.NewGuid().ToString("N")[0..6]);
        Directory.CreateDirectory(path);
        return path;
    }

    public static void OpendirSync(string path, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public static int OpenSync(string path, string flags, JsValue? mode = default)
    {
        var fileStream = new FileStream(
            path,
            flags switch
            {
                "r" => FileMode.Open,
                "r+" => FileMode.Open,
                "rs" => FileMode.Open,
                "rs+" => FileMode.Open,
                "w" => FileMode.Create,
                "wx" => FileMode.CreateNew,
                "w+" => FileMode.OpenOrCreate,
                "wx+" => FileMode.CreateNew,
                "a" => FileMode.Append,
                "ax" => FileMode.Append,
                "a+" => FileMode.Append,
                "ax+" => FileMode.Append,
                _ => throw new ArgumentException("Invalid flags", nameof(flags)),
            }
        );

        FileStreams.Add(FileStreams.GetHashCode(), fileStream);

        return FileStreams.GetHashCode();
    }

    public static string ReadFileSync(string path, JsValue? options = default)
    {
        if (options is null)
        {
            return File.ReadAllText(path);
        }
        else if (options.Type == Types.String)
        {
            return File.ReadAllText(path, Encoding.GetEncoding(options.AsString()));
        }
        else if (options.Type == Types.Object)
        {
            var encoding = options.AsObject().Get("encoding").AsString();
            return File.ReadAllText(path, Encoding.GetEncoding(encoding));
        }
        else
        {
            return File.ReadAllText(path);
        }
    }

    public static string[] ReaddirSync(string path, JsValue? options = default)
    {
        return Directory.GetFiles(path);
    }

    public static void ReadlinkSync(string path, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public static int ReadSync(int fd, byte[] buffer, int offset, int length, int position = 0)
    {
        var fileStream = GetFileStream(fd);
        fileStream.Seek(position, SeekOrigin.Begin);
        return fileStream.Read(buffer, offset, length);
    }

    public static void RealpathSync(string path, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public static void RenameSync(string oldPath, string newPath)
    {
        File.Move(oldPath, newPath);
    }

    public static void RmdirSync(string path, JsValue? options = default)
    {
        if (Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length > 0)
        {
            throw new IOException("Directory is not empty");
        }

        if (options?.Type == Types.Object)
        {
            var o = options.AsObject();
            var recursive = o.Get("recursive");
            if (recursive.IsUndefined())
            {
                Directory.Delete(path);
            }
            else
            {
                Directory.Delete(path, recursive.AsBoolean());
            }
        }
        else
        {
            Directory.Delete(path);
        }
    }

    public static void RmSync(string path, JsValue? options = default)
    {
        if (options?.Type == Types.Object)
        {
            var o = options.AsObject();
            var recursive = o.Get("recursive");
            if (recursive.IsUndefined())
            {
                Directory.Delete(path);
            }
            else
            {
                Directory.Delete(path, recursive.AsBoolean());
            }
        }
        else
        {
            Directory.Delete(path, true);
        }
    }

    public static void StatSync(string path, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public static void StatfsSync(string path, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public static void SymlinkSync(string target, string path, JsValue? type = default)
    {
        Directory.CreateSymbolicLink(target, path);
    }

    public static void TruncateSync(string path, int len = 0)
    {
        using var file = File.Open(path, FileMode.Open);
        file.SetLength(len);
        file.Flush();
        file.Close();
    }

    public static void UnlinkSync(string path)
    {
        File.Delete(path);
    }

    public static void UtimesSync(string path, DateTime atime, DateTime mtime)
    {
        File.SetLastAccessTime(path, atime);
        File.SetLastWriteTime(path, mtime);
    }

    public static void WriteFileSync(string path, string data, JsValue? options = default)
    {
        if (options is null)
        {
            WriteFileSync(path, EncodingMap.UTF8.GetBytes(data));
        }
        else if (options.Type == Types.String)
        {
            WriteFileSync(path, Encoding.GetEncoding(options.AsString()).GetBytes(data));
        }
        else if (options.Type == Types.Object)
        {
            var encoding = options.AsObject().Get("encoding").AsString();
            WriteFileSync(path, Encoding.GetEncoding(encoding).GetBytes(data));
        }
    }

    public static void WriteFileSync(string path, byte[] data, JsValue? options = default)
    {
        using var file = File.Open(path, FileMode.OpenOrCreate);
        file.Write(data);
        file.Flush();
        file.Close();
    }

    public static int WriteSync(int fd, byte[] buffer)
    {
        return WriteSync(fd, buffer, length: buffer.Length);
    }

    public static int WriteSync(
        int fd,
        byte[] buffer,
        int offset = 0,
        int? length = null,
        int position = 0
    )
    {
        var l = length ?? (buffer.Length - offset);

        var fileStream = GetFileStream(fd);
        fileStream.Seek(position, SeekOrigin.Begin);
        fileStream.Write(buffer, offset, l);
        fileStream.Flush();

        return l;
    }

    public static int WriteSync(int fd, string data, int position = 0, string encoding = "utf8")
    {
        var buffer = (
            encoding.Equals("utf8", StringComparison.InvariantCultureIgnoreCase)
                ? EncodingMap.UTF8
                : Encoding.GetEncoding(encoding)
        ).GetBytes(data);

        return WriteSync(fd, buffer, 0, buffer.Length, position);
    }
}

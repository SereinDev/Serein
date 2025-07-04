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

#pragma warning disable CA1822

public sealed class FileSystem
{
    internal void DisposeAll()
    {
        foreach (var stream in FileStreams.Values)
        {
            stream.Close();
            stream.Dispose();
        }
        FileStreams.Clear();
    }

    internal readonly Dictionary<long, FileStream> FileStreams = [];

    private FileStream GetFileStream(long fd)
    {
        return !FileStreams.TryGetValue(fd, out var stream)
            ? throw new IOException("Invalid file descriptor")
            : stream;
    }

    public void AccessSync(string path, int mode = 0)
    {
        throw new NotSupportedException();
    }

    public void AppendFileSync(string path, string data, JsValue? options = default)
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

    public void AppendFileSync(string path, byte[] data, JsValue? options = default)
    {
        using var file = File.Open(path, FileMode.Append);
        file.Write(data);
        file.Flush();
        file.Close();
    }

    public void ChmodSync(string path, int mode)
    {
        throw new NotSupportedException();
    }

    public void ChownSync(string path, int uid, int gid)
    {
        throw new NotSupportedException();
    }

    public void CloseSync(long fd)
    {
        var fileStream = GetFileStream(fd);
        fileStream.Close();
        fileStream.Dispose();
        FileStreams.Remove(fd);
    }

    public void CopyFileSync(string src, string dest, int flags = 0)
    {
        File.Copy(src, dest, flags != 1);
    }

    public void CpSync(string src, string dest, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public bool ExistsSync(string path)
    {
        return Path.Exists(path);
    }

    public void FchmodSync(long fd, int mode)
    {
        throw new NotSupportedException();
    }

    public void FchownSync(long fd, int uid, int gid)
    {
        throw new NotSupportedException();
    }

    public void FdatasyncSync(long fd)
    {
        throw new NotSupportedException();
    }

    public void FstatSync(long fd, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public void FsyncSync(long fd)
    {
        GetFileStream(fd).Flush();
    }

    public void FtruncateSync(long fd, int len = 0)
    {
        GetFileStream(fd).SetLength(len);
    }

    public void FutimesSync(long fd, DateTime atime, DateTime mtime)
    {
        var fileStream = GetFileStream(fd);
        File.SetLastAccessTime(fileStream.Name, atime);
        File.SetLastWriteTime(fileStream.Name, mtime);
    }

    public string[] GlobSync(string pattern, JsValue? options = default)
    {
        return GlobSync([pattern], options);
    }

    public string[] GlobSync(string[] pattern, JsValue? options = default)
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

    public void LchmodSync(string path, int mode)
    {
        throw new NotSupportedException();
    }

    public void LchownSync(string path, int uid, int gid)
    {
        throw new NotSupportedException();
    }

    public void LutimesSync(string path, int atime, int mtime)
    {
        throw new NotSupportedException();
    }

    public void LinkSync(string existingPath, string newPath)
    {
        throw new NotSupportedException();
    }

    public void LstatSync(string path, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public void MkdirSync(string path, JsValue? options = default)
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

    public void MkdirSync(
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

    public string MkdtempSync(string prefix, JsValue? options = default)
    {
        var path = Path.Combine(Path.GetTempPath(), prefix, Guid.NewGuid().ToString("N")[0..6]);
        Directory.CreateDirectory(path);
        return path;
    }

    public void OpendirSync(string path, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public int OpenSync(string path, string flags, JsValue? mode = default)
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

    public string ReadFileSync(string path, JsValue? options = default)
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

    public string[] ReaddirSync(string path, JsValue? options = default)
    {
        return Directory.GetFiles(path);
    }

    public void ReadlinkSync(string path, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public int ReadSync(long fd, byte[] buffer, int offset, int length, int position = 0)
    {
        var fileStream = GetFileStream(fd);
        fileStream.Seek(position, SeekOrigin.Begin);
        return fileStream.Read(buffer, offset, length);
    }

    public void RealpathSync(string path, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public void RenameSync(string oldPath, string newPath)
    {
        File.Move(oldPath, newPath);
    }

    public void RmdirSync(string path, JsValue? options = default)
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

    public void RmSync(string path, JsValue? options = default)
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

    public void StatSync(string path, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public void StatfsSync(string path, JsValue? options = default)
    {
        throw new NotSupportedException();
    }

    public void SymlinkSync(string target, string path, JsValue? type = default)
    {
        Directory.CreateSymbolicLink(target, path);
    }

    public void TruncateSync(string path, int len = 0)
    {
        using var file = File.Open(path, FileMode.Open);
        file.SetLength(len);
        file.Flush();
        file.Close();
    }

    public void UnlinkSync(string path)
    {
        File.Delete(path);
    }

    public void UtimesSync(string path, DateTime atime, DateTime mtime)
    {
        File.SetLastAccessTime(path, atime);
        File.SetLastWriteTime(path, mtime);
    }

    public void WriteFileSync(string path, string data, JsValue? options = default)
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

    public void WriteFileSync(string path, byte[] data, JsValue? options = default)
    {
        using var file = File.Open(path, FileMode.OpenOrCreate);
        file.Write(data);
        file.Flush();
        file.Close();
    }

    public int WriteSync(long fd, byte[] buffer)
    {
        return WriteSync(fd, buffer, length: buffer.Length);
    }

    public int WriteSync(
        long fd,
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

    public int WriteSync(long fd, string data, int position = 0, string encoding = "utf8")
    {
        var buffer = (
            encoding.Equals("utf8", StringComparison.InvariantCultureIgnoreCase)
                ? EncodingMap.UTF8
                : Encoding.GetEncoding(encoding)
        ).GetBytes(data);

        return WriteSync(fd, buffer, 0, buffer.Length, position);
    }
}

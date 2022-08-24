using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serein.Items;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Serein.Base
{
    internal static class Loader
    {
        public static void LoadAll()
        {
            LoadRegex();
            LoadMember();
        }

        public static void LoadRegex(string FileName = null)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                FileName = $"{Global.Path}\\data\\regex.json";
            }
            if (File.Exists(FileName))
            {
                StreamReader Reader = new StreamReader(
                    File.Open(
                        FileName,
                        FileMode.Open
                    ),
                    Encoding.UTF8
                    );
                if (FileName.ToUpper().EndsWith(".TSV"))
                {
                    string Line;
                    List<RegexItem> regexItems = new List<RegexItem>();
                    while ((Line = Reader.ReadLine()) != null)
                    {
                        RegexItem Item = new RegexItem();
                        Item.ConvertToItem(Line);
                        if (!Item.CheckItem())
                        {
                            continue;
                        }
                        regexItems.Add(Item);
                    }
                    Global.UpdateRegexItems(regexItems);
                }
                else if (FileName.ToUpper().EndsWith(".JSON"))
                {
                    string Text = Reader.ReadToEnd();
                    if (string.IsNullOrEmpty(Text)) { return; }
                    try
                    {
                        JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Text);
                        if (JsonObject["type"].ToString().ToUpper() != "REGEX")
                        {
                            return;
                        }
                        Global.UpdateRegexItems(((JArray)JsonObject["data"]).ToObject<List<RegexItem>>());
                    }
                    catch { }
                }
                Reader.Close();
                Reader.Dispose();
            }
        }

        public static void SaveRegex()
        {
            List<RegexItem> regexItems = new List<RegexItem>();
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            StreamWriter RegexWriter = new StreamWriter(
                File.Open(
                    $"{Global.Path}\\data\\regex.json",
                    FileMode.Create,
                    FileAccess.Write
                    ),
                Encoding.UTF8
                );
            JObject ListJObject = new JObject();
            ListJObject.Add("type", "REGEX");
            ListJObject.Add("comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失");
            ListJObject.Add("data", JObject.FromObject(Global.RegexItems));
            RegexWriter.Write(ListJObject.ToString());
            Global.UpdateRegexItems(regexItems);
            RegexWriter.Flush();
            RegexWriter.Close();
        }

        public static void LoadMember()
        {
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            if (File.Exists($"{Global.Path}\\data\\members.json"))
            {
                string Text = File.ReadAllText(
                    $"{Global.Path}\\data\\members.json",
                    Encoding.UTF8
                    );
                if (!string.IsNullOrEmpty(Text))
                {
                    try
                    {
                        JObject JsonObject = (JObject)JsonConvert.DeserializeObject(Text);
                        if (JsonObject["type"].ToString().ToUpper() != "MEMBERS")
                        {
                            return;
                        }
                        Global.UpdateMemberItems(((JArray)JsonObject["data"]).ToObject<List<MemberItem>>());
                    }
                    catch { }
                }
            }
            List<MemberItem> memberItems = Global.MemberItems;
            memberItems.Sort(
                (Item1, Item2) =>
                {
                    return Item1.ID > Item2.ID ? 1 : -1;
                }
                );
            Global.UpdateMemberItems(memberItems);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public static void SaveMember()
        {
            List<MemberItem> memberItems = Global.MemberItems;
            memberItems.Sort(
                (Item1, Item2) =>
                {
                    return Item1.ID > Item2.ID ? 1 : -1;
                }
                );
            Global.UpdateMemberItems(memberItems);
            if (!Directory.Exists(Global.Path + "\\data"))
            {
                Directory.CreateDirectory(Global.Path + "\\data");
            }
            JObject ListJObject = new JObject();
            JArray ListJArray = new JArray();
            foreach (MemberItem Item in Global.MemberItems)
            {
                ListJArray.Add(JObject.FromObject(Item));
            }
            ListJObject.Add("type", "MEMBERS");
            ListJObject.Add("comment", "非必要请不要直接修改文件，语法错误可能导致数据丢失");
            ListJObject.Add("data", ListJArray);
            File.WriteAllText(
                $"{Global.Path}\\data\\members.json",
                ListJObject.ToString(),
                Encoding.UTF8
                );
        }
    }
}
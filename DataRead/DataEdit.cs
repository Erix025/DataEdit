using System;
using System.Collections.Generic;
using System.IO;

namespace Index.DataEdit
{
    public static class ReadFiles
    {
        public static List<string> ReadFrom(string file, bool saveread)
        {
            string line;
            try
            {
                using (var reader = File.OpenText(file))
                {
                    List<string> result = new List<string>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("\\n"))
                        {
                            result.Add(Function.ToDataString(line));
                        }
                        else
                        {
                            result.Add(line);
                        }
                    }
                    return result;
                }
            }
            catch
            {
                if (saveread)
                {
                    WriteFiles.Write(new string[0], file);
                    return new List<string>();
                }
                else
                {
                    throw new Exception("无法找到文件");
                }
            }
        }
        public static string[] ReadOnly(string file, bool saveread)
        {
            var lines = ReadFrom(file, saveread);
            string[] output;
            int total = 0;
            foreach (string str in lines)
            {
                total++;
            }
            output = new string[total];
            int i = 0;
            foreach (string str in lines)
            {
                output[i] = str;
                i++;
            }
            return output;
        }
        public static List<List<Item>> ItemRead(string path, bool saveread)//读取文件，获得项目总数，项目名称与项目值
        {
            List<List<Item>> result = new List<List<Item>>();
            var lines = ReadFrom(path, saveread);
            foreach (string line in lines)
            {
                if (line != "")
                {
                    if (line.Substring(0, 1) == "[")
                    {
                        result.Add(new List<Item>());
                    }
                    else
                    {
                        int i = 0;
                        while (line.Substring(i, 1) != "=")
                        {
                            i++;
                        }
                        string key = line.Substring(0, i);
                        string value = line.Substring(i + 1, line.Length - i - 1);
                        result[result.Count - 1].Add(new Item(key, value));
                    }
                }
            }
            //获得项目值与项目名称
            return result;
            //输出
        }
        public enum ReadMode
        {
            SafeRead,
            UnSafeRead
        }
    }
    public static class WriteFiles
    {
        public static void Write(string str, string path)//写入字符串
        {
            FileStream fs = new FileStream(path, FileMode.Create);//创建变量
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(str);//写入
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
        public static void Write(string[] str, string path) //写入行集合
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            if (str != null)
            {
                foreach (string line in str)
                {
                    if (line.Contains("\n"))
                    {
                        sw.WriteLine(Function.ToFormatString(line));
                    }
                    else
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            else
            {
                sw.WriteLine("");
            }
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Dispose();
            fs.Dispose();
        }
        public static void Write(List<string> str , string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            if (str != null)
            {
                foreach (string line in str)
                {
                    if (line.Contains("\n"))
                    {
                        sw.WriteLine(Function.ToFormatString(line));
                    }
                    else
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            else
            {
                sw.WriteLine("");
            }
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Dispose();
            fs.Dispose();
        }
        /// <summary>
        /// 在文件指定行后插入文字
        /// </summary>
        /// <param name="str">插入文本</param>
        /// <param name="path">文件的完整路径</param>
        /// <param name="startIndex">插入行的索引（从零开始）</param>
        public static bool WriteAdd(string[] str, string path, int startIndex)
        {
            if (File.Exists(path))
            {
                string[] tem = ReadFiles.ReadOnly(path, false);    //读取保存先前的数据
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //处理字符串
                string[] tem_output = new string[str.Length + tem.Length];
                for (int i = 0; i <= startIndex; i++)    //写入插入行以前的部分
                {
                    tem_output[i] = tem[i];
                }
                int j = 1;
                foreach (string tem_str in str)     //写入插入内容
                {
                    tem_output[startIndex + j] = str[j - 1];
                    j++;
                }
                for (int i = 0; i < tem.Length - startIndex; i++)   //写入历史文本的剩余部分
                {
                    tem_output[startIndex + j] = tem[startIndex + i];
                    j++;
                }
                //开始写入
                foreach (string tem_str in tem_output)
                {
                    sw.WriteLine(tem_str);
                }
                //清空缓存区
                sw.Flush();
                //关闭文件
                sw.Close();
                fs.Close();
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 从文件尾写入文本
        /// </summary>
        /// <param name="str">要写入的文本</param>
        /// <param name="path">文件的完整路径</param>
        public static bool WriteAdd(string[] str, string path)
        {
            if (File.Exists(path))
            {
                string[] tem = ReadFiles.ReadOnly(path, false);    //读取保存先前的数据
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);     //声明文件流
                                                            //开始写入
                foreach (string line in tem)
                {
                    sw.WriteLine(line);
                }
                foreach (string line in str)
                {
                    sw.WriteLine(line);
                }
                //清除缓存区
                sw.Flush();
                //关闭文件
                sw.Close();
                fs.Close();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class EditFiles
    {
        public static string DeleteLines(string path, int startIndex, int endIndex)
        {
            if (File.Exists(path))
            {
                if (startIndex <= endIndex)
                {
                    string[] tem_read = ReadFiles.ReadOnly(path, false);   //缓存原内容
                    if (endIndex < tem_read.Length)
                    {
                        string[] output_str = new string[tem_read.Length - (endIndex - startIndex + 1)];    //声明输出变量
                        int j = 0;  //为输出变量添加索引
                        for (int i = 0; i < startIndex; i++)
                        {
                            output_str[j] = tem_read[i];
                            j++;
                        }
                        for (int i = endIndex + 1; i < tem_read.Length; i++)
                        {
                            output_str[j] = tem_read[i];
                            j++;
                        }
                        WriteFiles.Write(output_str, path);
                        return "OK";
                    }
                    else
                    {
                        return "[Error]Index超出范围";
                    }
                }
                throw new ArgumentException("[Error]StartIndex大于EndIndex");
            }
            else
            {
                return "[Error]找不到文件";
            }
        }
    }
    public class DataEdit
    {
        public static int[] Sort(int[] input)
        {
            int i = 0;
            while (i < input.Length)
            {
                bool input_Changed = false;
                for (int k = i; k < input.Length; k++)
                {
                    int tem_int = input[k];
                    if (tem_int < input[i])
                    {
                        for (int j = k; j > i; j--)
                        {
                            input[j] = input[j - 1];
                        }
                        input[i] = tem_int;
                        input_Changed = true;
                        break;
                    }
                }
                if (!input_Changed)
                {
                    i++;
                }
            }
            return input;
        }
    }
    public class Item
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Item(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
    public class Function
    {
        public static string ToFormatString(string input)
        {
            return input.Replace("\n", "\\n");
        }
        public static string ToDataString(string input)
        {
            return input.Replace("\\n", "\n");
        }

    }
}

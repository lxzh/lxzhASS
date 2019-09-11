using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace lxzh {
    /// <summary>
    /// 封装文本文件的基本读写命令
    /// </summary>
    public class Logger {

        private const string LOG_PATH = "log";

        private static Logger instance;

        public static Logger getInstance(string fullName) {
            if (instance == null) {
                instance = new Logger(fullName);
            }
            instance.FullName = fullName;
            return instance;
        }

        private string fullName;     //读写文件的完整路径

        public string FullName {
            get { return fullName; }
            private set { fullName = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="FullName">文件完整路径</param>
        public Logger(string FullName) {
            fullName = FullName;
        }

        /// <summary>
        /// 将信息写入文件中:追加
        /// </summary>
        /// <param name="msg">要写入的信息</param>
        /// <returns>返回写入的结果</returns>
        public bool writeMsg(string msg) {
            return writeMsg(msg, true);
        }

        /// <summary>
        /// 将信息写入文件中,如果文件存在则清空文件内容
        /// </summary>
        /// <param name="msg">要写入的信息</param>
        /// <returns>返回写入的结果</returns>
        public bool writeMsgInit(string msg) {
            return writeMsg(msg, false);
        }

        private bool writeMsg(string msg, bool append) {
            bool result = true;
            try {
                object ob = new object();
                lock (ob) {
                    using (StreamWriter swLog = new StreamWriter(fullName, append, Encoding.GetEncoding("GBK"))) {
                        swLog.WriteLine(msg);
                        result = true;
                    }
                }
            } catch (Exception errMsg) {
                result = false;
                throw new Exception(errMsg.Message);
            }
            return result;
        }

        public static void createDir(string dir) {
            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
        }

        /// <summary>
        /// 获取一个文件名，判断文件是否被占用，如果占用，拷贝文件到临时目录，并返回改临时文件名
        /// </summary>
        /// <param name="filename">源文件名</param>
        /// <returns></returns>
        private string getFileName(string filename) {
            if (!File.Exists(filename)) {
                throw new Exception("文件不存在!");
            }
            string result = filename;
            //判断文件是否被占用
            IntPtr vHandle = Win32._lopen(filename, Win32.OF_READWRITE | Win32.OF_SHARE_DENY_NONE);
            if (vHandle == Win32.HFILE_ERROR) {
                FileInfo fi = new FileInfo(filename);
                createDir("log");
                string tmpName = LOG_PATH + "/" + fi.Name.Replace(fi.Extension, Util.GUID + fi.Extension);
                File.Copy(filename, tmpName);
                result = tmpName;
            } else {
                Win32.CloseHandle(vHandle);
            }
            return result;
        }

        /// <summary>
        /// 从文件中一次性全部读取字符串
        /// </summary>
        /// <returns>返回读取的字符串</returns>
        public string readMsg() {
            string rtReturn = "";
            string filename = getFileName(fullName);
            try {
                object obj = new object();
                lock (obj) {
                    using (StreamReader sr = new StreamReader(filename, Encoding.GetEncoding("GBK"))) {
                        rtReturn = sr.ReadToEnd();
                    }
                }
            } catch {
                rtReturn = "";
                throw new Exception("打开文件出错，请检查文件" + fullName + "是否正被打开，请先关闭该文件");
            }
            return rtReturn;
        }

        /// <summary>
        /// 从文件中逐行读取内容
        /// </summary>
        /// <returns>返回逐行读取的列表</returns>
        public ArrayList readMsgByLine() {
            string filename = getFileName(fullName);
            ArrayList arrReturn = new ArrayList();
            try {
                object obj = new object();
                lock (obj) {
                    using (StreamReader sr = new StreamReader(filename, Encoding.GetEncoding("GBK"))) {
                        string str;
                        while ((str = sr.ReadLine()) != null) {
                            arrReturn.Add(str);
                        }
                    }
                }
            } catch {
                arrReturn = new ArrayList();
                throw new Exception("文件" + fullName + "正被打开，请先关闭该文件");
            }
            return arrReturn;
        }
    }
}

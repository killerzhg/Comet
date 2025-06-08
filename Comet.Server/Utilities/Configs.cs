using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace RFTEST.Function
{
    internal class Configs
    {
        private static readonly Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        /// <summary>
        /// 根据键名获得键值
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>获取到的键值，若无该键存在，则返回空字符串</returns>
        public static string GetConfig(string key)
        {
            string val = string.Empty;
            if (_config.AppSettings.Settings.AllKeys.Contains(key))
            {
                val = _config.AppSettings.Settings[key].Value;
            }
            return val;
        }

        /// <summary>
        /// 写配置文件,如果节点不存在则自动创建
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        /// <returns></returns>
        public static bool SetConfig(string key, string value)
        {
            try
            {
                if (_config.AppSettings.Settings.AllKeys.Contains(key))
                    _config.AppSettings.Settings[key].Value = value;
                else
                    _config.AppSettings.Settings.Add(key, value);
                _config.Save(ConfigurationSaveMode.Modified);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 写配置文件(用键值创建),如果节点不存在则自动创建
        /// </summary>
        /// <param name="dict">键值对集合</param>
        /// <returns></returns>
        public static bool SetConfig(Dictionary<string, string> dict)
        {
            try
            {
                if (dict == null || dict.Count == 0)
                {
                    return false;
                }
                foreach (string key in dict.Keys)
                {
                    if (_config.AppSettings.Settings.AllKeys.Contains(key))
                    {
                        _config.AppSettings.Settings[key].Value = dict[key];
                    }
                    else
                    {
                        _config.AppSettings.Settings.Add(key, dict[key]);
                    }
                }
                _config.Save(ConfigurationSaveMode.Modified);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}


using System.Collections.Generic;
using System.IO;
using DreamCMS.Encrypt;

namespace DreamCMS.Config
{
    /// <summary>
    /// Class lấy các giá trị đã thiết lập sẵn
    /// Sử dụng cho lần cài đặt đầu tiên
    /// </summary>
    public class DConfig
    {
        #region # Properties #
        /// <summary>
        /// KeyPass mã hóa file config riêng
        /// </summary>
        static string KeyPass = "Config0708";

        /// <summary>
        /// Hàm kiểm tra đã có file config gốc hay chưa.
        /// </summary>
        /// <returns>bool</returns>
        static public bool IsCheck
        {
            get
            {
                if (File.Exists(DDefault.PathFileConfig))
                {
                    Xmlconfig xmlcg = new Xmlconfig(DDefault.PathFileConfig, false);
                    return xmlcg.ValidateXML(true);
                }

                return false;
            }                        
        }

        #endregion

        #region # Function #

        /// <summary>
        /// Hàm lấy giá trị config
        /// </summary>
        /// <param name="strKey">Key thuộc tính</param>
        /// <returns></returns>
        static public string GetValue(string strKey)
        {
            Xmlconfig xmlcg = new Xmlconfig(DDefault.PathFileConfig, false);
            return DHash.Decrypt(xmlcg.Settings[strKey].Value, KeyPass);
        }
        
        /// <summary>
        /// Hàm lưu giá trị config
        /// </summary>
        /// <param name="strKey">Key thuộc tính</param>
        /// <param name="strValue">Giá trị lưu (string)</param>
        /// <returns></returns>
        static public bool SetValue(string strKey, string strValue)
        {
            Xmlconfig xmlcg = new Xmlconfig(DDefault.PathFileConfig, true);
            xmlcg.Settings[strKey].Value = DHash.Encrypt(strValue, KeyPass);
            try
            {
                xmlcg.Save(DDefault.PathFileConfig);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Hàm lấy mảng các giá trị config 
        /// <para>VD: "Roles/PermissionsAdmin"</para>
        /// </summary>
        /// <param name="strGroup">Key nhóm cần lấy</param>
        /// <returns></returns>
        static public Dictionary<string, string> GetGroupValue(string strGroup)
        {
            Xmlconfig xmlcg = new Xmlconfig(DDefault.PathFileConfig, false);
            
            if (xmlcg.Settings[strGroup].ChildCount(true) > 0)
            {
                Dictionary<string, string> arr = new Dictionary<string, string>();
                foreach (ConfigSetting cs in xmlcg.Settings[strGroup].Children())
                    arr[cs.Name] = DHash.Decrypt(cs.Value, KeyPass);
                return arr;
            }

            return null;
        }       

        /// <summary>
        /// Hàm lưu mảng giá trị config
        /// </summary>
        /// <param name="strGroup">Key nhóm cần lưu</param>
        /// <param name="dic">Mảng giá trị config</param>
        /// <returns></returns>
        static public bool SetGroupValue(string strGroup,Dictionary<string, string> dic)
        {
            if (dic == null) { return false; }
            if (dic.Count == 0) { return false; }

            Xmlconfig xmlcg = new Xmlconfig(DDefault.PathFileConfig, true);

            foreach(var d in dic)
            {
                xmlcg.Settings[strGroup][d.Key].Value = DHash.Encrypt(d.Value, KeyPass);
            }

            try
            {
                xmlcg.Save(DDefault.PathFileConfig);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
    
}

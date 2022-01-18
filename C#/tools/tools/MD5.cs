using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace tools
{
    public class MD5_tools
    {
        public MD5_tools()
        {
            string password = "pome123";
            string randText = "mes.pome.com.tw";
            string encr = Encrypet_To_MD5(password, randText);
            string dec = Decrypt_To_MD5(encr, randText);
            Console.WriteLine(" A    : " + password);
            Console.WriteLine(" 加密 : " + encr);
            Console.WriteLine(" 解密 : " + dec);
        }

        /// <summary>
        /// 對稱式加密演算法
        /// 創建DES
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private TripleDES CreateDES(string key)
        {
            //使用密碼編譯服務提供者 (CSP) 所提供之實作，計算輸入資料的 MD5 雜湊值。 
            MD5 md5 = new MD5CryptoServiceProvider();
            //以存取 TripleDES 演算法的密碼編譯服務供應者 (CSP) 版本。
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = md5.ComputeHash(Encoding.ASCII.GetBytes(key));//字串加密 
            des.IV = new byte[des.BlockSize / 8];
            return des;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string Encrypet_To_MD5(string plainText,string key)
        {
            //將其他的摻雜值轉ASCII
            byte[] bytes = Encoding.ASCII.GetBytes(plainText);

            //建立 TripleDES
            TripleDES des =  CreateDES(key);
            //建立記憶體流
            MemoryStream ms = new MemoryStream();

            //資料流與密碼編譯轉換的資料流
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            //寫入一位元組序列至目前的 CryptoStream，並依所寫入的位元組數目進階這個資料流裡的目前位置。
            cs.Write(bytes,0, bytes.Length);
            //以緩衝區的目前狀態更新基礎資料來源或存放庫，並接著清除緩衝區
            cs.FlushFinalBlock();

            //將加密後的位元組陣列轉換為加密字串
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// MD5解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string Decrypt_To_MD5(string str,string key)
        {
            byte[] bytes = Convert.FromBase64String(str);
            //建立 TripleDES
            TripleDES des = CreateDES(key);

            //加密的字串轉換為位元組陣列
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();
            return Encoding.ASCII.GetString(ms.ToArray());
        }

        //// <summary>
        /// </summary>
        /// <param name="strSource">需要加密的明文</param>
        /// <returns>返回16位加密結果，該結果取32位加密結果的第9位到25位</returns>
        public static string Get16MD5Two(string source)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //獲取密文位元組陣列
            byte[] bytResult = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(source));
            //轉換成字串，並取9到25位
            string strResult = BitConverter.ToString(bytResult, 4, 8);
            //BitConverter轉換出來的字串會在每個字元中間產生一個分隔符，需要去除掉
            strResult = strResult.Replace("-", "");
            return strResult.ToUpper();
        }

        /// <summary>
        /// 建立雜湊字串適用於任何 MD5 雜湊函式 （在任何平臺） 上建立 32 個字元的十六進位制格式雜湊字串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Get32MD5One(string source)
        {
            using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(source));
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                string hash = sBuilder.ToString();
                return hash.ToUpper();
            }
        }

        /// <summary>
        /// 獲取16位md5加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Get16MD5One(string source)
        {
            using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(source));
                //轉換成字串，並取9到25位
                string sBuilder = BitConverter.ToString(data, 4, 8);
                //BitConverter轉換出來的字串會在每個字元中間產生一個分隔符，需要去除掉
                sBuilder = sBuilder.Replace("-", "");
                return sBuilder.ToString().ToUpper();
            }
        }

        //// <summary>
        /// </summary>
        /// <param name="strSource">需要加密的明文</param>
        /// <returns>返回32位加密結果，該結果取32位加密結果的第9位到25位</returns>
        public static string Get32MD5Two(string source)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //獲取密文位元組陣列
            byte[] bytResult = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(source));
            //轉換成字串，32位
            string strResult = BitConverter.ToString(bytResult);
            //BitConverter轉換出來的字串會在每個字元中間產生一個分隔符，需要去除掉
            strResult = strResult.Replace("-", "");
            return strResult.ToUpper();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace cl_sstut
{
    /// <summary>
    /// Для хеширования строки
    /// </summary>
   public class HashText
    {
        /// <summary>
        /// Получаем обычный MD5 от сторки типа STRING
        /// </summary>
        /// <param name="inputHashText">Хуярь сюды</param>
        /// <returns></returns>
        public static string Md5hash(string inputHashText)
        {
            //я хз нахер это надо, мне тупо нехватает мозгов, чтобы понять.
            using (MD5 md5Hash = MD5.Create())
            {   
                string hash = GetMd5Hash(md5Hash, inputHashText);
                return hash.ToUpper();
            }            
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Получаем специальный MD5
        /// </summary>
        /// <param name="inputHashText">Текст для оработки</param>
        /// <returns></returns>
        public static string CkassaMD5(string inputHashText) => HashText.Md5hash(HashText.Md5hash(inputHashText)).ToUpper();
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace AddressProvider.Extensions
{
    public static class Conversions
    {

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }


        public static T To<T>(this object value)
        {
            Type conversionType = typeof(T);
            return (T)To(value, conversionType);
        }

        public static byte[] StringToByte(this string strByte)
        {
            var stringByte = strByte.Split(';');

            if (stringByte.Count() == 0)
                return null;

            var arquivo = new byte[stringByte.Count() - 1];
            for (int i = 0; i < stringByte.Count() - 1; i++)
            {
                arquivo[i] = stringByte[i].To<byte>();
            }

            return arquivo;
        }

        public static DateTime StringToDate(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return DateTime.MinValue;

            if (value.Replace("00:00:00", "").Trim().Replace("/", "").Length != 8)
                throw new Exception(string.Format("Formato incorreto de data [{0}]", value));

            if (value.Substring(2, 1).Equals("/") & value.Substring(5, 1).Equals("/"))
                return value.To<DateTime>();

            var day = value.Substring(0, 2).To<int>();
            var month = value.Substring(2, 2).To<int>();
            var year = value.Substring(4, 4).To<int>();

            return new DateTime(year, month, day);
        }

        public static object To(this object value, Type conversionType)
        {
            if (conversionType == null)
                throw new ArgumentNullException("conversionType");

            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;
                NullableConverter nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            else if (conversionType == typeof(Guid))
            {
                return new Guid(value.ToString());

            }
            else if (conversionType == typeof(Int64) && value.GetType() == typeof(int))
            {
                throw new InvalidOperationException("Can't convert an Int64 (long) to Int32(int).");
            }

            if ((value is string || value == null || value is DBNull) &&
                (conversionType == typeof(short) ||
                conversionType == typeof(int) ||
                conversionType == typeof(long) ||
                conversionType == typeof(double) ||
                conversionType == typeof(decimal) ||
                conversionType == typeof(float)))
            {
                decimal number;
                if (!decimal.TryParse(value as string, out number))
                    value = "0";
            }

            return Convert.ChangeType(value, conversionType);
        }

        /// <summary>
        /// Remover acentos da strins
        /// </summary>
        /// <param name="pValue">String com Acentos</param>
        /// <returns>Uma string sem accentos</returns>
        public static string NoAccents(this string pValue)
        {
            if (String.IsNullOrEmpty(pValue))
                return pValue;

            string normalizedString = pValue.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(normalizedString[i]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }



        /// <summary>
        /// Converter caracteres de simbols (&) para (&amp) < > " ' para códigos HMTLs
        /// </summary>
        /// <param name="pValue">String com Acentos</param>
        /// <returns>Uma string sem accentos</returns>
        public static string ToParserSimbol(this string pValue)
        {
            if (String.IsNullOrEmpty(pValue))
                return pValue;

            var dic = new Dictionary<char, string>(){ 
                { '<' , "&alt;" },
                { '>' , "&gt;" },
                { '&' , "&amp;" },
                { '"' , "&quot;" },
                {  "'"[0] , "&#39" }
            };

            foreach (var item in dic)
                pValue = pValue.Replace(item.Key.ToString(), item.Value);

            return pValue;
        }

        
        /// <summary>
        /// Converte os caracteres especias Unicode para UTF8
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns>
        public static string ReplaceUnicodeCaractersToUTF8(string originalString)
        {
            var retorno = originalString;
            try
            {
                while (retorno.IndexOf("\\u") > -1)
                {
                    var unicode = retorno.Substring(retorno.IndexOf("\\u"), 6);
                    var unicodeStrOnly = unicode.Remove(0, 1).ToUpper();
                    string letter;
                    if (GetUnicodes().TryGetValue(unicodeStrOnly, out letter))
                    {
                        retorno = retorno.Replace(unicode, letter);
                    }

                }
            }
            catch (Exception)
            {
                return originalString;                
            }
           

            return originalString;
        }

        private static Dictionary<string, string> GetUnicodes()
        {
            var unicodes = new Dictionary<string, string>();

            unicodes.Add("U0000", "	 	");
            unicodes.Add("U0001", "	 	");
            unicodes.Add("U0002", "	 	");
            unicodes.Add("U0003", "	 	");
            unicodes.Add("U0004", "	 	");
            unicodes.Add("U0005", "	 	");
            unicodes.Add("U0006", "	 	");
            unicodes.Add("U0007", "	 	");
            unicodes.Add("U0008", "	 	");
            unicodes.Add("U0009", "	 	");
            unicodes.Add("U000A", "	 	");
            unicodes.Add("U000B", "	 	");
            unicodes.Add("U000C", "	 	");
            unicodes.Add("U000D", "	 	");
            unicodes.Add("U000E", "	 	");
            unicodes.Add("U000F", "	 	");
            unicodes.Add("U0010", "	 	");
            unicodes.Add("U0011", "	 	");
            unicodes.Add("U0012", "	 	");
            unicodes.Add("U0013", "	 	");
            unicodes.Add("U0014", "	 	");
            unicodes.Add("U0015", "	 	");
            unicodes.Add("U0016", "	 	");
            unicodes.Add("U0017", "	 	");
            unicodes.Add("U0018", "	 	");
            unicodes.Add("U0019", "	 	");
            unicodes.Add("U001A", "	 	");
            unicodes.Add("U001B", "	 	");
            unicodes.Add("U001C", "	 	");
            unicodes.Add("U001D", "	 	");
            unicodes.Add("U001E", "	 	");
            unicodes.Add("U001F", "	 	");
            unicodes.Add("U0020", "		");
            unicodes.Add("U0021", "!");
            unicodes.Add("U0022", "\"");
            unicodes.Add("U0023", "#");
            unicodes.Add("U0024", "$");
            unicodes.Add("U0025", "%");
            unicodes.Add("U0026", "&");
            unicodes.Add("U0027", "'");
            unicodes.Add("U0028", "(");
            unicodes.Add("U0029", ")");
            unicodes.Add("U002A", "*");
            unicodes.Add("U002B", "+");
            unicodes.Add("U002C", ",");
            unicodes.Add("U002D", "-");
            unicodes.Add("U002E", ".");
            unicodes.Add("U002F", "/");
            unicodes.Add("U0030", "0");
            unicodes.Add("U0031", "1");
            unicodes.Add("U0032", "2");
            unicodes.Add("U0033", "3");
            unicodes.Add("U0034", "4");
            unicodes.Add("U0035", "5");
            unicodes.Add("U0036", "6");
            unicodes.Add("U0037", "7");
            unicodes.Add("U0038", "8");
            unicodes.Add("U0039", "9");
            unicodes.Add("U003A", ":");
            unicodes.Add("U003B", ";");
            unicodes.Add("U003C", "<");
            unicodes.Add("U003D", "=");
            unicodes.Add("U003E", ">");
            unicodes.Add("U003F", "?");
            unicodes.Add("U0040", "@");
            unicodes.Add("U0041", "A");
            unicodes.Add("U0042", "B");
            unicodes.Add("U0043", "C");
            unicodes.Add("U0044", "D");
            unicodes.Add("U0045", "E");
            unicodes.Add("U0046", "F");
            unicodes.Add("U0047", "G");
            unicodes.Add("U0048", "H");
            unicodes.Add("U0049", "I");
            unicodes.Add("U004A", "J");
            unicodes.Add("U004B", "K");
            unicodes.Add("U004C", "L");
            unicodes.Add("U004D", "M");
            unicodes.Add("U004E", "N");
            unicodes.Add("U004F", "O");
            unicodes.Add("U0050", "P");
            unicodes.Add("U0051", "Q");
            unicodes.Add("U0052", "R");
            unicodes.Add("U0053", "S");
            unicodes.Add("U0054", "T");
            unicodes.Add("U0055", "U");
            unicodes.Add("U0056", "V");
            unicodes.Add("U0057", "W");
            unicodes.Add("U0058", "X");
            unicodes.Add("U0059", "Y");
            unicodes.Add("U005A", "Z");
            unicodes.Add("U005B", "[");
            unicodes.Add("U005C", "\\");
            unicodes.Add("U005D", "]");
            unicodes.Add("U005E", "^");
            unicodes.Add("U005F", "_");
            unicodes.Add("U0060", "`");
            unicodes.Add("U0061", "a");
            unicodes.Add("U0062", "b");
            unicodes.Add("U0063", "c");
            unicodes.Add("U0064", "d");
            unicodes.Add("U0065", "e");
            unicodes.Add("U0066", "f");
            unicodes.Add("U0067", "g");
            unicodes.Add("U0068", "h");
            unicodes.Add("U0069", "i");
            unicodes.Add("U006A", "j");
            unicodes.Add("U006B", "k");
            unicodes.Add("U006C", "l");
            unicodes.Add("U006D", "m");
            unicodes.Add("U006E", "n");
            unicodes.Add("U006F", "o");
            unicodes.Add("U0070", "p");
            unicodes.Add("U0071", "q");
            unicodes.Add("U0072", "r");
            unicodes.Add("U0073", "s");
            unicodes.Add("U0074", "t");
            unicodes.Add("U0075", "u");
            unicodes.Add("U0076", "v");
            unicodes.Add("U0077", "w");
            unicodes.Add("U0078", "x");
            unicodes.Add("U0079", "y");
            unicodes.Add("U007A", "z");
            unicodes.Add("U007B", "{");
            unicodes.Add("U007C", "|");
            unicodes.Add("U007D", "}");
            unicodes.Add("U007E", "~");
            unicodes.Add("U007F", "	 	");
            unicodes.Add("U0080", "	 	");
            unicodes.Add("U0081", "	 	");
            unicodes.Add("U0082", "	 	");
            unicodes.Add("U0083", "	 	");
            unicodes.Add("U0084", "	 	");
            unicodes.Add("U0085", "	 	");
            unicodes.Add("U0086", "	 	");
            unicodes.Add("U0087", "	 	");
            unicodes.Add("U0088", "	 	");
            unicodes.Add("U0089", "	 	");
            unicodes.Add("U008A", "	 	");
            unicodes.Add("U008B", "	 	");
            unicodes.Add("U008C", "	 	");
            unicodes.Add("U008D", "	 	");
            unicodes.Add("U008E", "	 	");
            unicodes.Add("U008F", "	 	");
            unicodes.Add("U0090", "	 	");
            unicodes.Add("U0091", "	 	");
            unicodes.Add("U0092", "	 	");
            unicodes.Add("U0093", "	 	");
            unicodes.Add("U0094", "	 	");
            unicodes.Add("U0095", "	 	");
            unicodes.Add("U0096", "	 	");
            unicodes.Add("U0097", "	 	");
            unicodes.Add("U0098", "	 	");
            unicodes.Add("U0099", "	 	");
            unicodes.Add("U009A", "	 	");
            unicodes.Add("U009B", "	 	");
            unicodes.Add("U009C", "	 	");
            unicodes.Add("U009D", "	 	");
            unicodes.Add("U009E", "	 	");
            unicodes.Add("U009F", "	 	");
            unicodes.Add("U00A0", "	 	");
            unicodes.Add("U00A1", "¡");
            unicodes.Add("U00A2", "¢");
            unicodes.Add("U00A3", "£");
            unicodes.Add("U00A4", "¤");
            unicodes.Add("U00A5", "¥");
            unicodes.Add("U00A6", "¦");
            unicodes.Add("U00A7", "§");
            unicodes.Add("U00A8", "¨");
            unicodes.Add("U00A9", "©");
            unicodes.Add("U00AA", "ª");
            unicodes.Add("U00AB", "«");
            unicodes.Add("U00AC", "¬");
            unicodes.Add("U00AD", "");
            unicodes.Add("U00AE", "®");
            unicodes.Add("U00AF", "¯");
            unicodes.Add("U00B0", "°");
            unicodes.Add("U00B1", "±");
            unicodes.Add("U00B2", "²");
            unicodes.Add("U00B3", "³");
            unicodes.Add("U00B4", "´");
            unicodes.Add("U00B5", "µ");
            unicodes.Add("U00B6", "¶");
            unicodes.Add("U00B7", "·");
            unicodes.Add("U00B8", "¸");
            unicodes.Add("U00B9", "¹");
            unicodes.Add("U00BA", "º");
            unicodes.Add("U00BB", "»");
            unicodes.Add("U00BC", "¼");
            unicodes.Add("U00BD", "½");
            unicodes.Add("U00BE", "¾");
            unicodes.Add("U00BF", "¿");
            unicodes.Add("U00C0", "À");
            unicodes.Add("U00C1", "Á");
            unicodes.Add("U00C2", "Â");
            unicodes.Add("U00C3", "Ã");
            unicodes.Add("U00C4", "Ä");
            unicodes.Add("U00C5", "Å");
            unicodes.Add("U00C6", "Æ");
            unicodes.Add("U00C7", "Ç");
            unicodes.Add("U00C8", "È");
            unicodes.Add("U00C9", "É");
            unicodes.Add("U00CA", "Ê");
            unicodes.Add("U00CB", "Ë");
            unicodes.Add("U00CC", "Ì");
            unicodes.Add("U00CD", "Í");
            unicodes.Add("U00CE", "Î");
            unicodes.Add("U00CF", "Ï");
            unicodes.Add("U00D0", "Ð");
            unicodes.Add("U00D1", "Ñ");
            unicodes.Add("U00D2", "Ò");
            unicodes.Add("U00D3", "Ó");
            unicodes.Add("U00D4", "Ô");
            unicodes.Add("U00D5", "Õ");
            unicodes.Add("U00D6", "Ö");
            unicodes.Add("U00D7", "×");
            unicodes.Add("U00D8", "Ø");
            unicodes.Add("U00D9", "Ù");
            unicodes.Add("U00DA", "Ú");
            unicodes.Add("U00DB", "Û");
            unicodes.Add("U00DC", "Ü");
            unicodes.Add("U00DD", "Ý");
            unicodes.Add("U00DE", "Þ");
            unicodes.Add("U00DF", "ß");
            unicodes.Add("U00E0", "à");
            unicodes.Add("U00E1", "á");
            unicodes.Add("U00E2", "â");
            unicodes.Add("U00E3", "ã");
            unicodes.Add("U00E4", "ä");
            unicodes.Add("U00E5", "å");
            unicodes.Add("U00E6", "æ");
            unicodes.Add("U00E7", "ç");
            unicodes.Add("U00E8", "è");
            unicodes.Add("U00E9", "é");
            unicodes.Add("U00EA", "ê");
            unicodes.Add("U00EB", "ë");
            unicodes.Add("U00EC", "ì");
            unicodes.Add("U00ED", "í");
            unicodes.Add("U00EE", "î");
            unicodes.Add("U00EF", "ï");
            unicodes.Add("U00F0", "ð");
            unicodes.Add("U00F1", "ñ");
            unicodes.Add("U00F2", "ò");
            unicodes.Add("U00F3", "ó");
            unicodes.Add("U00F4", "ô");
            unicodes.Add("U00F5", "õ");
            unicodes.Add("U00F6", "ö");
            unicodes.Add("U00F7", "÷");
            unicodes.Add("U00F8", "ø");
            unicodes.Add("U00F9", "ù");
            unicodes.Add("U00FA", "ú");
            unicodes.Add("U00FB", "û");
            unicodes.Add("U00FC", "ü");
            unicodes.Add("U00FD", "ý");
            unicodes.Add("U00FE", "þ");
            unicodes.Add("U00FF", "ÿ");

            return unicodes;
        }
       


    }
}

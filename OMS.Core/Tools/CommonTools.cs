using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using RazorLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.DrawingCore;
using System.DrawingCore.Drawing2D;
using System.DrawingCore.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OMS.Core.Tools
{
    public class CommonTools
    {
        #region encrypt
        //默认密钥向量
        private static byte[] _aesKey = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCC, 0xEF };

        /// <summary>
        /// AES加密算法
        /// </summary>
        /// <param name="plainText">明文字符串</param>
        /// <returns>将加密后的密文转换为Base64编码，以便显示</returns>
        public static string AESEncrypt(string plainText, string Key)
        {
            //分组加密算法
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组
            //设置密钥及密钥向量
            des.Key = Encoding.UTF8.GetBytes(Key);
            des.IV = _aesKey;
            byte[] cipherBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cipherBytes = ms.ToArray();//得到加密后的字节数组
                    cs.Close();
                    ms.Close();
                }
            }
            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">密文字符串</param>
        /// <returns>返回解密后的明文字符串</returns>
        public static string AESDecrypt(string showText, string Key)
        {
            showText = showText.Replace("%3d", "=").Replace(" ", "+").Replace("%2b", "+");

            byte[] cipherText = Convert.FromBase64String(showText);
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(Key);
            des.IV = _aesKey;
            byte[] decryptBytes = new byte[cipherText.Length];
            using (MemoryStream ms = new MemoryStream(cipherText))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    cs.Close();
                    ms.Close();
                }
            }
            return Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");   ///将字符串后尾的'\0'去掉
        }

        /// <summary>
        /// md5摘要
        /// </summary>
        /// <param name="plain"></param>
        /// <param name="isMd5"></param>
        /// <returns></returns>
        public static string Md5Hash(string plain, string format = "X2")
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            byte[] result = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(plain));
            StringBuilder sb = new StringBuilder();
            foreach (var i in result)
                sb.Append(i.ToString(format));
            return sb.ToString();
        }
        #endregion

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string CreateRandomStr(int length, RandomType randomType = RandomType.Mix)
        {
            var digitArray = new string[] { "2", "3", "4", "5", "6", "7", "8", "9" };//去除0，1
            var letterArray = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };//小写字母，去除l、o
            var capArray = letterArray.Select(i => i.ToUpper());//大写字母
            IList<string> randomArray;
            switch (randomType)
            {
                case RandomType.Digit:
                    randomArray = digitArray;
                    break;
                case RandomType.Letter:
                    randomArray = letterArray.Concat(capArray).ToList();
                    break;
                default:
                    randomArray = digitArray.Concat(letterArray).Concat(capArray).ToList();
                    break;
            }
            var random = new Random();
            var builder = new StringBuilder();
            //生成随机字符串
            for (int i = 0; i < length; i++)
            {
                builder.Append(randomArray[random.Next(randomArray.Count)]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="validateCode">验证码内容</param>
        /// <returns></returns>
        public static byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 15.0), 25);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 编译Razor模板文件，生成字符串
        /// </summary>
        /// <remarks>
        /// 开源插件：RazorLight
        /// </remarks>
        /// <param name="key">缓存key</param>
        /// <param name="model">Model</param>
        /// <param name="viewbag">ViewBag</param>
        /// <returns></returns>
        public static string RunRazorTemplate(string key, string plain, object model = null, dynamic viewbag = null)
        {
            var engine = new RazorLightEngineBuilder().UseMemoryCachingProvider().Build();
            Type modelType = typeof(Nullable);
            if (model != null)
            {
                modelType = model.GetType();
            }
            return engine.CompileRenderAsync(key, plain, model, modelType, viewbag).Result;
        }

        #region excel
        /// <summary>
        /// 读取excel
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="dropTitle">舍弃标头</param>
        /// <returns></returns>
        public static DataTable ReadExcel(Stream stream, bool dropTitle = true)
        {
            try
            {
                var result = new DataTable();

                var hssfWorkBook = new HSSFWorkbook(stream);

                var sheet = hssfWorkBook.GetSheetAt(0);
                var rows = sheet.GetRowEnumerator();
                var rowNum = sheet.GetRow(0).LastCellNum;
                for (int j = 0; j < rowNum; j++)
                {
                    result.Columns.Add();
                }
                if (dropTitle)
                {
                    rows.MoveNext();
                }
                while (rows.MoveNext())
                {
                    HSSFRow row = rows.Current as HSSFRow;
                    DataRow dr = result.NewRow();
                    for (int i = 0; i < rowNum; i++)
                    {
                        var cell = row.GetCell(i);
                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                    result.Rows.Add(dr);
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static Stream WriteExcel(DataTable table, int[] widthArray = null)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                using (table)
                {
                    IWorkbook workbook = new HSSFWorkbook();
                    if (table.TableName == "" || table.TableName == null)
                    {
                        table.TableName = "sheet0";
                    }
                    var defaultStyle = workbook.CreateCellStyle();
                    var defaultFont = workbook.CreateFont();
                    defaultFont.FontName = "宋体";//字体样式
                    defaultFont.FontHeightInPoints = 11;//字体大小
                    defaultStyle.WrapText = true;//自动换行
                    defaultStyle.VerticalAlignment = VerticalAlignment.Center;//垂直居中
                    defaultStyle.SetFont(defaultFont);

                    ISheet sheet = workbook.CreateSheet(table.TableName);
                    sheet.DefaultRowHeight = 2 * 256;//行高

                    IRow headerRow = sheet.CreateRow(0);
                    var headStyle = workbook.CreateCellStyle();
                    headStyle.CloneStyleFrom(defaultStyle);
                    var headFont = workbook.CreateFont();
                    headFont.FontName = "宋体";//字体样式
                    headFont.FontHeightInPoints = 11;//字体大小
                    headFont.IsBold = true;
                    headStyle.SetFont(headFont);
                    headerRow.RowStyle = headStyle;

                    var isSetWidth = widthArray != null;
                    foreach (DataColumn column in table.Columns)//列名
                    {
                        var cell = headerRow.CreateCell(column.Ordinal);
                        if (isSetWidth)
                        {
                            sheet.SetColumnWidth(column.Ordinal, widthArray[column.Ordinal] * 256);//列宽
                        }
                        else
                        {
                            sheet.SetColumnWidth(column.Ordinal, 20 * 256);//列宽
                        }
                        cell.CellStyle = headStyle;
                        if (string.IsNullOrEmpty(column.ColumnName))
                        {
                            cell.SetCellValue("列" + column.Ordinal);
                        }
                        else
                        {
                            cell.SetCellValue(column.ColumnName);
                        }
                    }
                    int rowIndex = 1;
                    foreach (DataRow row in table.Rows)
                    {
                        IRow dataRow = sheet.CreateRow(rowIndex);
                        foreach (DataColumn column in table.Columns)
                        {
                            var cell = dataRow.CreateCell(column.Ordinal);
                            cell.CellStyle = defaultStyle;
                            cell.SetCellValue(row[column].ToString());
                        }
                        rowIndex++;
                    }
                    workbook.Write(ms);
                    ms.Flush();
                    ms.Position = 0;
                    return ms;
                }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region thread
        public static void ExcuteTask(Func<bool> action)
        {
            var tryTime = 0;
            bool isSuccess = false;
            var task = Task.Factory.StartNew(() =>
            {
                while (!isSuccess)
                {
                    isSuccess = action();
                    if (tryTime > 3)
                    {
                        break;
                    }
                    tryTime++;
                    if (!isSuccess)
                    {
                        Thread.Sleep(5000);
                    }
                }
                return isSuccess;
            });
        }
        #endregion

        #region io
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="filepaths">要压缩文件路径</param>
        /// <returns></returns>
        public static Stream ZipFiles(params string[] filePaths)
        {
            if (filePaths == null && filePaths.Length == 0)
                return null;
            var zipStream = new MemoryStream();
            using (var zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                foreach (var item in filePaths)
                {
                    var entry = zip.CreateEntry(Path.GetFileName(item));
                    using (var entryStream = entry.Open())
                    {
                        using (var fileStream = new FileStream(item, FileMode.Open))
                        {
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }
            }
            zipStream.Position = 0;
            return zipStream;
        }
        #endregion

        public enum RandomType
        {
            /// <summary>
            /// 纯数字
            /// </summary>
            Digit,
            /// <summary>
            /// 纯字母
            /// </summary>
            Letter,
            /// <summary>
            /// 混合
            /// </summary>
            Mix
        }

        /// <summary>
        /// 单据编号
        /// </summary>
        /// <param name="billNo">pf</param>
        /// <returns></returns>
        public static string GetSerialNumber(string billNo)
        {
            Random random = new Random();
            return billNo + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
    }
}

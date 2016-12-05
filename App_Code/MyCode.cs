using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace MyCode
{
    public class DatabaseConnection
    {
        private SqlConnection conn;

        public DatabaseConnection(string ConnectionString)
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ToString());
        }

        public SqlConnection Openconn()
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
            {
                conn.Open();
            }
            return conn;
        }

        public SqlConnection Closeconn()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            return conn;
        }

        public SqlDataReader ReturnDataReader(String StoredProcedure, Dictionary<string, object> Dic = null)
        {
            Closeconn();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = Openconn();
                cmd.CommandText = StoredProcedure;
                //cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader rd;
                if (Dic != null)
                {
                    foreach (var x in Dic)
                    {
                        cmd.Parameters.AddWithValue(x.Key, x.Value);
                    }
                }
                rd = cmd.ExecuteReader();
                return rd;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                cmd = null;
            }
        }

        public void ExecuteQuery(string StoredProcedure, Dictionary<string, object> Dic = null)
        {
            Closeconn();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = Openconn();
                cmd.CommandText = StoredProcedure;
                //cmd.CommandType = CommandType.StoredProcedure;
                if (Dic != null)
                {
                    foreach (var x in Dic)
                    {
                        cmd.Parameters.AddWithValue(x.Key, x.Value);
                    }
                }
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                cmd = null;
            }
        }

        public int ExecuteScalar(string StoredProcedure, Dictionary<string, object> Dic)
        {
            Closeconn();
            int i = 0;
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = Openconn();
                cmd.CommandText = StoredProcedure + "; SELECT CAST(scope_identity() AS int)";
                //cmd.CommandType = CommandType.StoredProcedure;
                if (Dic != null)
                {
                    foreach (var x in Dic)
                    {
                        cmd.Parameters.AddWithValue(x.Key, x.Value);
                    }
                }
                i = Convert.ToInt32(cmd.ExecuteScalar());
                return i;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                cmd = null;
            }
        }

        public bool Read()
        {
            throw new NotImplementedException();
        }
    }
    public class ImageResize
    {
        /// <summary>
        /// Opret et Thumb, baseret på en fil og en mappe
        /// </summary>
        /// <param name="Filename">Hvad hedder filen</param>
        /// <param name="UploadFolder">Hvor er den uploadet til</param>
        public string MakeThumb(string UploadFolder, string ThumbUploadFolder, FileUpload FileUploadId, int newWidth, Int64 Quality)
        {
            var GUID = Guid.NewGuid();
            // gem i mappen
            FileUploadId.SaveAs(HttpContext.Current.Server.MapPath("~/") + UploadFolder + GUID + FileUploadId.FileName);

            // find det uploadede image
            System.Drawing.Image OriginalImg = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath("~/") + UploadFolder + GUID + FileUploadId.FileName);

            // find højde og bredde på image
            int originalWidth = OriginalImg.Width;
            int originalHeight = OriginalImg.Height;

            double ratio = (double)originalWidth;
            int newHeight = originalHeight;

            if (originalWidth > newWidth)
            {
                // beregn den nye højde på thumbnailbilledet
                ratio = newWidth / (double)originalWidth;
                newHeight = Convert.ToInt32(ratio * originalHeight);
            }
            else
            {
                newWidth = originalWidth;
            }


            Bitmap Thumb = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            Thumb.SetResolution(OriginalImg.HorizontalResolution, OriginalImg.VerticalResolution);

            // dette er alt hvad der skal til, for at kunne beholde billedets transparens!
            Thumb.MakeTransparent();



            Graphics ThumbMaker = Graphics.FromImage(Thumb);
            ThumbMaker.InterpolationMode = InterpolationMode.HighQualityBicubic;

            ThumbMaker.DrawImage(OriginalImg,
                new Rectangle(0, 0, newWidth, newHeight),
                new Rectangle(0, 0, originalWidth, originalHeight),
                GraphicsUnit.Pixel);


            ImageCodecInfo encoder;
            string fileExt = System.IO.Path.GetExtension(GUID + FileUploadId.FileName);
            switch (fileExt)
            {
                case ".png":
                    encoder = GetEncoderInfo("image/png");
                    break;

                case ".gif":
                    encoder = GetEncoderInfo("image/gif");
                    break;

                default:
                    // default til JPG 
                    encoder = GetEncoderInfo("image/jpeg");
                    break;
            }

            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Quality);

            // gem thumbnail i mappen /Images/Uploads/Thumbs/
            Thumb.Save(HttpContext.Current.Server.MapPath("~/" + ThumbUploadFolder) + GUID + FileUploadId.FileName, encoder, encoderParameters);

            // Fjern originalbilledet, thumbnail mm, fra computerhukommelsen
            OriginalImg.Dispose();
            ThumbMaker.Dispose();
            Thumb.Dispose();

            return GUID + FileUploadId.FileName;

        }/// <summary>
        /// Opret et Thumb, baseret på en fil og en mappe
        /// </summary>
        /// <param name="Filename">Hvad hedder filen</param>
        /// <param name="UploadFolder">Hvor er den uploadet til</param>
        public string MultiThumbUploader(string UploadFolder, string ThumbUploadFolder, HttpPostedFile Postedfile, int newWidth, Int64 Quality)
        {
            var GUID = Guid.NewGuid();
            // gem i mappen
            Postedfile.SaveAs(HttpContext.Current.Server.MapPath("~/") + UploadFolder + GUID + Postedfile.FileName);

            // find det uploadede image
            System.Drawing.Image OriginalImg = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath("~/") + UploadFolder + GUID + Postedfile.FileName);

            // find højde og bredde på image
            int originalWidth = OriginalImg.Width;
            int originalHeight = OriginalImg.Height;

            double ratio = (double)originalWidth;
            int newHeight = originalHeight;

            if (originalWidth > newWidth)
            {
                // beregn den nye højde på thumbnailbilledet
                ratio = newWidth / (double)originalWidth;
                newHeight = Convert.ToInt32(ratio * originalHeight);
            }
            else
            {
                newWidth = originalWidth;
            }


            Bitmap Thumb = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            Thumb.SetResolution(OriginalImg.HorizontalResolution, OriginalImg.VerticalResolution);

            // dette er alt hvad der skal til, for at kunne beholde billedets transparens!
            Thumb.MakeTransparent();



            Graphics ThumbMaker = Graphics.FromImage(Thumb);
            ThumbMaker.InterpolationMode = InterpolationMode.HighQualityBicubic;

            ThumbMaker.DrawImage(OriginalImg,
                new Rectangle(0, 0, newWidth, newHeight),
                new Rectangle(0, 0, originalWidth, originalHeight),
                GraphicsUnit.Pixel);


            ImageCodecInfo encoder;
            string fileExt = System.IO.Path.GetExtension(GUID + Postedfile.FileName);
            switch (fileExt)
            {
                case ".png":
                    encoder = GetEncoderInfo("image/png");
                    break;

                case ".gif":
                    encoder = GetEncoderInfo("image/gif");
                    break;

                default:
                    // default til JPG 
                    encoder = GetEncoderInfo("image/jpeg");
                    break;
            }


            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Quality);
            
            // gem thumbnail i mappen 
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/" + ThumbUploadFolder) + GUID + Postedfile.FileName, FileMode.Create, FileAccess.ReadWrite))
                {
                    Thumb.Save(memory, encoder, encoderParameters);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            //// Fjern originalbilledet, thumbnail mm, fra computerhukommelsen
            OriginalImg.Dispose();
            ThumbMaker.Dispose();
            Thumb.Dispose();

            return GUID + Postedfile.FileName;

        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < encoders.Length; i++)
            {
                if (encoders[i].MimeType == mimeType)
                {
                    return encoders[i];
                }
            }
            return null;
        }
    }
    public class Security
    {
        public Security()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static String sha1_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA1 hash = SHA1.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        public static String sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        public static String sha384_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA384 hash = SHA384.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        public static String sha512_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA512 hash = SHA512.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        public static String md5_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (MD5 hash = MD5.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static Random random = new Random((int)DateTime.Now.Ticks);
        public static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
    public class Validator
    {
        public Validator()
        {

        }
        public static bool Bogstaver(string input, int længde = 0)
        {
            bool status = false;
            if (input.Length >= længde)
            {
                Match match = Regex.Match(input, @"^[a-zA-Z ]+$",
                    RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    status = true;
                }
            }
            return status;
        }
        public static bool Adresse(string input, int længde = 0)
        {
            bool status = false;
            if (input.Length >= længde)
            {
                Match match = Regex.Match(input, @"^[a-zA-Z0-9 ]+$",
                    RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    status = true;
                }
            }
            return status;
        }
        public static bool Tal(string input, int længde = 0)
        {
            bool status = false;

            if (input.Contains(' '))
            {
                string[] words = input.Split(' ');

                string s = "";

                foreach (string word in words)
                {
                    s += word;
                }
                if (s.Length == længde)
                {
                    Match match = Regex.Match(s, @"^\d+$",
                        RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        status = true;
                    }
                }
            }
            else if (input.Length == længde)
            {
                Match match = Regex.Match(input, @"^\d+$",
                    RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    status = true;
                }
            }
            return status;
        }
        public static bool Email(string emailaddress, bool onlinestatus = false)
        {
            bool status = false;

            Match match = Regex.Match(emailaddress, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                    RegexOptions.IgnoreCase);
            if (match.Success)
            {
                status = true;
                if (onlinestatus == true)
                {
                    int index = emailaddress.IndexOf('@');
                    string s = emailaddress.Substring(index + 1);

                    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create("http://www." + s);
                    request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
                    request.Method = "HEAD";
                    try
                    {
                        if (request.GetResponse() != null)
                        {
                            status = true;
                        }
                    }
                    catch
                    {
                        status = false;
                    }
                }
            }
            return status;
        }
        public static bool Dato(string input)
        {
            bool status = false;

            DateTime nyDato;

            if (DateTime.TryParse(input, out nyDato))
            {
                status = true;
            }

            return status;
        }
    }
}
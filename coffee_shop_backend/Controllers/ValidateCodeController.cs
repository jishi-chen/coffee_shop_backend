using coffee_shop_backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ValidateCodeController : BaseController
    {
        private HttpContext? _context;
        private IHttpContextAccessor contextAccessor;

        public ValidateCodeController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
            contextAccessor = accessor;
        }

        [HttpGet]
        [Route("ValidateCode")]
        public ActionResult Index(string type, string model, string PartialViewName = "")
        {
            ValidateCodeHelper validateCodeHelper = new ValidateCodeHelper(contextAccessor);
            ValidateCodeType validType = ValidateCodeHelper.GetValidateType();

            if (validType == ValidateCodeType.Classic)
            {
                byte[] bytes;
                string mimeType;
                MemoryStream ms;
                switch (type)
                {
                    case "GenerateImage":
                        string code = ValidateCodeHelper.GenerateValidateCodeAndKeepInSession(ConstString.FrontEndSessionName + model);
                        bytes = ValidateCodeHelper.GenerateValidateCodeImage(code);
                        mimeType = ValidateCodeHelper.ImageMimeType;
                        Response.Clear();
                        ms = new MemoryStream(bytes);
                        return new FileStreamResult(ms, mimeType);

                    //case "GenerateAudio":
                    //    bytes = ValidateCodeHelper.GenerateValidateCodeAudio(ConstString.FrontEndSessionName + model);
                    //    mimeType = ValidateCodeHelper.AudioMimeType;
                    //    Response.Clear();
                    //    ms = new MemoryStream(bytes);
                    //    return new FileStreamResult(ms, mimeType);
                }

                //----- 驗證輸入值 -----
                // 因為是 ChildAction 導致 POST 無法另外抽出
                // 所以自行拿 HttpVerb 判斷
                if (this.Request.Method == "POST")
                {
                    var validResult = ValidateCodeHelper.IsSuccess(ConstString.FrontEndSessionName + model);
                    this.ViewBag.IsValidated = validResult;
                }
                //----- 驗證輸入值 -----

                return PartialView("_Classic");
            }
            return Content(string.Empty);
        }

        #region "Ajax / Img / Audio"
        private class ValidResult
        {
            // 成功或失敗
            public bool success { get; set; }
            public string Msg { get; set; } = string.Empty;
        }

        [HttpPost]
        [Route("ValidateCode/Validate/")]
        public JsonResult Validate(string token, string model)
        {
            ValidateCodeHelper.Validate(token, ConstString.FrontEndSessionName + model);
            var validResult = ValidateCodeHelper.IsSuccess(ConstString.FrontEndSessionName + model);

            if (!validResult)
                Response.StatusCode = 500;

            return Json(new ValidResult() { success = validResult });
        }

        [HttpPost]
        [Route("ValidateCode/ValidateReset/")]
        public JsonResult ValidateReset(string token, string model)
        {
            ValidateCodeHelper.RemoveResult(ConstString.FrontEndSessionName + model);

            return Json(new ValidResult() { success = true });
        }
        #endregion
    }

    public static class ConstString
    {
        /// <summary> 後台用的 Key </summary>
        public const string SessionName = "AdminValidResult";

        /// <summary> 前台用的 Key </summary>
        public const string FrontEndSessionName = "FrondEndValidResult";
    }

    /// <summary> 驗證碼種類 </summary>
    public enum ValidateCodeType
    {
        /// <summary> 傳統 </summary>
        Classic = 0,

        /// <summary> Google V2 </summary>
        GoogleV2 = 1,

        /// <summary> Google V3 </summary>
        GoogleV3 = 2
    }

    public class ValidateCodeHelper
    {
        #region "Field"
        private const string _fontName_Symbol = "Symbol";
        private static Random _random = new Random(Guid.NewGuid().GetHashCode());
        private static Uri _googleValidApiUri = new Uri("https://www.google.com/recaptcha/api/siteverify");

        // 要排除的字體
        private static readonly string[] _excludeFonts =
        {
            "Wingdings", "Marlett", "Specialty", "Outlook", "Extra", "Math", "Symbol", "Webdings", "Matura MT Script Capitals",
            "Curlz MT", "Kunstler Script", "Parchment", "Edwardian Script ITC","Gigi"
        };

        // 可用文字，略過數字0,1， 英文 i, L, O
        private static readonly char[] _charArray = { '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        // 聲音檔路徑
        private const string _folderPath = "~/Sysadm/Files/mp3/";

        public const string ValidateCodePostName = "ValidateCode";
        #endregion

        /// <summary> 驗證碼圖片的輸出 MimeType </summary>
        public static string ImageMimeType { get; set; } = "image/Gif";

        /// <summary> 驗證碼語音的輸出 MimeType </summary>
        public static string AudioMimeType { get; set; } = "audio/mpeg";

        private static IHttpContextAccessor _httpContext;

        public ValidateCodeHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }

        /// <summary> 取得設定的驗證碼種類 </summary>
        public static ValidateCodeType GetValidateType()
        {
            var validateName = "Classic";
            ValidateCodeType type = ValidateCodeType.Classic;

            if (string.Compare(validateName, "Classic", true) == 0)
                type = ValidateCodeType.Classic;
            else if (string.Compare(validateName, "GoogleV2", true) == 0)
                type = ValidateCodeType.GoogleV2;
            else if (string.Compare(validateName, "GoogleV3", true) == 0)
                type = ValidateCodeType.GoogleV3;

            return type;
        }

        /// <summary> 比對驗證碼是否正確 </summary>
        /// <param name="validateCode"> 輸入的驗證碼 </param>
        /// <param name="keepSessionKey"> Session 名稱，前後台應使用不同名稱 </param>
        /// <returns></returns>
        public static void Validate(string validateCode, string keepSessionKey)
        {
            var type = GetValidateType();
            string validResultSessionKey = keepSessionKey + "_result";

            if (type == ValidateCodeType.Classic)
            {
                // 驗證碼不存在
                if (_httpContext.HttpContext?.Session.GetString(keepSessionKey) == null)
                {
                    _httpContext.HttpContext?.Session.SetString(validResultSessionKey, "");
                    return;
                }

                string? checkCode = _httpContext.HttpContext?.Session?.GetString(keepSessionKey)?.ToString();
                string decryptedCode = DataProtectionUtility.Create().Decrypt(checkCode);

                // 驗證碼是否正確
                var result = String.Compare(decryptedCode, validateCode, true) == 0;

                _httpContext.HttpContext?.Session.SetString(validResultSessionKey, result.ToString());
                _httpContext.HttpContext?.Session.Remove(keepSessionKey);
            }
        }

        /// <summary> 比對驗證碼是否正確 </summary>
        /// <param name="keepSessionKey"> Session 名稱，前後台應使用不同名稱 </param>
        /// <returns></returns>
        public static bool IsSuccess(string keepSessionKey)
        {
            string validResultSessionKey = keepSessionKey + "_result";
            var isValidSuccess = _httpContext.HttpContext?.Session.GetString(validResultSessionKey) == "True" ? true : false;

            return isValidSuccess;
        }

        /// <summary> 真正驗證完後，要移除驗證結果s </summary>
        /// <param name="keepSessionKey"></param>
        public static void RemoveResult(string keepSessionKey)
        {
            string validResultSessionKey = keepSessionKey + "_result";

            if (_httpContext.HttpContext?.Session != null && _httpContext.HttpContext.Session.GetString(validResultSessionKey) != null)
                _httpContext.HttpContext?.Session.Remove(validResultSessionKey);
        }

        /// <summary> 產生驗證碼字串，並保存至指定名稱的 Session 中 </summary>
        /// <param name="keepSessionKey"> Session 名稱，前後台應使用不同名稱 </param>
        /// <returns></returns>
        public static string GenerateValidateCodeAndKeepInSession(string keepSessionKey)
        {
            StringBuilder checkCode = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                int charIndex = _random.Next(0, _charArray.Length);
                char code = _charArray[charIndex];
                checkCode.Append(code);
            }

            string txt = checkCode.ToString();
            string encryCode = DataProtectionUtility.Create().Encrypt(txt);

            //儲存在cookie
            _httpContext.HttpContext?.Response.Cookies.Append(keepSessionKey, encryCode);

            //儲存在session
            _httpContext.HttpContext?.Session.SetString(keepSessionKey, encryCode);

            return txt;
        }

        /// <summary> 產生驗證碼圖片 </summary>
        /// <param name="validateCode"></param>
        /// <returns></returns>
        public static byte[] GenerateValidateCodeImage(string validateCode)
        {
            if (string.IsNullOrWhiteSpace(validateCode))
                return new byte[0];

            int maxPopEnglishCount = 20;
            int maxPointCount = 60, maxLineCount = 60;
            int maxColorCode = 60;
            int maxFontSize = 38, minFontSize = 30;
            int imgWidth = 250, imgHeight = 80;
            Bitmap image = new Bitmap(imgWidth, imgHeight);
            Graphics g = Graphics.FromImage(image);

            int fontPositionX = 0;
            FontFamily fontFamily;

            try
            {
                g.Clear(Color.White);

                Matrix FontMatrix = new Matrix();

                // 加入雜線
                for (int i = 0; i < maxLineCount; i++)
                {
                    int x1 = _random.Next(image.Width);
                    int x2 = _random.Next(image.Width);
                    int y1 = _random.Next(image.Height);
                    int y2 = _random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                for (int i = 0; i < validateCode.Length; i++)
                {
                    int intColor1 = _random.Next(0, maxColorCode);
                    int intColor2 = _random.Next(0, maxColorCode);
                    int intColor3 = maxColorCode - intColor1;

                    // Random 字(每個字元)的大小
                    int fontSize = _random.Next(minFontSize, maxFontSize);

                    // Random 字型
                    do
                    {
                        fontFamily = FontFamily.Families[_random.Next(FontFamily.Families.Length)];
                    } while (!fontFamily.IsStyleAvailable(FontStyle.Regular) || InExcludeFonts(fontFamily));


                    // Random 字的位置 
                    int fontPositionY = (fontSize / 2);


                    // 計算文字橫軸偏移量
                    if (i != 0)
                    {
                        if (fontSize > ((maxFontSize + minFontSize) / 2))
                            fontPositionX += (int)(fontSize * 1.33);
                        else
                            fontPositionX += (int)(fontSize * 1.37);
                    }
                    else
                    {
                        if (fontSize > ((maxFontSize + minFontSize) / 2))
                            fontPositionX += (fontSize / 2);
                        else
                            fontPositionX += (int)(fontSize / 1.8);
                    }


                    // Random 字(每個字元)的角度
                    FontMatrix.Reset();
                    FontMatrix.RotateAt(_random.Next(-20, 20), new PointF((fontPositionX - 10), (fontSize / 2))); //  以中心點
                    g.Transform = FontMatrix;

                    // 把字元填入圖片
                    g.DrawString(
                        validateCode[i].ToString(),
                        new Font(fontFamily, fontSize),
                        new SolidBrush(Color.FromArgb(intColor1, intColor2, intColor3)),
                        new PointF(fontPositionX, fontPositionY)
                    ); //  以左上角
                }

                for (int i = 0; i < maxPopEnglishCount; i++)
                {
                    int popFontSize = _random.Next(10, 16);
                    int intPopColor = _random.Next(130, 255);

                    string popEng = ((char)_random.Next(48, 90)).ToString();
                    FontFamily popfontFamily = new FontFamily(_fontName_Symbol);
                    int posX = _random.Next(imgWidth);
                    int posY = _random.Next(imgHeight);
                    g.DrawString(
                        popEng,
                        new Font(popfontFamily, popFontSize),
                        new SolidBrush(Color.FromArgb(intPopColor, intPopColor, intPopColor)),
                        new PointF(posX, posY)
                    ); //  以左上角
                }

                // Random 線的干擾圖
                for (int i = 0; i < maxPointCount; i++)
                {
                    int x = _random.Next(image.Width);
                    int y = _random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(_random.Next()));
                }
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                MemoryStream ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

                return ms.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        private static bool InExcludeFonts(FontFamily MyFamily)
        {
            foreach (string strName in _excludeFonts)
            {
                if (MyFamily.Name.IndexOf(strName) > -1)
                    return true;
            }

            return false;
        }

    }
}

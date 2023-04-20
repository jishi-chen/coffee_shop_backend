using System.Text.RegularExpressions;

namespace coffee_shop_backend.Utility
{
    public class DataCheckUtility
    {
        private readonly IConfiguration _config;

        public DataCheckUtility(IConfiguration config)
        {
            this._config = config;
        }

        #region 靜態公用方法
        /// <summary>
        /// 檢核是否為Email格式
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns></returns>
        public bool IsEmail(string email)
        {
            email = email.ToLower(); //全部用小寫檢測
            string regexFormat = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
            return Regex.IsMatch(email, regexFormat);
        }
        /// <summary>
        /// 檢核是否為網址格式
        /// </summary>
        /// <param name="url">網址</param>
        /// <returns></returns>
        public bool IsUrl(string url)
        {
            url = url.ToLower();//全部用小寫檢測
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                return false;
            return true;
        }
        /// <summary>
        /// 檢查是否為數字或者字母
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns></returns>
        public bool IsAlphaNumeric(String InputString)
        {
            return (InputString != string.Empty && !Regex.IsMatch(InputString, "[^a-zA-Z0-9]"));
        }
        /// <summary>
        /// 檢查是否為數字
        /// </summary>
        /// <param name="InputString"></param>
        /// <returns></returns>
        public bool IsNumeric(String InputString)
        {
            return (InputString != string.Empty && !Regex.IsMatch(InputString, "[^0-9]"));
        }

        /// <summary>
        /// 檢核是否為手機格式
        /// </summary>
        public bool IsCellPhone(string cellphone)
        {
            string regexFormat = @"^[0-9]{10}$";
            return Regex.IsMatch(cellphone, regexFormat);
        }

        /// <summary> 驗身分證 </summary>
        /// <param name="id">身分證字號</param>
        /// <returns>身分字號是否符合格式</returns>
        public bool CheckIdCardNumber(string id)
        {
            var format = new Regex(@"^[A-Z]\d{9}$");
            if (!format.IsMatch(id)) return false;

            id = id.ToUpper();

            var a = new[] { 10, 11, 12, 13, 14, 15, 16, 17, 34, 18, 19, 20, 21, 22, 35, 23, 24, 25, 26, 27, 28, 29, 32, 30, 31, 33 };

            var b = new int[11];
            b[1] = a[(id[0]) - 65] % 10;
            var c = b[0] = a[(id[0]) - 65] / 10;

            for (var i = 1; i <= 9; i++)
            {
                b[i + 1] = id[i] - 48;
                c += b[i] * (10 - i);
            }

            var result = ((c % 10) + b[10]) % 10 == 0;
            return result;
        }

        /// <summary> 外來人口統一證號驗證 </summary>
        /// <param name="numberToCheck">要驗證的證號</param>
        /// <returns>驗證結果</returns>
        public bool CheckForeignIdNumber(string numberToCheck)
        {
            // 基礎檢查 「任意1個字母」+「A~D其中一個字母」+「8個數字」
            if (!Regex.IsMatch(numberToCheck, @"^[A-Za-z][A-Da-d]\d{8}$")) return false;
            numberToCheck = numberToCheck.ToUpper();

            // 縣市區域碼
            var cityCode = new[] { 10, 11, 12, 13, 14, 15, 16, 17, 34, 18, 19, 20, 21, 22, 35, 23, 24, 25, 26, 27, 28, 29, 32, 30, 31, 33 };
            // 計算時使用的容器，最後一個位置拿來放檢查碼，所以有11個位置(縣市區碼佔2個位置)
            var valueContainer = new int[11];
            valueContainer[0] = cityCode[numberToCheck[0] - 65] / 10; // 區域碼十位數
            valueContainer[1] = cityCode[numberToCheck[0] - 65] % 10; // 區域碼個位數
            valueContainer[2] = cityCode[numberToCheck[1] - 65] % 10; // 性別碼個位數
                                                                      // 證號執行特定數規則所產生的結果值的加總，這裡把初始值訂為區域碼的十位數數字(特定數為1，所以不用乘)
            var sumVal = valueContainer[0];

            // 迴圈執行特定數規則
            for (var i = 1; i <= 9; i++)
            {
                // 跳過性別碼，如果是一般身分證字號則不用跳過
                if (i > 1)
                    // 將當前證號於索引位置的數字放到容器的下一個索引的位置
                    valueContainer[i + 1] = numberToCheck[i] - 48;

                // 特定數為: 1987654321 ，因為首個數字1已經在sumVal初始值算過了，所以這裡從9開始
                sumVal += valueContainer[i] * (10 - i);
            }

            // 此為「檢查碼 = 10 - 總和值的個位數數字 ; 若個位數為0則取0為檢查碼」的反推
            if ((sumVal + valueContainer[10]) % 10 == 0)
                return true;
            return false;
        }
        #endregion
    }
}

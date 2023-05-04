$(function () {
    "use strict";

    var PAGE_CODE = ".wrapper";

    var App = {
        init: function () {
            // 檢查目前頁面是否為系統設定相關頁面
            if ($(PAGE_CODE).length == 0) return;
            // 設定檔載入
            this.configs = null;

            this.loadConfig();
            this.cacheElement();
            this.bindEvent();
            this.activePlugin();

        },
        // 載入遠端設定的設定檔
        // 設定檔是先建立為 js 檔，並指定存在 window 物件中
        // 如果有設定檔被載入的話，window.mashop.config 就會有值
        loadConfig: function () {
            this.config = (window.config) || null;
        },

        // 啟用載入套件
        activePlugin: function () {
            
        },

        // 取得各頁面所需元件
        cacheElement: function () {
            // 區塊
            this.$app = $(PAGE_CODE);
            //區域
            this.$appSelectCity = this.$app.find("#Address_City");
            this.$appSelectArea = this.$app.find("#Address_Region");
            this.$appBtnSubmit = this.$app.find(".btn-submit");
        },

        // 事件監聽
        bindEvent: function () {
            this.$appSelectCity.on("change", $.proxy(this._selectCity, this));
            this.$appSelectArea.on("change", $.proxy(this._selectArea, this));
            this.$app.on("click", "#togglePassword", this._togglePassword);
            this.$appBtnSubmit.on("click", $.proxy(this._btnSubmit, this));
        },
        _selectCity: function () {
            if (this.$appSelectCity.val() == "") {
                this.$appSelectArea.empty();
                this.$appSelectArea.append($('<option></option>').val('').text('--請選擇地區--'));
            } else {
                var selectedValue = this.$appSelectCity.find('option:selected').val();
                $.ajax({
                    url: '/Application/Address',
                    data: {
                        cityName: selectedValue,
                        areaName: ""
                    },
                    type: 'post',
                    cache: false,
                    async: false,
                    dataType: 'json',
                    success: function (data) {
                        if (data.length > 0) {
                            var area = $("#Address_Region");
                            $('#Address_PostalCode').val("");
                            area.empty();
                            area.append($('<option></option>').val('').text('--請選擇地區--'));
                            $.each(data, function (i, item) {
                                var newOption = `<option value=${item.value}>${item.value}</option>`;
                                area.append(newOption);
                            });
                        }
                    }
                });
            };
        },
        _selectArea: function () {
            var selectedValue = this.$appSelectArea.find('option:selected').val();
            $.ajax({
                url: '/Application/Address',
                data: {
                    cityName: this.$appSelectCity.find('option:selected').val(),
                    areaName: selectedValue
                },
                type: 'post',
                cache: false,
                async: false,
                dataType: 'json',
                success: function (data) {
                    $('#Address_PostalCode').val("");
                    if (data.length > 0 && typeof data === 'string') {
                        $('#Address_PostalCode').val(data);
                    }
                }
            });
        },
        //顯示密碼
        _togglePassword: function () {
            let password = $('#Password');
            let type = password.attr('type') == 'password' ? 'text' : 'password';
            password.attr('type', type);
            this.classList.toggle('bi-eye');
        },
        //驗證輸入項目
        _checkValidInput: function () {
            let name = $('[id^=Name]');
            let startDate = $('[id^=StartDate]');
            let endDate = $('[id^=EndDate]');
            let phone = $('[id^=Phone]');
            let identityString = $('[id^=IdentityString]');
            let email = $('[id^=Email]');
            let account = $('[id^=Account]');
            let password = $('[id^=Password]');
            //必填
            if (!this._isEmptyInput(name)) {
                this._valid.valid = false;
                this._valid.errorMsg += '姓名為必填<br>';
            }
            if (!this._isEmptyInput(account)) {
                this._valid.valid = false;
                this._valid.errorMsg += '帳號為必填<br>';
            }
            if (!this._isEmptyInput(password)) {
                this._valid.valid = false;
                this._valid.errorMsg += '密碼為必填<br>';
            }
            //日期時間
            if (!this._isValidDateTime(startDate)) {
                this._valid.valid = false;
                this._valid.errorMsg += '開始日期時間格式錯誤<br>';
            }
            if (!this._isValidDateTime(endDate)) {
                this._valid.valid = false;
                this._valid.errorMsg += '結束日期時間格式錯誤<br>';
            }
            //手機號碼
            if (!this._isValidPhoneNumber(phone.val())) {
                this._valid.valid = false;
                this._valid.errorMsg += '電話號碼格式錯誤<br>';
            }
            //身分證字號
            if (!this._isValidIDCardNumber(identityString.val())) {
                this._valid.valid = false;
                this._valid.errorMsg += '身分證字號格式錯誤<br>';
            }
            //Email
            if (!this._isValidEmail(email.val())) {
                this._valid.valid = false;
                this._valid.errorMsg += 'Email格式錯誤<br>';
            }
        },
        //驗證日期時間格式
        _isValidDateTime: function (e) {
            var value = e.value;
            if (typeof myVar !== "undefined") {
                let datetime = value.split(' ');
                var regDate = /^(\d{4})-(\d{2})-(\d{2})$/;
                var regTime = /([0-1][0-9]|2[0-3]):[0-5][0-9]/;
                if (!regDate.test(datetime[0]) || RegExp.$2 > 12 || RegExp.$3 > 31 || !regTime.test(datetime[1])) {
                    return false;
                }
                return true;
            }
            return false;
        },
        //驗證電話號碼
        _isValidPhoneNumber : function (e) {
            // 驗證是否為 09 開頭的手機號碼，長度為 10 碼
            const regex = /^09\d{8}$/;
            return regex.test(e);
        },
        //驗證身分證字號
        _isValidIDCardNumber: function (e) {
            // 驗證格式是否正確
            const regex = /^[A-Z][1-2]\d{8}$/;
            if (!regex.test(e)) {
                return false;
            }

            // 計算檢查碼
            const idArray = e.split('');
            const letters = 'ABCDEFGHJKLMNPQRSTUVXYWZIO';
            const letterNumber = (letters.indexOf(idArray[0]) + 10).toString().split('');
            const checkSum =
                Number(letterNumber[0]) * 1 +
                Number(letterNumber[1]) * 9 +
                Number(idArray[1]) * 8 +
                Number(idArray[2]) * 7 +
                Number(idArray[3]) * 6 +
                Number(idArray[4]) * 5 +
                Number(idArray[5]) * 4 +
                Number(idArray[6]) * 3 +
                Number(idArray[7]) * 2 +
                Number(idArray[8]) * 1 +
                Number(idArray[9]) * 1

            console.log(checkSum);
            // 驗證檢查碼是否正確
            return checkSum % 10 === 0;
        },
        //驗證Email
        _isValidEmail: function (e) {
            const emailRule =
                /^\w+((-\w+)|(\.\w+)|(\+\w+))*@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z]+$/;
            return emailRule.test(e);
        },
        //驗證必填
        _isEmptyInput: function (e) {
            return e.value != "";
        },
        //送出表單
        _btnSubmit: function (e) {
            //this._checkValidInput();

            if (this._valid.valid === false) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    //text: 'Something went wrong!',
                    footer: '<a href="">Why do I have this issue?</a>',
                    html: 'Something went wrong! <br>' + this._valid.errorMsg,
                });
                this._valid.valid = true;
                this._valid.errorMsg = '';
            }
            else {
                $("form").submit();
            }
        },
        _valid: {
            valid: true,
            errorMsg: ""
        },
    };

    App.init();
});
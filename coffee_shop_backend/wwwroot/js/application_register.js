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
        //驗證日期時間格式
        _checkValidDateTime: function () {
            var regDate = /^(\d{4})-(\d{2})-(\d{2})$/;
            var regTime = /([0-1][0-9]|2[0-3]):[0-5][0-9]/;
            var value = "";
            let datetime = "";
            let startDate = $('[id^=StartDate]');
            let endDate = $('[id^=EndDate]');
            for (var i = 0; i < startDate.length; i++) {
                value = startDate[i].value;
                datetime = value.split(' ');
                if (!regDate.test(datetime[0]) || RegExp.$2 > 12 || RegExp.$3 > 31 || !regTime.test(datetime[1])) {
                    startDate[i].classList.add("is-invalid");
                    startDate[i].classList.remove("is-valid");
                }
                else {
                    startDate[i].classList.add("is-valid");
                    startDate[i].classList.remove("is-invalid");
                }
            }
            for (var i = 0; i < endDate.length; i++) {
                value = endDate[i].value;
                datetime = value.split(' ');
                if (!regDate.test(datetime[0]) || RegExp.$2 > 12 || RegExp.$3 > 31 || !regTime.test(datetime[1])) {
                    endDate[i].classList.add("is-invalid");
                    endDate[i].classList.remove("is-valid");
                }
                else {
                    endDate[i].classList.add("is-valid");
                    endDate[i].classList.remove("is-invalid");
                }
            }
        },
        //驗證日期時間格式
        _isValidDateTime: function (e) {
            var value = e.value;
            let datetime = value.split(' ');
            var regDate = /^(\d{4})-(\d{2})-(\d{2})$/;
            var regTime = /([0-1][0-9]|2[0-3]):[0-5][0-9]/;
            if (!regDate.test(datetime[0]) || RegExp.$2 > 12 || RegExp.$3 > 31 || !regTime.test(datetime[1])) {
                return false;
            }
            return true;
        },
        //送出表單
        _btnSubmit: function (e) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!',
                footer: '<a href="">Why do I have this issue?</a>'
            });
            //$("form").submit();
        },
        _valid: {
            valid: true,
            errorMsg: ""
        },
    };

    App.init();
});
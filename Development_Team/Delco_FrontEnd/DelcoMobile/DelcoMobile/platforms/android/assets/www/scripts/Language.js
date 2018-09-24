var language = {
    ChangeLanguage: function () {
        var lang = localStorage.getItem('lang');
        var arCssId = 'arCssFile';
        var enCssId = 'enCssFile';
        var mainCssId = 'mainCssFile';
        var frameworkCssId = 'framework7Element';
        var head = document.getElementsByTagName('head')[0];
        var enCssElement = document.getElementById(enCssId);
        var arCssElement = document.getElementById(arCssId);
        var mainCssElement = document.getElementById(mainCssId);
        var framework7Element = document.getElementById(frameworkCssId);

        if (lang == 'AR' || lang == null || typeof lang == 'undefined') {
            lang = 'AR';

            $('.open-panel').attr('data-panel', 'right');
            $('#divSidePanel').attr('class', 'panel panel-right panel-cover');

            if (enCssElement) {
                enCssElement.parentNode.removeChild(enCssElement);
            }

            if (arCssElement) {
                arCssElement.parentNode.removeChild(arCssElement);
            }

            if (framework7Element) {
                framework7Element.parentNode.removeChild(framework7Element);
            }

            if (mainCssElement) {
                mainCssElement.parentNode.removeChild(mainCssElement);
            }


            var link = document.createElement('link');
            link.id = frameworkCssId;
            link.rel = 'stylesheet';
            link.type = 'text/css';
            link.href = 'css/framework7.ios.rtl.min.css';
            head.appendChild(link);


            var link = document.createElement('link');
            link.id = arCssId;
            link.rel = 'stylesheet';
            link.type = 'text/css';
            link.href = 'css/main_ar.css';
            head.appendChild(link);


            var link = document.createElement('link');
            link.id = mainCssId;
            link.rel = 'stylesheet';
            link.type = 'text/css';
            link.href = 'css/main.css';
            head.appendChild(link);


        }
        else {
            lang = 'EN';

            $('.open-panel').attr('data-panel', 'left');
            $('#divSidePanel').attr('class', 'panel panel-left panel-cover');

            if (arCssElement) {
                arCssElement.parentNode.removeChild(arCssElement);
            }

            if (framework7Element) {
                framework7Element.parentNode.removeChild(framework7Element);
            }

            if (mainCssElement) {
                mainCssElement.parentNode.removeChild(mainCssElement);
            }

            if (enCssElement) {
                enCssElement.parentNode.removeChild(enCssElement);
            }

            var link = document.createElement('link');
            link.id = mainCssId;
            link.rel = 'stylesheet';
            link.type = 'text/css';
            link.href = 'css/main.css';
            head.appendChild(link);

            var link = document.createElement('link');
            link.id = enCssId;
            link.rel = 'stylesheet';
            link.type = 'text/css';
            link.href = 'css/main_en.css';
            head.appendChild(link);
        }

        $('.smart-select').each(function (index) {
            if (lang == 'AR' || lang == null || typeof lang == 'undefined') {
                $(this).attr('data-picker-close-text', 'تم');
            }
            else {
                $(this).attr('data-picker-close-text', 'Close');
            }
        });

        $('.close-picker').each(function (index) {
            if (lang == 'AR' || lang == null || typeof lang == 'undefined') {
                $(this).text('تم');
            }
            else {
                $(this).text('Close');
            }
        });

        $('.ion-android-arrow-forward').each(function (index) {
            if (lang == 'AR' || lang == null || typeof lang == 'undefined') {
                $(this).removeClass('ion-android-arrow-back');
                $(this).addClass('ion-android-arrow-forward');
            }
            else {
                $(this).removeClass('ion-android-arrow-forward');
                $(this).addClass('ion-android-arrow-back');
            }
        });

        $('.ion-android-arrow-back').each(function (index) {
            if (lang == 'AR' || lang == null || typeof lang == 'undefined') {
                $(this).removeClass('ion-android-arrow-back');
                $(this).addClass('ion-android-arrow-forward');
            }
            else {
                $(this).removeClass('ion-android-arrow-forward');
                $(this).addClass('ion-android-arrow-back');
            }
        });


        $('.lang').each(function (index) {
            var propType = this.nodeName;
            var element = $(this);
            var options = {};
            if (propType === 'INPUT') {
                var txt = $(this).attr('placeholder').trim();
                if (typeof txt != 'undefined' && txt != null && txt != '' && txt != ' ') {
                    var options = {
                        Language: lang,
                        value: txt
                    };
                    changeControlText(options, function (translatedValue) {
                        $(element).attr('placeholder', translatedValue);
                    });
                }
            }
            else {
                var txt = $(this).text().trim();
                if (typeof txt != 'undefined' && txt != null && txt != '' && txt != ' ') {
                    var options = {
                        Language: lang,
                        value: txt
                    };
                    changeControlText(options, function (translatedValue) {
                        propType = this.nodeName;
                        $(element).html(translatedValue);
                    });
                }
            }
        });
    },
    openFrameworkModal: function (title, text, type, callBack) {
        changeModalText(title, text, function (titleTranslated, textTranslated) {
            var fw7 = myApp.fw7;
            var app = myApp.fw7.app;

            if (type.toLowerCase() == 'alert') {
                //app.alert(textTranslated, titleTranslated, function () {
                //    callBack(true);
                //});
                window.plugins.toast.showWithOptions({
                    message: textTranslated,
                    duration: "short",
                    position: "bottom",
                    styling: {
                        opacity: 0.8,
                        backgroundColor: '#293955',
                        textColor: '#FFFFFF',
                        textSize: 15,
                        cornerRadius: 20,
                        horizontalPadding: 20,
                        verticalPadding: 16
                    }
                },
                function () {
                    callBack(true);
                },
                function () {
                    callBack(false);
                });
            }
            else {
                app.confirm(textTranslated, titleTranslated, function () {
                    callBack(true);
                });
            }
        });
    },
     alert : function (message) {
         window.plugins.toast.showLongBottom(message, function (a) {
             console.log('Toast success: ' + a);
         }, function (b) {
             console.log('Toast error: ' + b);
         });
    },
     ChangeTheme: function () {
         var fwMaterialCssId = 'fwMaterialCssFile';
         var fwMaterialColorsCssId = 'fwMaterialColorsCssFile';
         var fwRTLCssId = 'fwRTLCssFile';
         var fwIOSCssId = 'fwIOSCssFile';
         var fwIOSColorsCssId = 'fwIOSColorsCssFile';
         var fwIOSRTLCssId = 'fwIOSRTLCssFile';
         var myAppCssId = 'myAppCssFile';
         var ionicFontsCssId = 'ionicFontsCssFile';
         var rateYoCssId = 'rateYoCssFile';
         var fwMaterialCssIdElement = document.getElementById(fwMaterialCssId);
         var fwMaterialColorsCssIdElement = document.getElementById(fwMaterialColorsCssId);
         var fwRTLCssIdElement = document.getElementById(fwRTLCssId);

         var fwIOSCssIdElement = document.getElementById(fwIOSCssId);
         var fwIOSColorsCssIdElement = document.getElementById(fwIOSColorsCssId);
         var fwIOSRTLCssIdElement = document.getElementById(fwIOSRTLCssId);

         var myAppCssIdElement = document.getElementById(myAppCssId);
         var ionicFontsCssIdElement = document.getElementById(ionicFontsCssId);
         var rateYoCssIdElement = document.getElementById(rateYoCssId);

         if (fwMaterialCssIdElement) {
             fwMaterialCssIdElement.parentNode.removeChild(fwMaterialCssIdElement);
         }
         if (fwMaterialColorsCssIdElement) {
             fwMaterialColorsCssIdElement.parentNode.removeChild(fwMaterialColorsCssIdElement);
         }
         if (fwRTLCssIdElement) {
             fwRTLCssIdElement.parentNode.removeChild(fwRTLCssIdElement);
         }

         if (fwIOSCssIdElement) {
             fwIOSCssIdElement.parentNode.removeChild(fwIOSCssIdElement);
         }
         if (fwIOSColorsCssIdElement) {
             fwIOSColorsCssIdElement.parentNode.removeChild(fwIOSColorsCssIdElement);
         }
         if (fwIOSRTLCssIdElement) {
             fwIOSRTLCssIdElement.parentNode.removeChild(fwIOSRTLCssIdElement);
         }

         if (myAppCssIdElement) {
             myAppCssIdElement.parentNode.removeChild(myAppCssIdElement);
         }
         if (ionicFontsCssIdElement) {
             ionicFontsCssIdElement.parentNode.removeChild(ionicFontsCssIdElement);
         }
         if (rateYoCssIdElement) {
             rateYoCssIdElement.parentNode.removeChild(rateYoCssIdElement);
         }

        if (Framework7.prototype.device.android) {
            Dom7('head').append(
                '<link rel="stylesheet" href="css/framework7.material.min.css">' +
                '<link rel="stylesheet" href="css/framework7.material.colors.min.css">' +
                '<link rel="stylesheet" href="css/framework7.material.rtl.min.css">' +
                '<link rel="stylesheet" href="css/my-app.css">' +
                '<link rel="stylesheet" href="css/ionicons.min.css">' +
                '<link rel="stylesheet" href="css/jquery.rateyo.css">'
            );
        }
        else {
            Dom7('head').append(
                '<link rel="stylesheet" href="css/framework7.ios.min.css">' +
                '<link rel="stylesheet" href="css/framework7.ios.colors.min.css">' +
                '<link rel="stylesheet" href="css/framework7.ios.rtl.min.css">' +
                '<link rel="stylesheet" href="css/my-app.css">' +
                '<link rel="stylesheet" href="css/ionicons.min.css">' +
                '<link rel="stylesheet" href="css/jquery.rateyo.css">'
            );
        }
    }
};

function changeModalText(title, text, callBack) {
    var titleTranslated = 'none';
    var textTranslated = 'none';
    //var lang = localStorage.getItem('lang');
    //if (lang == 'AR' || lang == null || typeof lang == 'undefined') {
    //    lang = 'AR';
    //}
    //else {
    //    lang = 'EN';
    //}
    //var options = {
    //    Language: lang,
    //    value: title
    //};
    //changeControlText(options, function (translatedValue) {
    //    title = translatedValue;
    //});

    //var options = {
    //    Language: lang,
    //    value: text
    //};
    //changeControlText(options, function (translatedValue) {
    //    text = translatedValue;
    //});

    callBack(title, text);
}

function changeControlText(options, callBack) {
    var language = options.Language;
    var value = options.value;

    GetAllControls(language, value, function (result) {
        callBack(result);
    });
}

function GetAllControls(lang, value, callBack) {
    var langDict = [];
    langDict.push(
            { valueAR: '', valueEN: 'none' },
            { valueAR: ' ', valueEN: 'none' },
            { valueAR: 'إسم الدخول', valueEN: 'User Name' },
            { valueAR: 'كلمة المرور', valueEN: 'Password' },
            { valueAR: 'نسيت كلمة السر ؟', valueEN: 'Forget Password' },
            { valueAR: 'تسجيل الدخول', valueEN: 'Sign in' },
            { valueAR: 'لست مسجلاً لدينا ؟', valueEN: 'Not Registered Yet ?' },
            { valueAR: 'حساب جديد', valueEN: 'New Account' },
            { valueAR: 'تخطى', valueEN: 'Pass' },
            { valueAR: 'هل تريد الخروج من التطبيق ؟', valueEN: 'Do you want to exit application ?' },
            { valueAR: 'خطأ', valueEN: 'Error' },
            { valueAR: 'نجاح', valueEN: 'Success' },
            { valueAR: 'تأكيد', valueEN: 'Confirm' },
            { valueAR: 'إخطار', valueEN: 'Notify' },
            { valueAR: 'تم', valueEN: 'OK' },
            { valueAR: 'الخدمة غير متوفرة الآن بسبب بطء الإتصال بالإنترنت,من فضلك أعد المحاولة مرة أخري .', valueEN: 'Service Is Not Available Due To Internet Slow Connection,Please Try Again' },
            { valueAR: 'خطأ في الخدمة .', valueEN: 'Error in service please check administrator' },
            { valueAR: 'يوجد لديك إشعار جديد , هل تريد تفقد الإشعار ؟', valueEN: 'You have new notification , Check it ?' },
            { valueAR: 'يجب دفع قيمة 5% بعد اتمام 4 رحلات سيتم خصم القيمه من حسابك تلقائيا بعد كل 4 رحلات واذا لم يوجد حساب يتم ايقاف الخدمه حتى يتم السداد', valueEN: 'You must pay 5% from your account directly after each completed 4 trips and your account will be suspended if there is insufficient balance' },
            { valueAR: 'تنبيه', valueEN: 'Warning' },
            { valueAR: 'الصفحة الشخصية', valueEN: 'My Profile' },
            { valueAR: 'أموالي', valueEN: 'My Money' },
            { valueAR: 'محادثاتي', valueEN: 'My Messages' },
            { valueAR: 'عرض كل نقلاتى', valueEN: 'View All My Luggages' },
            { valueAR: 'عرض كل رحلاتى', valueEN: 'View All My Trips' },
            { valueAR: 'ما تم ربحة', valueEN: 'My Profits' },
            { valueAR: 'ما تم صرفة', valueEN: 'My Spendings' },
            { valueAR: 'الإجمالى', valueEN: 'Total' },
            { valueAR: 'إجمالي أسعار الرحلات', valueEN: 'Total Trips Prices' },
            { valueAR: 'إجمالي أسعار النقلات', valueEN: 'Total Luggages Prices' },
            { valueAR: 'لا توجد رحلات مسجلة لك بعد .', valueEN: 'You Have No Trips Yet' },
            { valueAR: 'لا توجد نقلات مسجلة لك بعد .', valueEN: 'You Have No Luggages Yet' },
            { valueAR: 'لا يوجد محادثات سابقة لك .', valueEN: 'You Have No Conversations Before' },
            { valueAR: 'النسخة الإنجليزية', valueEN: 'Arabic Version' },
            { valueAR: 'كل الرحلات والنقلات', valueEN: 'All Trips And Luggages' },
            { valueAR: 'إضافة رحلة', valueEN: 'Add Trip' },
            { valueAR: 'رحلات اليوم', valueEN: 'Today Trips' },
            { valueAR: 'عرض جميع الرحلات', valueEN: 'View All Trips' },
            { valueAR: 'إضافة نقلة', valueEN: 'Add Luggage' },
            { valueAR: 'نقلات اليوم', valueEN: 'Today Luggages' },
            { valueAR: 'عرض جميع النقلات', valueEN: 'View All Luggages' },
            { valueAR: 'الصفحة الشخصية', valueEN: 'My Profile' },
            { valueAR: 'تغيير كلمة السر', valueEN: 'Change Password' },
            { valueAR: 'الدعم الفني', valueEN: 'Technical Support' },
            { valueAR: 'تسجيل الخروج', valueEN: 'Logout' },
            { valueAR: 'تفاصيل ناقل', valueEN: 'Trip Owner Settings' },
            { valueAR: 'تفاصيل طالب نقلة', valueEN: 'Luggage Owner Details' },
            { valueAR: 'الإعدادات', valueEN: 'Settings' },
            { valueAR: 'أدخل بريدك الإلكتروني', valueEN: 'Enter Your Email' },
            { valueAR: 'يجب إدخال البريد الإلكتروني', valueEN: 'You Must Enter Your Email' },
            { valueAR: 'البريد الإلكتروني غير صحيح', valueEN: 'Email Is Not Entered Correctly' },
            { valueAR: 'إرسال', valueEN: 'Send' },
            { valueAR: 'عودة', valueEN: 'Back' },
            { valueAR: 'أدخل كلمة السر القديمة', valueEN: 'Enter Old Password' },
            { valueAR: 'يجب إدخال كلمة السر القديمة', valueEN: 'You Must Enter Old Password' },
            { valueAR: 'أدخل كلمة السر الجديدة', valueEN: 'Enter New Password' },
            { valueAR: 'يجب إدخال كلمة السر الجديدة', valueEN: 'You Must Enter New Password' },
            { valueAR: 'كلمة المرور يجب ألا تقل عن ستة خانات', valueEN: 'Old Password Must Exceed Six Characters' },
            { valueAR: 'كلمة المرور الجديدة يجب ألا تقل عن ستة خانات', valueEN: 'New Password Must Exceed Six Characters' },
            { valueAR: 'أعد إدخال كلمة السر الجديدة', valueEN: 'Re-enter New Password' },
            { valueAR: 'يجب تأكيد كلمة السر الجديدة', valueEN: 'You Must Confirm New Password' },
            { valueAR: 'كلمتا المرور غير متطابقتين', valueEN: 'Old And New Passwords Not Matching' },
            { valueAR: 'تغيير', valueEN: 'Change' },
            { valueAR: 'أدخل الكود', valueEN: 'Enter Code' },
            { valueAR: 'يجب إدخال الكود', valueEN: 'You Must Enter Code' },
            { valueAR: 'تغيير كلمة السر', valueEN: 'Change Password' },
            { valueAR: 'إسم المستخدم', valueEN: 'User Name' },
            { valueAR: 'يجب إدخال اسم المستخدم', valueEN: 'You Must Enter User Name' },
            { valueAR: 'أدخل اسم المستخدم باللغة الانجليزية ومن غير مسافات', valueEN: 'Enter Username In English And Without Spaces' },
            { valueAR: 'الجوال', valueEN: 'Mobile' },
            { valueAR: 'يجب إدخال رقم الجوال', valueEN: 'You Must Enter Mobile' },
            { valueAR: 'أدخل رقم الجوال بطريقة صحيحة', valueEN: 'Mobile Is Not Entered Correctly' },
            { valueAR: 'رقم الجوال عشرة خانات فقط', valueEN: 'Mobile Must Be 10 Numbers Only' },
            { valueAR: 'الاسم كامل', valueEN: 'Full Name' },
            { valueAR: 'يجب إدخال الإسم الكامل', valueEN: 'You Must Enter Full Name' },
            { valueAR: 'تم', valueEN: 'Done' },
            { valueAR: 'المدينة', valueEN: 'City' },
            { valueAR: 'يجب إدخال المدينة', valueEN: 'You Must Select City' },
            { valueAR: 'الدولة', valueEN: 'Country' },
            { valueAR: 'يجب إدخال الدولة', valueEN: 'You Must Select Country' },
            { valueAR: 'كلمة المرور', valueEN: 'Password' },
            { valueAR: 'يجب إدخال كلمة المرور', valueEN: 'You Must Enter Password' },
            { valueAR: 'تأكيد كلمة المرور', valueEN: 'Confirm Password' },
            { valueAR: 'يجب تأكيد كلمة المرور', valueEN: 'You Must Confirm Password' },
            { valueAR: 'كود التفعيل', valueEN: 'Activation Code' },
            { valueAR: 'أعد إرسال الكود', valueEN: 'Re-send Activation Code' },
            { valueAR: 'عدد النقلات', valueEN: 'Luggages' },
            { valueAR: 'عدد الرحلات', valueEN: 'Trips' },
            { valueAR: 'عدد المستخدمين', valueEN: 'Users' },
            { valueAR: 'عدد المدن', valueEN: 'Cities' },
            { valueAR: 'دخول للتطبيق', valueEN: 'Enter Application' },
            { valueAR: 'كل الرحلات والنقلات', valueEN: 'All Trips and Luggages' },
            { valueAR: 'لا يوجد أي رحلات أو نقلات .', valueEN: 'There Is No Trips Or Luggages' },
            { valueAR: 'جميع الرحلات', valueEN: 'All Trips' },
            { valueAR: 'رحلات اليوم', valueEN: 'Trips For Today' },
            { valueAR: 'لا يوجد رحلات .', valueEN: 'No Trips Found' },
            { valueAR: 'جميع النقلات', valueEN: 'All Luggages' },
            { valueAR: 'نقلات اليوم', valueEN: 'Tranports For Today' },
            { valueAR: 'لا يوجد نقلات .', valueEN: 'No Luggages Found' },
            { valueAR: 'إضافة الرحلة', valueEN: 'Add Trip' },
            { valueAR: 'ليس لديك رصيد كافي لإضافة رحلة , من فضلك تواصل مع الأدمن .', valueEN: 'You Have No Enough Balance To Add Trip , Please Contact Administrator' },
            { valueAR: 'مكان المغادرة', valueEN: 'Departure Place' },
            { valueAR: 'يجب إدخال مكان المغادرة', valueEN: 'You Must Enter Departure Place' },
            { valueAR: 'مكان الوصول', valueEN: 'Arrival Place' },
            { valueAR: 'يجب إدخال مكان الوصول', valueEN: 'You Must Enter Arrival Place' },
            { valueAR: 'تاريخ المغادرة', valueEN: 'Departure Date' },
            { valueAR: 'يجب إدخال تاريخ المغادرة', valueEN: 'You Must Enter Departure Date' },
            { valueAR: 'تاريخ الوصول', valueEN: 'Arrival Date' },
            { valueAR: 'يجب إدخال تاريخ الوصول', valueEN: 'You Must Enter Arrival Date' },
            { valueAR: 'وسيلة النقل', valueEN: 'Transportation Vehicle' },
            { valueAR: 'يجب إدخال وسيلة النقل', valueEN: 'You Must Enter Transportation Vehicle' },
            { valueAR: 'إضافة نقلة', valueEN: 'Add Luggage' },
            { valueAR: 'مكان المغادرة', valueEN: 'Departure Place' },
            { valueAR: 'يجب إدخال مكان المغادرة', valueEN: 'You Must Enter Departure Place' },
            { valueAR: 'مكان الوصول', valueEN: 'Arrival Place' },
            { valueAR: 'يجب إدخال مكان الوصول', valueEN: 'You Must Enter Arrival Place' },
            { valueAR: 'تاريخ المغادرة', valueEN: 'Departure Date' },
            { valueAR: 'يجب إدخال تاريخ المغادرة', valueEN: 'You Must Enter Departure Date' },
            { valueAR: 'يجب تسليم النقلة قبل يوم', valueEN: 'Date To Deliver Luggage On' },
            { valueAR: 'يجب إدخال تاريخ تسليم النقلة', valueEN: 'You Must Enter Date To Deliver Luggage On' },
            { valueAR: 'مواصفات النقلة', valueEN: 'Description' },
            { valueAR: 'تفاصيل رحلة', valueEN: 'Trip Details' },
            { valueAR: 'من', valueEN: 'From' },
            { valueAR: 'إلى', valueEN: 'To' },
            { valueAR: 'اليوم', valueEN: 'Day' },
            { valueAR: 'بدء الحساب النهائي', valueEN: 'Calculate Account Balance' },
            { valueAR: 'بدء المحادثة', valueEN: 'Start Conversation' },
            { valueAR: 'تفاصيل أخرى', valueEN: 'Other Details' },
            { valueAR: 'لا يوجد تفاصيل .', valueEN: 'No Details Found' },
            { valueAR: 'تفاصيل نقلة', valueEN: 'luggage Details' },
            { valueAR: 'المحادثات', valueEN: 'Conversations' },
            { valueAR: 'الرحلات المناسبة للنقلة / الرحلة المتفق عليها', valueEN: 'Suitable Trips / Trips Agreed' },
            { valueAR: 'لا يوجد رحلات مناسبة او رحلات متفق عليها .', valueEN: 'No Suitable Trips Or Trips Agreed Found' },
            { valueAR: 'تمت الموافقة علي السعر المقترح', valueEN: 'Suggested Price Accepted' },
            { valueAR: 'تم رفض السعر المقترح', valueEN: 'Suggested Price Rejected' },
            { valueAR: 'تم عرض سعر جديد', valueEN: 'New Price Suggested' },
            { valueAR: 'المحادثة', valueEN: 'Conversation' },
            { valueAR: 'سعر النقلة', valueEN: 'Luggage Price' },
            { valueAR: 'تم الإتفاق', valueEN: 'Agree' },
            { valueAR: 'إلغاء', valueEN: 'Cancel' },
            { valueAR: 'اكتب رسالتك', valueEN: 'Write Message' },
            { valueAR: 'إرسل', valueEN: 'Send' },
            { valueAR: 'البحث', valueEN: 'Search' },
            { valueAR: 'النوع', valueEN: 'Type' },
            { valueAR: 'تاريخ الوصول', valueEN: 'Arrive Date' },
            { valueAR: 'بحث', valueEN: 'Search' },
            { valueAR: 'المحادثات الخاصة بالنقلة', valueEN: 'Luggage Conversations' },
            { valueAR: 'لا يوجد محادثات .', valueEN: 'No Conversations Found' },
            { valueAR: 'رحلاتي', valueEN: 'My Trips' },
            { valueAR: 'نقلاتي', valueEN: 'My Luggages' },
            { valueAR: 'الدعم الفني', valueEN: 'Support' },
            { valueAR: 'إختر نقلة', valueEN: 'Choose Luggage' },
            { valueAR: 'إختر رحلة', valueEN: 'Choose Trip' },
            { valueAR: 'الإشعارات', valueEN: 'Notifications' },
            { valueAR: 'تعديل رحلة', valueEN: 'Update Trip' },
            { valueAR: 'تعديل نقلة', valueEN: 'Update Luggage' },
            { valueAR: 'يجب إدخال الوقت المقرر للمغادرة', valueEN: 'You Must Choose Estimated Departure Time' },
            { valueAR: 'يجب إدخال الوقت المقرر للوصول', valueEN: 'You Must Choose Estimated Arrival Time' },
            { valueAR: 'وسيلة النقل', valueEN: 'Transportation Vehicle' },
            { valueAR: 'يجب إختيار وسيلة النقل', valueEN: 'You Must Choose Transportation Vehicle' },
            { valueAR: 'الوقت المقرر للمغادرة', valueEN: 'Estimated Departure Time' },
            { valueAR: 'الوقت المقرر للوصول', valueEN: 'Estimated Arrival Time' },
            { valueAR: 'تفاصيل الرحلة', valueEN: 'Description' },
            { valueAR: 'تعديل', valueEN: 'Update' },
            { valueAR: 'تفاصيل الرحلة', valueEN: 'Description' },
            { valueAR: 'من فضلك إختر تاريخ أكبر من تاريخ المغادرة .', valueEN: 'Please Choose Date Later Than Departure Date' },
            { valueAR: 'من فضلك إختر تاريخ أقل من تاريخ الوصول .', valueEN: 'Please Choose Date Earlier Than Arrival Date' },
            { valueAR: 'لا يمكن إعادة تنشيط رمز التحقق الخاص بك لإنتهاء صلاحيته .', valueEN: 'You Security Token Is Expired And Cannot Be Reactivated' },
            { valueAR: 'خطأ في البريد الإلكتروني أو كلمة المرور .', valueEN: 'Error In Email Or Password' },
            { valueAR: 'تمت أرشفة حسابك, من فضلك اتصل بإدارة التطبيق .', valueEN: 'You Account Had Been Archived, Please Contact Administration' },
            { valueAR: 'حسابك غير مفعل...من فضلك فعل حسابك من خلال إدخال الكود الخاص ببريدك الإلكتروني .', valueEN: 'You Account Is Not Activated, Please Activate Your Account By Using Code Sent To Your Email' },
            { valueAR: 'يوجد خطأ في عملية التسجيل .', valueEN: 'Error In Registeration' },
            { valueAR: 'يجب تفعيل حسابك أولا من قبل الأدمن', valueEN: 'Your Account Must Be Granted By Admin First' },
            { valueAR: 'خطأ في اسم الدخول أو كلمة المرور .', valueEN: 'Error In Username Or Password' },
            { valueAR: 'مدة صلاحية رمز التحقق الخاص بك قد انتهت , جاري تنشيط رمز التحقق  .', valueEN: 'You Security Token Is Expired, System Is Currently Reactivating It' },
            { valueAR: 'تم تنشيط رمز التحقق الخاص بك  .', valueEN: 'You Security Token Is Reactivated Successfully' },
            { valueAR: 'هذا البريد الإلكتروني مستخدم من قبل .', valueEN: 'This Email Is Used Before' },
            { valueAR: 'كلمة السر القديمة غير صحيحة .', valueEN: 'Old Password Is Incorrect' },
            { valueAR: 'البريد الإلكتروني مسجل من قبل', valueEN: 'Email Is Registered Before' },
            { valueAR: 'كلمة السر 6 حروف على الاقل', valueEN: 'Password Must Be At Least 6 Characters' },
            { valueAR: 'لا تتطابق كلمة السر مع تأكيد كلمة السر', valueEN: 'Passwords Are Not Matching' },
            { valueAR: 'اسم المستخدم مسجل من قبل', valueEN: 'Username Is Used Before' },
            { valueAR: 'رقم الجوال مسجل من قبل', valueEN: 'Mobile Is Used Before' },
            { valueAR: 'اسم المستخدم يجب ان يحتوي  على حروف  وارقام فقط و يكون باللغه الانجليزية', valueEN: 'Username Must Be In English Form And Contains Letters Or Numbers Only' },
            { valueAR: 'كود التفعيل غير صحيح  , برجاء تفقد البريد الالكترونى', valueEN: 'Activation Code Is Incorrect, Please Check Your Email' },
            { valueAR: 'تم رفع الصورة بنجاح .', valueEN: 'Photo Is Uploaded Successfully' },
            { valueAR: 'خطأ في إضافة صورة للمستخدم.', valueEN: 'Error In Uploading Photo' },
            { valueAR: 'خطأ في إسترجاع بيانات المدن.', valueEN: 'Error in Cities Data Retreival' },
            { valueAR: 'خطأ في إسترجاع وسائل النقل.', valueEN: 'Error In Tranport Vehicles Data Retireval' },
            { valueAR: 'هل انت متاكد من الإبلاغ عن هذا التعليق؟', valueEN: 'Do You Want To Report This Comment?' },
            { valueAR: 'هل انت متاكد من حذف هذا التعليق؟', valueEN: 'Do You Want To Remove This Comment?' },
            { valueAR: 'تم تعديل بيانات المستخدم بنجاح .', valueEN: 'User Data Is Updated Successfully' },
            { valueAR: 'تم تسجيل المستخدم بنجاح .', valueEN: 'User Is Registered Successfully' },
            { valueAR: 'لا يمكن التحقق من البيانات .', valueEN: 'Error In Checking User Credentials' },
            { valueAR: 'من فضلك أدخل إسم الدخول وكلمة المرور', valueEN: 'Please Enter Username And Password' },
            { valueAR: 'تم إعادة إرسال الكود بنجاح .', valueEN: 'Activation Code Is Re-sent Successfully' },
            { valueAR: 'تم إضافة الرحلة بنجاح .', valueEN: 'Trip Is Added Successfully' },
            { valueAR: 'خطأ في إضافة رحلة جديدة.', valueEN: 'Error In Adding Trip' },
            { valueAR: 'تم إضافة النقلة بنجاح .', valueEN: 'Luggage Is Added Successfully' },
            { valueAR: 'خطأ في إضافة نقلة جديدة.', valueEN: 'Error In Adding Luggage' },
            { valueAR: 'تم إرسال الكود لبريدك الإلكتروني بنجاح .', valueEN: 'Activation Code Is Sent To Your Email Successfully' },
            { valueAR: 'خطأ في إرسال الكود لبريدك الإلكتروني .', valueEN: 'Error In Sending Activation Code To Your Email' },
            { valueAR: 'تم تغيير كلمة المرور القديمة بنجاح .', valueEN: 'Old Password Is Changed Successfully' },
            { valueAR: 'خطأ في تغيير كلمة المرور القديمة.', valueEN: 'Error In Changing Old Password' },
            { valueAR: 'تم تعديل كلمة السر بنجاح .', valueEN: 'Password Is Changed Successfully' },
            { valueAR: 'خطأ في تعديل كلمة السر.', valueEN: 'Error In Changing Password' },
            { valueAR: 'تم تعديل الرحلة بنجاح .', valueEN: 'Trip Is Updated Successfully' },
            { valueAR: 'خطأ في تعديل الرحلة .', valueEN: 'Error In Updating Trip' },
            { valueAR: 'تم تعديل النقلة بنجاح .', valueEN: 'Luggage Is Updated Successfully' },
            { valueAR: 'خطأ في تعديل النقلة .', valueEN: 'Error In Updating Luggage' },
            { valueAR: 'خطأ في إسترجاع تفاصيل الرحلة.', valueEN: 'Error In Trip Details Data Retreival' },
            { valueAR: 'تم تأكيد الرحلة بنجاح .', valueEN: 'Trip Is Confirmed Successfully' },
            { valueAR: 'خطأ في تأكيد الرحلة.', valueEN: 'Error In Confirming Trip' },
            { valueAR: 'تم إلغاء الرحلة بنجاح .', valueEN: 'Trip Is Cancelled Successfully' },
            { valueAR: 'خطأ في إلغاء الرحلة.', valueEN: 'Error In Cancelling Trip' },
            { valueAR: 'خطأ في إسترجاع تفاصيل النقلة.', valueEN: 'Error In Luggage Details Data Retrieval' },
            { valueAR: 'تم تسجيل السعر المقترح .', valueEN: 'New Price Has Been Set Successfully' },
            { valueAR: 'لا يمكنك تحديد سعر لأن هذه الرحلة مؤكدة بالفعل.', valueEN: 'You Cannot Set Price For Confirmed Trip' },
            { valueAR: 'لا يمكنك تحديد سعر لأن هذه النقلة مؤكدة بالفعل.', valueEN: 'You Cannot Set Price For Confirmed Luggage' },
            { valueAR: 'لا يمكنك الموافقة علي السعر لأن هذه الرحلة مؤكدة بالفعل.', valueEN: 'You Cannot Approve Price For Confirmed Trip' },
            { valueAR: 'لا يمكنك الموافقة علي السعر لأن هذه النقلة مؤكدة بالفعل.', valueEN: 'You Cannot Approve Price For Confirmed Luggage' },
            { valueAR: 'لا يمكنك رفض السعر لأن هذه الرحلة مؤكدة بالفعل.', valueEN: 'You Cannot Reject Price For Confirmed Trip' },
            { valueAR: 'لا يمكنك رفض السعر لأن هذه النقلة مؤكدة بالفعل.', valueEN: 'You Cannot Reject Price For Confirmed Luggage' },
            { valueAR: 'لا يمكنك تحديد سعر لأن هذه الرحلة قد تم إلغاؤها.', valueEN: 'You Cannot Set Price For Cancelled Trip' },
            { valueAR: 'لا يمكنك تحديد سعر لأن هذه النقلة قد تم إلغاؤها.', valueEN: 'You Cannot Set Price For Cancelled Luggage' },
            { valueAR: 'لا يمكنك الموافقة علي السعر لأن هذه الرحلة قد تم إلغاؤها.', valueEN: 'You Cannot Approve Price For Cancelled Trip' },
            { valueAR: 'لا يمكنك الموافقة علي السعر لأن هذه النقلة قد تم إلغاؤها.', valueEN: 'You Cannot Approve Price For Cancelled Luggage' },
            { valueAR: 'لا يمكنك رفض السعر لأن هذه الرحلة قد تم إلغاؤها.', valueEN: 'You Cannot Reject Price For Cancelled Trip' },
            { valueAR: 'لا يمكنك رفض السعر لأن هذه النقلة قد تم إلغاؤها.', valueEN: 'You Cannot Reject Price For Cancelled Luggage' },
            { valueAR: 'هذه الرحلة عليها نقلات مؤكدة ولا يمكن حذفها', valueEN: 'This Trip Has Confirmed Luggages On It And Cannot Be Deleted' },
            { valueAR: 'هذه النقلة مؤكدة ولا يمكن حذفها', valueEN: 'This Luggage Is Confirmed And Cannot Be Deleted' },
            { valueAR: 'خطأ في تغيير حالة المستخدم.', valueEN: 'Error In Updating User Status' },
            { valueAR: 'من فضلك سجل دخولك أولا.', valueEN: 'Please Login First' },
            { valueAR: 'شكرا لك علي التقييم .', valueEN: 'Thank You For Rating' },
            { valueAR: 'لا يمكنك إضافة تعليق...من فضلك سجل دخولك أولا', valueEN: 'You Cannot Add New Comment, Please Login First' },
            { valueAR: 'خطأ في إضافة تعليق جديد .', valueEN: 'Error In Adding New Comment' },
            { valueAR: 'تم حذف التعليق بنجاح .', valueEN: 'Comment Is Removed Successfully' },
            { valueAR: 'خطأ في حذف التعليق .', valueEN: 'Error In Removing Comment' },
            { valueAR: 'يوجد لديك إشعار جديد , هل تريد تقفد الإشعار ؟', valueEN: 'You Have New Notification, Check It?' },
            { valueAR: 'إضافة تعليق', valueEN: 'Add Comment' },
            { valueAR: 'إلغاء الرحلة', valueEN: 'Cancel Trip' },
            { valueAR: 'إلغاء النقلة', valueEN: 'Cancel Luggage' },
            { valueAR: 'تم إلغاء النقلة بنجاح .', valueEN: 'Luggage Is Cancelled Successfully' },
            { valueAR: 'خطأ في إلغاء النقلة.', valueEN: 'Error In Cancelling Luggage' },
            { valueAR: 'النقلات علي هذه الرحلة', valueEN: 'Luggages On This Trip' },
            { valueAR: 'لا يوجد نقلات علي هذه الرحلة .', valueEN: 'No Luggages Found On This Trip' },
            { valueAR: 'فضلاً اترك تقييمك وتعليقك', valueEN: 'Rate Or Comment This User' },
            { valueAR: 'فضلاً اترك تعليقك', valueEN: 'Comment This User' },
            { valueAR: 'لا يوجد إشعارات .', valueEN: 'There Is No Notifications' },
            { valueAR: 'رقم الرحلة', valueEN: 'Trip Number' },
            { valueAR: 'رقم النقلة', valueEN: 'Luggage Number' },
            { valueAR: 'إضافة تعليق', valueEN: 'Add Comment' },
            { valueAR: 'لا يوجد تعليقات .', valueEN: 'No Comments Found' },
            { valueAR: 'لا يمكنك إختيار مكاني مغادرة ووصول متماثلين', valueEN: 'You Cannot Select The Same Place For Departure And Destination' },
            { valueAR: 'لا يمكنك إضافة رحلة أخري بسبب وجود أربعة فواتير غير مدفوعة', valueEN: 'You Cannot Add New Trip As You Have Four Unpaid Invoices' },
            { valueAR: 'لا يمكنك حذف رحلة مؤكدة', valueEN: 'You Cannot Delete Confirmed Trip' },
            { valueAR: 'هذه الرحلة لا توجد في قاعدة البيانات أو لا يحق لك حذفها', valueEN: 'You Have No Right To Access This Trip Or This Trip Not Found' },
            { valueAR: 'لا يمكنك حذف نقلة مؤكدة السعر', valueEN: 'You Cannot Delete Locked Luggage' },
            { valueAR: 'هل أنت متأكد من حذف هذه الرحلة ؟', valueEN: 'Do You Want To Delete This Trip?' },
            { valueAR: 'هل أنت متأكد من حذف هذه النقلة ؟', valueEN: 'Do You Want To Delete This Luggage?' },
            { valueAR: 'هذه النقلة لا توجد في قاعدة البيانات أو لا يحق لك حذفها', valueEN: 'You Have No Right To Access This Luggage Or This Luggage Not Found' });

    var control = langDict.filter(function (obj) {
        return obj.valueAR.toLowerCase() === value.toLowerCase() || obj.valueEN.toLowerCase() === value.toLowerCase();
    })[0];

    if (typeof control != 'undefined' || control != null) {
        if (lang === 'EN') {
            callBack(control.valueEN);
        }
        else {
            callBack(control.valueAR);
        }
    }
    else {
        callBack('none');
    }
}

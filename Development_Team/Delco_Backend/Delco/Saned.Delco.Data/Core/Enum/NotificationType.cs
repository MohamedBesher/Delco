using System;
using System.Reflection;

namespace Saned.Delco.Data.Core.Enum
{
    public enum NotificationType
    {
        [StringValue(@" تم استلام الطلب الخاص بك من قبل المندوب (####) ")]
        [IconValue("ionicons ion-android-share")]
        [ColorValue("#27ae60")]
        AgentReceivedRequest = 1,


        [StringValue(@" بدأ التحرك للطلب الخاص بك من قبل المندوب  (####) ")]
        [IconValue("ionicons ion-android-car")]
        [ColorValue("#f39c12")]
        StartMoving = 2,

        
        [StringValue(@" تم الوصول لمكان الطلب الخاص بك من قبل المندوب (####) ")]
        [IconValue("ionicons ion-checkmark-circled")]
        [ColorValue("#27ae60")]
        AgentDelivere = 3,


        //send agent vs users

        [StringValue(@"لقد قام مدير التطبيق بالغاء الطلب الخاص بكم ")]
        [IconValue("ionicons ion-close-circled")]
        [ColorValue("#f44336")]
        RequestCancele = 4,

        [StringValue(@"لقد قام مدير التطبيق بحذف المندوب المسئول عن طلبكم")]
        [IconValue("ionicons ion-android-delete")]
        [ColorValue("#f44336")]
        AgentDelete = 5,

        [StringValue(@" لقد تم اضافة طلب جديد من المستخدم (####) ")]
        [IconValue("ionicons ion-android-add-circle")]
        [ColorValue("#27ae60")]
        AgentsuitableRequest = 6,


        [StringValue(@"لقد تم الغاء طلب من المستخدم (####) ")]
        [IconValue("ionicons ion-close-circled")]
        [ColorValue("#f44336")]
        CancelInProgressRequest = 7,


        [StringValue(@"لقد تم ايقاف الحساب الخاص بكم لتجاوز مرات الرفض المسموح بها")]
        [IconValue("ionicons ion-android-delete")]
        [ColorValue("#f39c12")]
        SuspendAgent = 8,


        [StringValue(@"لقد قام مدير التطبيق بحذف المستخدم صاحب الطلب او المشوار  ")]
        [IconValue("ionicons ion-close-circled")]
        [ColorValue("#f44336")]
        UserDelete = 9
    }

    public class StringValueAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        #endregion


       

    }

    public class IconValueAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string IconValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public IconValueAttribute(string value)
        {
            this.IconValue = value;
        }

        #endregion




    }

    public class ColorValueAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string ColorValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public ColorValueAttribute(string value)
        {
            this.ColorValue = value;
        }

        #endregion




    }

}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Asset.Infrastructure.Common
{
    public class GeneralEnums
    {
        public enum Order : byte
        {
            [Display(Name = "صعودی")]
            Asc = 0,

            [Display(Name = "نزولی")]
            Desc = 1,
        }

        public enum Gender : byte
        {
            [Display(Name = "آقا")]
            Male = 1,

            [Display(Name = "خانم")]
            Female = 2,

            [Display(Name = "آقا / خانم")]
            Undefined = 3,
        }

        public enum Status : byte
        {
            [Display(Name = "غیر فعال")]
            Inactive = 0,

            [Display(Name = "فعال")]
            Active = 1,

            [Display(Name = "معلق")]
            Pending = 2,

            [Display(Name = "حذف شده")]
            Deleted = 3
        }
        public enum CategoryStatus : byte
        {
            [Display(Name = "غیر فعال")]
            Inactive = 0,

            [Display(Name = "فعال")]
            Active = 1,

            [Display(Name = "معلق")]
            Pending = 2,

            [Display(Name = "حذف شده")]
            Deleted = 3,

            [Display(Name = "به زودی")]
            ComingSoon = 4
        }

        public enum AvilabilityStatus : byte
        {
            [Display(Name = "دردسترس")]
            Available = 1,

            [Display(Name = "خارج از دسترس")]
            NotAvailable = 2,

            [Display(Name = "فعلا خارج از دسترس")]
            Away = 3,

            [Display(Name = "درخواست حذف")]
            DeleteRequest = 4
        }

        public enum SearchFieldType
        {
            String = 1,
            Number = 2,
            DateTime = 3,
            Enum = 4,
            Boolean = 5
        }

        public enum ModuleCategory : byte
        {
            [Display(Name = "بدون گروه")]
            Empty = 0,

            [Display(Name = "مدیران")]
            Admins = 1,

            [Display(Name = "کاربران")]
            Users = 2,

            [Display(Name = "رویداد ها")]
            Events = 3,

            [Display(Name = "کلاب ها")]
            Clubs = 4,

            [Display(Name = "مسابقات")]
            Matches = 5,

            [Display(Name = "مدیریت نقش ها")]
            Roles = 6,

            [Display(Name = "ماژول ها")]
            Module = 7,

            [Display(Name = "آمار وب سایت")]
            SaiteState = 11,            
        }

        public enum ModuleCategoryIco : byte
        {
            [Display(Name = "dolly-flatbed-empty")]
            Empty = 0,

            [Display(Name = "menu-icon fa fa-users")]
            Customer = 1,

            [Display(Name = "menu-icon fa fa-suitcase")]
            ServiceManegment = 2,

            [Display(Name = "menu-icon fa fa-car")]
            CarReport = 3,

            [Display(Name = "menu-icon fa fa-user")]
            Moderator = 4,

            [Display(Name = "menu-icon fa fa-tablet-alt")]
            AppManegment = 5,

            [Display(Name = "menu-icon fa fa-bullhorn")]
            CustomerCom = 6,

            [Display(Name = "menu-icon fa fa-cog")]
            SystemConfig = 7,

            [Display(Name = "menu-icon fa fa-pencil")]
            BaseFeatures = 8,

            [Display(Name = "menu-icon fa fa-money")]
            ChargeManegment = 9,

            [Display(Name = "menu-icon fa fa-user")]
            Profile = 10,

            [Display(Name = "آمار وب سایت")]
            SaiteState = 11,

        }


        public enum MessageStatus : byte
        {
            [Display(Name = "تازه")]
            New = 1,

            [Display(Name = "خوانده شده")]
            Read = 2,

            [Display(Name = "حذف شده")]
            Deleted = 3
        }

        public enum IdentityProvider : byte
        {
            [Display(Name = "PredictionEngine")]
            PredictionEngine = 1,

            [Display(Name = "SoccerChampion")]
            SoccerChampion = 2,

            [Display(Name = "JaamBartar")]
            JaamBartar = 3,

            [Display(Name = "LeagueBartar")]
            LeagueBartar = 4
        }

        public enum GeoType : byte
        {
            [Display(Name = "شهر")]
            City = 1,

            [Display(Name = "محله")]
            District = 2
        }

        public enum BooleanType : byte
        {
            [Display(Name = "خیر")]
            No = 0,

            [Display(Name = "بله")]
            Yes = 1
        }

        public enum AttachmentType : byte
        {
            [Display(Name = "صوتی")]
            Audio = 1,

            [Display(Name = "عکس")]
            Image = 2,

            [Display(Name = "متن")]
            Text = 3,

            [Display(Name = "ویدیو")]
            Video = 4,
            [Display(Name = "نمایه")]
            Thumbnail = 5,
            [Display(Name = "نمایه ویدیویی")]
            VideoThumbnail = 6
        }

        public enum ContentType : byte
        {
            [Display(Name = "بلاگ")]
            News = 1,

            [Display(Name = "ویدیو")]
            Movie = 2,

            [Display(Name = "مستندات ")]
            Documentary = 3,

            [Display(Name = "محتوای صوتی")]
            Audio = 4
        }

        public enum QulaityType : byte
        {
            [Display(Name = "پایین")]
            Low = 1,

            [Display(Name = "متوسط")]
            Medium = 2,

            [Display(Name = "خوب")]
            High = 3,

            [Display(Name = "عالی")]
            Excellent = 4
        }

        public enum HistoryActionType : byte
        {
            [Display(Name = "ایجاد")]
            Create = 1,

            [Display(Name = "بروزرسانی")]
            Update = 2,

            [Display(Name = "خواندن")]
            Read = 3,

            [Display(Name = "حذف")]
            Delete = 4
        }

        public enum States : byte
        {
            [Display(Name = "پیش نویس")]
            Draft = 1,

            [Display(Name = "ارسال شده")]
            Sent = 2,

            [Display(Name = "دریافت شده")]
            Deliver = 3,

            [Display(Name = "دیده شده")]
            Seen = 4
        }

        public enum UserType : byte
        {
            [Display(Name = "مشتری")]
            Customer = 1,

            [Display(Name = "خدمات دهنده")]
            Provider = 2
        }

        public enum ProviderEvidenceDocType : byte
        {
            [Display(Name = "نامشخص")]
            Unknown = 0,

            [Display(Name = "کارت ملی")]
            NationalCard = 1,

            [Display(Name = "شناسنامه")]
            BirthCertificate = 2,

            [Display(Name = "گواهی عدم سوءپیشینه")]
            Clearances = 3,

            [Display(Name = "پروانه کسب")]
            BusinessLicense = 4,

            [Display(Name = "اجاره نامه | سند")]
            Lease = 5
        }
        public enum ProviderEvidenceDocTypeForProvider : byte
        {
            [Display(Name = "کارت ملی")]
            NationalCard = 1,

            [Display(Name = "شناسنامه")]
            BirthCertificate = 2
        }
        public enum ProviderEvidenceDocTypeForBusiness : byte
        {
            [Display(Name = "پروانه کسب")]
            BusinessLicense = 4
        }

        public enum EvidenceStatusType : byte
        {
            [Display(Name = "نامشخص")]
            Unknown = 0,

            [Display(Name = "تایید شده")]
            Approved = 1,

            [Display(Name = "رد شده")]
            Rejected = 2
        }

        public enum NotificationStatus : byte
        {
            [Display(Name = "پیش نویس")]
            Draft = 1,

            [Display(Name = "زمانبندی شده")]
            Schadule = 2,

            [Display(Name = "ارسال شده")]
            Sent = 3,

            [Display(Name = "ناموفق")]
            Failed = 4
        }

        public enum DateType : byte
        {
            [Display(Name = "میلادی")]
            Gregorian = 1,

            [Display(Name = "خورشیدی")]
            Persian = 2
        }
        public enum ActionType : int
        {
            [Display(Name = "ایجاد")]
            Create = 1,
            [Display(Name = "ویرایش")]
            Update = 2,
        }
        public enum EntityType : byte
        {
            Business = 2,
            Category = 3,
            Service = 8,
            Provider = 9,
            User = 20,
            Category2Business = 21,
            Business2Facility = 22
        }
        public enum ServiceRequestStatusType : byte
        {
            [Display(Name = "در انتظار ارسال به سرویس دهنده")]
            WaitToSend = 1,
            [Display(Name = "ارسال  شده به سرویس دهنده")]
            SentToProvider = 2,
            [Display(Name = "پذیرفته شده توسط سرویس دهنده")]
            AcceptedByProvider = 3,
            [Display(Name = "ارائه پیش فاکتور توسط سرویس دهنده")]
            PresentedPreInvoice = 4,
            [Display(Name = "تایید پیش فاکتور توسط مشتری و رزرو")]
            Reserved = 5,
            [Display(Name = "لغو توسط مشتری")]
            CancelByCustomer = 6,
            [Display(Name = "لغو توسط سرویس دهنده")]
            CancelByProvider = 7,
            [Display(Name = "لغو توسط ادمین")]
            CancelByAdmin = 8,
            [Display(Name = "در حال انجام سرویس")]
            InProgress = 9,
            [Display(Name = "پایان سرویس توسط مشتری ")]
            EndedByCustomer =10,
            [Display(Name = "پایان سرویس توسط سرویس دهنده")]
            EndedByProvider=11,
            [Display(Name = "بدون سرویس دهنده")]
            NoProviderFound = 15
        }
        public enum ServiceRequest2BusinessStatusType : byte
        {
            [Display(Name = "ارسال  شده به سرویس دهنده")]
            SentToProvider = 1,
            [Display(Name = "رد شده توسط سرویس دهنده")]
            RejectedByProvider = 2,
            [Display(Name = "پذیرفته شده توسط سرویس دهنده")]
            AcceptedByProvider = 3,
            [Display(Name = "ارائه پیش فاکتور توسط سرویس دهنده")]
            SentPreInvoice = 4,
            [Display(Name = "تایید پیش فاکتور توسط مشتری و رزرو ")]
            ReservedPreInvoice = 5,
            [Display(Name = "عدم پذیرش توسط مشتری")]
            NotAcceptedByCustomer = 6,
            [Display(Name = "لغو شده")]
            CancelledByCustomer = 7,
            [Display(Name = "در حال انجام سرویس")]
            InProgress=10,
            [Display(Name = "پایان سرویس")]
            EndedByCustomer=11
        }
        public enum UserInsertError
        {
            [Display(Name = "وارد کردن تلفن یا ایمیل الزامی است")]
            NoEmailOrCellphone = -5,
            [Display(Name = "این شماره تلفن قبلا ثبت شده و فعال است")]
            CellphoneExists = -3,
            [Display(Name = "این ایمیل قبلا ثبت شده و فعال است")]
            EmailExists = -4
        }
        public enum UserVerifyError
        {
            [Display(Name = "این کاربر قبلا تایید شده است.")]
            UserAlreadyVerified = -6,
            [Display(Name = "وارد کردن تلفن یا ایمیل الزامی است")]
            NoEmailOrCellphone = -5,
            [Display(Name = "این شماره تلفن قبلا ثبت شده و فعال است")]
            CellphoneExists = -3,
            [Display(Name = "این ایمیل قبلا ثبت شده و فعال است")]
            EmailExists = -4
        }
        public enum FacilityRequestProviderResponseStatus
        {
            Reject = 2,
            Accept = 3
        }
        public enum FacilityRequestServiceResponseStatus
        {
            [Display(Name = "خطایی رخ داده است.")]
            Unknown = 0,
            [Display(Name = "این درخواست قبلا کنسل شده است.")]
            Cancelled = -5,
            [Display(Name = "مشتری قبلا پیش فاکتور دیگری را پذیرفته است.")]
            Revoked = -4,
            [Display(Name = "این درخواست وجود ندارد.")]
            ServiceRequestNotFound = -3,
            [Display(Name = "عملیات با موفقیت انجام شد.")]
            Success = 1
        }
        public enum AddInvoiceResponseStatus
        {
            [Display(Name = "خطایی رخ داده است.")]
            Unknown = 0,
            [Display(Name = "این کسب و کار وجود ندارد.")]
            BusinessRequestNotFound = -3,
            [Display(Name = "مدارک تایید نشده است.")]
            EvidenceNotApproved = -8,
            [Display(Name = "آدرس کسب و کار موجود نیست.")]
            AddressNotSupplied = -9,
            [Display(Name = "این درخواست وجود ندارد.")]
            ServiceRequestNotFound = -4,
            [Display(Name = "این درخواست معتبر نیست یا توسط کسب و کار پذیرفته نشده است.")]
            InvalidRequest = -5,
            [Display(Name = "این درخواست قبلا کنسل شده است.")]
            Cancelled = -14,
            [Display(Name = "قبلا از طرف این کسب و کار پیش فاکتور صادر شده است.")]
            InvoiceAlreadyExists = -10,
            [Display(Name = "یک یا چند سرویس اضافه است.")]
            ExteraInvoiceItems = -6,
            [Display(Name = "یک یا چند سرویس کم است.")]
            FewerInvoiceItems = -7,
            [Display(Name = "زمان وارد شده صحیح نیست.")]
            PreferedScheduleExpired = -11,
            [Display(Name = "زمان وارد شده صحیح نیست.")]
            OutOfPreferedSchedule = -12,
            [Display(Name = "این سرویس قبلا رزرو شده است.")]
            AlreadyReserved = -13,
            [Display(Name = "عملیات با موفقیت انجام شد.")]
            Success = 1
        }
        public enum AutomatedMessageType : byte
        {
            [Display(Name = "پیامک")]
            SMS = 1,
            [Display(Name = "پست الکترونیکی")]
            Email = 2,
            [Display(Name = "اعلان")]
            Notification = 3
        }
        public enum AutomatedMessageGroup : byte
        {
            [Display(Name = "ثبت نام")]
            Register = 1,
            [Display(Name = "دستورات پیامکی")]
            SMSCommand = 2,
            [Display(Name = "لغو اشتراک")]
            Unsubscribe = 3,
            [Display(Name = "بازیابی کلمه عبور")]
            ForgotPassword = 4,
            [Display(Name = "جوایز")]
            Prize = 5,
            [Display(Name = "گزارش سیستمی")]
            SystemReport = 6,
            [Display(Name = "تلگرام")]
            Telegram = 7,
            [Display(Name = "OTP کد دعوت")]
            OtpInviteCode = 8
        }
        public enum FactorItemStatus : byte
        {
            [Display(Name = "جدید")]
            New = 1,
            [Display(Name = "فروخته شده")]
            Sold = 2,
            [Display(Name = "نا معتبر")]
            Invalid = 3,
            [Display(Name = "کنسل شده")]
            Cancel = 4,
        }
        public enum PinFactorStatus : byte
        {
            [Display(Name = "جدید")]
            New = 1,
            [Display(Name = "تائید شده")]
            Approved = 2,
            [Display(Name = "نامعتبر")]
            Invalid = 3,
            [Display(Name = "کنسل شده")]
            Cancelled = 4,
        }
        public enum OperatorType : byte
        {
            [Display(Name = "MCI")]
            MCI = 1,
            [Display(Name = "TCI")]
            TCI = 2,
            [Display(Name = "ایرانسل")]
            Irancell = 3
        }
        public enum PinType : byte
        {
            [Display(Name = "10,000")]
            _10_000 = 1,
            [Display(Name = "20,000")]
            _20_000 = 2,
            [Display(Name = "50,000")]
            _50_000 = 3,
            [Display(Name = "100,000")]
            _100_000 = 4,
            [Display(Name = "200,000")]
            _200_000 = 5,
        }
        public enum PinInputType : byte
        {
            [Display(Name = "وب سرویس")]
            WebService = 1,
            [Display(Name = "فایل")]
            File = 2
        }

        public enum SmsType : byte
        {
            [Display(Name = "ارسالی")]
            Send = 1,
            [Display(Name = "دریافتی")]
            Recieve = 0
        }
        public enum SMSGeteway
        {
            Null,
            sms_ir,
            candoo,
            vas
        }
        public enum SmsSendErrorCode
        {
            Success = 1,
            Faild = 0
        }
        public enum AutomatedMessageGetStatus
        {
            BodyIsEmpty = -1,
            UnhandledError = 0,
            Success = 1
        }

        //public enum BusinessAttachmentType
        //{
        //    [Display(Name = "عکس")]
        //    Photo = 1,
        //    [Display(Name = "ویدیو")]
        //    Video = 2
        //}
        public enum ServiceRequestPreInvoiceStatusType : byte
        {
            [Display(Name = "در انتظار تایید")]
            WaitForAccept = 1,
            [Display(Name = "رد شده توسط مشتری")]
            RejectedByCustomer = 2,
            [Display(Name = "پذیرفته شده توسط مشتری")]
            AcceptedByCustomer = 3,
            [Display(Name = "لغوشده توسط مشتری")]
            CancelledByCustomer = 4,
            [Display(Name = "لغو شده توسط سرویس دهنده")]
            CancelledByProvider = 5
        }

        public enum CommentEntityType
        {
            [Display(Name = "کسب و کار")]
            Business = 1,
            [Display(Name = "محتوا")]
            Content = 2
        }

        public enum SubStatus : byte {

            [Display(Name = "غیر فعال")]
            Inactive = 0,
            [Display(Name = "فعال")]
            Active = 1
        }

        public enum ForceUpdateState : byte
        {

            [Display(Name = "غیر ضروری")]
            NotForce = 0,
            [Display(Name = "ضروری")]
            Force = 1
        }

        public enum EventType : byte
        {

            [Display(Name = "غیر ضروری")]
            NotForce = 0,
            [Display(Name = "ضروری")]
            Force = 1
        }

    }

}
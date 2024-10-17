using System.ComponentModel;

namespace PRM_API.Common.Enum;

public enum StatusBooking
{
    [Description("Chưa Thanh Toán")]
    Processing = 1,

    [Description("Thanh Toán Thành Công")]
    Paid = 2,

    [Description("Hết hạn thanh toán")]
    Expired = 3,

    [Description("Hủy Đơn")]
    Canceled = 4,
}
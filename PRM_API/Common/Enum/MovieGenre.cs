using System.ComponentModel;

namespace PRM_API.Common.Enum
{
    public enum MovieGenre
    {
        [Description("Hành động")]
        Action,

        [Description("Phiêu lưu")]
        Adventure,

        [Description("Chính kịch")]
        Drama,

        [Description("Giả tưởng")]
        Fantasy,

        [Description("Kinh dị")]
        Horror,

        [Description("Bí ẩn")]
        Mystery,

        [Description("Lãng mạn")]
        Romance,

        [Description("Khoa học viễn tưởng")]
        ScienceFiction,

        [Description("Giật gân")]
        Thriller,

        [Description("Hoạt hình")]
        Animation
    }
}

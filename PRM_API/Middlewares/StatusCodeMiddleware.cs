using System.Text.Json;
using PRM_API.Common.Payloads;

namespace PRM_API.Middlewares;

public class StatusCodeMiddleware
{
    private readonly RequestDelegate _next;

    public StatusCodeMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        // Kiểm tra mã trạng thái sau khi middleware tiếp theo được thực thi
        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            await HandleUnauthorizedAsync(context);
        }
        else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
        {
            await HandleForbiddenAsync(context);
        }
    }

    private static async Task HandleUnauthorizedAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        // Tạo đối tượng ApiResult cho lỗi 401
        var response = ApiResult<object>.Error(new { message = "You are not authorized to access this resource." });

        // Sử dụng JsonSerializer để chuyển đổi đối tượng response thành JSON
        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Đảm bảo sử dụng kiểu camelCase cho JSON
        });

        // Ghi phản hồi JSON vào response body
        await context.Response.WriteAsync(jsonResponse);
    }

    private static async Task HandleForbiddenAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        // Tạo đối tượng ApiResult cho lỗi 403
        var response = ApiResult<object>.Error(new { message = "You do not have permission to access this resource." });

        // Sử dụng JsonSerializer để chuyển đổi đối tượng response thành JSON
        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Đảm bảo sử dụng kiểu camelCase cho JSON
        });

        // Ghi phản hồi JSON vào response body
        await context.Response.WriteAsync(jsonResponse);
    }
}
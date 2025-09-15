// File: Services/BlockMobileForAdminMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Logging;
using Syncfusion.EJ2.Layouts;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Zaid.Services
{

    public class BlockMobileForAdminMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<BlockMobileForAdminMiddleware> _logger;
        private const string VerifiedDeviceCookie = "VerifiedDevice"; 

        public BlockMobileForAdminMiddleware(RequestDelegate next, ILogger<BlockMobileForAdminMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (context.User?.Identity != null && context.User.Identity.IsAuthenticated && context.User.IsInRole("Admin"))
                {
                    var path = context.Request.Path.Value ?? "";

                    bool isProtectedPath =
                        path.Contains("ManagerAppController1", StringComparison.OrdinalIgnoreCase)
                        || path.StartsWith("/admin", StringComparison.OrdinalIgnoreCase)
                        || path.StartsWith("/Manager", StringComparison.OrdinalIgnoreCase);

                    if (isProtectedPath)
                    {
                        var cookie = context.Request.Cookies[VerifiedDeviceCookie];

                        if (!string.IsNullOrEmpty(cookie))
                        {
                            if (string.Equals(cookie, "Desktop", StringComparison.OrdinalIgnoreCase))
                            {
                                await _next(context);
                                return;
                            }

                            if (string.Equals(cookie, "Mobile", StringComparison.OrdinalIgnoreCase))
                            {
                                _logger.LogWarning("Blocked admin mobile access (cookie). User: {User}, IP: {IP}, Path: {Path}",
                                                   context.User.Identity?.Name ?? "unknown",
                                                   context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                                                   path);

                                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                context.Response.ContentType = "text/html; charset=utf-8";
                                await context.Response.WriteAsync(GetBlockedHtml());
                                return;
                            }

                        }

                        context.Response.StatusCode = StatusCodes.Status200OK;
                        context.Response.ContentType = "text/html; charset=utf-8";
                        await context.Response.WriteAsync(GetDeviceCheckPageHtml());
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BlockMobileForAdminMiddleware");
            }

            await _next(context);
        }

       
        private string GetDeviceCheckPageHtml()
        {
            return @"<!doctype html>
<html lang='en'>
<head>
  <meta charset='utf-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1'>
  <title>Device verification</title>
  <style>
    body{font-family:Arial,Helvetica,sans-serif;display:flex;align-items:center;justify-content:center;height:100vh;background:#f2f4f7;margin:0}
    .card{background:#fff;padding:20px;border-radius:8px;box-shadow:0 8px 30px rgba(0,0,0,0.08);max-width:520px;text-align:center}
    .muted{color:#666;font-size:0.95rem}
  </style>
</head>
<body>
  <div class='card'>
    <h3>Verifying device…</h3>
    <p class='muted'>Quick security check. This takes less than a second.</p>
    <div id='status' class='muted'>Please wait…</div>
  </div>

  <script>
    (function(){
      function safe(val, fallback){ return (typeof val === 'undefined' || val === null) ? fallback : val; }
      try {
        var device = {
          width: safe(window.innerWidth || document.documentElement.clientWidth, 0),
          height: safe(window.innerHeight || document.documentElement.clientHeight, 0),
          touch: safe(navigator.maxTouchPoints || (navigator.msMaxTouchPoints || 0), 0),
          ua: navigator.userAgent
        };

        fetch('/device-check', {
          method: 'POST',
          headers: {'Content-Type':'application/json'},
          body: JSON.stringify(device),
          credentials: 'same-origin'
        }).then(function(resp){
          return resp.json();
        }).then(function(data){
          if (data && data.allowed) {
            window.location.reload(true);
          } else {
            document.body.innerHTML = '<div style=""display:flex;align-items:center;justify-content:center;height:100vh;background:#f8f9fa""><div style=""text-align:center""><h2 style=""color:#b91c1c"">Access Denied</h2><p>You cannot access admin pages from mobile devices. Use a desktop/laptop.</p></div></div>';
          }
        }).catch(function(err){
          document.getElementById('status').innerText = 'Verification failed. Try again.';
        });
      } catch (e) {
        document.getElementById('status').innerText = 'Verification failed. Please enable JavaScript.';
      }
    })();
  </script>
</body>
</html>";
        }


        private string GetBlockedHtml()
{
    return @"<!doctype html>
<html lang='en'>
<head><meta charset='utf-8'><meta name='viewport' content='width=device-width, initial-scale=1'><title>Access Denied</title>
<style>
 body{font-family:Arial,Helvetica,sans-serif;display:flex;align-items:center;justify-content:center;height:100vh;background:#f8f9fa;margin:0}
 .card{background:#fff;padding:24px;border-radius:8px;box-shadow:0 10px 30px rgba(0,0,0,0.08);max-width:640px;text-align:center}
 .btn{display:inline-block;margin-top:12px;padding:10px 18px;background:#2563eb;color:#fff;border-radius:6px;text-decoration:none}
</style>
</head>
<body>
  <div class='card'>
    <h1 style='color:#b91c1c'>Access Denied</h1>
    <p>For security reasons, administrators are not allowed to use the management interface from mobile devices. Please use a desktop or laptop computer.</p>
    <a class='btn' href='/'>Go back</a>
  </div>
</body>
</html>";
}
    }
}

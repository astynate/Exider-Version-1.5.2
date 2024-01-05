using Cloud_Storage.Controllers;
using Cloud_Storage.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

    .AddJwtBearer(options =>
    {

        options.TokenValidationParameters = new TokenValidationParameters
        {

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Options.Issuer,
            ValidAudience = Options.Audience,
            IssuerSigningKey = Options.GetSecurityKey(),
            
        };

    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();

}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllerRoute(name: "default", pattern: "{controller=chats}/{action=index}");
app.UseRouting();

app.Use(async (context, next) =>
{
    var token = context.Request.Cookies[".AspNetCore.Mvc.CookieTempDataProvider"];

    if (!string.IsNullOrEmpty(token)) 
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }

    await next.Invoke();

});

app.UseStatusCodePages(async (context) => 
{

    var response = context.HttpContext.Response;

    if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        response.Redirect("/login");
    }

});

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<UserHub>("/chatHub");

app.Run();

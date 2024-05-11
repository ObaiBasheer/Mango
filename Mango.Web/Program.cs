using Mango.Web.Services;
using Mango.Web.Services.Auth;
using Mango.Web.Services.Coupon;
using Mango.Web.Services.RequestProvider;
using Mango.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Resister And Resolve IHttpClientFactory
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();

//Get API URL 
SD.CouponURLBase = builder.Configuration["ServiceURL:API-URL"];
SD.AuthAPIBase = builder.Configuration["ServiceURL:Auth-API-URL"];


builder.Services.AddScoped<IRequestProvider, RequestProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();    


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

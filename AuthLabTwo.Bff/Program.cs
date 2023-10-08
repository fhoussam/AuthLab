using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .WithOrigins("https://localhost:44450")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("cookies", options =>
{
    options.Cookie.Name = "bff";
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.Path = "/";
    options.Cookie.Domain = ".localhost";
})
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://localhost:5001/";
    options.ClientId = "interactive";
    options.ClientSecret = "secret";

    options.ResponseType = "code";
    options.GetClaimsFromUserInfoEndpoint = true;
    options.SaveTokens = true;

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("scope2");
    options.Scope.Add("offline_access");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(async (context, next) =>
    {
        var token = await context.GetTokenAsync("access_token");
        context.Request.Headers.Add("Authorization", $"Bearer {token}");
        await next().ConfigureAwait(false);
    });
});

app.Run();

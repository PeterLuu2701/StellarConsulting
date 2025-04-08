using StellarConsulting.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "StellarApp", // 🔁 must match appsettings.json
            ValidAudience = "StellarUsers", // 🔁 must match appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKey123!")) // 🔁 must match appsettings.json
        };
    });

// ✅ Add Razor Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ✅ Register HttpClient for API access
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(sp.GetRequiredService<NavigationManager>().BaseUri) });

var app = builder.Build();

// ✅ Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ✅ Add authentication/authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// ✅ Map Razor components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

using Resume.Application.Utility.JWTSecret;
using Resume.IOC.DbContainer;
using Resume.IOC.JWTContainer;
using Resume.IOC.ServiceContainer;
using Resume.IOC.SwaggerContainer;

#region Services

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.DataBaseConfig(builder.Configuration.GetConnectionString("ResumeCon"));

builder.Services.AddOurSwagger();

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSetting>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSetting>();
builder.Services.AddOurJWT(appSettings);

builder.Services.ServiceConfing();

#endregion


#region MiddlewareS

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();


#endregion
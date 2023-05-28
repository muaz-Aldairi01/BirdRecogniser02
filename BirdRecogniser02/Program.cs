using BirdRecogniser02.Authorization;
using BirdRecogniser02.Data;
using BirdRecogniser02.ML;
using BirdRecogniser02.ML.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.ML;
using Microsoft.ML;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


/////////////////////////////////////////////////////////////////
//Configure the ML.NET model for the pre-trained TensorFlow model.
string _tensorFlowModelFilePath = GetAbsolutePath(builder.Configuration["MLModel:TensorFlowModelFilePath"]);
var tensorFlowModelConfigurator = new TensorFlowModelConfigurator(_tensorFlowModelFilePath);
ITransformer _mlnetModel = tensorFlowModelConfigurator.Model;
/////////////////////////////////////////////////////////////////////////////
// Register the PredictionEnginePool as a service in the IoC container for DI.
//
builder.Services.AddPredictionEnginePool<ImageInputData, ImageLabelPrediction>();
builder.Services.AddOptions<PredictionEnginePoolOptions<ImageInputData, ImageLabelPrediction>>()
    .Configure(options =>
    {
        options.ModelLoader = new InMemoryModelLoader(_mlnetModel);
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});


// Authorization handlers.
builder.Services.AddScoped<IAuthorizationHandler,
                      SubmissionIsOwnerAuthorizationHandler>();

builder.Services.AddSingleton<IAuthorizationHandler,
                      SubmissionAdministratorsAuthorizationHandler>();

builder.Services.AddSingleton<IAuthorizationHandler,
                      SubmissionManagerAuthorizationHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    // requires using Microsoft.Extensions.Configuration;
    // Set password with the Secret Manager tool.
    // dotnet user-secrets set SeedUserPW <pw>

    var testUserPw = builder.Configuration.GetValue<string>("SeedUserPW");//"Bird$Recogniser2023";

    await SeedData.Initialize(services, testUserPw);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();

string GetAbsolutePath(string relativePath)
{
    var _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
    string assemblyFolderPath = _dataRoot.Directory.FullName;

    string fullPath = Path.Combine(assemblyFolderPath, relativePath);
    return fullPath;
}
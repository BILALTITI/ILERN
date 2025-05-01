using Assigment2.Filters;
using Assigment2.Models;
using Assigment2.Models.interfaces;
using Assigment2.Models.Repositroy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//// Get the connection string from appsettings.json
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//// Register DbContext with the connection string
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(connectionString)); 



// service type 
//1- bult in service and aalready registered in IOS ( built-in service) e.g "Iconfiguration "
//2- bult in service not register in IOS ( custom service) "add Session" 

builder.Services.AddControllersWithViews(
   // configure =>
//[Authorize]
    //configure.Filters.Add(new MyFilter()) ,// إضافة الفلتر إلى كل الـ Controllers
      //configure.Filters.Add(typeof(MyFilter))  // إضافة الفلتر إلى جميع الأكشنز بدون إنشاء كائن جديد
      //configure.Filters.Add(new MyFilter("parameterValue")) // with params 
    );
builder.Services.AddSession(config =>
{
    config.Cookie.Name = "Cart";
    config.IdleTimeout = TimeSpan.FromMinutes(20);
    config.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
});

// Register Identity we can custom the password (Override the default password )
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Option =>
//{ 
////{
////    Option.Password.RequireDigit = false;
////    Option.Password.RequireLowercase = false;
////    Option.Password.RequireUppercase = false;
////    Option.Password.RequireNonAlphanumeric = false;
////    Option.Password.RequiredLength = 6;

////})
//    .AddEntityFrameworkStores<AppDbContext>();

// Add Identity first
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Then add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Default for MVC
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Default for API
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Redirect for MVC views
    options.LogoutPath = "/Account/Logout";
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

// lossly couple i depand on any Repoisotiry apply this Interface

// when every service distroy the object will distroy
//1- AddTransient: create new object every time
//2- AddScoped: create new object one time per request
//3- AddSingleton: create one object and use it every time

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepoistory>();
builder.Services.AddScoped<ICourseRepository, CourseRepoistory>();
builder.Services.AddScoped<ITraineeRepository, TraineeRepoistory>();
builder.Services.AddScoped<IinstructorRepository, instructorRepostory>();
builder.Services.AddScoped<ICourseResultReipoistory, CourseResultRepository>();
builder.Services.AddCors(corsoptions =>
{
    corsoptions.AddPolicy("MyPolicy", corspolicybulder =>
    {
        corspolicybulder.AllowAnyOrigin(  /* i can allow for specfic orign like angular */).AllowAnyMethod(/*// i can allo only get or post or all*/).AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();
app.UseStaticFiles();// to cant catch html,css js files request and serve 
                     // setting cors policy // to allow the request from the client side ex angular , react external server
app.UseCors("MyPolicy");// open or block the request from the client side

app.UseAuthentication();// check JWT Token
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
         pattern: "{controller=Account}/{action=Login}/{id?}");
app.Run();

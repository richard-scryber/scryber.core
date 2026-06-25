using Scryber;
using Scryber.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var config = builder.Configuration;
Scryber.ServiceProvider.Init(config);

var scryberConfig = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();

var fontRegistration = new FontRegistrationOption()
{
    Family = "OpenSans-light",
    Style = "Regular",
    Weight = 300,
    //File = "./fonts/OpenSans-light.ttf",
    Resource = "Scryber.UnitWebTest.Fonts.OpenSans-Light.ttf, Scryber.UnitWebTest"
};

var all = new List<FontRegistrationOption>(scryberConfig.FontOptions.Register ?? new List<FontRegistrationOption>());
all.Add(fontRegistration);
scryberConfig.FontOptions.Register = all;


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
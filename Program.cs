using Microsoft.EntityFrameworkCore;
using SeafoodApp.Data;
using SeafoodApp.Repositories;
using SeafoodApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình dịch vụ
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IAllocationService, AllocationService>();
builder.Services.AddScoped<IProductionOrderService, ProductionOrderService>();
builder.Services.AddScoped<IProcessingTicketService, ProcessingTicketService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IProcessingStageService, ProcessingStageService>();
builder.Services.AddScoped<IProductWageRateService, ProductWageRateService>();
builder.Services.AddScoped<IProcessingStageRepository, ProcessingStageRepository>();
builder.Services.AddScoped<IProductWageRateRepository, ProductWageRateRepository>(); // Thêm đăng ký này

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cấu hình pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
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

#pragma warning disable CS0219 // Suppress warning about 'app' if it occurs
var unused = app; // Optional: Explicitly use 'app' to suppress warnings
#pragma warning restore CS0219
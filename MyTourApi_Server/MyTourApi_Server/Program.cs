
using MyTourApi_Server.Services.Impls;
using MyTourApi_Server.Repositories;
using MyTourApi_Server.Repositories.Impls;
using MyTourApi_Server.Repositories.Interfaces;
using MyTourApi_Server.Services;
using MyTourApi_Server.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                         ?? "Server=desktop-l1qa5o7\\sqlexpress;Database=WB43;Trusted_Connection=True;TrustServerCertificate=True;";

// Add services to the container.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITouristSpotService, TouristSpotService>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ITouristSpotRepository, TouristSpotRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddSingleton(new AccommodationRepository(connectionString));
builder.Services.AddSingleton<AccommodationService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddSingleton<Db>(); // DB 연결 도우미 등록
builder.Services.AddScoped<TouristSpotImportRepository>(); // 수집용 레포지토리 등록

builder.Services.AddHttpClient<TourApiService>(); // HttpClient가 내장된 TourApi 서비스 등록
builder.Services.AddScoped<INaverLocalApiService, NaverLocalApiService>();

builder.Services.AddScoped<IViewPointRepository, ViewPointRepository>();
builder.Services.AddScoped<IViewPointService, ViewPointService>();

builder.Services.AddScoped<ICampSiteRepository, CampSiteRepository>();
builder.Services.AddScoped<ICampSiteService, CampSiteService>();
builder.Services.AddScoped<ICampSiteCsvImportService, CampSiteCsvImportService>();
builder.Services.AddScoped<IViewPointRepository, ViewPointRepository>();
builder.Services.AddScoped<IViewPointService, ViewPointService>();

builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IChatService, ChatService>();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

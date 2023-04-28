
using BlogAPI.Data;
using BlogAPI.Data.Repositories;
using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace BlogAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("default") ?? throw new InvalidOperationException("Connection string 'default' not found");
            builder.Services.AddDbContext<BlogContext>(options => options.UseSqlServer(connectionString));
            // Add services to the container.
            builder.Services.AddCors();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWTSecretKey"))
                        )
                    };
                });
            builder.Services.AddSingleton<IAuthService>(
                new AuthService(
                    builder.Configuration.GetValue<string>("JWTSecretKey"),
                    builder.Configuration.GetValue<int>("JWTLifespan")
                )
            );

            builder.Services.AddScoped(typeof(IUserRepository<User, string>), typeof(UserRepository<User, string>)); 
            builder.Services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
            builder.Services.AddScoped(typeof(IBlogService<,>), typeof(BlogService<,>));
            builder.Services.AddScoped<IBaseRepository<BlogPost, int>, PostRepository>();

            builder.Services.AddControllers().AddNewtonsoftJson(
                opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                SeedData.Seed(scope.ServiceProvider);
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                //.AllowCredentials()
                );
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

using Microsoft.Extensions.FileProviders;

namespace ImagesHandling
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Default

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #endregion

            #region CORS Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllDomains", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            #endregion

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            var staticFilesPath = Path.Combine(Environment.CurrentDirectory, "Images");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider=new PhysicalFileProvider(staticFilesPath),
                RequestPath="/Images"
            });
            app.UseHttpsRedirection();

            app.UseCors("AllowAllDomains");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
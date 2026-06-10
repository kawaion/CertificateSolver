using CertificateSolver.Core.Interfaces;
using CertificateSolver.Core.Rules;
using CertificateSolver.Core.Services;
using CertificateSolver.Infrastructure.Repositories;
using CertificateSolver.Infrastructure.Storage;

namespace CertificateSolver 
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IRequestStatusTransition, RequestStatusTransition>();
            builder.Services.AddSingleton<IIdempotencyStore, IdempotencyStore>();
            builder.Services.AddSingleton<IIdempotencyKeyGenerator, IdempotencyKeyGenerator>();
            builder.Services.AddSingleton<IRequestRepository, RequestRepository>();
            builder.Services.AddScoped<IRequestService, RequestService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

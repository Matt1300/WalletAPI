
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WalletAPI.Application.Interfaces;
using WalletAPI.Application.Movements.Create;
using WalletAPI.Application.Shared;
using WalletAPI.Application.Wallets.Create;
using WalletAPI.Infrastructure.Persistence;
using WalletAPI.Infrastructure.Repositories;

namespace WalletAPI.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IWalletRepository, WalletRepository>();
            builder.Services.AddScoped<IMovementRepository, MovementRepository>();

            builder.Services.Scan(scan => scan
                .FromAssemblies(typeof(Result).Assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            builder.Services.AddValidatorsFromAssembly(typeof(Result).Assembly);

            builder.Services.Decorate(typeof(ICommandHandler<CreateWalletCommand, Result<int>>), typeof(ValidationBehavior<CreateWalletCommand, Result<int>>));
            builder.Services.Decorate(typeof(ICommandHandler<TransferBalanceCommand, Result>), typeof(ValidationBehavior<TransferBalanceCommand, Result>));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

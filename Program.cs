using AppAny.HotChocolate.FluentValidation;
using CacheManager.Core;
using EFCoreSecondLevelCacheInterceptor;
using FluentValidation.AspNetCore;
using GraphQLLearning;
using GraphQLLearning.DataLoaders;
using GraphQLLearning.Schema.Mutations;
using GraphQLLearning.Schema.Queries;
using GraphQLLearning.Services;
using GraphQLLearning.Services.Courses;
using GraphQLLearning.Services.Instructors;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddFluentValidationAutoValidation();
//builder.Services.AddTransient<CourseInputValidator>();
builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<CourseType>()
    .AddType<InstructorType>()
    .AddTypeExtension<CourseQuery>()
    .AddFiltering()
    .AddSorting()
    .AddProjections()
    .AddFluentValidation(_ => _.UseDefaultErrorMapper());

builder.Services
    .AddPooledDbContextFactory<SchoolDbContext>((serviceProvider, option) => option.UseSqlServer("Server=(localdb)\\MyInstance;Database=SchoolDb;Trusted_Connection=True;")
    .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>()));

var jss = new JsonSerializerSettings
{
    NullValueHandling = NullValueHandling.Ignore,
    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
    TypeNameHandling = TypeNameHandling.Auto,
    Converters = { new SpecialTypesConverter() }
};

const string redisConfigurationKey = "redis";
builder.Services.AddSingleton(typeof(ICacheManagerConfiguration),
    new CacheManager.Core.ConfigurationBuilder()
        .WithJsonSerializer(serializationSettings: jss, deserializationSettings: jss)
        .WithUpdateMode(CacheUpdateMode.Up)
        .WithRedisConfiguration(redisConfigurationKey, config =>
        {
            config.WithAllowAdmin()
            .WithSsl("TODO redis cache name")
                .WithDatabase(0)
                .WithEndpoint("TODO redis cache name", 6380)
                .WithPassword("TODO redis cache credential")
                // Enables keyspace notifications to react on eviction/expiration of items.
                // Make sure that all servers are configured correctly and 'notify-keyspace-events' is at least set to 'Exe', otherwise CacheManager will not retrieve any events.
                // You can try 'Egx' or 'eA' value for the `notify-keyspace-events` too.
                // See https://redis.io/topics/notifications#configuration for configuration details.
                .EnableKeyspaceEvents();
        })
        .WithMaxRetries(100)
        .WithRetryTimeout(50)
        .WithRedisCacheHandle(redisConfigurationKey)
        .Build());
builder.Services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));

builder.Services.AddEFSecondLevelCache(options =>
    options.UseCacheManagerCoreProvider().DisableLogging(true).UseCacheKeyPrefix("EF_")
);

builder.Services.AddScoped<ICoursesRepository, CoursesRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<InstructorDataLoader>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL("/graphql");

//using (var scope = app.Services.CreateScope())
//{
//    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SchoolDbContext>>();
//    using SchoolDbContext context = contextFactory.CreateDbContext();
//    context.Database.EnsureCreated();
//}

app.Run();

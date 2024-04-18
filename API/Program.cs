using API;

var builder = WebApplication.CreateBuilder(args);


builder.AddAppServices();
builder.AddPersistence();
builder.AddAppInjections();
builder.AddAutoMapper();
builder.Services.AddCors(x => x.AddPolicy(name: "Web App", policy =>
{
    policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
}));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Web App");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

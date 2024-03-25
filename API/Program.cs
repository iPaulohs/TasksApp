using API;

var builder = WebApplication.CreateBuilder(args);


builder.AddAppServices();
builder.AddAppPersistence();
builder.AddAppIdentity();
builder.AddAppInjections();
builder.AddAutoMapper();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

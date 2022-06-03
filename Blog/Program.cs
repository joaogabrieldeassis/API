using Blog.DataContext;
using Blog.Services;

var builder = WebApplication.CreateBuilder(args);
//Desabilitar a validação automatica da minha api
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});   ;
builder.Services.AddDbContext<BlogDataContext>();
builder.Services.AddTransient<TokenServices>(); 
var app = builder.Build();

app.MapControllers();
app.Run();

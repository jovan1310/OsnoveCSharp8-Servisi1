var builder = WebApplication.CreateBuilder(args);
// Kreira samo jednu instancu objekta:
// builder.Services.AddSingleton<IMojServis, MojServis>();
// Kreira instancu objekta svaki put kada se posalje zahtev, posrednici dele isti servis:
// builder.Services.AddScoped<IMojServis, MojServis>();
// Kreira posebnu instancu za svaki od posrednika za svaki zahtevi, odnosno, svako od njih dobija novi id za svaki zahtev:
// builder.Services.AddTransient<IMojServis, MojServis>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var MojServis2 = context.RequestServices.GetRequiredService<IMojServis>();
    MojServis2.PravljenjeLoga("Prvi posrednik");
    await next.Invoke();
});

app.Use(async (context, next) =>
{
    var MojServis3 = context.RequestServices.GetRequiredService<IMojServis>();
    MojServis3.PravljenjeLoga("Drugi posrednik");
    await next.Invoke();
});

app.MapGet("/", (IMojServis mojServis1) =>
{
    mojServis1.PravljenjeLoga("Administrator");
    return Results.Ok("Proveri konzolu za ID servisa.");
});

app.Run();

public interface IMojServis
{
    void PravljenjeLoga(string logPoruka);
}

public class MojServis : IMojServis
{
    private readonly int _servisID;

    public MojServis()
    {
        _servisID = new Random().Next(100000,999999);
    }
    public void PravljenjeLoga(string logPoruka)
    {
        Console.WriteLine($"{logPoruka} - Service ID: {_servisID}");
    }
}
using Cph.DDD.Meetup.Logistics.Domain;
using Cph.DDD.Meetup.Logistics.Ui;
using Cph.DDD.Meetup.Logistics.Ui.Components;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddMarten(s =>
    {
        var o = new StoreOptions();
        o.Connection("Host=localhost;Port=5432;Username=postgres;Password=Password12!;Database=postgres");
        o.Projections.Add(new PubSubProjection(), ProjectionLifecycle.Async);
        return o;
    })
    .ApplyAllDatabaseChangesOnStartup().AddAsyncDaemon(DaemonMode.Solo);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

Task.Run(async () =>
{
    var k = app.Services.GetRequiredService<IDocumentStore>();
    var i = 0;
    do
    {
        var lightweightSession = k.LightweightSession();
        lightweightSession.Events.Append(Guid.NewGuid(),
            new ContainerStoreInitialized(new ContainerStoreId(Guid.NewGuid()),
                new FreightLocationId(++i),
                "Store",
                new HashSet<ContainerState>()));
        await lightweightSession.SaveChangesAsync();
        await Task.Delay(2000);
    } while (true);
});

app.Run();

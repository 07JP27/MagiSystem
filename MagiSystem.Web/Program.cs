using MagiSystem.Web.Components;
using MagiSystem.Core;
using Microsoft.Extensions.AI;
using Azure.AI.OpenAI;
using Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register AI services
// Azure OpenAI configuration - in production, use configuration/environment variables
builder.Services.AddSingleton<IChatClient>(_ =>
{
    var endpoint = builder.Configuration["AzureOpenAI:Endpoint"] ?? "https://your-resource.openai.azure.com/";
    var apiKey = builder.Configuration["AzureOpenAI:ApiKey"] ?? "your-api-key";
    var deploymentName = builder.Configuration["AzureOpenAI:DeploymentName"] ?? "gpt-35-turbo";
    
    var azureClient = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
    var openAIChatClient = azureClient.GetChatClient(deploymentName);
    
    // Convert OpenAI ChatClient to Microsoft.Extensions.AI.IChatClient
    return openAIChatClient.AsIChatClient();
});
builder.Services.AddScoped<MagiService>();

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

app.Run();

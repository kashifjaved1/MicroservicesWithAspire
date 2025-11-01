var builder = DistributedApplication.CreateBuilder(args);

var restaurantsapi = builder.AddProject<Projects.Restaurants_API>("restaurantsapi");

var urlshortenerapi = builder.AddProject<Projects.UrlShortener_API>("urlshortenerapi"); // grpc server

var authapi = builder.AddProject<Projects.Auth_API>("authapi") // grpc client
    .WithReference(urlshortenerapi);

var storeapi = builder.AddProject<Projects.Store_API>("storeapi");

var gateway = builder.AddProject<Projects.ApiGateway>("apigateway")
    .WithReference(restaurantsapi) // automatically wires endpoint with gateway
    .WithReference(urlshortenerapi)
    .WithReference(authapi)
    .WithReference(storeapi);

builder.Build().Run();

// client

builder.Services.AddOrleansClient(client =>
{
    client.UseLocalhostClustering();
    client.Configure<ClusterOptions>(options =>
     {
         options.ClusterId = "orleans-cluster";
         options.ServiceId = "MyApporleans-service";
     });
    client.AddActivityPropagation();
});


// silo

    client.AddActivityPropagation();

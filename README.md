# stream-manager

A service written in dotnet core 3 which exposes an API allowing a client start, stop and retrieve a count of video streams for a given username.

A client will be prevented from starting a new stream for a given user if the user already has 3 concurrent streams (this is configurable in the appsettings.json file).

# Assumptions

The client will always call the stop endpoint when a stream stops. If this is not the case then it may be necessary for a facility to be developed to allow streams to be removed from the count after a period of inactivity, when starting a stream a stream id could be issued, this could be passed periodically to an endpoint which would update a timestamp in the datastore.

# To Run The Service

Download and install .NET Core 3.0 SDK

Navigate to src directory and run the following command:

dotnet run

Check the application is running by going to the URL shown in the console + /health, e.g. http://localhost:5000/health

To test the api navigate to http://localhost:5000/swagger

To debug the application, open in VS Code and press F5

# To Run Tests
Navigate to tests/StreamManager.Tests

run the following command:

dotnet test

# Deployment

- Run Docker build in the directory containing the Dockerfile
- Tag and Push the container to ECR
- Create the DynamoDb table 'user-streams' with a key of userName (string)
- Create an ECS Cluster (I did this using the Get Started wizzard, selecting the custom container and inputting the image URI from ECR)


# Scalability
For local development I have implemented an in memory repository to hold user stream counts, however, in order to allow the app to be horizontally scalable the stream count must be stored in an external database.

As this service is to be deployed on AWS I've chosen DynamoDb, by default this is eventually consistent which may present a scaling problem as the count may be modified simultaneously by more than one instance of the service, this can be negated by using consistent reads. (https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/HowItWorks.ReadConsistency.html)

The app is currently deployed as a Docker container running in ECS, more instances of the container can be created manually or in response to demand.

# Further Considerations
- The API may need to be secured, requiring clients to supply a token in order to access resources, this could be done using IdentityServer and the associated middlewear (http://docs.identityserver.io/en/latest/).

- It may be necessary to validate usernames before allowing a stream to be started, however, this logic probably belongs in a separate service.

- A CI/CD pipeline should be developed, as part of this code should be peer reviewed, unit tests should be run automatically, in addition to this automated end to end tests could be run using tools like Postman/Newman.

- Creation of the ECS Cluster, Service, Task, Load Balancer, DynamoDb table and IAM configuration should all be scripted to allow simple and consistent deployment of infrastructure.



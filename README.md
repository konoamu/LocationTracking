# LocationTracking

LocationTracking is a project aimed at managing user locations and providing location-based services.

## Table of Contents

- [MongoDB Requirement](#mongodb-requirement)
- [Docker Support](#docker-support)
- [LocationTracking.API](#locationtracking-api)
- [LocationTracking.Core](#locationtracking-core)
- [LocationTracking.Data](#locationtracking-data)
- [LocationTracking.Events](#locationtracking-events)

## MongoDB Requirement

LocationTracking requires MongoDB as its database backend. Ensure that MongoDB is running and accessible before starting the application.

## Docker Support

This project includes Docker support for containerized deployment. The Dockerfile provided builds the project into a container, exposing ports 8080 and 8081. To use Docker for development or deployment, follow the instructions provided in the Dockerfile.

### Building the Docker Image

To build the Docker image, execute the following command in the root directory of the project:

```bash
docker build -t location-tracking-api .
```

### Running the Docker Container

To run the Docker container, execute the following command:

```bash
docker run -p 8080:8080 -p 8081:8081 location-tracking-api
```

### Docker Compose

For a more streamlined setup, you can use Docker Compose. The provided `docker-compose.yml` file defines the services required for the application, including MongoDB and the LocationTracking API.

```yaml
version: '3.4'

services:
  mongodb:
    image: mongo
    container_name: mongodb
    environment:
      MONGO_INITDB_ROOT_USERNAME: root
      MONGO_INITDB_ROOT_PASSWORD: root
  locationtracking.api:
    image: ${DOCKER_REGISTRY-}locationtrackingapi
    build:
      context: .
      dockerfile: LocationTracking.API/Dockerfile
    depends_on:
      - mongodb
```

To start the services using Docker Compose, run the following command:

```bash
docker-compose up
```

## LocationTracking.API

LocationTracking.API is an ASP.NET Core Web API project responsible for handling HTTP requests related to user locations.

### Controllers

- **LocationController**: Manages operations related to user locations such as adding locations, retrieving latest locations, and retrieving location history.
- **UserController**: Manages operations related to users such as retrieving user details and adding new users.

## LocationTracking.Core

LocationTracking.Core contains the core business logic and services for managing user locations and users.

### Services

- **LocationService**: Provides functionality for managing user locations including retrieving latest locations, updating locations, and retrieving location history.
- **UserService**: Handles user-related operations such as retrieving user details and adding new users.

## LocationTracking.Data

LocationTracking.Data contains data transfer objects (DTOs) and entities used within the application.

### DTOs

- **LocationDto**: Represents a location with latitude, longitude, and timestamp.
- **UserDto**: Represents user data including the user's name.

### Entities

- **Location**: Represents a location entity stored in the database.
- **User**: Represents a user entity stored in the database.

## LocationTracking.Events

LocationTracking.Events contains event-related functionality for handling location-related events.

### Event Handlers

- **LocationAddedEventArgs**: Provides event arguments for location added events, including user ID, location data, and timestamp.
- **ILocationAddedEventHandler**: Interface for handling location added events.



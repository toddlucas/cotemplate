Project structure
=================

## Server

The server is a .NET Web API project.
It's structured as a classic 3-tier monolith.

* API
* Services
* Data

It runs one process for the API, exposed at one of these ports, depending on the launch configuration selected:

* https://localhost:9595
* http://localhost:8585
* https://localhost:8081 - Docker (exposed port is random)
* http://localhost:8080 - Docker (exposed port is random)

A second process hosts Hangfire to execute background tasks.
The hangfire UI runs as part of the API project.

* http://localhost:8585/hangfire

The API documentation is hosted here:

* http://localhost:8585/scalar/v1
* http://localhost:8585/openapi/v1.json

## Client

The client is a Vite SPA.
It's structured as two projects, at the following URLs:

* http://localhost:8686 - app
* http://localhost:8787 - admin

They're set up as React+TypeScript projects.
The client accesses the local server using CORS configuration in `Program.cs`.

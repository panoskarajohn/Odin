# Odin [![.NET](https://github.com/panoskarajohn/Odin/actions/workflows/dotnet.yml/badge.svg)](https://github.com/panoskarajohn/Odin/actions/workflows/dotnet.yml)

# Introduction

This is a playground, trying to become a decent solution.
This is in progress, trying to put everything together.

## Local development

In order to bring up all the infrastructure you need to first start replication of Mongo.
So you can run and debug the service you develop without worrying about installing all kind of different infrastructure.
Everything is dependent on Docker :).

# Bring up the infrastructure

Run the following commands:

```
➜  Odin (master) ./start_infra.yml
```

Then running the applications can be enough for local testing and debugging
BUT
You can bring up the apps as containers as well

```
➜  Odin (master) docker-compose up -d
```

You can send requests to odin/event.api
## Send Requests

I have attached some postman requests on the root of the project
Have fun! :)

# Community:

If you don't like what you see do not hesitate to open an issue or create a pull request of your own.

This is a repo with a lot of stolen code, maybe a bit modified.

Mostly from some repositories which I really like:

- https://github.com/dotnet-architecture/eShopOnContainers
- https://github.com/meysamhadeli/Airline-Microservices
- https://github.com/devmentors/Pacco

Inspired a lot from https://github.com/devmentors/

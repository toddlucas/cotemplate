# CorpOS

Corp OS is an operating system for small businesses — helping founders navigate formation, compliance, and day-to-day operations with guided checklists, reminders, and governance tools.

# Setup

From the repo root, run

```sh
$ dotnet tool restore
```

## First time setup

From the repo root, run

```sh
$ dotnet new tool-manifest
```

```sh
$ dotnet tool install --local dotnet-ef
```

```sh
$ dotnet tool install --local dotnet-typegen
```

## Projects and ports

client

* web 8383
* sys 8484
* lib

server

* API http 8181
* API https 8282

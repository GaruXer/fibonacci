# Étape 1 : Construction de l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.19 AS build
WORKDIR /src
COPY . .
RUN apk update && apk upgrade
RUN apk add --no-cache clang build-base zlib-dev
RUN dotnet publish "demo.csproj" -c Release -r linux-musl-x64 /p:PublishAot=true /p:PublishTrimmed=true /p:PublishReadyToRun=true -o /publish

# Étape 2 : Préparer l'image finale avec un utilisateur non-root
FROM alpine:3.19 AS base
WORKDIR /app

# Créer un utilisateur non-root
RUN addgroup -S appgroup && adduser -S appuser -G appgroup

# Copier les fichiers depuis l'étape de build
COPY --from=build /publish .

# Changer les permissions pour permettre à l'utilisateur non-root d'accéder aux fichiers
RUN chown -R appuser:appgroup /app

# Définir les variables d'environnement
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

# Changer d'utilisateur pour exécuter l'application en tant qu'utilisateur non-root
USER appuser

# Définir le point d'entrée
ENTRYPOINT ["/app/demo"]

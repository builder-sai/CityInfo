FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Jif/Jif.csproj Jif/
RUN dotnet restore Jif/Jif.csproj
COPY . .
WORKDIR /src/Jif
RUN dotnet build Jif.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Jif.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Jif.dll"]

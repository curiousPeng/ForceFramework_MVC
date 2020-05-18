FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Force.App/Force.App.csproj", "Force.App/"]
RUN dotnet restore "Force.App/Force.App.csproj"
COPY . .
WORKDIR "/src/Force.App"
RUN dotnet build "Force.App.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Force.App.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Force.App.dll"]
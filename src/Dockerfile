FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY TasksManager.Web/*.csproj ./TasksManager.Web/
RUN dotnet restore

# copy everything else and build app
COPY TasksManager.Web/. ./TasksManager.Web/
WORKDIR /app/TasksManager.Web
RUN dotnet publish -c Release -o out


FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/TasksManager.Web/out ./
ENTRYPOINT ["dotnet", "TasksManager.Web.dll"]
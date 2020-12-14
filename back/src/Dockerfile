FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

COPY *.sln .
COPY Chatbot.Abstractions/*.csproj ./Chatbot.Abstractions/
COPY Chatbot.Common/*.csproj ./Chatbot.Common/
COPY Chatbot.Core/*.csproj ./Chatbot.Core/
COPY Chatbot.Data/*.csproj ./Chatbot.Data/
COPY Chatbot.Ef/*.csproj ./Chatbot.Ef/
COPY Chatbot.Ef.Data/*.csproj ./Chatbot.Ef.Data/
COPY Chatbot.Hosting/*.csproj ./Chatbot.Hosting/
COPY Chatbot.Ioc/*.csproj ./Chatbot.Ioc/
#RUN dotnet restore
#
#COPY . .
#RUN dotnet publish -c release -o /app --no restore
#
#FROM mcr.microsoft.com/dotnet/nightly/aspnet:5.0
#WORKDIR /app
#COPY --from=build /app .
#ENTRYPOINT ["dotnet", "Chatbot.Hosting.dll", "--environment", "production"]
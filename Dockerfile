FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
RUN apt update && apt install ffmpeg -y
COPY --from=build-env /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "YTMP3DownloadAPI.dll"]

# build container
# docker build -t yt-mp3download-api .

# 啟動container
# docker run -d --name yt-mp3download-api -p 127.0.0.1:8082:80 yt-mp3download-api
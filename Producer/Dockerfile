FROM microsoft/dotnet
WORKDIR /app
COPY ./artifacts .
RUN apt-get update
RUN apt-get --assume-yes install curl unzip
RUN curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l ~/vsdbg
ENTRYPOINT ["dotnet", "Producer.dll"]